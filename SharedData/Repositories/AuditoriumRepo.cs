using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SharedData.Models;

namespace SharedData.Repositories
{
    public class AuditoriumRepo
    {
        // Lấy tất cả phòng + loại phòng
        public List<Auditorium> GetAll()
        {
            var list = new List<Auditorium>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT a.auditorium_id,
                           a.auditorium_type_id,
                           a.name,
                           a.number_of_seats,
                           t.auditorium_type
                    FROM auditorium a
                    LEFT JOIN auditorium_type t 
                         ON a.auditorium_type_id = t.auditorium_type_id
                    ORDER BY a.auditorium_id;
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new Auditorium
                        {
                            auditorium_id = rd["auditorium_id"].ToString(),
                            auditorium_type_id = rd["auditorium_type_id"].ToString(),
                            name = rd["name"].ToString(),
                            number_of_seats = Convert.ToInt32(rd["number_of_seats"]),
                        });
                    }
                }
            }

            return list;
        }

        public Auditorium GetById(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT a.auditorium_id,
                           a.auditorium_type_id,
                           a.name,
                           a.number_of_seats,
                           t.auditorium_type
                    FROM auditorium a
                    LEFT JOIN auditorium_type t
                         ON a.auditorium_type_id = t.auditorium_type_id
                    WHERE a.auditorium_id = @auditorium_id;
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@auditorium_id", id);

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            return new Auditorium
                            {
                                auditorium_id = rd["auditorium_id"].ToString(),
                                auditorium_type_id = rd["auditorium_type_id"].ToString(),
                                name = rd["name"].ToString(),
                                number_of_seats = Convert.ToInt32(rd["number_of_seats"]),
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Insert(Auditorium a)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO auditorium (auditorium_id, auditorium_type_id, name, number_of_seats)
                    VALUES (@auditorium_id, @auditorium_type_id, @name, @number_of_seats);
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@auditorium_id", a.auditorium_id);
                    cmd.Parameters.AddWithValue("@auditorium_type_id", a.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@name", a.name);
                    cmd.Parameters.AddWithValue("@number_of_seats", a.number_of_seats);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Auditorium a)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE auditorium
                    SET auditorium_type_id = @auditorium_type_id,
                        name = @name,
                        number_of_seats = @number_of_seats
                    WHERE auditorium_id = @auditorium_id;
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@auditorium_id", a.auditorium_id);
                    cmd.Parameters.AddWithValue("@auditorium_type_id", a.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@name", a.name);
                    cmd.Parameters.AddWithValue("@number_of_seats", a.number_of_seats);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"DELETE FROM auditorium WHERE auditorium_id = @auditorium_id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@auditorium_id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
