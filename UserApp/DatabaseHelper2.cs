using Microsoft.Data.Sqlite;
using System.Text;
using System.Text.RegularExpressions;

public static class DatabaseHelper2
{
    
        
        public static string GetConnectionString()
        {
            // Lấy thư mục bắt đầu = thư mục .exe
            string dir = AppDomain.CurrentDomain.BaseDirectory;

            // Lùi dần đến khi gặp file .sln => thư mục gốc của solution
            while (dir != null && !Directory.GetFiles(dir, "*.sln").Any())
            {
                dir = Directory.GetParent(dir)?.FullName;
            }

            // Nếu không tìm thấy .sln => báo lỗi
            if (dir == null)
                throw new Exception("Không tìm thấy thư mục solution (.sln)!");

            // Ghép đường dẫn DB thực sự
            string dbPath = Path.Combine(dir, "SharedDatabase", "Cinema.db");

            return $"Data Source={dbPath}";
        }
        // 2. Lấy phim đang chiếu
        public static string GetMoviesInTheaters()
        {
            try
            {
                using var conn = new SqliteConnection(GetConnectionString());
                conn.Open();
                string query = "SELECT title, genre, director, duration FROM Movie WHERE status='Đang chiếu'";
                using var cmd = new SqliteCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                var sb = new StringBuilder();
                sb.AppendLine("Dưới đây là danh sách phim ĐANG CHIẾU tại rạp:");
                while (reader.Read())
                {
                    sb.AppendLine($"- Tên phim: {reader["title"]}");
                    sb.AppendLine($"  Thể loại: {reader["genre"]}");
                    sb.AppendLine($"  Thời lượng: {reader["duration"]} phút");
                    sb.AppendLine($"  Đạo diễn: {reader["director"]}");
                    sb.AppendLine("---");
                }
                return sb.Length > 0 ? sb.ToString() : ""; // Trả về rỗng để code UI xử lý
            }
            catch { return ""; }
        }

        // 3. Lấy phim sắp chiếu
        public static string GetComingSoonMovies()
        {
            try
            {
                using var conn = new SqliteConnection(GetConnectionString());
                conn.Open();
                string query = "SELECT title, genre, release_date FROM Movie WHERE status='Sắp chiếu'";
                using var cmd = new SqliteCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                var sb = new StringBuilder();
                sb.AppendLine("Dưới đây là danh sách phim SẮP CHIẾU:");
                while (reader.Read())
                {
                    sb.AppendLine($"- Tên phim: {reader["title"]}");
                    sb.AppendLine($"  Thể loại: {reader["genre"]}");
                    sb.AppendLine($"  Ngày khởi chiếu: {reader["release_date"]}");
                    sb.AppendLine("---");
                }
                return sb.Length > 0 ? sb.ToString() : "";
            }
            catch { return ""; }
        }

