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
            private static string ConnStr => DatabaseHelper.GetConnectionString();
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
