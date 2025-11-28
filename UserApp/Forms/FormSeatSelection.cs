using Guna.UI2.WinForms;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SharedData.Models;
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
        private string _auditoriumId = "R01";      // demo phòng 1
        private string _showtimeId = "T001";       // demo suất chiếu
        private string _roomJsonFolder;
        private List<SeatUser> _allSeats = new();
        private List<SeatUser> _selectedSeats = new();
        private Dictionary<string, int> _price = new();

        public FormSeatSelection()
        {
            InitializeComponent();
            _roomJsonFolder = GetRoomFolder();
        }

        private string GetRoomFolder()
        {
            var csb = new SqliteConnectionStringBuilder(DatabaseHelper.GetConnectionString());
            string db = csb.DataSource;
            string root = Directory.GetParent(Path.GetDirectoryName(db)).FullName;
            return Path.Combine(root, "SharedData", "RoomDesign");
        }

        private void FormSeatSelection_Load(object sender, EventArgs e)
        {
            LoadSeatPrices();
            LoadRoom(_auditoriumId, _showtimeId);
        }

        private void LoadSeatPrices()
        {
            _price["Thường"] = 70000;
            _price["VIP"] = 90000;
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

            // 1) đọc JSON
            string json = Path.Combine(_roomJsonFolder, "Room_1.json");
            var jsonSeats = new List<SeatData>();

            if (File.Exists(json))
            {
                jsonSeats = JsonConvert.DeserializeObject<List<SeatData>>(File.ReadAllText(json));
            }

            // 2) JOIN database
            var seatRows = new List<(string seatIdDb, string location, string seatType, string baseStatus)>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
SELECT seat_id, location, st.seat_type, s.status
FROM seat s
LEFT JOIN seat_type st ON s.seat_type_id = st.seat_type_id
WHERE s.auditorium_id = $aud";
                cmd.Parameters.AddWithValue("$aud", auditoriumId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        seatRows.Add((
                            r.GetString(0),
                            r.GetString(1),
                            r.GetString(2),
                            r.GetString(3)
                        ));
                    }
                }
            }

            // 3) Load status theo showtime
            var showtimeStatus = new Dictionary<string, string>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
SELECT seat_id, status
FROM seat_for_showtime
WHERE showtime_id = $st";
                cmd.Parameters.AddWithValue("$st", showtimeId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        showtimeStatus[r.GetString(0)] = r.GetString(1);
                    }
                }
            }

            // 4) Gộp JSON + DB
            foreach (var s in jsonSeats)
            {
                string fullId = s.SeatId + auditoriumId; // A01 + R01 = A01R01
                var db = seatRows.FirstOrDefault(x => x.seatIdDb == fullId);

                SeatUser u = new SeatUser
                {
                    SeatId = fullId,
                    Row = s.Row,
                    Col = s.Col,
                    SeatType = db.seatType,
                    BaseStatus = db.baseStatus,
                    ShowtimeStatus = showtimeStatus.ContainsKey(fullId) ? showtimeStatus[fullId] : "Trống",
                    X = s.X,
                    Y = s.Y
                };

                _allSeats.Add(u);

                var btn = CreateSeatButton(u);
                panelRoom.Controls.Add(btn);
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

            if (_selectedSeats.Contains(seat))
            {
                // bỏ chọn
                _selectedSeats.Remove(seat);
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

        // ===================================================
        // TÍNH TIỀN
        // ===================================================
        private void UpdateTotal()
        {
            int total = 0;

            foreach (var s in _selectedSeats)
            {
                total += _price[s.SeatType];
            }

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
    }
}
