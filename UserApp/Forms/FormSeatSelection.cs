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

        // ====== THÔNG TIN SUẤT CHIẾU ======
        private ShowtimeInfo _showtime;
        private string _auditoriumId;     // R01, R02...
        private string _showtimeId;       // T001, T002...
        private ImageRepo _imageRepo = new ImageRepo();

        // ====== THƯ MỤC JSON SƠ ĐỒ PHÒNG ======
        private string _roomJsonFolder;

        // ====== DANH SÁCH GHẾ ======
        private List<SeatUser> _allSeats = new();
        private List<SeatUser> _selectedSeats = new();

        // ====== HẸN GIỜ CHỌN GHẾ (5 phút) ======
        private int countdown = 300;
        private bool isCounting = false;

        // ============================
        // CONSTRUCTOR
        // ============================

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

        // ===================================================
        // LOAD POSTER PHIM
        // ===================================================
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

            // ====== 1) JSON sơ đồ phòng: Room_1.json, Room_5.json, ...
            string digits = new string(auditoriumId.Where(char.IsDigit).ToArray()); // R05 -> "05"
            int roomNumber = int.Parse(digits);                                     // -> 5
            string jsonPath = Path.Combine(_roomJsonFolder, $"Room_{roomNumber}.json");

            if (!File.Exists(jsonPath))
            {
                MessageBox.Show($"Không tìm thấy sơ đồ phòng: {jsonPath}",
                    "Lỗi sơ đồ phòng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var jsonSeats = JsonConvert.DeserializeObject<List<SeatData>>(File.ReadAllText(jsonPath))
                            ?? new List<SeatData>();

            // ====== 2) Load GHẾ từ database + loại ghế + giá
            var dbSeats = new Dictionary<string, (string type, string status, double price)>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT s.seat_id, st.seat_type, s.status, s.per_seat_ticket_price
                    FROM seat s
                    LEFT JOIN seat_type st ON s.seat_type_id = st.seat_type_id
                    WHERE s.auditorium_id = @aud";
                cmd.Parameters.AddWithValue("@aud", auditoriumId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string id = r.GetString(0);
                        string type = r.GetString(1);   // VIP / thường
                        string status = r.GetString(2); // Bình thường / Bảo trì
                        double price = r.GetDouble(3);  // giá ghế

                        dbSeats[id] = (type, status, price);
                    }
                }
            }

            // ====== 3) Load trạng thái ghế theo suất chiếu
            var stStatus = new Dictionary<string, string>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT seat_id, status 
                    FROM seat_for_showtime 
                    WHERE showtime_id = $id";
                cmd.Parameters.AddWithValue("$id", showtimeId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        stStatus[r.GetString(0)] = r.GetString(1); // Full / Bảo trì
                    }
                }
            }

            // ====== 4) Merge JSON + DB -> SeatUser + vẽ ghế
            foreach (var s in jsonSeats)
            {
                string logicalCode = $"{s.Row}{s.Col:00}";   // A01, B05...
                string fullId = logicalCode + auditoriumId;  // A01R05

                if (!dbSeats.TryGetValue(fullId, out var db))
                    continue; // ghế chưa có trong DB thì bỏ qua

                string mergedStatus = stStatus.ContainsKey(fullId)
                ? stStatus[fullId]        // Full / Bảo trì lấy từ seat_for_showtime
                : "Trống";                // KHÔNG dùng db.status nữa

                var seatUser = new SeatUser
                {
                    SeatId = fullId,
                    Row = s.Row,
                    Col = s.Col,

                    Type = db.type,

                    // TRẠNG THÁI
                    Status = mergedStatus,

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
            btn.Size = new Size(50, 50);
            btn.Location = new Point(seat.X, seat.Y);
            btn.Text = $"{seat.Row}{seat.Col:00}";
            btn.Tag = seat;
            btn.Font = new Font("Segoe UI", 7, FontStyle.Bold);

            ApplySeatStyle(btn, seat);
            btn.Click += Seat_Click;

            return btn;
        }

        private void ApplySeatStyle(Guna2Button btn, SeatUser seat)
        {
            // Bảo trì
            if (seat.Status.Equals("Bảo trì", StringComparison.OrdinalIgnoreCase))
            {
                btn.FillColor = Color.Gray;
                btn.ForeColor = Color.White;
                btn.Enabled = false;
                return;
            }

            // Đã đặt (Full)
            if (seat.Status.Equals("Full", StringComparison.OrdinalIgnoreCase))
            {
                btn.FillColor = Color.LightCoral;
                btn.ForeColor = Color.White;
                btn.Enabled = false;
                return;
            }

            // Trống
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

        // ===================================================
        // NÚT THANH TOÁN → CHUYỂN PAYMENT1
        // ===================================================
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế trước khi thanh toán!",
                    "Chưa chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dừng timer giữ ghế (phần lock ghế / cập nhật DB để Payment2 xử lý)
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
