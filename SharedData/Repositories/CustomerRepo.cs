using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;

namespace SharedData.Repositories
{
    public class CustomerRepo
    {
        private string ConnStr => DatabaseHelper.GetConnectionString();
        public Customer GetById(string id)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string sql = @"SELECT * FROM customer WHERE customer_id = @id";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new Customer
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
        }
        public List<Customer> GetAll()
        {
            List<Customer> list = new();
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string sql = "SELECT * FROM customer";
            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Customer
                {
                    customer_id = reader["customer_id"].ToString(),
                    full_name = reader["full_name"].ToString(),
                    email = reader["email"].ToString(),
                    phone_number = reader["phone_number"].ToString(),
                    gender = reader["gender"].ToString(),
                    date_of_birth = reader["date_of_birth"].ToString(),
                    address = reader["address"].ToString(),
                    create_date = reader["create_date"].ToString()
                });
            }

            return list;
        }

       
        public bool CheckEmailExist(string email)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Customer WHERE email = @email";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void UpdateAvatarPath(string customerId, string avatarPath)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE customer SET avatar_path = @p WHERE customer_id = @id";
                    cmd.Parameters.AddWithValue("@p", avatarPath);
                    cmd.Parameters.AddWithValue("@id", customerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool Update(Customer c)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string sql = @"
                UPDATE customer SET
                    full_name = @name,
                    email = @mail,
                    phone_number = @phone,
                    gender = @gender,
                    date_of_birth = @dob,
                    address = @address,
                    create_date = @created
                WHERE customer_id = @id
            ";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", c.customer_id);
            cmd.Parameters.AddWithValue("@name", c.full_name);
            cmd.Parameters.AddWithValue("@mail", c.email);
            cmd.Parameters.AddWithValue("@phone", c.phone_number);
            cmd.Parameters.AddWithValue("@gender", c.gender);
            cmd.Parameters.AddWithValue("@dob", c.date_of_birth);
            cmd.Parameters.AddWithValue("@address", c.address);
            cmd.Parameters.AddWithValue("@created", c.create_date);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(string id)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string sql = "DELETE FROM customer WHERE customer_id = @id";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}


