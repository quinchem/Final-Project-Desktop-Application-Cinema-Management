using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminApp.Models;
using Microsoft.Data.Sqlite;

namespace AdminApp.Repositories
{
    public class ShowtimeRepo
    {
        public static List<Showtime> GetAll()
        {
            List<Showtime> list = new List<Showtime>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM Showtime"; 

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Gọi lại hàm ReadShowtime bạn đã viết sẵn bên dưới
                        list.Add(ReadShowtime(reader));
                    }
                }
            }
            return list;
        }
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
        public static List<Showtime> GetShowByFilm(string id)
        {
            List<Showtime> list = new List<Showtime>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT showtime_id, movie_id, auditorium_id, show_date, start_time, end_time
                    FROM Showtime
                    WHERE movie_id = @movie_id
                    ORDER BY show_date ASC, start_time ASC";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@movie_id", id);

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

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT showtime_id, movie_id, auditorium_id, show_date, start_time, end_time
                    FROM Showtime
                    WHERE show_date = @show_date
                    ORDER BY start_time ASC";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@show_date", date.ToString("dd-MM-yyyy"));

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
        public static List<Showtime> GetShowByFilmAndDate(string id, DateTime date)
        {
            List<Showtime> list = new List<Showtime>();
            string dateStr = date.ToString("dd-MM-yyyy");

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT showtime_id, movie_id, auditorium_id, show_date, start_time, end_time
                    FROM Showtime
                    WHERE movie_id = @movieId AND show_date = @date
                    ORDER BY start_time ASC";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@movieId", id);
                    cmd.Parameters.AddWithValue("@show_date", date.ToString("dd-MM-yyyy"));

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
        // 4) Lấy 1 suất chiếu theo ID
        // ---------------------------------------------------------
        public static Showtime GetById(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT showtime_id, movie_id, auditorium_id, show_date, start_time, end_time
            FROM Showtime
            WHERE showtime_id = @showtime_id";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@showtime_id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return ReadShowtime(reader);
                    }
                }
            }
            return null;
        }

        // ---------------------------------------------------------
        // 5) Xóa suất chiếu
        // ---------------------------------------------------------
        public static void Delete(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"DELETE FROM Showtime WHERE showtime_id = @showtime_id";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@showtime_id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }

}
