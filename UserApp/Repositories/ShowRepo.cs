using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using UserApp.Models;

namespace UserApp.Repositories
{
    public static class ShowRepo
    {
        // ------------------------------
        // Đọc 1 dòng Showtime từ Reader
        // ------------------------------
        private static Showtime ReadShowtime(SqliteDataReader reader)
        {
            return new Showtime
            {
                showtime_id = reader["showtime_id"]?.ToString(),
                movie_id = reader["movie_id"]?.ToString(),
                auditorium_id = reader["auditorium_id"]?.ToString(),
                show_date = reader["show_date"]?.ToString(),
                start_time = reader["start_time"]?.ToString(),
                end_time = reader["end_time"]?.ToString()
            };
        }

        // ---------------------------------------------------------
        // 1) Lấy tất cả suất chiếu theo phim (movie_id)
        // ---------------------------------------------------------
        public static List<Showtime> GetShowByFilm(string movieId)
        {
            List<Showtime> list = new List<Showtime>();

            using (var conn = DatabaseHelper2.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT showtime_id, movie_id, auditorium_id, show_date, start_time, end_time
                    FROM showtime
                    WHERE movie_id = @movieId
                    ORDER BY show_date ASC, start_time ASC";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@movieId", movieId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(ReadShowtime(reader));
                        }
                    }
                }
            }

            return list;
        }

        // ---------------------------------------------------------
        // 2) Lấy suất chiếu theo ngày (mọi phim)
        // ---------------------------------------------------------
        public static List<Showtime> GetShowByDate(DateTime date)
        {
            List<Showtime> list = new List<Showtime>();
            string dateStr = date.ToString("dd-MM-yyyy");

            using (var conn = DatabaseHelper2.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT showtime_id, movie_id, auditorium_id, show_date, start_time, end_time
                    FROM showtime
                    WHERE show_date = @date
                    ORDER BY start_time ASC";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@date", dateStr);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(ReadShowtime(reader));
                        }
                    }
                }
            }

            return list;
        }

        // ---------------------------------------------------------
        // 3) Lấy suất chiếu theo phim + ngày
        // ---------------------------------------------------------
        public static List<Showtime> GetShowByFilmAndDate(string movieId, DateTime date)
        {
            List<Showtime> list = new List<Showtime>();
            string dateStr = date.ToString("dd-MM-yyyy");

            using (var conn = DatabaseHelper2.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT showtime_id, movie_id, auditorium_id, show_date, start_time, end_time
                    FROM showtime
                    WHERE movie_id = @movieId AND show_date = @date
                    ORDER BY start_time ASC";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@movieId", movieId);
                    cmd.Parameters.AddWithValue("@date", dateStr);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(ReadShowtime(reader));
                        }
                    }
                }
            }

            return list;
        }
    }
}
