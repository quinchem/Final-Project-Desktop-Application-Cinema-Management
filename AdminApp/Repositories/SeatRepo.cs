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
                    cmd.Parameters.AddWithValue("@id", auditoriumId);

                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToDouble(result);

                    return 0;
                }
            }
        }
    }
}
