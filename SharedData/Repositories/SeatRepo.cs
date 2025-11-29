using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SharedData.Models;

namespace SharedData.Repositories
{
    public class SeatRepo
    {
            // Connection dùng chung
            private static string ConnStr => DatabaseHelper.GetConnectionString();

            // ============================================================
            // INSERT SEAT (dùng khi lưu layout phòng)
            // ============================================================
            public static void InsertSeat(Seat s)
            {
                using var conn = new SqliteConnection(ConnStr);
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO seat(seat_id, seat_type_id, auditorium_id, location, status, per_seat_ticket_price)
                VALUES ($id, $type, $aud, $loc, $st, $price)
            ";

                cmd.Parameters.AddWithValue("$id", s.seat_id);
                cmd.Parameters.AddWithValue("$type", s.seat_type_id);
                cmd.Parameters.AddWithValue("$aud", s.auditorium_id);
                cmd.Parameters.AddWithValue("$loc", s.location);
                cmd.Parameters.AddWithValue("$st", s.status);
                cmd.Parameters.AddWithValue("$price", s.per_seat_ticket_price);

                cmd.ExecuteNonQuery();
            }

            // ============================================================
            // LẤY GIÁ GHẾ THEO PHÒNG
            // ============================================================
            public double GetTicketPriceByAuditorium(string auditoriumId)
            {
                using var conn = new SqliteConnection(ConnStr);
                conn.Open();

                string sql = @"
                SELECT per_seat_ticket_price
                FROM seat
                WHERE auditorium_id = @aid
                AND per_seat_ticket_price > 0
                LIMIT 1
            ";

                using var cmd = new SqliteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@aid", auditoriumId);

                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value
                    ? Convert.ToDouble(result)
                    : 0;
            }

            // ============================================================
            // LẤY GIÁ GHẾ THEO LOẠI PHÒNG (auditorium_type_id)
            // ============================================================
            public double GetTicketPriceByAuditoriumType(string auditoriumTypeId)
            {
                using var conn = new SqliteConnection(ConnStr);
                conn.Open();

                string sql = @"
                SELECT s.per_seat_ticket_price
                FROM seat s
                JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                WHERE a.auditorium_type_id = @typeId
                AND s.per_seat_ticket_price > 0
                LIMIT 1
            ";

                using var cmd = new SqliteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@typeId", auditoriumTypeId);

                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value
                    ? Convert.ToDouble(result)
                    : 0;
            }
        
    }
}