    // 4. Lấy lịch chiếu (Showtime)
    // Thêm hàm này vào trong class DatabaseHelper2
    public static string GetShowtimesByMovie(string movieName)
    {
        try
        {
            using var conn = new SqliteConnection(GetConnectionString());
            conn.Open();

            // --- BƯỚC 1: TÌM PHIM TRONG BẢNG MOVIE TRƯỚC (ĐỂ LẤY ID) ---
            // Lấy tất cả phim ra để so sánh tên bằng C# cho chính xác 100%
            string movieQuery = "SELECT movie_id, title, status FROM Movie";
            using var cmdMovie = new SqliteCommand(movieQuery, conn);
            using var readerMovie = cmdMovie.ExecuteReader();

            string targetMovieId = "";
            string targetMovieTitle = "";
            string targetMovieStatus = "";

            // Chuẩn hóa input của user: "bẫy tiền" -> "bay tien"
            string searchKey = VietnameseHelper.ConvertToUnSign(movieName).ToLower().Trim();

            while (readerMovie.Read())
            {
                string dbTitle = readerMovie["title"].ToString();
                string dbTitleUnsign = VietnameseHelper.ConvertToUnSign(dbTitle).ToLower();

                if (dbTitleUnsign.Contains(searchKey))
                {
                    targetMovieId = readerMovie["movie_id"].ToString();
                    targetMovieTitle = dbTitle;
                    targetMovieStatus = readerMovie["status"].ToString();
                    break; // Tìm thấy rồi thì dừng lại
                }
            }

            // Nếu quét hết bảng Movie mà không thấy tên phim -> Trả về rỗng để Bot báo lỗi "Sai tên phim"
            if (string.IsNullOrEmpty(targetMovieId))
            {
                return "";
            }

            // --- BƯỚC 2: CÓ ID RỒI, GIỜ MỚI TÌM LỊCH CHIẾU ---
            string showtimeQuery = @"
            SELECT s.show_date, s.start_time, s.end_time, a.name AS auditorium
            FROM Showtime s
            JOIN Auditorium a ON s.auditorium_id = a.auditorium_id
            WHERE s.movie_id = @mid
            ORDER BY s.show_date, s.start_time";

            using var cmdShow = new SqliteCommand(showtimeQuery, conn);
            cmdShow.Parameters.AddWithValue("@mid", targetMovieId);

            using var readerShow = cmdShow.ExecuteReader();
            var sb = new StringBuilder();

            while (readerShow.Read())
            {
                sb.AppendLine($"🕒 {readerShow["show_date"]} | {readerShow["start_time"]} - {readerShow["end_time"]} | 📍 {readerShow["auditorium"]}");
            }

            // --- BƯỚC 3: XỬ LÝ KẾT QUẢ ---

            // Trường hợp A: Tìm thấy lịch chiếu
            if (sb.Length > 0)
            {
                return $"LỊCH CHIẾU PHIM '{targetMovieTitle.ToUpper()}':\n" + sb.ToString();
            }

            // Trường hợp B: Phim tồn tại nhưng KHÔNG có lịch chiếu (VD: Phim sắp chiếu)
            return $"Hệ thống: Phim '{targetMovieTitle}' ({targetMovieStatus}) hiện tại CHƯA ĐƯỢC XẾP LỊCH CHIẾU tại rạp. Mời bạn quay lại sau hoặc chọn phim khác.";
        }
        catch (Exception ex)
        {
            return $"[LỖI DB]: {ex.Message}";
        }
    }

    // 5. Lấy thông tin giá vé (Hardcode hoặc lấy từ DB đều được, ở đây dùng mẫu của bạn)
    public static string GetSeatPricesSummary()
        {
            return @"THÔNG TIN GIÁ VÉ NIÊM YẾT:
        - Phòng 2D: Ghế thường 70000k, Ghế VIP 75000k.
        - Phòng 3D: Ghế thường 90000k, Ghế VIP 95000k.";
        }

        // 6. Gợi ý phim theo thể loại
        public static string SuggestNowOrSoonByGenre(string genre)
        {
            try
            {
                using var conn = new SqliteConnection(GetConnectionString());
                conn.Open();

                string query = @"
                SELECT title, genre, status, description 
                FROM Movie 
                WHERE LOWER(genre) LIKE @g 
                AND (status='Đang chiếu' OR status='Sắp chiếu')";

                using var cmd = new SqliteCommand(query, conn);
                cmd.Parameters.AddWithValue("@g", $"%{genre.ToLower()}%");

                using var reader = cmd.ExecuteReader();
                var sb = new StringBuilder();

                if (reader.HasRows)
                {
                    sb.AppendLine($"Các phim thuộc thể loại '{genre}' đang có tại rạp:");
                    while (reader.Read())
                    {
                        sb.AppendLine($"- {reader["title"]} ({reader["status"]})");
                    }
                    return sb.ToString();
                }
                return "";
            }
            catch { return ""; }
        }

