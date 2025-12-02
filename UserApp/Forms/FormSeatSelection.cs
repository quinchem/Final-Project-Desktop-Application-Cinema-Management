using Guna.UI2.WinForms;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormSeatSelection : Form
    {
        private UserMainForm parentForm;

        // Khai báo thông tin suất chiếu và sơ đồ ghế
        private ShowtimeInfo _showtime;
        private string _auditoriumId;
        private string _showtimeId;
        private ImageRepo _imageRepo = new ImageRepo();
        private string _roomJsonFolder;

        // ====== DANH SÁCH GHẾ ======
        private List<SeatUser> _allSeats = new();
        private List<SeatUser> _selectedSeats = new();

        // ====== HẸN GIỜ CHỌN GHẾ (5 phút) ======
        private int countdown = 600;
        private bool isCounting = false;



        // 1. Constructor mặc định – cho Designer
        public FormSeatSelection()
        {
            InitializeComponent();
            _roomJsonFolder = GetRoomFolder();
        }

        // 2. Constructor dùng thật – có MainForm + Showtime
        public FormSeatSelection(UserMainForm parent, ShowtimeInfo showtime) : this()
        {
            parentForm = parent;
            _showtime = showtime;
            _auditoriumId = showtime.auditorium_id;
            _showtimeId = showtime.showtime_id;
        }

        // ============================
        // PATH ĐẾN FOLDER ROOM JSON
        // ============================
        private string GetRoomFolder()
        {
            var csb = new SqliteConnectionStringBuilder(DatabaseHelper.GetConnectionString());
            string db = csb.DataSource;
            string root = Directory.GetParent(Path.GetDirectoryName(db)).FullName;
            return Path.Combine(root, "SharedData", "RoomDesign");
        }

        private void FormSeatSelection_Load(object sender, EventArgs e)
        {
            if (_showtime == null ||
                string.IsNullOrEmpty(_auditoriumId) ||
                string.IsNullOrEmpty(_showtimeId))
                return;

            // Thông tin suất chiếu
            lblTenPhim.Text = _showtime.title;
            lblPhong.Text = _showtime.name;
            lblSuatChieu.Text = $"{_showtime.show_date} - {_showtime.start_time}";

            LoadRoom(_auditoriumId, _showtimeId);
            LoadPoster();

            timer1.Stop();
            isCounting = false;
            countdown = 300;
            lblTime.Text = "05:00";
        }

        private void LoadPoster()
        {
            try
            {
                byte[] imgData = _imageRepo.GetMoviePoster(_showtime.movie_id);

                if (imgData != null && imgData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        picturePhim.Image = Image.FromStream(ms);
                        picturePhim.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                else
                {
                    picturePhim.Image = null; // hoặc ảnh default
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load poster: " + ex.Message);
            }
        }

        // ===================================================
        // LOAD PHÒNG: JSON + DB
        // ===================================================
        private void LoadRoom(string auditoriumId, string showtimeId)
        {
            _allSeats.Clear();
            _selectedSeats.Clear();
            panelRoom.Controls.Clear();
            CreateScreenBar();
            UpdateTotal();
            UpdateSelectedSeatLabel();

            //  1) Load file JSON
            string digits = new string(auditoriumId.Where(char.IsDigit).ToArray());
            int roomNumber = int.Parse(digits);
            string jsonPath = Path.Combine(_roomJsonFolder, $"Room_{roomNumber}.json");

            if (!File.Exists(jsonPath))
            {
                MessageBox.Show("Không tìm thấy layout phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var jsonSeats = JsonConvert.DeserializeObject<List<SeatData>>(File.ReadAllText(jsonPath));

            // 2) Load ghế từ bảng seat
            var dbSeats = new Dictionary<string, (string type, string status, double price)>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = @"
                    SELECT s.seat_id, st.seat_type, s.status, s.per_seat_ticket_price
                    FROM seat s
                    LEFT JOIN seat_type st ON s.seat_type_id = st.seat_type_id
                    WHERE s.auditorium_id = $aud";
                cmd.Parameters.AddWithValue("$aud", auditoriumId);

                using var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    dbSeats[r.GetString(0)] = (
                        r.GetString(1),
                        r.GetString(2),
                        r.GetDouble(3)
                    );
                }
            }

            // 3) Load ghế FULL theo suất chiếu 
            var fullSeats = SeatForShowtimeRepo.GetSeatStatus(showtimeId);
            // chỉ chứa FULL

            // 4) Merge
            foreach (var s in jsonSeats)
            {
                string logical = $"{s.Row}{s.Col:00}";
                string fullId = logical + auditoriumId;

                if (!dbSeats.ContainsKey(fullId)) continue;

                var db = dbSeats[fullId];
                string finalStatus;

                // Ghế Bảo trì từ bảng seat
                if (db.status == "Bảo trì")
                    finalStatus = "Bảo trì";

                // Ghế đã FULL ở suất chiếu
                else if (fullSeats.ContainsKey(fullId))
                    finalStatus = "Full";

                // Còn lại là TRỐNG
                else
                    finalStatus = "Trống";

                SeatUser seatUser = new SeatUser
                {
                    SeatId = fullId,
                    Row = s.Row,
                    Col = s.Col,
                    Type = db.type,
                    Status = finalStatus,
                    Price = (int)db.price,
                    X = s.X,
                    Y = s.Y
                };

                _allSeats.Add(seatUser);
                panelRoom.Controls.Add(CreateSeatButton(seatUser));
            }
        }

        // ===================================================
        // GHẾ BUTTON
        // ===================================================
        private Guna2Button CreateSeatButton(SeatUser seat)
        {
            var btn = new Guna2Button();
            btn.Size = new Size(55, 55);
            btn.Location = new Point(seat.X, seat.Y);
            btn.Text = $"{seat.Row}{seat.Col:00}";
            btn.Tag = seat;
            btn.Font = new Font("Segoe UI", 8, FontStyle.Bold);

            ApplySeatStyle(btn, seat);
            btn.Click += Seat_Click;

            return btn;
        }

        private void ApplySeatStyle(Guna2Button btn, SeatUser seat)
        {
            if (seat.Status == "Bảo trì" || seat.Status == "Full")
            {
                btn.FillColor = Color.Gray;
                btn.Enabled = false;
                return;
            }

            // TRỐNG
            btn.FillColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.BorderColor = seat.Type == "VIP" ? Color.Gold : Color.DimGray;
            btn.BorderThickness = 3;
        }

        // ===================================================
        // CHỌN GHẾ
        // ===================================================
        private void Seat_Click(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            var seat = (SeatUser)btn.Tag;

            bool exists = _selectedSeats.Any(su => su.SeatId == seat.SeatId);

            if (exists)
            {
                _selectedSeats.RemoveAll(su => su.SeatId == seat.SeatId);
                ApplySeatStyle(btn, seat);
            }
            else
            {
                _selectedSeats.Add(seat);
                btn.FillColor = Color.ForestGreen;
                btn.ForeColor = Color.White;
            }

            UpdateTotal();
            UpdateSelectedSeatLabel();

            // Bắt đầu đếm ngược 5 phút lần đầu chọn
            if (!isCounting && _selectedSeats.Count > 0)
            {
                countdown = 300;
                isCounting = true;
                timer1.Start();
            }
            // Nếu bỏ hết ghế → dừng đếm
            else if (_selectedSeats.Count == 0)
            {
                isCounting = false;
                timer1.Stop();
                countdown = 300;
                lblTime.Text = "05:00";
            }
        }

        private void UpdateSelectedSeatLabel()
        {
            if (_selectedSeats.Count == 0)
            {
                lblGheDaChon.Text = "Chưa chọn ghế";
                return;
            }

            var list = _selectedSeats
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Col)
                .Select(s => $"{s.Row}{s.Col:00}");

            lblGheDaChon.Text = string.Join(", ", list);
        }

        // ===================================================
        // TÍNH TIỀN
        // ===================================================
        private void UpdateTotal()
        {
            double total = _selectedSeats.Sum(s => s.Price);
            lblSotien.Text = total.ToString("N0") + " VND";
        }

        // ===================================================
        // MÀN HÌNH
        // ===================================================
        private void CreateScreenBar()
        {
            var p = new Guna2Panel();
            p.Size = new Size(panelRoom.Width - 100, 50);
            p.Left = 50;
            p.Top = 20;
            p.FillColor = Color.WhiteSmoke;

            var l = new Label();
            l.Text = "MÀN HÌNH";
            l.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            l.AutoSize = true;
            l.Left = (p.Width - l.Width) / 2;
            l.Top = (p.Height - l.Height) / 2;
            l.BackColor = Color.Transparent;

            p.Controls.Add(l);
            panelRoom.Controls.Add(p);
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế trước khi thanh toán!",
                    "Chưa chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dừng timer giữ ghế 
            isCounting = false;
            timer1.Stop();

            var parent = this.ParentForm as UserMainForm ?? parentForm;
            if (parent != null)
            {
                parent.OpenChildForm(new FormPayment1(_showtime, _selectedSeats, parent.CurrentUser));
            }
        }

        // ===================================================
        // TIMER GIỮ GHẾ
        // ===================================================
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (countdown <= 0)
            {
                timer1.Stop();
                isCounting = false;

                MessageBox.Show("Hết thời gian giữ ghế! Vui lòng chọn lại.",
                                "Hết hạn", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                _selectedSeats.Clear();
                UpdateSelectedSeatLabel();
                UpdateTotal();
                LoadRoom(_auditoriumId, _showtimeId);
                lblTime.Text = "05:00";
                return;
            }

            countdown--;

            int minutes = countdown / 60;
            int seconds = countdown % 60;

            lblTime.Text = $"{minutes:00}:{seconds:00}";
        }
    }
}
