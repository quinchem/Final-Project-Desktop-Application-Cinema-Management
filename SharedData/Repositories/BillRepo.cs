using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{
    public class BillRepo
    {
        private string ConnStr => DatabaseHelper.GetConnectionString();
        // Lấy chuỗi kết nối tới database từ DatabaseHelper

        // Hàm tạo mã hóa đơn tự động theo dạng B001, B002, B003,...
        // Mỗi lần tạo bill mới, hệ thống phải đọc bill cuối cùng để tăng số thứ tự.
        private string GenerateBillId(SqliteConnection conn)
        {
            var cmd = conn.CreateCommand();
            // Tạo command để truy vấn DB trong kết nối hiện tại.

            cmd.CommandText = "SELECT bill_id FROM bill ORDER BY bill_id DESC LIMIT 1";
            // Truy vấn bill_id lớn nhất hiện có (theo thứ tự giảm dần) → Bill mới sẽ tăng lên từ đây.

            var result = cmd.ExecuteScalar();
            // ExecuteScalar trả về giá trị đầu tiên của hàng đầu tiên kết quả truy vấn.

            if (result == null) return "B001";
            // Nếu chưa có bill nào → trả về mã mặc định B001.

            string lastId = result.ToString();
            // Lấy bill_id cuối cùng từ DB dưới dạng chuỗi.

            if (lastId.Length < 2) return "B001";
            // Kiểm tra độ dài bill_id hợp lệ (phải có chữ B + số).

            if (int.TryParse(lastId.Substring(1), out int num))
            {
                // Lấy phần số phía sau ký tự B và chuyển thành số nguyên.
                return "B" + (num + 1).ToString("D3");
                // Tăng số lên 1 và format lại thành dạng D3 (ví dụ: 1 → 001).
            }

            // Nếu bill_id trong  bị lỗi format thì fallback tạo mã ngẫu nhiên.
            return "B" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
        }

        // Hàm tạo hóa đơn đầy đủ (BILL, SEAT_FOR_SHOWTIME, BILL_SEAT)
        // - customerId: ID khách hàng mua vé
        // - showtimeId: ID suất chiếu
        // - total: tổng tiền thanh toán
        // - seatIds: danh sách seat_id dạng A01R01 đã được đặt
        // Hàm thực hiện transaction để đảm bảo dữ liệu đồng bộ.
        public string CreateBill(string customerId, string showtimeId, double total, List<string> seatIds)
        {
            using (var conn = new SqliteConnection(ConnStr))
            {
                conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    // Tạo transaction để các thao tác INSERT diễn ra đồng bộ
                    try
                    {
                        // 1) Tạo BILL mới
                        // Tạo mã hóa đơn mới theo đúng quy tắc tăng dần
                        string billId = GenerateBillId(conn);
                        
                       // Mọi lệnh SQL phía dưới đều chạy trong transaction
                        var cmdBill = conn.CreateCommand();
                        cmdBill.Transaction = tran;
                        
                        // Thêm bản ghi hóa đơn mới vào bảng bill
                        cmdBill.CommandText = @"
                            INSERT INTO bill (bill_id, customer_id, showtime_id, bill_date, total)
                            VALUES ($bill, $cus, $show, $date, $total) ";
                        
                        // Lưu thời gian thanh toán theo định dạng dd/MM/yyyy HH:mm
                        cmdBill.Parameters.AddWithValue("$bill", billId);
                        cmdBill.Parameters.AddWithValue("$cus", customerId);
                        cmdBill.Parameters.AddWithValue("$show", showtimeId);
                        cmdBill.Parameters.AddWithValue("$date", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        
                        cmdBill.Parameters.AddWithValue("$total", total);
                        cmdBill.ExecuteNonQuery();

                        // 2) Đánh dấu ghế FULL cho suất chiếu tương ứng
                        // seat_for_showtime cho biết ghế trong suất chiếu đã được đặt hay còn trống.
                        // Khi user thanh toán xong → trạng thái phải set về 'Full'.
                        foreach (var seatId in seatIds)
                        {
                            var cmdSeatShow = conn.CreateCommand();
                            cdiện hiển thị thông báo cho người dùng
                        throw;
                        
                    }
                }
            }
        }
    }
}
