using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using AdminApp.Models;

namespace AdminApp.Repositories
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
                            number_of_seats = rd.GetInt32(rd.GetOrdinal("number_of_seats")),
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
                    WHERE a.auditorium_id = @id;
                    ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            return new Auditorium
                            {
                                auditorium_id = rd["auditorium_id"].ToString(),
                                auditorium_type_id = rd["auditorium_type_id"].ToString(),
                                name = rd["name"].ToString(),
                                number_of_seats = rd.GetInt32(rd.GetOrdinal("number_of_seats")),
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
                    VALUES (@id, @type, @name, @seats);
                    ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", a.auditorium_id);
                    cmd.Parameters.AddWithValue("@type", a.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@name", a.name);
                    cmd.Parameters.AddWithValue("@seats", a.number_of_seats);
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
                    SET auditorium_type_id = @type,
                        name = @name,
                        number_of_seats = @seats
                    WHERE auditorium_id = @id;
                    ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", a.auditorium_id);
                    cmd.Parameters.AddWithValue("@type", a.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@name", a.name);
                    cmd.Parameters.AddWithValue("@seats", a.number_of_seats);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"DELETE FROM auditorium WHERE auditorium_id = @id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
