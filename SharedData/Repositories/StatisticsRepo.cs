using DocumentFormat.OpenXml.Drawing;
using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{

    public class StatisticRepo
    {
        // Các hàm sử dụng cho Form Statistic3: Thống kê về phim
        // Hàm truy vấn KPI doanh thu bao gồm: tổng doanh thu, tổng số vé doanh thu trung bình theo phim (movie_id), sử dụng bảng bill và bảng movie
        public (decimal totalRevenue, int totalTickets, decimal avgRevenuePerMovie) GetRevenueKPI(DateTime from, DateTime to, string movieTitle = null)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
                SELECT 
                    SUM(b.total) AS TotalRevenue,
                    COUNT(b.bill_id) AS TotalTickets,
                    COUNT(DISTINCT m.movie_id) AS MovieCount
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
            if (!string.IsNullOrEmpty(movieTitle))
                sql += " AND m.title = @movieTitle";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(movieTitle))
                cmd.Parameters.AddWithValue("@movieTitle", movieTitle);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                decimal totalRevenue = reader["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalRevenue"]) : 0;
                int totalTickets = reader["TotalTickets"] != DBNull.Value ? Convert.ToInt32(reader["TotalTickets"]) : 0;
                int movieCount = reader["MovieCount"] != DBNull.Value ? Convert.ToInt32(reader["MovieCount"]) : 1;
                decimal avgRevenue = movieCount > 0 ? totalRevenue / movieCount : 0;
                return (totalRevenue, totalTickets, avgRevenue);
            }
            return (0, 0, 0);
        }

        // Hàm truy vấn doanh thu theo từng ngày, sử dụng bảng bill và movie, showtime
        public List<(string date, decimal revenue)> GetRevenueByDay(DateTime fromDate, DateTime toDate, string filterMovie = null)
        {
            var result = new List<(string date, decimal revenue)>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
        SELECT 
            substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) AS BillDate,
            SUM(b.total) AS Revenue
        FROM bill b
        JOIN showtime s ON b.showtime_id = s.showtime_id
        JOIN movie m ON s.movie_id = m.movie_id
        WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
    ";
            if (!string.IsNullOrEmpty(filterMovie))
                sql += " AND m.title = @filterMovie";

            sql += " GROUP BY BillDate ORDER BY BillDate";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", fromDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", toDate.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(filterMovie))
                cmd.Parameters.AddWithValue("@filterMovie", filterMovie);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string date = reader.GetString(0);
                decimal revenue = reader.GetDecimal(1);
                result.Add((date, revenue));
            }
            return result;
        }

        // Hàm truy vấn phim có doanh thu cao nhất trong khoản thời gian, sử dụng bảng bill, showtime, movie
        public (string movieTitle, decimal totalRevenue, int totalTickets) GetTopMovie(DateTime fromDate, DateTime toDate, string filterMovie = null)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
                SELECT m.title, SUM(b.total) AS TotalRevenue, COUNT(b.bill_id) AS TotalTickets
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
            if (!string.IsNullOrEmpty(filterMovie))
                sql += " AND m.title = @filterMovie";
            sql += " GROUP BY m.movie_id ORDER BY TotalRevenue DESC LIMIT 1";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", fromDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", toDate.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(filterMovie))
                cmd.Parameters.AddWithValue("@filterMovie", filterMovie);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (reader.GetString(0), reader.GetDecimal(1), reader.GetInt32(2));
            }
            return ("N/A", 0, 0);
        }

        //Hàm lấy danh sách tên những phim đang có trạng thái "Đang chiếu" (để đưa vào ComboBox), sử dụng bảng movie.
        public List<string> GetMoviesCurrentlyShowing()
        {
            var list = new List<string>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT title FROM movie WHERE status='Đang chiếu'";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(reader.GetString(0));
            return list;
        }

        // Lấy doanh thu theo từng phim để vẽ biểu đồ Pie Chart, sử dụng bảng bill, showtime, movie
        public List<(string movieTitle, decimal total)> GetRevenueForPie(DateTime from, DateTime to, string selectedMovie = null)
        {
            var list = new List<(string, decimal)>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
                SELECT m.title, SUM(b.total) AS Total
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
            if (!string.IsNullOrEmpty(selectedMovie))
                sql += " AND m.title = @selectedMovie";
            sql += " GROUP BY m.movie_id ORDER BY Total DESC";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(selectedMovie))
                cmd.Parameters.AddWithValue("@selectedMovie", selectedMovie);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add((reader.GetString(0), reader.GetDecimal(1)));

            return list;
        }

        // Hàm lấy dữ liệu doanh thu theo từng phim để vẽ Bar Chart, sử dụng bảng bill. showtime, movie
        public List<(string movieTitle, decimal total)> GetRevenueBar(DateTime from, DateTime to, string movieTitle = null)
        {
            var list = new List<(string, decimal)>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
                SELECT m.title, SUM(b.total) AS Total
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
            if (!string.IsNullOrEmpty(movieTitle))
                sql += " AND m.title = @movieTitle";
            sql += " GROUP BY m.movie_id ORDER BY Total DESC";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(movieTitle))
                cmd.Parameters.AddWithValue("@movieTitle", movieTitle);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add((reader.GetString(0), reader.GetDecimal(1)));

            return list;
        }

        // Các hàm sử dụng cho form statistics4: Thống kê phòng chiếu
        
        private string ToSqlDate(DateTime dt) => dt.ToString("yyyy-MM-dd");

        // Hàm lấy danh sách tên phòng (để hiển thị vào ComboBox)
        public List<string> GetRooms()
        {
            var list = new List<string>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name FROM auditorium ORDER BY name";

            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(reader.GetString(0));
            return list;
        }

        // Hàm lấy KPI 1: truy xuất tổng quan rạp có bao nhiêu phòng đang có suất chiếu, sử dụng bảng showtime
        public int GetActiveRoomCount(DateTime from, DateTime to)
        {
            var rooms = new HashSet<string>();

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
        SELECT show_date, auditorium_id
        FROM showtime
    ";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string dateStr = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                string roomId = reader.IsDBNull(1) ? "" : reader.GetString(1);

                if (dateStr.Contains(" ")) dateStr = dateStr.Split(' ')[0];

                if (IsValidDate(dateStr, from, to) && !string.IsNullOrEmpty(roomId))
                    rooms.Add(roomId);
            }
            return rooms.Count;
        }

        // Hàm lấy KPI 2: truy xuất tổng số suất chiếu, sử dụng bảng showtime và auditorium
        public int GetTotalShowtimes(DateTime from, DateTime to, string roomFilter = null)
        {
            int count = 0;

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
        SELECT s.show_date, a.name
        FROM showtime s
        LEFT JOIN auditorium a ON s.auditorium_id = a.auditorium_id
    ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string dateStr = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                string dbRoom = reader.IsDBNull(1) ? "" : reader.GetString(1);

                if (dateStr.Contains(" ")) dateStr = dateStr.Split(' ')[0];

                if (IsValidDate(dateStr, from, to))
                {
                    if (string.IsNullOrEmpty(roomFilter) || roomFilter == "Tất cả")
                        count++;
                    else if (dbRoom.Equals(roomFilter, StringComparison.OrdinalIgnoreCase))
                        count++;
                }
            }
            return count;
        }

        //Hàm để huẩn hóa chuỗi ngày trong CSDL rồi so sánh với khoảng thời gian.
        private bool IsValidDate(string dateStr, DateTime from, DateTime to)
        {
            // Định dạng ngày tháng khớp với dữ liệu bạn cung cấp (dd/MM/yyyy)
            string[] formats = { "d/M/yyyy", "dd/MM/yyyy", "yyyy-MM-dd", "d/MM/yyyy", "dd/M/yyyy" };

            if (DateTime.TryParseExact(dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime showDate))
            {
                // So sánh chỉ lấy phần Date (bỏ qua giờ phút giây)
                return showDate.Date >= from.Date && showDate.Date <= to.Date;
            }
            return false;
        }

        // // Hàm lấy KPI 3: truy xuất phòng có doanh thu cao nhất với % doanh thu, sử dụng bảng bill, showtime,auditorium 
        // 4. KPI: Phòng có doanh thu cao nhất (với %)
        public (string RoomName, double Revenue, double Percentage) GetTopRevenueRoom(DateTime from, DateTime to)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            // Truy xuất tổng doanh thu toàn rạp
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
        SELECT COALESCE(SUM(b.total), 0) 
        FROM bill b
        WHERE (substr(substr('0' || b.bill_date, -10, 10), 7, 4) || '-' || 
               substr(substr('0' || b.bill_date, -10, 10), 4, 2) || '-' || 
               substr(substr('0' || b.bill_date, -10, 10), 1, 2)) 
               BETWEEN @from AND @to
    ";
            cmd.Parameters.AddWithValue("@from", ToSqlDate(from));
            cmd.Parameters.AddWithValue("@to", ToSqlDate(to));

            double totalRevenue = Convert.ToDouble(cmd.ExecuteScalar() ?? 0);

            if (totalRevenue == 0) return ("N/A", 0, 0);

            // Truy xuất doanh thu theo từng phòng
            cmd.CommandText = @"
        SELECT a.name, COALESCE(SUM(b.total), 0) AS revenue
        FROM bill b
        JOIN showtime s ON b.showtime_id = s.showtime_id
        JOIN auditorium a ON s.auditorium_id = a.auditorium_id
        WHERE (substr(substr('0' || b.bill_date, -10, 10), 7, 4) || '-' || 
               substr(substr('0' || b.bill_date, -10, 10), 4, 2) || '-' || 
               substr(substr('0' || b.bill_date, -10, 10), 1, 2)) 
               BETWEEN @from AND @to
        GROUP BY a.name
        ORDER BY revenue DESC
        LIMIT 1
    ";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@from", ToSqlDate(from));
            cmd.Parameters.AddWithValue("@to", ToSqlDate(to));

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string name = reader.GetString(0);
                double rev = reader.GetDouble(1);
                // Tính phần trăm đóng góp của phòng này so với tổng doanh thu
                return (name, rev, (rev / totalRevenue) * 100);
            }

            return ("N/A", 0, 0);
        }


        // Hàm lấy doanh thu theo giờ chiếu khi chọn trong 1 ngày, để vẽ Line Chart, sử dụng bảng bill, showtime, auditorium
        public List<(int hour, decimal revenue)> GetRevenueByHour(DateTime from, DateTime to, string roomName = null)
        {
            var list = new List<(int, decimal)>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
                SELECT 
                    CAST(substr(s.start_time, 1, 2) AS INTEGER) AS Hour,
                    COALESCE(SUM(b.total), 0) AS Revenue
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                WHERE (substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2)) 
                      BETWEEN @from AND @to
            ";

            if (!string.IsNullOrEmpty(roomName)) sql += " AND a.name = @roomName";

            sql += " GROUP BY Hour ORDER BY Hour";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", ToSqlDate(from));
            cmd.Parameters.AddWithValue("@to", ToSqlDate(to));
            if (!string.IsNullOrEmpty(roomName)) cmd.Parameters.AddWithValue("@roomName", roomName);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add((reader.GetInt32(0), Convert.ToDecimal(reader.GetDouble(1))));
            }
            return list;
        }

       // Hàm lấy doanh thu theo ngày chiếu khi chọn trong nhiều ngày, để vẽ Line Chart, sử dụng bảng bill, showtime, auditorium
        public List<(string date, decimal revenue)> GetRevenueShowTimeByDay(DateTime from, DateTime to, string roomName = null)
        {
            var list = new List<(string, decimal)>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
                SELECT 
                    b.bill_date,
                    COALESCE(SUM(b.total), 0) AS Revenue
                FROM bill b
                LEFT JOIN showtime s ON b.showtime_id = s.showtime_id
                LEFT JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                WHERE (substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2)) 
                      BETWEEN @from AND @to
            ";

            if (!string.IsNullOrEmpty(roomName)) sql += " AND a.name = @roomName";

            // Sắp xếp theo format chuẩn yyyy-MM-dd để biểu đồ vẽ đúng thứ tự thời gian
            sql += " GROUP BY b.bill_date ORDER BY substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2)";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", ToSqlDate(from));
            cmd.Parameters.AddWithValue("@to", ToSqlDate(to));
            if (!string.IsNullOrEmpty(roomName)) cmd.Parameters.AddWithValue("@roomName", roomName);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add((reader.GetString(0), Convert.ToDecimal(reader.GetDouble(1))));
            }
            return list;
        }

        // Hàm lấy doanh thu theo phòng chiếu, sử dụng bảng bill, showtime, auditorium
        public List<(string RoomName, decimal Revenue)> GetRevenueByRoom(DateTime from, DateTime to, string roomName = null)
        {
            var list = new List<(string, decimal)>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            string sql = @"
                SELECT 
                    a.name, 
                    COALESCE(SUM(b.total), 0) AS Revenue
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                WHERE (substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2)) 
                      BETWEEN @from AND @to
            ";

            if (!string.IsNullOrEmpty(roomName)) sql += " AND a.name = @roomName";

            sql += " GROUP BY a.name ORDER BY a.name";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", ToSqlDate(from));
            cmd.Parameters.AddWithValue("@to", ToSqlDate(to));
            if (!string.IsNullOrEmpty(roomName)) cmd.Parameters.AddWithValue("@roomName", roomName);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add((reader.GetString(0), Convert.ToDecimal(reader.GetDouble(1))));
            }
            return list;
        }
    }
}


