using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class Showtime
    {
        public string showtime_id { get; set; }
        public string movie_id { get; set; }
        public string auditorium_id { get; set; }
        public string show_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }
}
