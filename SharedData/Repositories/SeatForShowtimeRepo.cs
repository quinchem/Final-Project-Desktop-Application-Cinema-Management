using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{
        public class SeatForShowtimeRepo
        {
            // Lấy trạng thái ghế theo suất chiếu
            public Dictionary<string, string> GetStatusByShowtime(string showtimeId)
            {
                var result = new Dictionary<string, string>();

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                    SELECT seat_id, status
                    FROM seat_for_showtime
                    WHERE showtime_id = $show";

                    cmd.Parameters.AddWithValue("$show", showtimeId);

                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            string seatId = r.GetString(0);
                            string status = r.GetString(1);
                            result[seatId] = status;
                        }
                    }
                }

                return result;
            }

            // Update trạng thái FULL khi thanh toán
            public void UpdateSeatStatus(string seatId, string showtimeId, string status)
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                    INSERT INTO seat_for_showtime (seat_id, showtime_id, status)
                    VALUES ($seat, $show, $status)
                    ON CONFLICT(seat_id, showtime_id)
                    DO UPDATE SET status = $status";

                    cmd.Parameters.AddWithValue("$seat", seatId);
                    cmd.Parameters.AddWithValue("$show", showtimeId);
                    cmd.Parameters.AddWithValue("$status", status);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    
}
