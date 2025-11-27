using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SharedData.Models;

namespace UserApp
{
    public partial class FormSeatSelection : Form
    {
        // ================== CONFIG ==================
        private readonly string roomDesignFolder;

        private readonly string _showtimeId;
        private readonly string _customerId;
        private string _auditoriumId;          // R01 / R02...

        // Danh sách toàn bộ ghế của suất chiếu hiện tại
        private readonly List<SeatUser> _allSeats = new List<SeatUser>();

        // Ghế đang chọn (user)
        private readonly List<SeatUser> _selectedSeats = new List<SeatUser>();

        // Map button -> SeatUser
        private readonly Dictionary<Guna2Button, SeatUser> _btnToSeat
            = new Dictionary<Guna2Button, SeatUser>();

        // Giá từng ghế (key: seat_id thực trong DB: A01R01)
        private readonly Dictionary<string, decimal> _seatPrices
            = new Dictionary<string, decimal>();

        private decimal _totalPrice = 0m;

        // ================== CTOR ==================
        public FormSeatSelection(string showtimeId, string customerId)
        {
            InitializeComponent();

            _showtimeId = showtimeId;
            _customerId = customerId;

            roomDesignFolder = GetRoomDesignFolder();
        }

        private string GetRoomDesignFolder()
        {
            // Giống bên Admin: suy ra <SolutionRoot>\SharedData\RoomDesign
            var csb = new SqliteConnectionStringBuilder(DatabaseHelper.GetConnectionString());
            string dbPath = csb.DataSource;                           // ...\SharedDatabase\Cinema.db
            string dbDir = Path.GetDirectoryName(dbPath);             // ...\SharedDatabase
            string solutionRoot = Directory.GetParent(dbDir)?.FullName ?? dbDir;

            string folder = Path.Combine(solutionRoot, "SharedData", "RoomDesign");
            Directory.CreateDirectory(folder);
            return folder;
        }

        // ================== FORM LOAD ==================
        private void FormSeatSelection_Load(object sender, EventArgs e)
        {
            LoadShowtimeAndAuditorium();
            LoadSeatLayout();
            UpdateTotalPrice();
        }

        // 1) Lấy auditorium_id từ showtime
        private void LoadShowtimeAndAuditorium()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
SELECT auditorium_id
FROM showtime
WHERE showtime_id = $id;
";
                cmd.Parameters.AddWithValue("$id", _showtimeId);

                object res = cmd.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    MessageBox.Show("Không tìm thấy suất chiếu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                _auditoriumId = Convert.ToString(res);   // R01 / R02...
            }
        }

        // 2) Load layout ghế: JSON + DB seat + seat_for_showtime
        private void LoadSeatLayout()
        {
            panelRoomLayout.Controls.Clear();
            _allSeats.Clear();
            _selectedSeats.Clear();
            _btnToSeat.Clear();
            _seatPrices.Clear();

            CreateScreenBar();

            // ====== 2.1 Đọc JSON layout (SeatData) ======
            // auditorium_id = R0X → X = roomIndex
            int roomIndex = 1;
            if (!string.IsNullOrEmpty(_auditoriumId) &&
                _auditoriumId.Length == 3 &&
                _auditoriumId[0] == 'R')
            {
                // "R01" → 1
                roomIndex = int.Parse(_auditoriumId.Substring(2, 1));
            }

            string jsonFile = Path.Combine(roomDesignFolder, $"Room_{roomIndex}.json");
            var jsonSeats = new List<SeatData>();

            if (File.Exists(jsonFile))
            {
                jsonSeats = JsonConvert.DeserializeObject<List<SeatData>>(
                               File.ReadAllText(jsonFile)
                           ) ?? new List<SeatData>();
            }

            // Map JSON theo location: A01 → SeatData
            var jsonMap = jsonSeats.ToDictionary(
                s => s.SeatId,   // A01, A02...
                s => s,
                StringComparer.OrdinalIgnoreCase
            );

            // ====== 2.2 Đọc ghế từ bảng seat ======
            var seatRows = new List<(string SeatIdDb, string Location, string Status, string SeatTypeId, decimal Price)>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
SELECT seat_id, location, status, seat_type_id, per_seat_ticket_price
FROM seat
WHERE auditorium_id = $aud;
";
                cmd.Parameters.AddWithValue("$aud", _auditoriumId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string seatIdDb = r.GetString(0);      // A01R01
                        string loc = r.GetString(1);           // A01
                        string status = r.GetString(2);        // Bình thường / Bảo trì
                        string seatTypeId = r.GetString(3);    // ST01 / ST02
                        decimal price = r.GetDecimal(4);       // 70000 / 90000

                        seatRows.Add((seatIdDb, loc, status, seatTypeId, price));
                        _seatPrices[seatIdDb] = price;
                    }
                }

                // ====== 2.3 Đọc trạng thái từ seat_for_showtime ======
                var showStatusMap = new Dictionary<string, string>();   // seat_id -> status

                var cmd2 = conn.CreateCommand();
                cmd2.CommandText = @"
SELECT seat_id, status
FROM seat_for_showtime
WHERE showtime_id = $stid;
";
                cmd2.Parameters.AddWithValue("$stid", _showtimeId);

