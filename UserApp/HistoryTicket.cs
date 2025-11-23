using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp
{
    public partial class HistoryTicket : UserControl
    {
        public HistoryTicket()
        {
            InitializeComponent();
            // Thêm các cột vào dgvHistoryTicket
            dgvHistoryTicket.Columns.Add("STT", "STT");
            dgvHistoryTicket.Columns.Add("MaDatVe", "Mã đặt vé");
            dgvHistoryTicket.Columns.Add("TenPhim", "Tên phim");
            dgvHistoryTicket.Columns.Add("SuatChieu", "Suất chiếu");
            dgvHistoryTicket.Columns.Add("NgayDatVe", "Ngày đặt vé");
            dgvHistoryTicket.Columns.Add("TongTien", "Tổng tiền");
            dgvHistoryTicket.Columns.Add("TicketCode", "Ticket code");

            // Thêm cột nút Xem chi tiết
            DataGridViewButtonColumn btnXemChiTiet = new DataGridViewButtonColumn();
            btnXemChiTiet.HeaderText = "Xem chi tiết";
            btnXemChiTiet.Text = "XEM";
            btnXemChiTiet.UseColumnTextForButtonValue = true;
            dgvHistoryTicket.Columns.Add(btnXemChiTiet);
        }
    }
}
