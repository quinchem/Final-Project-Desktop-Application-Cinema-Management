using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.Models
{
    public class ShowtimeDisplay
    {
        public string showtime_id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string auditorium_type { get; set; }
        public string per_seat_ticket_price { get; set; }
        public string show_date { get; set; }
        public string start_time { get; set; }
    }
}
