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
        private static string ConnStr => DatabaseHelper.GetConnectionString();

        // Lấy trạng thái (Full / Bảo trì)
        public static Dictionary<string, string> GetSeatStatus(string showtimeId)
        {
            var dict = new Dictionary<string, string>();

            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT seat_id, status
                FROM seat_for_showtime
                WHERE showtime_id = $stid";

            cmd.Parameters.AddWithValue("$stid", showtimeId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                dict[rd.GetString(0)] = rd.GetString(1);
            }

            return dict;
        }

        public static void SetFull(string seatId, string showId)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO seat_for_showtime(seat_id, showtime_id, status)
                VALUES ($sid, $stid, 'Full')
                ON CONFLICT(seat_id, showtime_id)
                DO UPDATE SET status = 'Full'";
            cmd.Parameters.AddWithValue("$sid", seatId);
            cmd.Parameters.AddWithValue("$stid", showId);
            cmd.ExecuteNonQuery();
        }
    }
}
