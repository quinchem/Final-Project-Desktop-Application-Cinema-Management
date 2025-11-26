using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.Models
{
    public class ShowtimeDisplay
    {
        public string ShowtimeId { get; set; }
        public string TenPhim { get; set; }
        public string Phong { get; set; }
        public string LoaiPhong { get; set; }
        public string NgayChieu { get; set; }
        public string GioChieu { get; set; } // format "HH:mm - HH:mm"
    }
}
