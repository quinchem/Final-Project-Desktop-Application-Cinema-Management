using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SharedData.Repositories
{
    public class ShowtimeRepo
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

        // Lấy suất chiếu theo khoảng ngày (JOIN với Auditorium và AuditoriumType)
        // ✅ SỬA: Lấy suất chiếu theo khoảng ngày
        public List<ShowtimeInfo> GetShowtimesByDateRange(DateTime startDate, int days)
        {
            List<ShowtimeInfo> list = new List<ShowtimeInfo>();

            try
            {
                // 1️⃣ Format ngày chuẩn ISO yyyy-MM-dd
                string strStart = startDate.ToString("yyyy-MM-dd");
                string strEnd = startDate.AddDays(days - 1).ToString("yyyy-MM-dd");

                // 1. Sửa lại câu Query dùng JOIN
                string query = $@"
                        SELECT 
                            s.showtime_id, 
                            s.show_date, 
                            s.start_time, 
                            s.end_time, 
                            s.movie_id,
                            m.title,    
                            m.duration,
                            a.name,     
                            at.auditorium_type    
                        FROM showtime s
                        LEFT JOIN movie m ON s.movie_id = m.movie_id          -- Nối với bảng Phim
                        LEFT JOIN auditorium a ON s.auditorium_id = a.auditorium_id  -- Nối với bảng Phòng
                        LEFT JOIN auditorium_type at ON a.auditorium_type_id = at.auditorium_type_id -- Nối với bảng Loại Phòng  
                       
                        ORDER BY s.show_date, s.start_time
                    ";

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var info = new ShowtimeInfo
                                {
                                    // Đảm bảo tên cột trong chuỗi khớp với Database của bạn
                                    showtime_id = reader["showtime_id"]?.ToString(),
                                    movie_id = reader["movie_id"]?.ToString(),
                                    title = reader["title"]?.ToString(),
                                    show_date = reader["show_date"]?.ToString(),
                                    start_time = reader["start_time"]?.ToString(),
                                    duration = reader["duration"] != DBNull.Value ? Convert.ToInt32(reader["duration"]) : 0,
                                    auditorium_type = reader["auditorium_type"]?.ToString(),
                                    name = reader["name"]?.ToString(),
                                    // Nếu cột nào null thì bỏ qua hoặc handle, đừng để crash
                                };
                                list.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 🔥 QUAN TRỌNG: Hiện lỗi lên để biết đường sửa
                throw new Exception("Lỗi tại GetShowtimesByDateRange: " + ex.Message);
            }

            return list;
        }

        public List<ShowtimeInfo> GetShowtimesByDateRangeAndMovie(DateTime startDate, int days, string movieId)
        {
            List<ShowtimeInfo> list = new List<ShowtimeInfo>();

            if (string.IsNullOrEmpty(movieId)) return list;
            string cleanId = movieId.Trim();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // 🔥 CHỐT HẠ: CHỈ WHERE movie_id, BỎ HẾT ĐIỀU KIỆN NGÀY THÁNG
                    // Vì SQLite so sánh chuỗi ngày '29/11' > '28/11' bị sai logic nếu định dạng không chuẩn ISO
                    string query = @"
                SELECT 
                    s.showtime_id, s.show_date, s.start_time, s.end_time, 
                    s.movie_id, m.title, m.duration,
                    a.name, at.auditorium_type
                FROM showtime s
                LEFT JOIN movie m ON s.movie_id = m.movie_id
                LEFT JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                LEFT JOIN auditorium_type at ON a.auditorium_type_id = at.auditorium_type_id
                WHERE s.movie_id = @movieId 
            ";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        // Dùng tham số cho an toàn
                        cmd.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@movieId", cleanId));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new ShowtimeInfo
                                {
                                    showtime_id = reader["showtime_id"]?.ToString(),
                                    movie_id = reader["movie_id"]?.ToString(),
                                    title = reader["title"]?.ToString(),
                                    duration = reader["duration"] != DBNull.Value ? Convert.ToInt32(reader["duration"]) : 0,
                                    show_date = reader["show_date"]?.ToString(),
                                    start_time = reader["start_time"]?.ToString(),
                                    end_time = reader["end_time"]?.ToString(),
                                    auditorium_type = reader["auditorium_type"]?.ToString(),
                                    name = reader["name"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Ném lỗi ra console hoặc bỏ qua
                Console.WriteLine("Repo Error: " + ex.Message);
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
