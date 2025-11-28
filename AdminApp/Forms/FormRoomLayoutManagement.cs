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

namespace AdminApp
{
    public partial class FormRoomLayoutManagement : Form
    {
        // ============= CẤU HÌNH =============

        // Folder RoomDesign đặt trong solution:  <SolutionRoot>\SharedData\RoomDesign
        private readonly string roomDesignFolder;

        private int currentRoom = 1;                       // 1..5
        private Dictionary<string, int> initialSeatMap;    // lưu cấu trúc rows ban đầu

        // GHẾ ĐANG DRAG
        private Guna2Button draggingSeat = null;
        private bool dragging = false;
        private Point dragCursorPoint, dragStartPoint;

        // DRAG MÀN HÌNH
        private bool draggingScreen = false;
        private Point screenDragStartPoint;

        // Chế độ chỉnh sửa
        private bool editMode = false;

        // DANH SÁCH GHẾ THEO HÀNG (logic)
        private Dictionary<string, int> seatMap = new Dictionary<string, int>
        {
            { "A", 15 },
            { "B", 15 },
            { "C", 15 },
            { "D", 15 },
            { "E", 15 },
            { "F", 15 }
        };

        // Danh sách ghế được chọn
        private readonly List<Guna2Button> selectedSeats = new List<Guna2Button>();

        // ==========================================
        //  CTOR
        // ==========================================
        public FormRoomLayoutManagement()
        {
            InitializeComponent();

            initialSeatMap = seatMap.ToDictionary(x => x.Key, x => x.Value);

            roomDesignFolder = GetRoomDesignFolder();
        }

        private string GetRoomDesignFolder()
        {
            // Lấy đường dẫn DB từ DatabaseHelper
            var csb = new SqliteConnectionStringBuilder(DatabaseHelper.GetConnectionString());
            string dbPath = csb.DataSource;                           // ...\SharedDatabase\Cinema.db
            string dbDir = Path.GetDirectoryName(dbPath);             // ...\SharedDatabase
            string solutionRoot = Directory.GetParent(dbDir)?.FullName ?? dbDir;

            string folder = Path.Combine(solutionRoot, "SharedData", "RoomDesign");
            Directory.CreateDirectory(folder);
            return folder;
        }

        // ==========================================
        //  FORM LOAD
        // ==========================================
        private void FormRoomLayoutManagement_Load(object sender, EventArgs e)
        {
            LoadRoom(1);
            if (btnPhong1 != null) btnPhong1.Checked = true;
        }

        // ==========================================
        //  LOAD PHÒNG
        // ==========================================
        private void LoadRoom(int roomIndex)
        {
            currentRoom = roomIndex;
            string auditoriumId = $"R0{roomIndex}";

            int seatCount = 0;
            string auditoriumTypeName = "";

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // ----- 1. Đọc thông tin PHÒNG từ bảng auditorium + auditorium_type -----
                var cmdInfo = conn.CreateCommand();
                cmdInfo.CommandText = @"
SELECT atype.auditorium_type, a.number_of_seats
FROM auditorium a
LEFT JOIN auditorium_type atype
    ON a.auditorium_type_id = atype.auditorium_type_id
WHERE a.auditorium_id = $id;
";
                cmdInfo.Parameters.AddWithValue("$id", auditoriumId);

                using (var reader = cmdInfo.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        auditoriumTypeName = reader.IsDBNull(0) ? "" : reader.GetString(0); // 2D / 3D
                        seatCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);           // 90
                    }
                }

                // Gán UI
                txtSoGhe.Text = seatCount.ToString();
                txtDinhDang.Text = auditoriumTypeName;

                // ----- 2. Load JSON layout ghế -----
                Directory.CreateDirectory(roomDesignFolder);
                string jsonFile = Path.Combine(roomDesignFolder, $"Room_{roomIndex}.json");

                panelRoomLayout.Controls.Clear();
                CreateScreenBar();
                selectedSeats.Clear();
                SetEditMode(false);
                txtMaGhe.Text = "";

