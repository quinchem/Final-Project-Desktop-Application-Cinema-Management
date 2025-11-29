using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class Seat
    {
        public string seat_id { get; set; }
        public string seat_type_id { get; set; }
        public string auditorium_id { get; set; }
        public string location { get; set; }          // A04, B05...
        public string status { get; set; }            // Bình thường / Bảo trì
        public int per_seat_ticket_price { get; set; }
    }
}
