using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;

namespace SharedData.Repositories
{
    public class AccountRepo
    {
        private string ConnStr => DatabaseHelper.GetConnectionString();

        // Generate Customer ID 
        private string GenerateCustomerId(SqliteConnection conn)
        {
            string sql = @"SELECT customer_id FROM customer ORDER BY customer_id DESC LIMIT 1";
            using var cmd = new SqliteCommand(sql, conn);

            var result = cmd.ExecuteScalar();
            if (result == null) return "C001";

            int num = int.Parse(result.ToString().Substring(1));
            return "C" + (num + 1).ToString("D3");
        }

        // Generate Account ID (Axxx)
        private string GenerateAccountId(SqliteConnection conn)
        {
            string sql = @"SELECT account_id FROM account ORDER BY account_id DESC LIMIT 1";
            using var cmd = new SqliteCommand(sql, conn);

            var result = cmd.ExecuteScalar();
            if (result == null) return "A001";

            int num = int.Parse(result.ToString().Substring(1));
            return "A" + (num + 1).ToString("D3");
        }

        // Check duplicate email
        private bool EmailExists(SqliteConnection conn, string email)
        {
            string sql = @"SELECT COUNT(*) FROM customer WHERE email = @e";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@e", email);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // ===========================
        // REGISTER (Customer + Account)
        // ===========================
        public bool Register(Customer c, Account a, out string message)
            {
            message = "";

            using var conn = new SqliteConnection(ConnStr);
                conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                if (EmailExists(conn, c.email))
                {
                    message = "Email đã tồn tại.";
                    return false;
                }

                string cid = GenerateCustomerId(conn);
                string aid = GenerateAccountId(conn);

                c.customer_id = cid;
                a.customer_id = cid;
                a.account_id = aid;

                // username = email
                a.username = c.email;

                // INSERT customer
                string sqlC = @"
                    INSERT INTO customer
                    (customer_id, full_name, email, phone_number, gender,
                     date_of_birth, address, create_date)
                    VALUES
                    (@id, @name, @mail, @phone, @gender, @dob, @address, @created)
                ";

                using (var cmd = new SqliteCommand(sqlC, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@id", c.customer_id);
                    cmd.Parameters.AddWithValue("@name", c.full_name);
                    cmd.Parameters.AddWithValue("@mail", c.email);
                    cmd.Parameters.AddWithValue("@phone", c.phone_number);
                    cmd.Parameters.AddWithValue("@gender", c.gender);
                    cmd.Parameters.AddWithValue("@dob", c.date_of_birth);
                    cmd.Parameters.AddWithValue("@address", c.address);
                    cmd.Parameters.AddWithValue("@created", c.create_date);
                    cmd.ExecuteNonQuery();
                }

                // INSERT account
                string sqlA = @"
                    INSERT INTO account
                    (account_id, username, password, role_account, staff_id, customer_id)
                    VALUES
                    (@aid, @user, @pass, @role, NULL, @cid)
                ";

                using (var cmd = new SqliteCommand(sqlA, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@aid", a.account_id);
                    cmd.Parameters.AddWithValue("@user", a.username);
                    cmd.Parameters.AddWithValue("@pass", a.password);
                    cmd.Parameters.AddWithValue("@role", a.role_account);
                    cmd.Parameters.AddWithValue("@cid", a.customer_id);
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

        // ===========================
        // LOGIN
        // ===========================
        public bool Login(string email, string password, out Customer customer, out string msg)
            {
            msg = "";
            customer = null;

            using var conn = new SqliteConnection(ConnStr);
                conn.Open();

            string sql = @"
                SELECT a.password,
                       c.customer_id, c.full_name, c.email, c.phone_number,
                       c.gender, c.date_of_birth, c.address, c.create_date
                FROM account a
                JOIN customer c ON c.customer_id = a.customer_id
                WHERE c.email = @e
            ";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@e", email);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                {
                msg = "Email không tồn tại.";
                return false;
            }

            string dbPass = reader["password"].ToString();
            if (dbPass != password)
            {
                msg = "Mật khẩu không đúng.";
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
        public bool CheckOldPassword(string staffId, string oldPassword)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT COUNT(*) 
                    FROM Account
                    WHERE staff_id = @staffId
                      AND password = @oldPass";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@staffId", staffId);
                    cmd.Parameters.AddWithValue("@oldPass", oldPassword);

                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool UpdatePassword(string staffId, string newPassword)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
                    UPDATE Account
                    SET password = @newPass
                    WHERE staff_id = @staffId";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@newPass", newPassword);
                    cmd.Parameters.AddWithValue("@staffId", staffId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

    }
}
