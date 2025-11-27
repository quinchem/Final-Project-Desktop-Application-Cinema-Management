using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;
using UserApp.Models;

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
        public List<ShowtimeInfo> GetShowtimesByDateRange(DateTime startDate, int days = 7)
        {
            List<ShowtimeInfo> showtimes = new List<ShowtimeInfo>();

            // 1. Tạo danh sách 7 chuỗi ngày cần lấy (Ví dụ: "27-11-2025", "28-11-2025"...)
            // Format "dd-MM-yyyy" phải GIỐNG HỆT format bạn lưu trong Database
            List<string> targetDates = new List<string>();
            for (int i = 0; i < days; i++)
            {
                targetDates.Add($"'{startDate.AddDays(i).ToString("dd-MM-yyyy")}'");
            }

            // Nối lại thành chuỗi để đưa vào câu SQL: '27-11-2025','28-11-2025',...
            string inClause = string.Join(",", targetDates);

            using (SqliteConnection conn = DatabaseHelper.GetConnection())
            {
                // 2. Dùng câu lệnh WHERE IN (...)
                // Nghĩa là: Lấy suất chiếu mà ngày chiếu NẰM TRONG danh sách 7 ngày kia
                string query = $@"
                        SELECT 
                            s.showtime_id,
                            m.title,
                            
                            s.show_date,
                            s.start_time,
                            s.end_time,
                            a.auditorium_id,
                            a.name,
                            at.auditorium_type
                        FROM showtime s
                        INNER JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                        INNER JOIN auditorium_type at ON a.auditorium_type_id = at.auditorium_type_id
                        INNER JOIN movie m ON s.movie_id = m.movie_id
                        WHERE s.show_date IN ({inClause}) 
                        ORDER BY s.show_date, s.start_time";

                SqliteCommand cmd = new SqliteCommand(query, conn);

                conn.Open();
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        showtimes.Add(new ShowtimeInfo
                        {
                            showtime_id = Convert.ToInt32(reader["showtime_id"]),
                            title = reader["title"].ToString(),
                            // Nếu cột poster null thì để chuỗi rỗng
                            //poster_path = reader["poster_path"] != DBNull.Value ? reader["poster_path"].ToString() : "",

                            // Lấy thẳng string lên, không cần Parse DateTime gì cả
                            show_date = reader["show_date"].ToString(),
                            start_time = reader["start_time"].ToString(),
                            end_time = reader["end_time"].ToString(),
                            auditorium_id = Convert.ToInt32(reader["auditorium_id"]),
                            name = reader["name"].ToString(),
                            auditorium_type = reader["auditorium_type"].ToString(),
                        });
                    }
                }
            }
            return showtimes;
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
