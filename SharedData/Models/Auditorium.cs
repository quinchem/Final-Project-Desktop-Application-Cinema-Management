using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class Auditorium
    {
        public string auditorium_id { get; set; }
        public string auditorium_type_id { get; set; }
        public string name { get; set; }
        public int number_of_seats { get; set; }
    }
}
