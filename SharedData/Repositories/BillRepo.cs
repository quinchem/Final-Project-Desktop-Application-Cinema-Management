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

        private string GenerateBillId(SqliteConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT bill_id FROM bill ORDER BY bill_id DESC LIMIT 1";

            var result = cmd.ExecuteScalar();
            if (result == null) return "B001";

            string lastId = result.ToString();
            // Xử lý trường hợp ID có độ dài khác chuẩn (nếu có)
            if (lastId.Length < 2) return "B001";

            // Lấy phần số (bỏ chữ 'B')
            if (int.TryParse(lastId.Substring(1), out int num))
            {
                return "B" + (num + 1).ToString("D3");
            }
            return "B" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper(); // Fallback nếu ID lạ
        }

        // ===== TẠO BILL + BILL_SEAT =====
        public string CreateBill(string customerId, string showtimeId,
                                 double total,
                                 List<string> seatIds)
        {
            using (var conn = new SqliteConnection(ConnStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        string billId = GenerateBillId(conn);

                        // 1. Insert BILL
                        var cmd = conn.CreateCommand();
                        cmd.Transaction = tran;
                        cmd.CommandText = @"
                            INSERT INTO bill (bill_id, customer_id, showtime_id, bill_date, 
                                            quantity_ticket, per_seat_ticket_price, total)
                            VALUES ($bill, $cus, $show, $date, $qty, 0, $total)
                        ";
                        cmd.Parameters.AddWithValue("$bill", billId);
                        cmd.Parameters.AddWithValue("$cus", customerId);
                        cmd.Parameters.AddWithValue("$show", showtimeId);
                        cmd.Parameters.AddWithValue("$date", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        cmd.Parameters.AddWithValue("$qty", seatIds.Count);
                        cmd.Parameters.AddWithValue("$total", total);

                        cmd.ExecuteNonQuery();

                        // 2. Insert SEAT_FOR_SHOWTIME (Đánh dấu ghế đã đặt)
                        // LƯU Ý: Làm bước này TRƯỚC khi insert bill_seat để đảm bảo dữ liệu ghế đã tồn tại 
                        // (nếu sau này có trigger kiểm tra ngược lại)
                        foreach (var seat in seatIds)
                        {
                            var cmdSeatShow = conn.CreateCommand();
                            cmdSeatShow.Transaction = tran;
                            // SỬA ĐỔI: Chuyển từ UPDATE sang INSERT vì ghế trống không có trong DB
                            cmdSeatShow.CommandText = @"
                                INSERT INTO seat_for_showtime (seat_id, showtime_id, status)
                                VALUES ($sid, $stid, 'Full')
                            ";
                            cmdSeatShow.Parameters.AddWithValue("$sid", seat);
                            cmdSeatShow.Parameters.AddWithValue("$stid", showtimeId);
                            cmdSeatShow.ExecuteNonQuery();
                        }

                        // 3. Insert BILL_SEAT
                        foreach (var seat in seatIds)
                        {
                            var cmdSeat = conn.CreateCommand();
                            cmdSeat.Transaction = tran;
                            cmdSeat.CommandText = @"
                                INSERT INTO bill_seat (bill_id, seat_id)
                                VALUES ($bill, $seat)
                            ";
                            cmdSeat.Parameters.AddWithValue("$bill", billId);
                            cmdSeat.Parameters.AddWithValue("$seat", seat);
                            cmdSeat.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return billId;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw; // Ném lỗi ra ngoài để FormPayment bắt được
                    }
                }
            }
        }
    }
}