using Guna.UI2.WinForms;
using Newtonsoft.Json;
using SharedData.Models;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace UserApp
{
    public partial class FormSeatSelection : Form
    {
        public FormSeatSelection()
        {
            InitializeComponent();
        }
        int timeLeft = 600;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;

            // Hiển thị dạng mm:ss
            lblTime.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
            lblTime.Refresh();

            if (timeLeft <= 0)
            {
                timer1.Stop();
                lblTime.Text = "00:00";
                MessageBox.Show("Hết giờ rồi!");
            }
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
            timeLeft = 600;       // reset 10 phút
            lblTime.Text = "10:00";
        }
        private void CreateScreenBar()
        {

            // Tạo panel màn hình
            Guna2Panel screen = new Guna2Panel();
            screen.Name = "screenBar";
            screen.FillColor = Color.WhiteSmoke;
            screen.BorderRadius = 0;
            screen.Height = 45;

            int width = panelSeat.Width - 150;
            screen.Width = width;
            screen.Left = (panelSeat.Width - width) / 2;
            screen.Top = 20;

            // Label MÀN HÌNH
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
            lbl.Text = "MÀN HÌNH";
            lbl.Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(50, 50, 50);
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize = true;

            screen.Controls.Add(lbl);

            lbl.Left = (screen.Width - lbl.Width) / 2;
            lbl.Top = (screen.Height - lbl.Height) / 2;

            panelSeat.Controls.Add(screen);
            screen.BringToFront();
        }
        private string sharedRoomFolder = @"\\LAPTOP-HQN1B4JJ\CinemaData\Room";
        private List<Guna2Button> selectedSeats = new List<Guna2Button>();
        private void LoadRoomLayout(int roomId)
        {
            string filePath = Path.Combine(sharedRoomFolder, $"Room_1.json");

            panelSeat.Controls.Clear();
            CreateScreenBar();

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tìm thấy sơ đồ ghế!", "Thông báo", MessageBoxButtons.OK,
    MessageBoxIcon.Information);
                return;
            }

            var list = JsonConvert.DeserializeObject<List<SeatData>>(File.ReadAllText(filePath));

            foreach (var seat in list)
            {
                Guna2Button btn = new Guna2Button();
                btn.Text = seat.SeatId;
                btn.Tag = seat;

                btn.Width = 50;
                btn.Height = 50;
                btn.Location = new Point(seat.X, seat.Y);

                btn.Font = new Font("Segoe UI", 7, FontStyle.Bold);

                // STYLE GHẾ
                ApplyUserSeatStyle(btn, seat);

                if (seat.Status == "Active")
                    btn.Click += UserSelectSeat;  // chỉ Active mới click

                panelSeat.Controls.Add(btn);
            }
        }

        private void ApplyUserSeatStyle(Guna2Button btn, SeatData seat)
        {
            btn.AutoRoundedCorners = false;
            btn.BorderRadius = 0;
            btn.MouseEnter += Seat_Hover;
            btn.MouseLeave += Seat_Unhover;

            if (seat.Status == "Disabled")
            {
                btn.FillColor = Color.LightGray;
                btn.ForeColor = Color.Black;
                btn.Image = Properties.Resources.close;
                btn.Text = "";
                btn.BorderThickness = 0;
                btn.Enabled = false;  // khóa click
                return;
            }
            // GHẾ ĐÃ ĐẶT → BOOKED
            if (seat.Status == "Booked")
            {
                btn.FillColor = Color.LightCoral;
                btn.ForeColor = Color.Black;
                btn.Enabled = false;         // KHÓA CLICK
                return;
            }

            if (seat.Type == "VIP")
            {
                btn.FillColor = Color.White;
                btn.BorderColor = Color.FromArgb(255, 193, 7);
                btn.ForeColor = Color.Black;
                btn.BorderThickness = 3;
            }
            else
            {
                btn.FillColor = Color.White;
                btn.BorderColor = Color.DimGray;
                btn.ForeColor = Color.Black;
                btn.BorderThickness = 3;
            }
        }
        private void Seat_Hover(object sender, EventArgs e)
        {
            Guna2Button btn = (Guna2Button)sender;

            if (selectedSeats.Contains(btn))
                return;

            btn.FillColor = Color.FromArgb(138, 177, 222); // xanh dương nhạt
        }

        private void Seat_Unhover(object sender, EventArgs e)
        {
            Guna2Button btn = (Guna2Button)sender;

            if (selectedSeats.Contains(btn))
                return;

            var seat = (SeatData)btn.Tag;
            ApplyUserSeatStyle(btn, seat); // trở lại style gốc
        }
        private void UserSelectSeat(object sender, EventArgs e)
        {
            Guna2Button btn = (Guna2Button)sender;
            SeatData seat = (SeatData)btn.Tag;
            timer1.Start();  // BẮT ĐẦU ĐẾM NGƯỢC

            // Nếu ghế Disabled -> không chọn
            if (seat.Status == "Disabled")
                return;

            // GHẾ ĐÃ ĐƯỢC CHỌN → BỎ CHỌN
            if (selectedSeats.Contains(btn))
            {
                selectedSeats.Remove(btn);
                ApplyUserSeatStyle(btn, seat);  // trả về màu gốc
                return;

            }
            // GHẾ CHƯA ĐƯỢC CHỌN → CHỌN THÊM
            selectedSeats.Add(btn);
            HighlightUserSeat(btn);  // highlight xanh lá
        }
        private void HighlightUserSeat(Guna2Button btn)
        {
            btn.FillColor = Color.FromArgb(35, 150, 62);  // xanh lá
            btn.ForeColor = Color.White;
        }

        private void FormSeatSelection_Load(object sender, EventArgs e)
        {
            LoadRoomLayout(1);
        }

    }


}
