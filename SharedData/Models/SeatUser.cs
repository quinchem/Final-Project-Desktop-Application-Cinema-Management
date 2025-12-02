using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
  public class SeatUser
    {
        public string SeatId { get; set; }
        public string Location { get; set; }     
        public string Type { get; set; }         
        public string Status { get; set; }       
        public int Price { get; set; }

        // từ JSON
        public string Row { get; set; }
        public int Col { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