                if (File.Exists(jsonFile))
                {
                    var list = JsonConvert.DeserializeObject<List<SeatData>>(File.ReadAllText(jsonFile)) ?? new List<SeatData>();

                    foreach (var seat in list)
                    {
                        // Chuẩn hóa Type từ file cũ: Normal / VIP / ST01 / ST02 / thường / Thường
                        string t = (seat.Type ?? "").Trim().ToLower();
                        if (t == "vip" || t == "st02")
                            seat.Type = "VIP";
                        else
                            seat.Type = "Thường";

                        // Chuẩn hóa Status: Active/Disabled → Bình thường/Bảo trì
                        string st = (seat.Status ?? "").Trim().ToLower();
                        if (st == "bảo trì" || st == "bao tri" || st == "disabled")
                            seat.Status = "Bảo trì";
                        else
                            seat.Status = "Bình thường";

                        var btn = CreateSeat(seat);
                        btn.Width = 50;
                        btn.Height = 50;
                    }
                }
                else
                {
                    GenerateSeatLayout();
                }

                UpdateSeatCountUI();
            }
        }

        // ==========================================
        //  MÀN HÌNH
        // ==========================================
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

            screen.MouseDown += Screen_MouseDown;
            screen.MouseMove += Screen_MouseMove;
            screen.MouseUp += Screen_MouseUp;

            panelRoomLayout.Controls.Add(screen);
            screen.BringToFront();
        }

        private void Screen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            draggingScreen = true;
            dragCursorPoint = Cursor.Position;

            var screen = (Guna2Panel)sender;
            screenDragStartPoint = screen.Location;
            screen.FillColor = Color.LightGray;
        }

        private void Screen_MouseMove(object sender, MouseEventArgs e)
        {
            if (!draggingScreen) return;

            var diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            var screen = (Guna2Panel)sender;

            screen.Location = Point.Add(screenDragStartPoint, new Size(diff));
        }

        private void Screen_MouseUp(object sender, MouseEventArgs e)
        {
            draggingScreen = false;
            var screen = (Guna2Panel)sender;
            screen.FillColor = Color.WhiteSmoke;
        }

        // ==========================================
        //  TẠO GHẾ + CHỌN GHẾ
        // ==========================================
        private Guna2Button CreateSeat(SeatData seat)
        {
            var btn = new Guna2Button
            {
                Size = new Size(50, 50),
                Location = new Point(seat.X, seat.Y),
                Text = seat.SeatId,      // A01, A02,...
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                Tag = seat
            };

            ApplySeatStyle(btn, seat);

            btn.MouseEnter += Seat_Hover;
            btn.MouseLeave += Seat_Unhover;
            btn.MouseDown += Seat_MouseDown;
            btn.MouseMove += Seat_MouseMove;
            btn.MouseUp += Seat_MouseUp;
            btn.Click += Seat_Select;

            panelRoomLayout.Controls.Add(btn);
            return btn;
        }

        private void Seat_Select(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            var seat = (SeatData)btn.Tag;

            // Nếu ghế đang được chọn → bỏ chọn
            if (selectedSeats.Contains(btn))
            {
                selectedSeats.Remove(btn);
                ApplySeatStyle(btn, seat); // trả về style gốc
                return;
            }

            // Chưa chọn → Chọn mới
            selectedSeats.Add(btn);

            // HIGHLIGHT: xanh lá
            btn.FillColor = Color.FromArgb(35, 150, 62);
            btn.ForeColor = Color.White;
            btn.BorderColor = seat.Type == "VIP"
                                ? Color.FromArgb(255, 193, 7)
                                : Color.DimGray;
            btn.BorderThickness = 4;
        }

        private void SetEditMode(bool enable)
        {
            editMode = enable;

            txtMaGhe.ReadOnly = !enable;
            rdoVip.Enabled = enable;
            rdoThuong.Enabled = enable;
            rdoBaoTri.Enabled = enable;
            rdoBinhThuong.Enabled = enable;
        }

        // ==========================================
        //  STYLE GHẾ + HOVER
        // ==========================================
        private void ApplySeatStyle(Guna2Button btn, SeatData seat)
        {
            btn.AutoRoundedCorners = false;
            btn.BorderRadius = 0;
            btn.ForeColor = Color.Black;

            // Status
            if (seat.Status == "Bảo trì")
            {
                btn.FillColor = Color.DimGray;
                btn.ForeColor = Color.White;
                btn.BorderThickness = 0;
                return;
            }

            // Type
            if (seat.Type == "VIP")
            {
                btn.FillColor = Color.White;
                btn.BorderColor = Color.FromArgb(255, 193, 7);
                btn.BorderThickness = 4;
            }
            else
            {
                btn.FillColor = Color.White;
                btn.BorderColor = Color.DimGray;
                btn.BorderThickness = 4;
            }
        }

        private void Seat_Hover(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            if (selectedSeats.Contains(btn)) return;

            btn.FillColor = Color.FromArgb(167, 238, 250);
        }

        private void Seat_Unhover(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            if (selectedSeats.Contains(btn)) return;

            var seat = (SeatData)btn.Tag;
            ApplySeatStyle(btn, seat);
        }

        private void FormatSeatPositions()
        {
            if (panelRoomLayout.Controls.Count == 0)
                return;

            // PANEL SIZE
            int panelW = panelRoomLayout.Width;

            // Request GHẾ SIZE CƠ BẢN
            int baseSeatW = 50;
            int baseSeatH = 50;
            int baseSpaceX = 8;
            int baseSpaceY = 10;

            int maxSeats = seatMap.Max(r => r.Value);

            // ======= SCALE GHẾ THEO PANEL =======
            int wantedWidth = (maxSeats * baseSeatW) + ((maxSeats - 1) * baseSpaceX);
            float scale = (float)(panelW - 40) / wantedWidth;
            if (scale > 1) scale = 1;

            int seatW = (int)(baseSeatW * scale);
            int seatH = (int)(baseSeatH * scale);
            int spaceX = (int)(baseSpaceX * scale);
            int spaceY = baseSpaceY;

            int startY = 90;

            foreach (var row in seatMap)
            {
                string rowName = row.Key;
                int count = row.Value;

                int rowWidth = (count * seatW) + ((count - 1) * spaceX);
                int startX = (panelW - rowWidth) / 2;

                var rowSeats = panelRoomLayout.Controls
                                .OfType<Guna2Button>()
                                .Where(b => b.Tag is SeatData sd && sd.Row == rowName)
                                .OrderBy(b => ((SeatData)b.Tag).Col)
                                .ToList();

                foreach (var btn in rowSeats)
                {
                    var seat = (SeatData)btn.Tag;

                    btn.Width = seatW;
                    btn.Height = seatH;

                    btn.Left = startX;
                    btn.Top = startY;

                    seat.X = btn.Left;
                    seat.Y = btn.Top;

                    startX += seatW + spaceX;
                }

                startY += seatH + spaceY;
            }
        }

        // ==========================================
        //  DRAG GHẾ
        // ==========================================
        private void Seat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            draggingSeat = (Guna2Button)sender;
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragStartPoint = draggingSeat.Location;
        }

        private void Seat_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;

            var diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            draggingSeat.Location = Point.Add(dragStartPoint, new Size(diff));
        }

        private void Seat_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            if (draggingSeat == null) return;

            var seat = (SeatData)draggingSeat.Tag;
            seat.X = draggingSeat.Location.X;
            seat.Y = draggingSeat.Location.Y;
        }

        // ==========================================
        //  TẠO LAYOUT TỰ ĐỘNG
        // ==========================================
        private void GenerateSeatLayout()
        {
            panelRoomLayout.Controls.Clear();
            CreateScreenBar();

            int panelW = panelRoomLayout.Width;

            int baseSeatW = 50;
            int baseSeatH = 50;
            int baseSpaceX = 8;
            int baseSpaceY = 10;

            int maxSeats = seatMap.Max(r => r.Value);
            int wantedWidth = (maxSeats * baseSeatW) + ((maxSeats - 1) * baseSpaceX);

            float scale = (float)(panelW - 40) / wantedWidth;
            if (scale > 1) scale = 1;

            int seatW = (int)(baseSeatW * scale);
            int seatH = (int)(baseSeatH * scale);
            int spaceX = (int)(baseSpaceX * scale);
            int spaceY = baseSpaceY;

            int startY = 90;

            foreach (var row in seatMap)
            {
                string rowName = row.Key;
                int count = row.Value;

                int rowWidth = (count * seatW) + ((count - 1) * spaceX);
                int startX = (panelW - rowWidth) / 2;

                for (int col = 1; col <= count; col++)
                {
                    string displayId = $"{rowName}{col:00}"; // A01, A02...

                    var seat = new SeatData
                    {
                        SeatId = displayId,
                        Row = rowName,
                        Col = col,
                        Type = (rowName == "A" || rowName == "B" || rowName == "C") ? "Thường" : "VIP",
                        Status = "Bình thường",
                        X = startX,
                        Y = startY
                    };

                    var btn = CreateSeat(seat);
                    btn.Width = seatW;
                    btn.Height = seatH;

                    startX += seatW + spaceX;
                }

                startY += seatH + spaceY;
            }
        }

        // ==========================================
        //  HỖ TRỢ: CẬP NHẬT SỐ GHẾ UI
        // ==========================================
        private void UpdateSeatCountUI()
        {
            int count = panelRoomLayout.Controls
                         .OfType<Guna2Button>()
                         .Count(b => b.Tag is SeatData);
            txtSoGhe.Text = count.ToString();
        }

        // ==========================================
        //  THÊM / XOÁ HÀNG / GHẾ
        // ==========================================
        private void btnThemHang_Click(object sender, EventArgs e)
        {
            char last = seatMap.Keys.Last()[0];
            char next = (char)(last + 1);
            seatMap.Add(next.ToString(), 15);

            GenerateSeatLayout();
            UpdateSeatCountUI();
        }

        private void btnThemGhe_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Hãy chọn 1 ghế trong hàng trước khi thêm!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var firstBtn = selectedSeats[0];
            var seat = (SeatData)firstBtn.Tag;
            string rowName = seat.Row;

            int currentCount = seatMap[rowName];
            int newCol = currentCount + 1;
            seatMap[rowName] = newCol;

            int lastX = 0;
            int lastY = seat.Y;

            foreach (Control c in panelRoomLayout.Controls)
            {
                if (c is Guna2Button btn)
                {
                    var s = (SeatData)btn.Tag;
                    if (s.Row == rowName && s.Col == currentCount)
                    {
                        lastX = btn.Left;
                        lastY = btn.Top;
                        break;
                    }
                }
            }

            int seatW = firstBtn.Width;
            int seatH = firstBtn.Height;
            int spaceX = 8;

            string displayId = $"{rowName}{newCol:00}";

            var newSeat = new SeatData
            {
                SeatId = displayId,
                Row = rowName,
                Col = newCol,
                Type = seat.Type,          // Thường / VIP
                Status = "Bình thường",
                X = lastX + seatW + spaceX,
                Y = lastY
            };

            CreateSeat(newSeat);
            UpdateSeatCountUI();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Hãy chọn ít nhất 1 ghế!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa {selectedSeats.Count} ghế đã chọn?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.No)
                return;

            foreach (var btn in selectedSeats)
            {
                panelRoomLayout.Controls.Remove(btn);
                btn.Dispose();
            }

            selectedSeats.Clear();
            UpdateSeatCountUI();
        }

        private void btnXoaHang_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Hãy chọn ít nhất 1 ghế!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var firstBtn = selectedSeats[0];
            var seat = (SeatData)firstBtn.Tag;
            string rowName = seat.Row;

            if (MessageBox.Show($"Xóa toàn bộ hàng {rowName}?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.No)
                return;

            var seatsToRemove = panelRoomLayout.Controls
                .OfType<Guna2Button>()
                .Where(b => ((SeatData)b.Tag).Row == rowName)
                .ToList();

            foreach (var btn in seatsToRemove)
            {
                panelRoomLayout.Controls.Remove(btn);
                btn.Dispose();
            }

            if (seatMap.ContainsKey(rowName))
                seatMap.Remove(rowName);

            selectedSeats.Clear();
            UpdateSeatCountUI();
        }

        // ==========================================
        //  LƯU → JSON + SEAT + SEAT_FOR_SHOWTIME + AUDITORIUM
        // ==========================================
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string auditoriumId = $"R0{currentRoom}";

            // 1) Thu ghế từ UI
            var seats = new List<SeatData>();

            foreach (Control c in panelRoomLayout.Controls)
            {
                if (c is Guna2Button btn && btn.Tag is SeatData seat)
                {
                    seat.X = btn.Left;
                    seat.Y = btn.Top;

                    // Chuẩn hóa Type cho DB + JSON
                    string t = (seat.Type ?? "").Trim().ToLower();
                    if (t == "vip" || t == "st02")
                        seat.Type = "VIP";
                    else
                        seat.Type = "Thường";

                    // Chuẩn hóa Status cho DB
                    string st = (seat.Status ?? "").Trim().ToLower();
                    if (st == "bảo trì" || st == "bao tri")
                        seat.Status = "Bảo trì";
                    else
                        seat.Status = "Bình thường";

                    seats.Add(seat);
                }
            }

            // 2) Lưu JSON
            Directory.CreateDirectory(roomDesignFolder);
            string file = Path.Combine(roomDesignFolder, $"Room_{currentRoom}.json");
            File.WriteAllText(file, JsonConvert.SerializeObject(seats, Formatting.Indented));

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    // ----- 3. Seat: xoá ghế cũ -----
                    var delSeat = conn.CreateCommand();
                    delSeat.Transaction = tran;
                    delSeat.CommandText = "DELETE FROM Seat WHERE auditorium_id = $room";
                    delSeat.Parameters.AddWithValue("$room", auditoriumId);
                    delSeat.ExecuteNonQuery();

                    // ----- 3b. Insert ghế mới -----
                    foreach (var s in seats)
                    {
                        string dbSeatId = $"{s.SeatId}{auditoriumId}";   // A01R01
                        string seatTypeId = s.Type == "VIP" ? "ST02" : "ST01";
                        int price = s.Type == "VIP" ? 90000 : 70000;

                        var cmd = conn.CreateCommand();
                        cmd.Transaction = tran;
                        cmd.CommandText = @"
INSERT INTO Seat(seat_id, seat_type_id, auditorium_id, location, status, per_seat_ticket_price)
VALUES ($id, $type, $room, $loc, $status, $price);
";
                        cmd.Parameters.AddWithValue("$id", dbSeatId);
                        cmd.Parameters.AddWithValue("$type", seatTypeId);   // ST01 / ST02
                        cmd.Parameters.AddWithValue("$room", auditoriumId);
                        cmd.Parameters.AddWithValue("$loc", s.SeatId);      // A01
                        cmd.Parameters.AddWithValue("$status", s.Status);   // Bình thường / Bảo trì
                        cmd.Parameters.AddWithValue("$price", price);

                        cmd.ExecuteNonQuery();
                    }

                    // ----- 4. Cập nhật số ghế trong auditorium -----
                    var updateAud = conn.CreateCommand();
                    updateAud.Transaction = tran;
                    updateAud.CommandText = @"
UPDATE auditorium
SET number_of_seats = $count
WHERE auditorium_id = $id;
";
                    updateAud.Parameters.AddWithValue("$count", seats.Count);
                    updateAud.Parameters.AddWithValue("$id", auditoriumId);
                    updateAud.ExecuteNonQuery();

                    // ----- 5. Đồng bộ seat_for_showtime -----
                    var stCmd = conn.CreateCommand();
                    stCmd.Transaction = tran;
                    stCmd.CommandText = "SELECT showtime_id FROM showtime WHERE auditorium_id = $room";
                    stCmd.Parameters.AddWithValue("$room", auditoriumId);

                    var showtimes = new List<string>();
                    using (var r = stCmd.ExecuteReader())
                    {
                        while (r.Read())
                            showtimes.Add(r.GetString(0));
                    }

                    foreach (var showId in showtimes)
                    {
                        foreach (var s in seats)
                        {
                            string dbSeatId = $"{s.SeatId}{auditoriumId}";

                            if (s.Status == "Bảo trì")
                            {
                                // Bắt buộc = Bảo trì
                                var up = conn.CreateCommand();
                                up.Transaction = tran;
                                up.CommandText = @"
UPDATE seat_for_showtime
SET status = 'Bảo trì'
WHERE seat_id = $sid AND showtime_id = $stid;
";
                                up.Parameters.AddWithValue("$sid", dbSeatId);
                                up.Parameters.AddWithValue("$stid", showId);

                                int rows = up.ExecuteNonQuery();

                                if (rows == 0)
                                {
                                    var ins = conn.CreateCommand();
                                    ins.Transaction = tran;
                                    ins.CommandText = @"
INSERT INTO seat_for_showtime(seat_id, showtime_id, status)
VALUES ($sid, $stid, 'Bảo trì');
";
                                    ins.Parameters.AddWithValue("$sid", dbSeatId);
                                    ins.Parameters.AddWithValue("$stid", showId);
                                    ins.ExecuteNonQuery();
                                }
                            }
                            else // Bình thường
                            {
                                // Nếu trước đó Bảo trì -> reset về Trống
                                var up = conn.CreateCommand();
                                up.Transaction = tran;
                                up.CommandText = @"
UPDATE seat_for_showtime
SET status = 'Trống'
WHERE seat_id = $sid AND showtime_id = $stid AND status = 'Bảo trì';
";
                                up.Parameters.AddWithValue("$sid", dbSeatId);
                                up.Parameters.AddWithValue("$stid", showId);
                                up.ExecuteNonQuery();

                                // Tạo mới nếu chưa có, mặc định Trống
                                var ins = conn.CreateCommand();
                                ins.Transaction = tran;
                                ins.CommandText = @"
INSERT OR IGNORE INTO seat_for_showtime(seat_id, showtime_id, status)
VALUES ($sid, $stid, 'Trống');
";
                                ins.Parameters.AddWithValue("$sid", dbSeatId);
                                ins.Parameters.AddWithValue("$stid", showId);
                                ins.ExecuteNonQuery();
                            }
                        }
                    }

                    tran.Commit();
                }
            }

            MessageBox.Show(
                $"Đã lưu sơ đồ phòng {currentRoom}!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ==========================================
        //  NÚT KHÁC
        // ==========================================
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FormatSeatPositions();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Chọn ít nhất 1 ghế để chỉnh sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetEditMode(true);

            if (selectedSeats.Count == 1)
            {
                var seat = (SeatData)selectedSeats[0].Tag;

                txtMaGhe.Text = seat.SeatId;
                rdoVip.Checked = seat.Type == "VIP";
                rdoThuong.Checked = seat.Type == "Thường";
                rdoBaoTri.Checked = seat.Status == "Bảo trì";
                rdoBinhThuong.Checked = seat.Status == "Bình thường";
            }
            else
            {
                txtMaGhe.Text = "";
            }
        }

        private void txtMaGhe_TextChanged(object sender, EventArgs e)
        {
            if (!editMode || selectedSeats.Count != 1)
                return;

            var btn = selectedSeats[0];
            var seat = (SeatData)btn.Tag;

            seat.SeatId = txtMaGhe.Text;
            btn.Text = seat.SeatId;
        }

        private void rdoVip_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || !rdoVip.Checked || selectedSeats.Count == 0)
                return;

            foreach (var btn in selectedSeats)
            {
                var seat = (SeatData)btn.Tag;
                seat.Type = "VIP";
                ApplySeatStyle(btn, seat);
            }
        }

        private void rdoThuong_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || !rdoThuong.Checked || selectedSeats.Count == 0)
                return;

            foreach (var btn in selectedSeats)
            {
                var seat = (SeatData)btn.Tag;
                seat.Type = "Thường";
                ApplySeatStyle(btn, seat);
            }
        }

        private void rdoBinhThuong_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || !rdoBinhThuong.Checked || selectedSeats.Count == 0)
                return;

            foreach (var btn in selectedSeats)
            {
                var seat = (SeatData)btn.Tag;
                seat.Status = "Bình thường";
                ApplySeatStyle(btn, seat);
            }
        }

        private void rdoBaoTri_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || !rdoBaoTri.Checked || selectedSeats.Count == 0)
                return;

            foreach (var btn in selectedSeats)
            {
                var seat = (SeatData)btn.Tag;
                seat.Status = "Bảo trì";
                ApplySeatStyle(btn, seat);
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            LoadRoom(currentRoom);
            MessageBox.Show("Đã khôi phục về ban đầu!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ==========================================
        //  CHỌN PHÒNG
        // ==========================================
        private void btnPhong1_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong1.Checked) LoadRoom(1);
        }

        private void btnPhong2_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong2.Checked) LoadRoom(2);
        }

        private void btnPhong3_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong3.Checked) LoadRoom(3);
        }

        private void btnPhong4_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong4.Checked) LoadRoom(4);
        }

        private void btnPhong5_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong5.Checked) LoadRoom(5);
        }
    }
}
