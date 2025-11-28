using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SharedData.Repositories
{
    public class AccountRepo
    {
        private string ConnStr => DatabaseHelper.GetConnectionString();

        // =========================
        // REGISTER
        // =========================
        public bool Register(Customer customer, Account account, out string message)
        {
            message = "";

            if (customer == null || account == null)
            {
                message = "Customer hoặc Account không hợp lệ.";
                return false;
            }

            // ✅ Validate phone
            if (!Regex.IsMatch(customer.phone_number ?? "", @"^\d{10}$"))
            {
                message = "SĐT phải đúng 10 chữ số.";
                return false;
            }

            // ✅ Validate password
            if (!Regex.IsMatch(account.password ?? "", @"^(?=.{8,})(?=.*\W).*$"))
            {
                message = "Mật khẩu phải ≥8 ký tự và có ký tự đặc biệt.";
                return false;
            }

            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            using var pragma = conn.CreateCommand();
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();

            using var tran = conn.BeginTransaction();

            try
            {
                // ✅ Check trùng email / phone
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"SELECT COUNT(*) FROM customer 
                                       WHERE email=@e OR phone_number=@p";
                    cmd.Parameters.AddWithValue("@e", customer.email ?? "");
                    cmd.Parameters.AddWithValue("@p", customer.phone_number ?? "");

                    if ((long)cmd.ExecuteScalar() > 0)
                    {
                        message = "Email hoặc SĐT đã tồn tại.";
                        tran.Rollback();
                        return false;
                    }
                }

                // ✅ Generate customer_id
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"
                        SELECT 'C' || printf('%03d',
                        IFNULL(MAX(CAST(SUBSTR(customer_id,2) AS INT)),0)+1)
                        FROM customer";
                    customer.customer_id = cmd.ExecuteScalar().ToString();
                }

                customer.create_date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                // ✅ Insert customer
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"
                        INSERT INTO customer
                        (customer_id, full_name, email, phone_number, gender,
                         date_of_birth, address, create_date)
                        VALUES
                        (@id,@name,@email,@phone,@gender,@dob,@addr,@created)";
                    cmd.Parameters.AddWithValue("@id", customer.customer_id);
                    cmd.Parameters.AddWithValue("@name", customer.full_name ?? "");
                    cmd.Parameters.AddWithValue("@email", customer.email ?? "");
                    cmd.Parameters.AddWithValue("@phone", customer.phone_number ?? "");
                    cmd.Parameters.AddWithValue("@gender", customer.gender ?? "");
                    cmd.Parameters.AddWithValue("@dob",
                        string.IsNullOrWhiteSpace(customer.date_of_birth) ? DBNull.Value : customer.date_of_birth);
                    cmd.Parameters.AddWithValue("@addr", customer.address ?? "");
                    cmd.Parameters.AddWithValue("@created", customer.create_date);

                    cmd.ExecuteNonQuery();
                }

                // ✅ Hash password
                account.password = HashPassword(account.password);
                account.account_id = Guid.NewGuid().ToString();
                account.customer_id = customer.customer_id;
                account.role_account ??= "customer";

                // ✅ Insert account
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"
                        INSERT INTO account
                        (account_id, username, password, role_account, staff_id, customer_id)
                        VALUES
                        (@aid,@user,@pass,@role,@staff,@cid)";
                    cmd.Parameters.AddWithValue("@aid", account.account_id);
                    cmd.Parameters.AddWithValue("@user", account.username ?? customer.email);
                    cmd.Parameters.AddWithValue("@pass", account.password);
                    cmd.Parameters.AddWithValue("@role", account.role_account);
                    cmd.Parameters.AddWithValue("@staff",
                        string.IsNullOrWhiteSpace(account.staff_id) ? DBNull.Value : (object)account.staff_id);
                    cmd.Parameters.AddWithValue("@cid", account.customer_id);

                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                message = ex.Message;
                return false;
            }
        }

        // =========================
        // LOGIN
        // =========================
        public bool Login(string usernameOrEmail, string password, out Customer customer, out string msg)
        {
            msg = "";
            customer = null;

            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT a.password,
                       c.customer_id,c.full_name,c.email,c.phone_number,
                       c.gender,c.date_of_birth,c.address,c.create_date
                FROM account a
                JOIN customer c ON a.customer_id = c.customer_id
                WHERE a.username=@u OR c.email=@u";

            cmd.Parameters.AddWithValue("@u", usernameOrEmail);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                msg = "Tài khoản không tồn tại.";
                return false;
            }

            string hash = reader["password"].ToString();
            if (!VerifyPassword(password, hash))
            {
                msg = "Sai mật khẩu.";
                return false;
            }

            customer = new Customer
            {
                customer_id = reader["customer_id"].ToString(),
                full_name = reader["full_name"].ToString(),
                email = reader["email"].ToString(),
                phone_number = reader["phone_number"].ToString(),
                gender = reader["gender"].ToString(),
                date_of_birth = reader["date_of_birth"].ToString(),
                address = reader["address"].ToString(),
                create_date = reader["create_date"].ToString()
            };

            return true;
        }

        // =========================
        // CHANGE PASSWORD
        // =========================
        public bool CheckOldPassword(string accountId, string oldPassword)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT password FROM account WHERE account_id=@id";
            cmd.Parameters.AddWithValue("@id", accountId);

            var hash = cmd.ExecuteScalar()?.ToString();
            return hash != null && VerifyPassword(oldPassword, hash);
        }

        public bool UpdatePassword(string accountId, string newPassword)
        {
            string newHash = HashPassword(newPassword);

            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE account SET password=@p WHERE account_id=@id";
            cmd.Parameters.AddWithValue("@p", newHash);
            cmd.Parameters.AddWithValue("@id", accountId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // =========================
        // HASH & VERIFY
        // =========================
        private static string HashPassword(string password)
        {
            const int iter = 100_000;
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iter, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            return $"{iter}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string stored)
        {
            var parts = stored.Split('.');
            if (parts.Length != 3) return false;

            int iter = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] hash = Convert.FromBase64String(parts[2]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iter, HashAlgorithmName.SHA256);
            byte[] test = pbkdf2.GetBytes(hash.Length);

            return CryptographicOperations.FixedTimeEquals(hash, test);
        }
    }
}
