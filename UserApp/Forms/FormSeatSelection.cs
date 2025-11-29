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
        // ====== THÔNG TIN SUẤT CHIẾU ======
        private ShowtimeInfo _showtime;
        private string _auditoriumId;     // R01, R02...
        private string _showtimeId;       // T001, T002...

        // ====== THƯ MỤC JSON SƠ ĐỒ PHÒNG ======
        private string _roomJsonFolder;

        // ====== DANH SÁCH GHẾ ======
        private List<SeatUser> _allSeats = new();
        private List<SeatUser> _selectedSeats = new();
        private readonly SeatForShowtimeRepo _seatForShowtimeRepo = new SeatForShowtimeRepo();


        // ============================
        // CONSTRUCTOR
        // ============================
        // Dùng cho Designer
        public FormSeatSelection()
        {
            InitializeComponent();
            _roomJsonFolder = GetRoomFolder();
        }

        // Dùng thực tế: nhận ShowtimeInfo từ FormShowtimeList
        public FormSeatSelection(ShowtimeInfo showtime) : this()
        {
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
            // Tránh crash khi mở bằng Designer
            if (string.IsNullOrEmpty(_auditoriumId) || string.IsNullOrEmpty(_showtimeId))
                return;

            LoadRoom(_auditoriumId, _showtimeId);

            timer1.Start();
        }
        //private void LoadShowtimeInfo()
        //{
        //    lblSuatChieu.Text = _showtime.title;
        //    lblTime.Text = $"{_showtime.StartTime:hh\\:mm}";
        //}

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
            //UpdateSelectedSeatLabel();

            // ====== 1) JSON sơ đồ phòng: Room_5.json, Room_1.json, ...
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
            WHERE s.auditorium_id = $aud";
                cmd.Parameters.AddWithValue("$aud", auditoriumId);

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
                        stStatus[r.GetString(0)] = r.GetString(1); // Trống / Full / Bảo trì
                    }
                }
            }

            // ====== 4) Merge JSON + DB -> SeatUser + vẽ ghế
            foreach (var s in jsonSeats)
            {
                // chuẩn hóa từ Row + Col: A + 1 => A01
                string logicalCode = $"{s.Row}{s.Col:00}";   // A01, B05...
                string fullId = logicalCode + auditoriumId;  // A01R05

                if (!dbSeats.TryGetValue(fullId, out var db))
                    continue; // ghế chưa có trong DB thì bỏ qua

                string showStatus = stStatus.ContainsKey(fullId)
                    ? stStatus[fullId]
                    : "Trống";

                var seatUser = new SeatUser
                {
                    SeatId = fullId,
                    Row = s.Row,
                    Col = s.Col,

                    SeatType = db.type,
                    BaseStatus = db.status,
                    ShowtimeStatus = showStatus,
                    Price = db.price,

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
            btn.Font = new Font("Segoe UI", 8, FontStyle.Bold);

            ApplySeatStyle(btn, seat);

            btn.Click += Seat_Click;

            return btn;
        }

        private void ApplySeatStyle(Guna2Button btn, SeatUser seat)
        {
            if (seat.BaseStatus == "Bảo trì" || seat.ShowtimeStatus == "Bảo trì")
            {
                btn.FillColor = Color.Gray;
                btn.ForeColor = Color.White;
                btn.Enabled = false;
                return;
            }

            if (seat.ShowtimeStatus == "Full")
            {
                btn.FillColor = Color.DarkRed;
                btn.ForeColor = Color.White;
                btn.Enabled = false;
                return;
            }

            // Trống
            btn.FillColor = Color.White;
            btn.BorderColor = seat.SeatType == "VIP" ? Color.Gold : Color.DimGray;
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
            lblSotien.Text = total.ToString("N0") + " đ";
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

            p.Controls.Add(l);
            panelRoom.Controls.Add(p);
        }
        // ===================================================
        // THANH TOÁN → CẬP NHẬT seat_for_showtime
        // ===================================================
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế trước khi thanh toán!",
                    "Chưa chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    foreach (var seat in _selectedSeats)
                    {
                        string seatId = seat.SeatId;    // A01R05
                        string showId = _showtimeId;    // T001

                        var cmd = conn.CreateCommand();
                        cmd.Transaction = tran;
                        cmd.CommandText = @"
INSERT INTO seat_for_showtime(seat_id, showtime_id, status)
VALUES ($sid, $stid, 'Full')
ON CONFLICT(seat_id, showtime_id)
DO UPDATE SET status = 'Full';";

                        cmd.Parameters.AddWithValue("$sid", seatId);
                        cmd.Parameters.AddWithValue("$stid", showId);
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
            }

            MessageBox.Show("Đặt vé thành công! Ghế đã được giữ.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reload để lock ghế Full
            LoadRoom(_auditoriumId, _showtimeId);
            _selectedSeats.Clear();
            UpdateTotal();
            UpdateSelectedSeatLabel();
        }
    }
}