                using (var r2 = cmd2.ExecuteReader())
                {
                    while (r2.Read())
                    {
                        string sid = r2.GetString(0);   // A01R01
                        string st = r2.GetString(1);    // Trống / Full / Bảo trì
                        showStatusMap[sid] = st;
                    }
                }

                // ====== 2.4 Kết hợp tạo SeatUser + Button ======
                foreach (var row in seatRows)
                {
                    string seatIdDb = row.SeatIdDb;   // A01R01
                    string loc = row.Location;        // A01

                    if (string.IsNullOrWhiteSpace(loc) || loc.Length < 2)
                        continue;

                    string rowChar = loc.Substring(0, 1);     // A
                    int col = int.Parse(loc.Substring(1));    // 1

                    // Tìm vị trí (X, Y) từ JSON theo SeatId A01
                    jsonMap.TryGetValue(loc, out var jsonSeat);

                    int posX = jsonSeat?.X ?? 0;
                    int posY = jsonSeat?.Y ?? 0;

                    string baseStatus = row.Status;               // Bình thường / Bảo trì
                    string seatTypeId = row.SeatTypeId;           // ST01 / ST02
                    string showStatus = showStatusMap.ContainsKey(seatIdDb)
                        ? showStatusMap[seatIdDb]
                        : "";                                     // null → chưa phát sinh đặt

                    string seatType = seatTypeId == "ST02" ? "VIP" : "thường";

                    // Nếu seat bảng seat là Bảo trì thì showtimeStatus luôn coi như Bảo trì
                    string finalShowtimeStatus;
                    if (string.Equals(baseStatus, "Bảo trì", StringComparison.OrdinalIgnoreCase))
                    {
                        finalShowtimeStatus = "Bảo trì";
                    }
                    else
                    {
                        // Trống / Full / Bảo trì / "" (chưa phát sinh)
                        if (string.IsNullOrWhiteSpace(showStatus))
                            finalShowtimeStatus = "Trống";
                        else
                            finalShowtimeStatus = showStatus;
                    }

                    var su = new SeatUser
                    {
                        SeatId = seatIdDb,          // A01R01
                        Row = rowChar,              // A
                        Col = col,                  // 1
                        SeatType = seatType,        // VIP / thường
                        BaseStatus = baseStatus,    // Bình thường / Bảo trì
                        ShowtimeStatus = finalShowtimeStatus, // Trống / Full / Bảo trì
                        X = posX,
                        Y = posY
                    };

                    _allSeats.Add(su);

                    // Tạo nút ghế
                    var btn = CreateSeatButton(su);
                    _btnToSeat[btn] = su;
                }
            }
        }

        // ================== TẠO MÀN HÌNH ==================
        private void CreateScreenBar()
        {
            var screen = new Guna2Panel
            {
                Name = "screenBar",
                FillColor = Color.WhiteSmoke,
                BorderRadius = 0,
                Height = 50
            };

            int width = panelRoomLayout.Width - 150;
            screen.Width = width;
            screen.Left = (panelRoomLayout.Width - width) / 2;
            screen.Top = 20;

            var lbl = new Label
            {
                Text = "MÀN HÌNH",
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                BackColor = Color.Transparent,
                AutoSize = true
            };

            screen.Controls.Add(lbl);
            lbl.Left = (screen.Width - lbl.Width) / 2;
            lbl.Top = (screen.Height - lbl.Height) / 2;

            panelRoomLayout.Controls.Add(screen);
            screen.BringToFront();
        }

        // ================== TẠO BUTTON GHẾ ==================
        private Guna2Button CreateSeatButton(SeatUser seat)
        {
            var btn = new Guna2Button
            {
                Size = new Size(50, 50),
                Location = new Point(seat.X, seat.Y),
                Text = $"{seat.Row}{seat.Col:00}",           // A01, A02...
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                Tag = seat
            };

            ApplySeatStyle(btn, seat);

            btn.Click += SeatButton_Click;
            btn.MouseEnter += Seat_Hover;
            btn.MouseLeave += Seat_Unhover;

            panelRoomLayout.Controls.Add(btn);
            return btn;
        }

        private void ApplySeatStyle(Guna2Button btn, SeatUser seat, bool isSelected = false)
        {
            btn.AutoRoundedCorners = false;
            btn.BorderRadius = 0;

            // GHẾ KHÔNG CHỌN
            if (!isSelected)
            {
                // Bảo trì
                if (seat.BaseStatus == "Bảo trì" || seat.ShowtimeStatus == "Bảo trì")
                {
                    btn.FillColor = Color.DimGray;
                    btn.ForeColor = Color.White;
                    btn.BorderThickness = 0;
                    return;
                }

                // Full
                if (seat.ShowtimeStatus == "Full")
                {
                    btn.FillColor = Color.LightCoral;
                    btn.ForeColor = Color.White;
                    btn.BorderThickness = 0;
                    return;
                }

                // Trống
                btn.FillColor = Color.White;
                btn.ForeColor = Color.Black;
                btn.BorderThickness = 3;
                btn.BorderColor = seat.SeatType == "VIP"
                    ? Color.FromArgb(255, 193, 7)
                    : Color.DimGray;

                return;
            }

            // GHẾ ĐANG ĐƯỢC CHỌN
            btn.FillColor = Color.FromArgb(35, 150, 62);
            btn.ForeColor = Color.White;
            btn.BorderThickness = 3;
            btn.BorderColor = seat.SeatType == "VIP"
                ? Color.FromArgb(255, 193, 7)
                : Color.White;
        }

        // ================== HOVER ==================
        private void Seat_Hover(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            var seat = (SeatUser)btn.Tag;

            if (_selectedSeats.Contains(seat)) return;

            if (seat.BaseStatus == "Bảo trì" || seat.ShowtimeStatus == "Bảo trì" || seat.ShowtimeStatus == "Full")
                return;

            btn.FillColor = Color.FromArgb(167, 238, 250);
        }

        private void Seat_Unhover(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            var seat = (SeatUser)btn.Tag;

            bool isSelected = _selectedSeats.Contains(seat);
            ApplySeatStyle(btn, seat, isSelected);
        }

        // ================== CLICK GHẾ ==================
        private void SeatButton_Click(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            var seat = (SeatUser)btn.Tag;

            // GHẾ KHÔNG ĐƯỢC CHỌN
            if (seat.BaseStatus == "Bảo trì" || seat.ShowtimeStatus == "Bảo trì")
            {
                MessageBox.Show("Ghế này đang bảo trì, không thể chọn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (seat.ShowtimeStatus == "Full")
            {
                MessageBox.Show("Ghế này đã được đặt trước!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Toggle chọn
            if (_selectedSeats.Contains(seat))
            {
                _selectedSeats.Remove(seat);
            }
            else
            {
                _selectedSeats.Add(seat);
            }

            bool isSelected = _selectedSeats.Contains(seat);
            ApplySeatStyle(btn, seat, isSelected);
            UpdateTotalPrice();
        }

        // ================== TÍNH TIỀN ==================
        private void UpdateTotalPrice()
        {
            _totalPrice = 0m;

            foreach (var seat in _selectedSeats)
            {
                if (_seatPrices.TryGetValue(seat.SeatId, out var price))
                {
                    _totalPrice += price;
                }
            }

            // Hiển thị: 120.000 → "120,000"
            lblSoTien.Text = _totalPrice.ToString("#,##0 VNĐ");
        }

        // ================== ĐẶT VÉ & LƯU BILL ==================
        private void btnDatVe_Click(object sender, EventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Hãy chọn ít nhất 1 ghế!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // 1) Tạo bill
                    string billId = Guid.NewGuid().ToString("N").Substring(0, 12);
                    int quantity = _selectedSeats.Count;

                    var cmdBill = conn.CreateCommand();
                    cmdBill.Transaction = tran;
                    cmdBill.CommandText = @"
INSERT INTO bill(bill_id, customer_id, showtime_id, bill_date,
                 quantity_ticket, per_seat_ticket_price, note, total)
VALUES ($id, $cus, $show, $date, $qty, $price, $note, $total);
";
                    cmdBill.Parameters.AddWithValue("$id", billId);
                    cmdBill.Parameters.AddWithValue("$cus", _customerId);
                    cmdBill.Parameters.AddWithValue("$show", _showtimeId);
                    cmdBill.Parameters.AddWithValue("$date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmdBill.Parameters.AddWithValue("$qty", quantity);
                    cmdBill.Parameters.AddWithValue("$price", 0);  // không dùng, total mới là chuẩn
                    cmdBill.Parameters.AddWithValue("$note", "");
                    cmdBill.Parameters.AddWithValue("$total", _totalPrice);

                    cmdBill.ExecuteNonQuery();

                    // 2) Cập nhật seat_for_showtime (Full)
                    foreach (var seat in _selectedSeats)
                    {
                        var cmdUpdate = conn.CreateCommand();
                        cmdUpdate.Transaction = tran;
                        cmdUpdate.CommandText = @"
UPDATE seat_for_showtime
SET status = 'Full'
WHERE seat_id = $sid AND showtime_id = $stid;
";
                        cmdUpdate.Parameters.AddWithValue("$sid", seat.SeatId);   // A01R01
                        cmdUpdate.Parameters.AddWithValue("$stid", _showtimeId);

                        int rows = cmdUpdate.ExecuteNonQuery();

                        if (rows == 0)
                        {
                            var cmdInsert = conn.CreateCommand();
                            cmdInsert.Transaction = tran;
                            cmdInsert.CommandText = @"
INSERT INTO seat_for_showtime(seat_id, showtime_id, status)
VALUES ($sid, $stid, 'Full');
";
                            cmdInsert.Parameters.AddWithValue("$sid", seat.SeatId);
                            cmdInsert.Parameters.AddWithValue("$stid", _showtimeId);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                }
            }

            MessageBox.Show("Đặt vé thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Refresh lại trạng thái ghế
            LoadSeatLayout();
            UpdateTotalPrice();
        }
    }
}
