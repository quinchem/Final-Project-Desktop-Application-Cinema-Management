using Microsoft.Data.Sqlite;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedData.Repositories
{
    public class ChatbotRepo
    {
        public ChatbotRepo() { }

        // Lấy thông tin phim đang chiếu
        public string GetMoviesInTheaters()
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"SELECT title, genre, director, duration 
                               FROM Movie 
                               WHERE status = 'Đang chiếu'";

                using var cmd = new SqliteCommand(sql, conn);
                using var rd = cmd.ExecuteReader();

                if (!rd.HasRows) return "Hiện chưa có phim đang chiếu.";

                var sb = new StringBuilder();
                sb.AppendLine("DANH SÁCH PHIM ĐANG CHIẾU:");

                while (rd.Read())
                {
                    sb.AppendLine($"- {rd["title"]} | {rd["genre"]} | {rd["duration"]} phút");
                }

                return sb.ToString();
            }
            catch
            {
                return "Đã xảy ra lỗi khi lấy danh sách phim đang chiếu.";
            }
        }

        //Lấy thông tin phim sắp chiếu
        public string GetComingSoonMovies()
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"SELECT title, genre, release_date 
                               FROM Movie 
                               WHERE status = 'Sắp chiếu'";

                using var cmd = new SqliteCommand(sql, conn);
                using var rd = cmd.ExecuteReader();

                if (!rd.HasRows) return "Hiện chưa có phim sắp chiếu.";

                var sb = new StringBuilder();
                sb.AppendLine("DANH SÁCH PHIM SẮP CHIẾU:");

                while (rd.Read())
                {
                    sb.AppendLine($"- {rd["title"]} | {rd["genre"]} | Khởi chiếu: {rd["release_date"]}");
                }

                return sb.ToString();
            }
            catch
            {
                return "Đã xảy ra lỗi khi lấy danh sách phim sắp chiếu.";
            }
        }
        //Lấy lịch chiếu theo tên phim
        public string GetShowtimesByMovie(string movieName)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string findSql = "SELECT movie_id, title, status FROM Movie";
                using var cmdMovie = new SqliteCommand(findSql, conn);
                using var rdMovie = cmdMovie.ExecuteReader();

                string searchKey = VietnameseHelper.ConvertToUnSign(movieName).ToLower();
                string movieId = "", title = "", status = "";

                while (rdMovie.Read())
                {
                    string t = rdMovie["title"].ToString();
                    string unsign = VietnameseHelper.ConvertToUnSign(t).ToLower();

                    if (unsign.Contains(searchKey))
                    {
                        movieId = rdMovie["movie_id"].ToString();
                        title = t;
                        status = rdMovie["status"].ToString();
                        break;
                    }
                }

                if (movieId == "") return $"Không tìm thấy phim giống '{movieName}'.";

                string sql = @"
                    SELECT s.show_date, s.start_time, s.end_time, a.name AS auditorium
                    FROM Showtime s
                    JOIN Auditorium a ON s.auditorium_id = a.auditorium_id
                    WHERE s.movie_id = @id
                    ORDER BY s.show_date, s.start_time";

                using var cmd = new SqliteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", movieId);

                using var rd = cmd.ExecuteReader();
                var sb = new StringBuilder();

                while (rd.Read())
                {
                    sb.AppendLine($"{rd["show_date"]} | {rd["start_time"]}-{rd["end_time"]} | Phòng: {rd["auditorium"]}");
                }

                if (sb.Length == 0)
                    return $"Phim '{title}' ({status}) hiện chưa có lịch chiếu.";

                return $" Lịch chiếu phim '{title}':\n{sb}";
            }
            catch
            {
                return "Đã xảy ra lỗi khi lấy lịch chiếu.";
            }
        }
        //Trả lời thông tin giá vé
        public string GetSeatPricesSummary()
        {
            return @"Giá vé:
- Phòng 2D: Ghế thường 70.000 VND | Ghế VIP 75.000 VND
- Phòng 3D: Ghế thường 90.000 VND | Ghế VIP 95.000 VND";
        }

        // Gợi ý phim theo thể loại
        public string SuggestNowOrSoonByGenre(string genre)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT title, status 
                    FROM Movie 
                    WHERE LOWER(genre) LIKE @g
                    AND status IN ('Đang chiếu', 'Sắp chiếu')";

                using var cmd = new SqliteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@g", "%" + genre.ToLower() + "%");

                using var rd = cmd.ExecuteReader();

                if (!rd.HasRows) return $"Không tìm thấy phim thể loại '{genre}'.";

                var sb = new StringBuilder();
                sb.AppendLine($" Phim thể loại {genre.ToUpper()}:");

                while (rd.Read())
                {
                    sb.AppendLine($"- {rd["title"]} ({rd["status"]})");
                }

                return sb.ToString();
            }
            catch
            {
                return "Đã xảy ra lỗi khi gợi ý phim theo thể loại.";
            }
        }

        // Lấy dữu liệu chi tiết phim
        public string GetMovieDetails(string movieName, string infoType = "all")
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = "SELECT * FROM Movie";
                using var cmd = new SqliteCommand(sql, conn);
                using var rd = cmd.ExecuteReader();

                string searchKey = VietnameseHelper.ConvertToUnSign(movieName).ToLower();

                while (rd.Read())
                {
                    string title = rd["title"].ToString();
                    string unsign = VietnameseHelper.ConvertToUnSign(title).ToLower();

                    if (!unsign.Contains(searchKey)) continue;

                    switch (infoType)
                    {
                        case "director": return $"Đạo diễn: {rd["director"]}";
                        case "actor": return $"Diễn viên: {rd["actor"]}";
                        case "duration": return $"Thời lượng: {rd["duration"]} phút";
                        case "genre": return $"Thể loại: {rd["genre"]}";
                        case "language": return $"Ngôn ngữ: {rd["language"]}";
                        default:
                            return BuildMovieFullInfo(rd);
                    }
                }

                return $"Không tìm thấy phim '{movieName}'.";
            }
            catch
            {
                return "Đã xảy ra lỗi khi lấy thông tin phim.";
            }
        }

        private string BuildMovieFullInfo(SqliteDataReader rd)
        {
            var sb = new StringBuilder();
            sb.AppendLine("THÔNG TIN CHI TIẾT PHIM:");
            sb.AppendLine($"Tên phim: {rd["title"]}");
            sb.AppendLine($"Thể loại: {rd["genre"]}");
            sb.AppendLine($"Đạo diễn: {rd["director"]}");
            sb.AppendLine($"Diễn viên: {rd["actor"]}");
            sb.AppendLine($"Thời lượng: {rd["duration"]} phút");
            sb.AppendLine($"Khởi chiếu: {rd["release_date"]}");
            sb.AppendLine($"Nội dung: {rd["description"]}");
            return sb.ToString();
        }

        // Lấy dữ liệu theo ngôn ngữ
        public string GetMoviesByLanguage(string lang)
        {
            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT title, language, genre, status 
                    FROM Movie 
                    WHERE status IN ('Đang chiếu', 'Sắp chiếu')";

                using var cmd = new SqliteCommand(sql, conn);
                using var rd = cmd.ExecuteReader();

                var sb = new StringBuilder();
                string searchKey = VietnameseHelper.ConvertToUnSign(lang).ToLower();
                int count = 0;

                while (rd.Read())
                {
                    string unsign = VietnameseHelper.ConvertToUnSign(rd["language"].ToString()).ToLower();

                    if (!unsign.Contains(searchKey)) continue;

                    count++;
                    if (count == 1)
                        sb.AppendLine($"🎧 PHIM NGÔN NGỮ '{lang}':");

                    sb.AppendLine($"- {rd["title"]} ({rd["status"]})");
                    sb.AppendLine($"  • Thể loại: {rd["genre"]}");
                }

                return count > 0 ? sb.ToString() : $"Không tìm thấy phim ngôn ngữ '{lang}'.";
            }
            catch
            {
                return "Đã xảy ra lỗi khi lọc phim theo ngôn ngữ.";
            }
        }
    }

    //Bổ chuyển dấu tiếng việt
    public static class VietnameseHelper
    {
        public static string ConvertToUnSign(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            string formD = s.Normalize(NormalizationForm.FormD);
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            return regex.Replace(formD, "")
                        .Replace('đ', 'd')
                        .Replace('Đ', 'D');
        }
    }
}


