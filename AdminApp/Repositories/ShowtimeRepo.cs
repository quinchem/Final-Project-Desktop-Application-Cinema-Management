using AdminApp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace AdminApp.Repositories
{
    public static class ShowtimeRepo
    {
        private static string connStr = DatabaseHelper.GetConnectionString();

        // Convert DateTime -> string dd/MM/yyyy
        private static string ConvertDate(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        // ===================== GET ALL =====================
        public static List<Showtime> GetAll()
        {
            List<Showtime> list = new List<Showtime>();

            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = "SELECT * FROM showtime";

                using (var cmd = new SqliteCommand(query, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(ReadShowtime(rd));
                    }
                }
            }
            return list;
        }

        // ===================== GET BY FILM =====================
        public static List<Showtime> GetByFilm(string movieId)
        {
            List<Showtime> list = new List<Showtime>();

            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = "SELECT * FROM showtime WHERE movie_id = @m";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@m", movieId);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(ReadShowtime(rd));
                        }
                    }
                }
            }
            return list;
        }

        // ===================== GET BY DATE =====================
        public static List<Showtime> GetByDate(DateTime date)
        {
            List<Showtime> list = new List<Showtime>();
            string dateStr = ConvertDate(date); // dd/MM/yyyy

            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = "SELECT * FROM showtime WHERE show_date = @d";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@d", dateStr);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(ReadShowtime(rd));
                        }
                    }
                }
            }
            return list;
        }

        // ===================== GET BY FILM + DATE =====================
        public static List<Showtime> GetByFilmAndDate(string movieId, DateTime date)
        {
            List<Showtime> list = new List<Showtime>();
            string dateStr = ConvertDate(date);

            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT * FROM showtime
                                 WHERE movie_id = @m AND show_date = @d";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@m", movieId);
                    cmd.Parameters.AddWithValue("@d", dateStr);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(ReadShowtime(rd));
                        }
                    }
                }
            }
            return list;
        }

        // ===================== DELETE =====================
        public static void Delete(string id)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = "DELETE FROM showtime WHERE showtime_id = @id";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Thêm vào ShowtimeRepo.cs

        // INSERT
        public static void Insert(Showtime showtime)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = @"INSERT INTO showtime (showtime_id, movie_id, auditorium_id, show_date, start_time, end_time)
                        VALUES (@id, @mid, @aid, @date, @start, @end)";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", showtime.showtime_id);
                    cmd.Parameters.AddWithValue("@mid", showtime.movie_id);
                    cmd.Parameters.AddWithValue("@aid", showtime.auditorium_id);
                    cmd.Parameters.AddWithValue("@date", showtime.show_date);
                    cmd.Parameters.AddWithValue("@start", showtime.start_time);
                    cmd.Parameters.AddWithValue("@end", showtime.end_time);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // UPDATE
        public static void Update(Showtime showtime)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = @"UPDATE showtime 
                        SET movie_id = @mid, auditorium_id = @aid, show_date = @date, 
                            start_time = @start, end_time = @end
                        WHERE showtime_id = @id";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", showtime.showtime_id);
                    cmd.Parameters.AddWithValue("@mid", showtime.movie_id);
                    cmd.Parameters.AddWithValue("@aid", showtime.auditorium_id);
                    cmd.Parameters.AddWithValue("@date", showtime.show_date);
                    cmd.Parameters.AddWithValue("@start", showtime.start_time);
                    cmd.Parameters.AddWithValue("@end", showtime.end_time);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ===================== MAPPING =====================
        private static Showtime ReadShowtime(SqliteDataReader rd)
        {
            return new Showtime
            {
                showtime_id = rd["showtime_id"].ToString(),
                movie_id = rd["movie_id"].ToString(),
                auditorium_id = rd["auditorium_id"].ToString(),
                show_date = rd["show_date"].ToString(),
                start_time = rd["start_time"].ToString(),
                end_time = rd["end_time"].ToString()
            };
        }
    }
}
