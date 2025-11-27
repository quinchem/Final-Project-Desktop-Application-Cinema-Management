using AdminApp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.Repositories
{
    public class StaffRepo
    {
        public Staff GetStaffById(string staff_id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT
                        staff_id,
                        full_name,
                        date_of_birth,
                        gender,
                        email,
                        phone_number,
                        role
                    FROM Staff
                    WHERE staff_id = @staff_id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@staff_id", staff_id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Staff
                            {
                                staff_id = reader["staff_id"].ToString(),
                                full_name = reader["full_name"].ToString(),
                                date_of_birth = reader["date_of_birth"].ToString(),
                                gender = reader["gender"].ToString(),
                                email = reader["email"].ToString(),
                                phone_number = reader["phone_number"].ToString(),
                                role = reader["role"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool UpdateStaff(Staff staff)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Staff
                    SET
                        full_name = @full_name,
                        date_of_birth = @date_of_birth,
                        gender = @gender,
                        email = @email,
                        phone_number = @phone_number
                    WHERE staff_id = @staff_id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@full_name", staff.full_name);
                    cmd.Parameters.AddWithValue("@date_of_birth", staff.date_of_birth);
                    cmd.Parameters.AddWithValue("@gender", staff.gender);
                    cmd.Parameters.AddWithValue("@email", staff.email);
                    cmd.Parameters.AddWithValue("@phone_number", staff.phone_number);
                    cmd.Parameters.AddWithValue("@staff_id", staff.staff_id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
