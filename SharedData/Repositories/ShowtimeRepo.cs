using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SharedData.Repositories
{
    public class ShowtimeRepo
    {
        private static string connStr = DatabaseHelper.GetConnectionString();

        // Đổi DateTime sang string dd/MM/yyyy
        private static string ConvertDate(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        // Lấy tất cả suất chiếu
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

        // Lấy suất chiếu theo phim
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

        // Lấy suất chiếu theo ngày
        public static List<Showtime> GetByDate(DateTime date)
        {
            List<Showtime> list = new List<Showtime>();
            string dateStr = ConvertDate(date); 

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

        // Lấy suất chiếu theo ngày và phim
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

        // Lấy suất chiếu theo khoảng ngày
        public List<ShowtimeInfo> GetShowtimesByDateRange(DateTime startDate, int days)
        {
            List<ShowtimeInfo> list = new List<ShowtimeInfo>();
            try
            {
                string strStart = startDate.ToString("yyyy-MM-dd");
                string strEnd = startDate.AddDays(days - 1).ToString("yyyy-MM-dd");
                string query = $@"
            SELECT 
                s.showtime_id,
                s.movie_id,
                s.auditorium_id,          
                s.show_date,
                s.start_time,
                s.end_time,
                m.title,
                m.duration,
                a.name AS auditorium_name, 
                at.auditorium_type
            FROM showtime s
            LEFT JOIN movie m ON s.movie_id = m.movie_id
            LEFT JOIN auditorium a ON s.auditorium_id = a.auditorium_id
            LEFT JOIN auditorium_type at ON a.auditorium_type_id = at.auditorium_type_id
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
                                    showtime_id = reader["showtime_id"]?.ToString(),
                                    movie_id = reader["movie_id"]?.ToString(),
                                    auditorium_id = reader["auditorium_id"]?.ToString(),
                                    title = reader["title"]?.ToString(),
                                    duration = reader["duration"] != DBNull.Value ? Convert.ToInt32(reader["duration"]) : 0,
                                    show_date = reader["show_date"]?.ToString(),
                                    start_time = reader["start_time"]?.ToString(),
                                    end_time = reader["end_time"]?.ToString(),
                                    name = reader["auditorium_name"]?.ToString(),  
                                    auditorium_type = reader["auditorium_type"]?.ToString()
                                };
                                list.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tại GetShowtimesByDateRange: " + ex.Message);
            }
            return list;
        }

        // Lấy suất chiếu theo khoảng ngày và phim
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
                    string query = @"
                   SELECT 
                    s.showtime_id,
                    s.movie_id,
                    s.auditorium_id,
                    s.show_date,
                    s.start_time,
                    s.end_time,
                    m.title,
                    m.duration,
                    a.name AS auditorium_name,  
                    at.auditorium_type
                FROM showtime s
                LEFT JOIN movie m ON s.movie_id = m.movie_id
                LEFT JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                LEFT JOIN auditorium_type at ON a.auditorium_type_id = at.auditorium_type_id
                WHERE s.movie_id = @movieId
                ORDER BY s.show_date, s.start_time
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
                                    auditorium_id = reader["auditorium_id"]?.ToString(),  // THÊM!!!
                                    title = reader["title"]?.ToString(),
                                    duration = reader["duration"] != DBNull.Value ? Convert.ToInt32(reader["duration"]) : 0,
                                    show_date = reader["show_date"]?.ToString(),
                                    start_time = reader["start_time"]?.ToString(),
                                    end_time = reader["end_time"]?.ToString(),
                                    name = reader["auditorium_name"]?.ToString(),
                                    auditorium_type = reader["auditorium_type"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Repo Error: " + ex.Message);
            }

            return list;
        }

        // Xóa
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
        private static string GenerateNextShowtimeId()
        {
            int maxNum = 0;

            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                string query = "SELECT showtime_id FROM showtime";

                using (var cmd = new SqliteCommand(query, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var idRaw = rd["showtime_id"]?.ToString();
                        if (string.IsNullOrWhiteSpace(idRaw)) continue;
                        var id = idRaw.Trim();
                        var m = Regex.Match(id, @"^[Tt](\d+)$");
                        if (m.Success && int.TryParse(m.Groups[1].Value, out int num))
                        {
                            if (num > maxNum) maxNum = num;
                        }
                    }
                }
            }

            int next = maxNum + 1;
            return "T" + next.ToString("D3");
        }
        
        // Thêm suất chiếu
        public static void Insert(Showtime showtime)
        {
            if (!string.IsNullOrWhiteSpace(showtime.showtime_id))
            {
                showtime.showtime_id = showtime.showtime_id.Trim().ToUpper();
                if (!Regex.IsMatch(showtime.showtime_id, @"^[T]\d+$"))
                {
                    showtime.showtime_id = null;
                }
            }
            if (string.IsNullOrWhiteSpace(showtime.showtime_id))
            {
                showtime.showtime_id = GenerateNextShowtimeId();
            }
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var cmd = conn.CreateCommand();
                    cmd.Transaction = tran;
                    cmd.CommandText = @"
                INSERT INTO showtime (showtime_id, movie_id, auditorium_id, show_date, start_time, end_time)
                VALUES (@id, @mid, @aid, @date, @start, @end)";

                    cmd.Parameters.AddWithValue("@id", showtime.showtime_id);
                    cmd.Parameters.AddWithValue("@mid", showtime.movie_id);
                    cmd.Parameters.AddWithValue("@aid", showtime.auditorium_id);
                    cmd.Parameters.AddWithValue("@date", showtime.show_date);
                    cmd.Parameters.AddWithValue("@start", showtime.start_time);
                    cmd.Parameters.AddWithValue("@end", showtime.end_time);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqliteException ex) when (ex.SqliteErrorCode == 19) 
                    {
                        showtime.showtime_id = GenerateNextShowtimeId();
                        cmd.Parameters["@id"].Value = showtime.showtime_id;
                        cmd.ExecuteNonQuery();
                    }
                    var cmdMaint = conn.CreateCommand();
                    cmdMaint.Transaction = tran;
                    cmdMaint.CommandText = @"
                INSERT INTO seat_for_showtime(seat_id, showtime_id, status)
                SELECT seat_id, @stid, 'Bảo trì'
                FROM seat
                WHERE auditorium_id = @aid
                  AND status = 'Bảo trì';
            ";
                    cmdMaint.Parameters.AddWithValue("@stid", showtime.showtime_id);
                    cmdMaint.Parameters.AddWithValue("@aid", showtime.auditorium_id);
                    cmdMaint.ExecuteNonQuery();

                    tran.Commit();
                }
            }
        }

        // Cập nhật suất chiếu
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

        // Hàm Map dữ liệu
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
