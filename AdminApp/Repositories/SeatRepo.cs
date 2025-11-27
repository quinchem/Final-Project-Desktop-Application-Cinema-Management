using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace AdminApp.Repositories
{
    public class SeatRepo
    {
        private string connectionString;

        public SeatRepo()
        {
            connectionString = DatabaseHelper.GetConnectionString();
        }

        public double GetTicketPriceByAuditorium(string auditoriumId)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT per_seat_ticket_price 
                             FROM seat 
                             WHERE auditorium_id = @id
                             LIMIT 1";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@aid", auditoriumId);

                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToDouble(result);
                    
                }
            }
            return 0;
        }
        public double GetTicketPriceByAuditoriumType(string auditoriumTypeId)
        {
            double price = 0;
            using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();

                // SQL: Lấy giá của ghế, bằng cách nối bảng Seat với Auditorium
                // Điều kiện: Tìm các ghế thuộc các phòng có loại (type) tương ứng
                string query = @"
                    SELECT s.per_seat_ticket_price
                    FROM seat s
                    JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                    WHERE a.auditorium_type_id = @typeId
                    AND s.per_seat_ticket_price > 0
                    LIMIT 1";
                // Thêm điều kiện price > 0 để tránh lấy trúng ghế chưa set giá

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@typeId", auditoriumTypeId);

                    var result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        price = Convert.ToDouble(result);
                    }
                }
            }
            return price;
        }
    }
}
