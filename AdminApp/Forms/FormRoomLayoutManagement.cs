using Guna.UI2.WinForms;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormRoomLayoutManagement : Form
    {
        // ============= CẤU HÌNH =============

        // Folder RoomDesign đặt trong solution:  <SolutionRoot>\SharedData\RoomDesign
        private readonly string roomDesignFolder;

        private Dictionary<string, int> initialSeatMap;

        // GHẾ ĐANG DRAG
        private Guna2Button draggingSeat = null;
        private bool dragging = false;
        private Point dragCursorPoint, dragStartPoint;
        private int currentRoom = 1;

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

        // Form load
        private void FormRoomLayoutManagement_Load(object sender, EventArgs e)
        {
            LoadRoom(1);
            if (btnPhong1 != null) btnPhong1.Checked = true;
        }

        // Load room
        private void LoadRoom(int roomIndex)
        {
            currentRoom = roomIndex;
            string auditoriumId = $"R0{roomIndex}";

            int seatCount = 0;
            string auditoriumTypeName = "";

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // 1. Đọc thông tin phòng từ bảng auditorium và auditorium_type 
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
                        auditoriumTypeName = reader.IsDBNull(0) ? "" : reader.GetString(0); 
                        seatCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);           
                    }
                }

                txtSoGhe.Text = seatCount.ToString();
                txtDinhDang.Text = auditoriumTypeName;

                // 2. Load JSON layout ghế
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
                        string t = (seat.Type ?? "").Trim().ToLower();
                        if (t == "vip" || t == "st02")
                            seat.Type = "VIP";
                        else
                            seat.Type = "Thường";

                        string st = (seat.Status ?? "").Trim().ToLower();
                        if (st == "bảo trì" || st == "bao tri")
                            seat.Status = "Bảo trì";
                        else
                            seat.Status = "Bình thường";

                        var btn = CreateSeat(seat);
                        btn.Width = 60;
                        btn.Height = 60;
                    }
                }
                else
                {
                    GenerateSeatLayout();
                }

                UpdateSeatCountUI();
            }
        }

        // 
        //  MÀN HÌNH
        // 
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

        private Guna2Button CreateSeat(SeatData seat)
        {
            var btn = new Guna2Button
            {
                Size = new Size(60, 60),
                Location = new Point(seat.X, seat.Y),
                Text = $"{seat.Row}{seat.Col:00}",
                // A01, A02,...
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
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
            int baseSeatW = 60;
            int baseSeatH = 60;
            int baseSpaceX = 10;
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

        // 
        //  TẠO LAYOUT TỰ ĐỘNG
        // 
        private void GenerateSeatLayout()
        {
            panelRoomLayout.Controls.Clear();
            CreateScreenBar();
            int panelW = panelRoomLayout.Width;
            int baseSeatW = 60;
            int baseSeatH = 60;
            int baseSpaceX = 12;
            int baseSpaceY = 12;
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
                    string displayId = $"{rowName}{col:00}";
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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
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

                    // Chuẩn hoá type
                    seat.Type = seat.Type.Trim().ToLower() == "vip" ? "VIP" : "Thường";

                    // Chuẩn hoá status (bảo trì / bình thường)
                    seat.Status = seat.Status.Trim().ToLower() == "bảo trì" ? "Bảo trì" : "Bình thường";

                    // Chuẩn SeatId A01
                    seat.SeatId = $"{seat.Row}{seat.Col:00}";

                    seats.Add(seat);
                }
            }

            // 2) Lưu JSON
            Directory.CreateDirectory(roomDesignFolder);
            string jsonPath = Path.Combine(roomDesignFolder, $"Room_{currentRoom}.json");
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(seats, Formatting.Indented));

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // 3) XÓA GHẾ CŨ TRONG DB (để insert mới)
                    var delSeat = conn.CreateCommand();
                    delSeat.Transaction = tran;
                    delSeat.CommandText = "DELETE FROM seat WHERE auditorium_id = $aud";
                    delSeat.Parameters.AddWithValue("$aud", auditoriumId);
                    delSeat.ExecuteNonQuery();

                    // 4) XÓA seat_for_showtime của ghế bị xoá (chỉ xóa những seat FOR showtime thuộc phòng này)
                    var delSfs = conn.CreateCommand();
                    delSfs.Transaction = tran;
                    delSfs.CommandText =
                        @"DELETE FROM seat_for_showtime WHERE seat_id LIKE '%' || $aud";
                    delSfs.Parameters.AddWithValue("$aud", auditoriumId);
                    delSfs.ExecuteNonQuery();

                    // 5) Insert GHẾ MỚI
                    foreach (var s in seats)
                    {
                        string logicalId = $"{s.Row}{s.Col:00}";       // A01
                        string dbSeatId = $"{logicalId}{auditoriumId}"; // A01R01

                        string typeId = s.Type == "VIP" ? "ST02" : "ST01";
                        int price = s.Type == "VIP" ? 90000 : 70000;

                        var cmd = conn.CreateCommand();
                        cmd.Transaction = tran;
                        cmd.CommandText = @"
                    INSERT INTO seat(seat_id, seat_type_id, auditorium_id, location, status, per_seat_ticket_price)
                    VALUES ($id, $type, $aud, $loc, $status, $price)
                ";

                        cmd.Parameters.AddWithValue("$id", dbSeatId);
                        cmd.Parameters.AddWithValue("$type", typeId);
                        cmd.Parameters.AddWithValue("$aud", auditoriumId);
                        cmd.Parameters.AddWithValue("$loc", logicalId);
                        cmd.Parameters.AddWithValue("$status", s.Status); // Bình thường / Bảo trì
                        cmd.Parameters.AddWithValue("$price", price);

                        cmd.ExecuteNonQuery();
                    }

                    // 6) Update số ghế của phòng
                    var updateAud = conn.CreateCommand();
                    updateAud.Transaction = tran;
                    updateAud.CommandText = @"
                UPDATE auditorium 
                SET number_of_seats = $count 
                WHERE auditorium_id = $aud
            ";
                    updateAud.Parameters.AddWithValue("$count", seats.Count);
                    updateAud.Parameters.AddWithValue("$aud", auditoriumId);
                    updateAud.ExecuteNonQuery();

                    tran.Commit();
                }
            }

            SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
            player.Play();
            MessageBox.Show($"Đã lưu sơ đồ phòng {currentRoom}!", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            string input = txtMaGhe.Text.Trim().ToUpper();

            // Tách Row (A,B,C...) và số ghế
            if (input.Length >= 2)
            {
                string row = new string(input.TakeWhile(char.IsLetter).ToArray());
                string numStr = new string(input.SkipWhile(char.IsLetter).ToArray());

                if (int.TryParse(numStr, out int num))
                {
                    input = $"{row}{num:00}";   // Format thành A01
                }
            }

            seat.SeatId = input;
            btn.Text = input;
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

            string auditoriumId = $"R0{currentRoom}";

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // Lấy danh sách showtime thuộc phòng
                    var stCmd = conn.CreateCommand();
                    stCmd.Transaction = tran;
                    stCmd.CommandText = "SELECT showtime_id FROM showtime WHERE auditorium_id = $room";
                    stCmd.Parameters.AddWithValue("$room", auditoriumId);

                    List<string> showtimes = new();
                    using (var r = stCmd.ExecuteReader())
                        while (r.Read()) showtimes.Add(r.GetString(0));

                    foreach (var btn in selectedSeats)
                    {
                        var seat = (SeatData)btn.Tag;
                        seat.Status = "Bình thường";
                        ApplySeatStyle(btn, seat);

                        string seatId = $"{seat.SeatId}{auditoriumId}";

                        // Update bảng Seat
                        var upSeat = conn.CreateCommand();
                        upSeat.Transaction = tran;
                        upSeat.CommandText =
                            @"UPDATE seat SET status = 'Bình thường' WHERE seat_id = $id";
                        upSeat.Parameters.AddWithValue("$id", seatId);
                        upSeat.ExecuteNonQuery();

                        // GHẾ thường → seat_for_showtime = Trống
                        foreach (var show in showtimes)
                        {
                            var upSfs = conn.CreateCommand();
                            upSfs.Transaction = tran;
                            upSfs.CommandText =
                            @"
                            INSERT INTO seat_for_showtime(seat_id, showtime_id, status)
                            VALUES ($sid, $stid, 'Trống')
                            ON CONFLICT(seat_id, showtime_id)
                            DO UPDATE SET status = 'Trống';
                            ";
                            upSfs.Parameters.AddWithValue("$sid", seatId);
                            upSfs.Parameters.AddWithValue("$stid", show);
                            upSfs.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                }
            }
        }

        private void rdoBaoTri_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || !rdoBaoTri.Checked || selectedSeats.Count == 0)
                return;

            string auditoriumId = $"R0{currentRoom}";

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // Lấy danh sách showtime thuộc phòng
                    var stCmd = conn.CreateCommand();
                    stCmd.Transaction = tran;
                    stCmd.CommandText = "SELECT showtime_id FROM showtime WHERE auditorium_id = $room";
                    stCmd.Parameters.AddWithValue("$room", auditoriumId);

                    List<string> showtimes = new();
                    using (var r = stCmd.ExecuteReader())
                        while (r.Read()) showtimes.Add(r.GetString(0));

                    foreach (var btn in selectedSeats)
                    {
                        var seat = (SeatData)btn.Tag;
                        seat.Status = "Bảo trì";
                        ApplySeatStyle(btn, seat);

                        string seatId = $"{seat.SeatId}{auditoriumId}";

                        // Update bảng Seat
                        var upSeat = conn.CreateCommand();
                        upSeat.Transaction = tran;
                        upSeat.CommandText =
                            @"UPDATE seat SET status = 'Bảo trì' WHERE seat_id = $id";
                        upSeat.Parameters.AddWithValue("$id", seatId);
                        upSeat.ExecuteNonQuery();

                        // Update seat_for_showtime = Bảo trì (overwrite)
                        foreach (var show in showtimes)
                        {
                            var upSfs = conn.CreateCommand();
                            upSfs.Transaction = tran;
                            upSfs.CommandText =
                            @"
                            INSERT INTO seat_for_showtime(seat_id, showtime_id, status)
                            VALUES ($sid, $stid, 'Bảo trì')
                            ON CONFLICT(seat_id, showtime_id)
                            DO UPDATE SET status = 'Bảo trì';
                            ";
                            upSfs.Parameters.AddWithValue("$sid", seatId);
                            upSfs.Parameters.AddWithValue("$stid", show);
                            upSfs.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                }
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            LoadRoom(currentRoom);
            SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
            player.Play();
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