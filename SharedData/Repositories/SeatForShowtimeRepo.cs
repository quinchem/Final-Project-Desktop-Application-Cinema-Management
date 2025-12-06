using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{
    public class SeatForShowtimeRepo
    {
        // Lấy chuỗi kết nối đến cơ sở dữ liệu từ lớp DatabaseHelper
        private static string ConnStr => DatabaseHelper.GetConnectionString();

        // Hàm lấy danh sách trạng thái ghế theo từng suất chiếu
        // Mục đích của hàm là đọc toàn bộ seat_id và status từ bảng seat_for_showtime
        // Sau đó trả về một dictionary với key là seat_id và value là trạng thái ghế tại suất chiếu tương ứng
        // Hàm này hỗ trợ giao diện người dùng hiển thị ghế nào đang trống, full hoặc bảo trì
        public static Dictionary<string, string> GetSeatStatus(string showtimeId)
        {
            // Tạo dictionary để lưu kết quả truy vấn
            // Dạng: ["A01R01"] = "Full"
            var dict = new Dictionary<string, string>();

            // Tạo kết nối SQLite bằng chuỗi kết nối đã khai báo phía trên
            // using var đảm bảo tự động giải phóng kết nối ngay cả khi có lỗi
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            // Tạo câu lệnh truy vấn để lấy thông tin ghế trong suất chiếu
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT seat_id, status
                FROM seat_for_showtime
                WHERE showtime_id = $stid";

            // Truyền tham số showtimeId vào câu lệnh SQL để tránh SQL injection
            cmd.Parameters.AddWithValue("$stid", showtimeId);

            // Thực thi truy vấn dưới dạng đọc tuần tự từng dòng dữ liệu
            using var rd = cmd.ExecuteReader();

            // Mỗi lần rd.Read đọc một dòng dữ liệu từ kết quả truy vấn
            // rd.GetString(0) là seat_id
            // rd.GetString(1) là status
            while (rd.Read())
            {
                string seatId = rd.GetString(0);
                string status = rd.IsDBNull(1) ? "" : rd.GetString(1);

                dict[seatId] = status;
            }

            // Trả về dictionary đã chứa đầy đủ trạng thái của từng ghế trong suất chiếu
            return dict;
        }

    }
}
