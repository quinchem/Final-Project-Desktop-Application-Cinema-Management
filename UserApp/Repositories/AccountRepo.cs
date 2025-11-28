using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserApp.Models;

namespace UserApp.Repositories
{
    public static class CustomerRepo
    {
        public static class AccountRepo
        {
            /// <summary>
            /// Đăng ký tài khoản: lưu thông tin Customer và Account trong cùng 1 transaction
            /// </summary>
            public static bool Register(Customer customer, Account account, out string message)
            {
                message = "";

                if (customer == null || account == null)
                {
                    message = "Customer hoặc Account không được null.";
                    return false;
                }

                // Validate phone number 10 chữ số
                if (!Regex.IsMatch(customer.phone_number ?? "", @"^\d{10}$"))
                {
                    message = "Số điện thoại phải đúng 10 chữ số.";
                    return false;
                }

                // Validate password: tối thiểu 8 ký tự + ký tự đặc biệt
                if (!Regex.IsMatch(account.password ?? "", @"^(?=.{8,})(?=.*\W).*$"))
                {
                    message = "Mật khẩu phải tối thiểu 8 ký tự và chứa ít nhất một ký tự đặc biệt.";
                    return false;
                }

                try
                {
                    using (var conn = DatabaseHelper2.GetConnection())
                    {
                        conn.Open();

                        // Bật foreign key trong SQLite
                        using (var pragma = conn.CreateCommand())
                        {
                            pragma.CommandText = "PRAGMA foreign_keys = ON;";
                            pragma.ExecuteNonQuery();
                        }

                        using (var tx = conn.BeginTransaction())
                        {
                            // 1️⃣ Kiểm tra trùng email/phone trong customer
                            using (var checkCustomerCmd = conn.CreateCommand())
                            {
                                checkCustomerCmd.Transaction = tx;
                                checkCustomerCmd.CommandText = "SELECT COUNT(1) FROM customer WHERE email = @e OR phone_number = @p";
                                checkCustomerCmd.Parameters.AddWithValue("@e", customer.email ?? "");
                                checkCustomerCmd.Parameters.AddWithValue("@p", customer.phone_number ?? "");
                                if ((long)checkCustomerCmd.ExecuteScalar() > 0)
                                {
                                    message = "Email hoặc số điện thoại đã tồn tại.";
                                    tx.Rollback();
                                    return false;
                                }
                            }

                            // 2️⃣ Kiểm tra trùng username trong account
                            using (var checkAccountCmd = conn.CreateCommand())
                            {
                                checkAccountCmd.Transaction = tx;
                                checkAccountCmd.CommandText = "SELECT COUNT(1) FROM account WHERE username = @u";
                                checkAccountCmd.Parameters.AddWithValue("@u", account.username ?? "");
                                if ((long)checkAccountCmd.ExecuteScalar() > 0)
                                {
                                    message = "Username đã tồn tại.";
                                    tx.Rollback();
                                    return false;
                                }
                            }

                            // 3️⃣ Chèn Customer
                            using (var getIdCmd = conn.CreateCommand())
                            {
                                getIdCmd.Transaction = tx;
                                getIdCmd.CommandText = @"
                                    SELECT 'C' || printf('%03d',
                                        IFNULL(MAX(CAST(SUBSTR(customer_id, 2) AS INTEGER)), 0) + 1
                                    )
                                    FROM customer;
                                    ";

                                customer.customer_id = getIdCmd.ExecuteScalar().ToString();
                            }

                            customer.create_date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                            using (var insertCustomerCmd = conn.CreateCommand())
                            {
                                insertCustomerCmd.Transaction = tx;
                                insertCustomerCmd.CommandText = @"
                                    INSERT INTO customer
                                    (customer_id, full_name, email, phone_number, gender, date_of_birth, address, create_date)
                                    VALUES
                                    (@customerId, @fullName, @email, @phone, @gender, @dob, @address, @createDate);
                                ";

                                insertCustomerCmd.Parameters.AddWithValue("@customerId", customer.customer_id);
                                insertCustomerCmd.Parameters.AddWithValue("@fullName", customer.full_name ?? "");
                                insertCustomerCmd.Parameters.AddWithValue("@email", customer.email ?? "");
                                insertCustomerCmd.Parameters.AddWithValue("@phone", customer.phone_number ?? "");
                                insertCustomerCmd.Parameters.AddWithValue("@gender", customer.gender ?? "");
                                insertCustomerCmd.Parameters.AddWithValue("@dob",
                                    string.IsNullOrWhiteSpace(customer.date_of_birth) ? DBNull.Value : customer.date_of_birth);
                                insertCustomerCmd.Parameters.AddWithValue("@address", customer.address ?? "");
                                insertCustomerCmd.Parameters.AddWithValue("@createDate", customer.create_date);

                                if (insertCustomerCmd.ExecuteNonQuery() <= 0)
                                {
                                    message = "Không thể lưu thông tin khách hàng.";
                                    tx.Rollback();
                                    return false;
                                }
                            }

                            // 4️⃣ Hash password
                            account.password = HashPassword(account.password);

                            // 5️⃣ Chèn Account
                            account.account_id = Guid.NewGuid().ToString();
                            account.customer_id = customer.customer_id; // liên kết
                            using (var insertAccountCmd = conn.CreateCommand())
                            {
                                insertAccountCmd.Transaction = tx;
                                insertAccountCmd.CommandText = @"
                                INSERT INTO account
                                (account_id, username, password, role_account, staff_id, customer_id)
                                VALUES
                                (@accountId, @username, @password, @role, @staffId, @customerId);";

                                insertAccountCmd.Parameters.AddWithValue("@accountId", account.account_id);
                                insertAccountCmd.Parameters.AddWithValue("@username", account.username ?? "");
                                insertAccountCmd.Parameters.AddWithValue("@password", account.password);
                                insertAccountCmd.Parameters.AddWithValue("@role", account.role_account ?? "customer");
                                insertAccountCmd.Parameters.AddWithValue("@staffId", string.IsNullOrWhiteSpace(account.staff_id) ? DBNull.Value : (object)account.staff_id);
                                insertAccountCmd.Parameters.AddWithValue("@customerId", account.customer_id);

                                if (insertAccountCmd.ExecuteNonQuery() <= 0)
                                {
                                    message = "Không thể lưu tài khoản.";
                                    tx.Rollback();
                                    return false;
                                }
                            }

                            tx.Commit();
                            return true;
                        } // transaction
                    } // connection
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }

            /// <summary>
            /// Đăng nhập
            /// </summary>
            public static bool Login(string username, string password, out Customer customer, out string message)
            {
                message = "";
                customer = null;

                try
                {
                    using (var conn = DatabaseHelper2.GetConnection())
                    {
                        conn.Open();

                        // Bật foreign key
                        using (var pragma = conn.CreateCommand())
                        {
                            pragma.CommandText = "PRAGMA foreign_keys = ON;";
                            pragma.ExecuteNonQuery();
                        }

                        // Lấy account + customer theo username
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"
                            SELECT a.account_id, a.username, a.password, a.role_account,
                                   c.customer_id, c.full_name, c.email, c.phone_number,
                                   c.gender, c.date_of_birth, c.address, c.create_date
                            FROM account a
                            LEFT JOIN customer c ON a.customer_id = c.customer_id
                            WHERE a.username = @username";
                            cmd.Parameters.AddWithValue("@username", username);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    message = "Tài khoản không tồn tại.";
                                    return false;
                                }

                                string storedHash = reader.GetString(reader.GetOrdinal("password"));

                                if (!VerifyPassword(password, storedHash))
                                {
                                    message = "Sai mật khẩu.";
                                    return false;
                                }

                                // Nếu login thành công, build Customer object
                                customer = new Customer
                                {
                                    customer_id = reader["customer_id"]?.ToString(),
                                    full_name = reader["full_name"]?.ToString(),
                                    email = reader["email"]?.ToString(),
                                    phone_number = reader["phone_number"]?.ToString(),
                                    gender = reader["gender"]?.ToString(),
                                    date_of_birth = reader["date_of_birth"]?.ToString(),
                                    address = reader["address"]?.ToString(),
                                    create_date = reader["create_date"]?.ToString()
                                };

                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }

            // ====================
            // PBKDF2 password hashing & verify
            // ====================
            private static string HashPassword(string password)
            {
                const int iterations = 100_000;
                const int saltSize = 16;
                const int hashSize = 32;

                using (var rng = RandomNumberGenerator.Create())
                {
                    byte[] salt = new byte[saltSize];
                    rng.GetBytes(salt);

                    using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                    {
                        byte[] hash = pbkdf2.GetBytes(hashSize);
                        return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
                    }
                }
            }

            private static bool VerifyPassword(string password, string stored)
            {
                var parts = stored.Split('.');
                if (parts.Length != 3)
                    return false;

                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] hash = Convert.FromBase64String(parts[2]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] computedHash = pbkdf2.GetBytes(hash.Length);
                    return CryptographicOperations.FixedTimeEquals(hash, computedHash);
                }
            }
        }
    }
}
