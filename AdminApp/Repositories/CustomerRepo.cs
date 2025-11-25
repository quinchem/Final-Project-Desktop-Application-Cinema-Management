using AdminApp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.Repositories
{
    public class CustomerRepository
    {
        public List<Customer> GetAllCustomers()
        {
            List<Customer> list = new List<Customer>();

            using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();

                string query = @"
                    SELECT customer_id, full_name, email, phone_number,
                           gender, date_of_birth, address, create_date
                    FROM customer";

                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
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
                }
            }

            return list;
        }
        public bool DeleteCustomer(string id)
        {
            using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();

                string query = "DELETE FROM customer WHERE customer_id = @id";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool UpdateCustomer(string id, string fullName, string gender,
                           string birth, string phone, string email,
                           string address, string createDate)
        {
            using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();

                string query = @"
            UPDATE customer
            SET full_name = @full_name,
                gender = @gender,
                date_of_birth = @dob,
                phone_number = @phone,
                email = @email,
                address = @address,
                create_date = @create_date
            WHERE customer_id = @id
        ";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@full_name", fullName);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@dob", birth);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@create_date", createDate);
                    cmd.Parameters.AddWithValue("@id", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

    }
}