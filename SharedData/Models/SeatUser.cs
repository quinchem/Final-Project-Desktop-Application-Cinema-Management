using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
  public class SeatUser
    {
        public string SeatId { get; set; }         // A01R01
        public string Row { get; set; }            // A
        public int Col { get; set; }               // 1

        public string SeatType { get; set; }       // VIP / Thường (from DB join seat_type)
        public string BaseStatus { get; set; }     // Bình thường / Bảo trì (from seat table)
        public string ShowtimeStatus { get; set; } // Trống / Full / Bảo trì (from seat_for_showtime)

        public int X { get; set; }                 // From JSON
        public int Y { get; set; }                 // From JSON
    }
}
