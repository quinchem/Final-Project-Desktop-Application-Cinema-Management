using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class TicketPrintData
    {
        public string MaPhieu { get; set; }
        public string MaDonDatVe { get; set; }
        public string TicketCode { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string NgayDatVe { get; set; }
        public string TenPhim { get; set; }
        public string SuatChieu { get; set; }
        public string Ghe { get; set; }
        public int SoLuongGhe { get; set; }
        public string PhongChieu { get; set; }
        public decimal TongTien { get; set; }
    }
}
