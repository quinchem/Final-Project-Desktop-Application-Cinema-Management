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

        // Tạo Bill ID tự động dạng B001, B002,...
        private string GenerateBillId(SqliteConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT bill_id FROM bill ORDER BY bill_id DESC LIMIT 1";

            var result = cmd.ExecuteScalar();
            if (result == null) return "B001";

            string lastId = result.ToString();
            if (lastId.Length < 2) return "B001";

            if (int.TryParse(lastId.Substring(1), out int num))
            {
                return "B" + (num + 1).ToString("D3");
            }

            return "B" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
        }

        // ======================================================
        //  TẠO BILL + BILL_SEAT + SEAT_FOR_SHOWTIME
        // ======================================================
        public string CreateBill(string customerId, string showtimeId, double total, List<string> seatIds)
        {
            using (var conn = new SqliteConnection(ConnStr))
            {
                conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Tạo BILL
                        string billId = GenerateBillId(conn);

                        var cmdBill = conn.CreateCommand();
                        cmdBill.Transaction = tran;
                        cmdBill.CommandText = @"
                            INSERT INTO bill (bill_id, customer_id, showtime_id, bill_date, total)
                            VALUES ($bill, $cus, $show, $date, $total)
                        ";
                        cmdBill.Parameters.AddWithValue("$bill", billId);
                        cmdBill.Parameters.AddWithValue("$cus", customerId);
                        cmdBill.Parameters.AddWithValue("$show", showtimeId);
                        cmdBill.Parameters.AddWithValue("$date", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        cmdBill.Parameters.AddWithValue("$total", total);
                        cmdBill.ExecuteNonQuery();

                        // 2) Insert ghế FULL vào seat_for_showtime
                        foreach (var seatId in seatIds)
                        {
                            var cmdSeatShow = conn.CreateCommand();
                            cmdSeatShow.Transaction = tran;
                            cmdSeatShow.CommandText = @"
                                INSERT INTO seat_for_showtime (seat_id, showtime_id, status)
                                VALUES ($sid, $stid, 'Full')
                                ON CONFLICT(seat_id, showtime_id)
                                DO UPDATE SET status = 'Full';
                            ";
                            cmdSeatShow.Parameters.AddWithValue("$sid", seatId);
                            cmdSeatShow.Parameters.AddWithValue("$stid", showtimeId);
                            cmdSeatShow.ExecuteNonQuery();
                        }

                        // 3) Insert chi tiết seat cho bill
                        foreach (var seatId in seatIds)
                        {
                            var cmdDetail = conn.CreateCommand();
                            cmdDetail.Transaction = tran;
                            cmdDetail.CommandText = @"
                                INSERT INTO bill_seat (bill_id, seat_id)
                                VALUES ($bill, $seat)
                            ";
                            cmdDetail.Parameters.AddWithValue("$bill", billId);
                            cmdDetail.Parameters.AddWithValue("$seat", seatId);
                            cmdDetail.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return billId;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}