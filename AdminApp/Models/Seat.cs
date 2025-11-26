using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.Models
{
    public class Seat
    {
        public string seat_id { get; set; }
        public string seat_type_id { get; set; }
        public string auditorium_id { get; set; }
        public string location { get; set; }
        public string status { get; set; }
        public double per_seat_ticket_price { get; set; }
    }
}