    // 7. Lấy MÔ TẢ CHI TIẾT (Quan trọng nhất)
    public static string GetMovieDetails(string movieName, string infoType = "all")
    {
        try
        {
            using var conn = new SqliteConnection(GetConnectionString());
            conn.Open();

            // Vẫn lấy hết lên để lọc tên phim cho chính xác
            string query = "SELECT * FROM Movie";
            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            // Chuẩn hóa tên phim user nhập
            string searchKey = VietnameseHelper.ConvertToUnSign(movieName).ToLower().Trim();

            while (reader.Read())
            {
                string dbTitle = reader["title"].ToString();
                string dbTitleUnsign = VietnameseHelper.ConvertToUnSign(dbTitle).ToLower();

                if (dbTitleUnsign.Contains(searchKey))
                {
                    // TÌM THẤY PHIM -> XỬ LÝ THEO LOẠI THÔNG TIN YÊU CẦU
                    string title = reader["title"].ToString();

                    switch (infoType)
                    {
                        case "director":
                            return $"[DỮ LIỆU HỆ THỐNG]: Đạo diễn của phim '{title}' là: {reader["director"]}";

                        case "actor":
                            return $"[DỮ LIỆU HỆ THỐNG]: Diễn viên tham gia phim '{title}': {reader["actor"]}";

                        case "duration":
                            return $"[DỮ LIỆU HỆ THỐNG]: Thời lượng phim '{title}' là: {reader["duration"]} phút.";

                        case "genre":
                            return $"[DỮ LIỆU HỆ THỐNG]: Thể loại của phim '{title}' là: {reader["genre"]}";

                        case "language":
                            return $"[DỮ LIỆU HỆ THỐNG]: Ngôn ngữ/Phụ đề của phim '{title}': {reader["language"]}";

                        case "release_date":
                            return $"[DỮ LIỆU HỆ THỐNG]: Ngày khởi chiếu phim '{title}' là: {reader["release_date"]}";

                        case "all":
                        default:
                            // Trả về full thông tin như cũ
                            var sb = new StringBuilder();
                            sb.AppendLine($"[THÔNG TIN ĐẦY ĐỦ]");
                            sb.AppendLine($"Tên phim: {title}");
                            sb.AppendLine($"Đạo diễn: {reader["director"]}");
                            sb.AppendLine($"Diễn viên: {reader["actor"]}");
                            sb.AppendLine($"Thể loại: {reader["genre"]}");
                            sb.AppendLine($"Thời lượng: {reader["duration"]} phút");
                            sb.AppendLine($"Khởi chiếu: {reader["release_date"]}");
                            sb.AppendLine($"Nội dung: {reader["description"]}");
                            return sb.ToString();
                    }
                }
            }
            return ""; // Không tìm thấy
        }
        catch (Exception ex) { return $"[LỖI]: {ex.Message}"; }
    }

    // Thêm vào DatabaseHelper2.cs
    // Thêm hàm này vào trong DatabaseHelper2.cs
    public static string GetMoviesByLanguage(string langKeyword)
    {
        try
        {
            using var conn = new SqliteConnection(GetConnectionString());
            conn.Open();

            // Chỉ lấy phim Đang chiếu hoặc Sắp chiếu để gợi ý
            string query = "SELECT title, language, status, genre FROM Movie WHERE status IN ('Đang chiếu', 'Sắp chiếu')";

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var sb = new StringBuilder();
            // Chuyển từ khóa tìm kiếm sang không dấu (vd: "tiếng anh" -> "tieng anh")
            string searchKey = VietnameseHelper.ConvertToUnSign(langKeyword).ToLower().Trim();

            bool hasResult = false;
            int count = 0;

            while (reader.Read())
            {
                string dbLang = reader["language"].ToString();
                // Chuyển dữ liệu DB sang không dấu để so sánh
                string dbLangUnsign = VietnameseHelper.ConvertToUnSign(dbLang).ToLower();

                // So sánh: Nếu trong cột language có chứa từ khóa
                if (dbLangUnsign.Contains(searchKey))
                {
                    if (!hasResult)
                    {
                        sb.AppendLine($"DANH SÁCH PHIM '{langKeyword.ToUpper()}' HIỆN CÓ:");
                        hasResult = true;
                    }
                    count++;
                    sb.AppendLine($"{count}. {reader["title"]} ({reader["status"]})");
                    sb.AppendLine($"   - Thể loại: {reader["genre"]}");
                    sb.AppendLine($"   - Ngôn ngữ: {reader["language"]}");
                    sb.AppendLine(""); // Dòng trống cho thoáng
                }
            }

            if (hasResult) return sb.ToString();

            return $"Hiện tại rạp chưa có phim nào thuộc ngôn ngữ '{langKeyword}' bạn nhé.";
        }
        catch (Exception ex)
        {
            return $"[LỖI DB]: {ex.Message}";
        }
    }
    // Hàm xoá dấu tiếng Việt: "BẪY TIỀN" -> "BAY TIEN"
    public static class VietnameseHelper
    {
        public static string ConvertToUnSign(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }

    public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(GetConnectionString());
        }
    }

