using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;                 
using Guna.UI2.WinForms;
using SharedData.Models;

namespace AdminApp
{
    public partial class FormRoomLayoutManagement : Form
    {
        private int currentRoom = 1;
        private Dictionary<string, int> initialSeatMap;

        // GHẾ ĐANG DRAG
        private Guna2Button draggingSeat = null;

        // DRAG THÔNG SỐ
        bool dragging = false;
        Point dragCursorPoint, dragStartPoint;

        // DRAG MÀN HÌNH
        bool draggingScreen = false;
        Point screenDragStartPoint;


        //Chọn ghế
        private Guna2Button selectedSeat = null;
        private bool editMode = false;

        // DANH SÁCH GHẾ THEO HÀNG
        private Dictionary<string, int> seatMap = new Dictionary<string, int>
        {
            { "A", 15 },
            { "B", 15 },
            { "C", 15 },
            { "D", 15 },
            { "E", 15 },
            { "F", 15 }
        };

        // Folder lưu trữ file phòng
        private string localRoomFolder = @"D:\Desktop app\Do an cuoi ky\Cinema\CinemaData\Room";

        public FormRoomLayoutManagement()
        {
            InitializeComponent();
            initialSeatMap = seatMap.ToDictionary(x => x.Key, x => x.Value);
        }

        private void FormRoomLayoutManagement_Load(object sender, EventArgs e)
        {
            LoadRoom(1);
            btnPhong1.Checked = true;
            txtDinhDang.Text = "2D";
        }
        private void LoadRoom(int roomId)
        {
            currentRoom = roomId;

            string file = $"Room_{roomId}.json";

            // Clear panel trước khi load
            panelRoomLayout.Controls.Clear();

            // Tạo lại thanh MÀN HÌNH
            CreateScreenBar();

            if (File.Exists(file))
            {
                // Load JSON
                var json = File.ReadAllText(file);
                var list = JsonConvert.DeserializeObject<List<SeatData>>(json);

                foreach (var seat in list)
                {
                    // Những dữ liệu thiếu trong file (trường hợp file cũ)
                    if (seat.Status == null) seat.Status = "Active";
                    if (seat.Type == null) seat.Type = "Normal";

                    var btn = CreateSeat(seat);

                    // KÍCH THƯỚC GHẾ CHUẨN
                    btn.Width = 50;
                    btn.Height = 50;
                }
            }
            else
            {
                // Không có file -> tạo layout mặc định
                GenerateSeatLayout();
            }

            // Reset dữ liệu chọn ghế
            selectedSeat = null;
            txtMaGhe.Text = "";
            rdoThuong.Checked = true;
            rdoBinhThuong.Checked = true;

            // Tắt edit mode
            SetEditMode(false);
        }

        // ================= TẠO THANH MÀN HÌNH ================= //
        private void CreateScreenBar()
        {

            // Tạo panel màn hình
            Guna2Panel screen = new Guna2Panel();
            screen.Name = "screenBar";
            screen.FillColor = Color.WhiteSmoke;
            screen.BorderRadius = 0;
            screen.Height = 50;


            int width = panelRoomLayout.Width - 150;
            screen.Width = width;
            screen.Left = (panelRoomLayout.Width - width) / 2;
            screen.Top = 20;

            // Label chữ MÀN HÌNH
            Label lbl = new Label();
            lbl.Text = "MÀN HÌNH";
            lbl.Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(50, 50, 50);
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize = true;

            screen.Controls.Add(lbl);

            lbl.Left = (screen.Width - lbl.Width) / 2;
            lbl.Top = (screen.Height - lbl.Height) / 2;

            // SỰ KIỆN DRAG MÀN HÌNH
            screen.MouseDown += Screen_MouseDown;
            screen.MouseMove += Screen_MouseMove;
            screen.MouseUp += Screen_MouseUp;
            panelRoomLayout.Controls.Add(screen);
            screen.BringToFront();
        }

        // ================= DRAG PANEL MÀN HÌNH ================= //
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
            screen.FillColor = Color.WhiteSmoke; // màu bình thường sau khi thả
        }

        // ================= GHẾ: TẠO MỚI ================= //
        private Guna2Button CreateSeat(SeatData seat)
        {
            var btn = new Guna2Button();

            btn.Size = new Size(50, 50);
            btn.Location = new Point(seat.X, seat.Y);
            btn.Text = seat.SeatId;

            btn.Font = new Font("Segoe UI", 7, FontStyle.Bold);
            btn.Tag = seat;

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

        //CHỌN GHẾ
        private void Seat_Select(object sender, EventArgs e)
        {
            // reset ghế cũ
            if (selectedSeat != null)
            {
                var old = (SeatData)selectedSeat.Tag;
                ApplySeatStyle(selectedSeat, old);
            }

            // ghế mới
            selectedSeat = (Guna2Button)sender;
            var seat = (SeatData)selectedSeat.Tag;

            // highlight XANH LÁ
            selectedSeat.FillColor = Color.FromArgb(35, 150, 62);
            selectedSeat.ForeColor = Color.White;

            if (seat.Type == "VIP")
            {
                selectedSeat.BorderColor = Color.FromArgb(255, 193, 7);   // vàng VIP
                selectedSeat.BorderThickness = 4;
            }
            else
            {
                selectedSeat.BorderColor = Color.DimGray;
                selectedSeat.BorderThickness = 4;
            }


            // HIỂN THỊ THÔNG TIN
            txtMaGhe.Text = seat.SeatId;
            rdoVip.Checked = seat.Type == "VIP";
            rdoThuong.Checked = seat.Type == "Normal";
            rdoBaoTri.Checked = seat.Status == "Disabled";
            rdoBinhThuong.Checked = seat.Status == "Active";

            // TẮT chỉnh sửa
            SetEditMode(false);
        }
        //Hàm bật/tắt chỉnh sửa
        private void SetEditMode(bool enable)
        {
            editMode = enable;

            txtMaGhe.ReadOnly = !enable;
            rdoVip.Enabled = enable;
            rdoThuong.Enabled = enable;
            rdoBaoTri.Enabled = enable;
            rdoBinhThuong.Enabled = enable;
        }

        // ================= GHẾ: STYLE ================= //
        private void ApplySeatStyle(Guna2Button btn, SeatData seat)
        {
            btn.AutoRoundedCorners = false;
            btn.BorderRadius = 0;
            btn.ForeColor = Color.Black;

            // ======= STATUS ======= //
            if (seat.Status == "Disabled")  // GHẾ BẢO TRÌ
            {
                btn.FillColor = Color.DimGray;
                btn.ForeColor = Color.White;
                btn.BorderThickness = 0;
                return;
            }

            // ======= TYPE ======= //
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

        // ================= GHẾ: HOVER ================= //
        private void Seat_Hover(object sender, EventArgs e)
        {
            Guna2Button btn = (Guna2Button)sender;

            if (btn == selectedSeat)
                return;

            btn.FillColor = Color.FromArgb(167, 238, 250); // xanh dương nhạt
        }

        private void Seat_Unhover(object sender, EventArgs e)
        {
            Guna2Button btn = (Guna2Button)sender;

            if (btn == selectedSeat)
                return;

            var seat = (SeatData)btn.Tag;
            ApplySeatStyle(btn, seat); // trở lại style gốc
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

                // Lấy danh sách GHẾ THẬT đang có trên giao diện
                var rowSeats = panelRoomLayout.Controls
                                .OfType<Guna2Button>()
                                .Where(btn => ((SeatData)btn.Tag).Row == rowName)
                                .OrderBy(btn => ((SeatData)btn.Tag).Col)
                                .ToList();

                foreach (var btn in rowSeats)
                {
                    var seat = (SeatData)btn.Tag;

                    // UPDATE VỊ TRÍ BUTTON
                    btn.Width = seatW;
                    btn.Height = seatH;

                    btn.Left = startX;
                    btn.Top = startY;

                    // UPDATE lại seat.X seat.Y trong model
                    seat.X = btn.Left;
                    seat.Y = btn.Top;

                    startX += seatW + spaceX;
                }

                startY += seatH + spaceY;
            }
        }
        // ================= GHẾ: DRAG ================= //
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

        // ================= TẠO TOÀN BỘ LAYOUT ================= //
        private void GenerateSeatLayout()
        {
            panelRoomLayout.Controls.Clear();
            CreateScreenBar();

            // PANEL SIZE
            int panelW = panelRoomLayout.Width;
            int panelH = panelRoomLayout.Height;

            // Request GHẾ SIZE CƠ BẢN
            int baseSeatW = 50;
            int baseSeatH = 50;
            int baseSpaceX = 8;
            int baseSpaceY = 10;

            int maxSeats = seatMap.Max(r => r.Value);

            // ======= SCALE GHẾ THEO PANEL =======
            // Tính tổng chiều rộng dự kiến nếu không scale
            int wantedWidth = (maxSeats * baseSeatW) + ((maxSeats - 1) * baseSpaceX);

            // Tính scale để ghế lọt vừa panel
            float scale = (float)(panelW - 40) / wantedWidth;
            if (scale > 1) scale = 1; // không phóng to, chỉ thu nhỏ

            // SCALE GHẾ
            int seatW = (int)(baseSeatW * scale);
            int seatH = (int)(baseSeatH * scale);
            int spaceX = (int)(baseSpaceX * scale);
            int spaceY = baseSpaceY;

            int startY = 90;

            foreach (var row in seatMap)
            {
                string rowName = row.Key;
                int count = row.Value;

                // Tổng chiều rộng HÀNG sau scale
                int rowWidth = (count * seatW) + ((count - 1) * spaceX);

                // Căn giữa panel
                int startX = (panelW - rowWidth) / 2;

                for (int col = 1; col <= count; col++)
                {
                    SeatData seat = new SeatData
                    {
                        SeatId = $"{rowName}{col}",
                        Row = rowName,
                        Col = col,
                        Type = (rowName == "A" || rowName == "B" || rowName == "C") ? "Normal" : "VIP",
                        Status = "Active",
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

        private void btnThemHang_Click(object sender, EventArgs e)
        {
            char last = seatMap.Keys.Last()[0];
            char next = (char)(last + 1);
            seatMap.Add(next.ToString(), 15);

            GenerateSeatLayout();
        }

        private void btnThemGhe_Click(object sender, EventArgs e)
        {
            if (selectedSeat == null)
            {
                MessageBox.Show("Hãy chọn 1 ghế trong hàng trước khi thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seat = (SeatData)selectedSeat.Tag;
            string rowName = seat.Row;

            // Tìm số ghế hiện tại trong hàng
            int currentCount = seatMap[rowName];

            // Tạo seat mới đứng sau ghế cuối
            int newCol = currentCount + 1;
            seatMap[rowName] = newCol; // tăng số ghế trong hàng đó

            // Tính vị trí ghế mới theo ghế cuối cùng
            int seatW = selectedSeat.Width;
            int seatH = selectedSeat.Height;
            int spaceX = 8;

            // Lấy vị trí ghế cuối cùng trong hàng
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

            // Tạo seat mới
            SeatData newSeat = new SeatData
            {
                SeatId = $"{rowName}{newCol}",
                Row = rowName,
                Col = newCol,
                Type = seat.Type,   // mặc định theo loại ghế hiện tại
                X = lastX + seatW + spaceX,
                Y = lastY
            };

            CreateSeat(newSeat);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedSeat == null) return;

            panelRoomLayout.Controls.Remove(selectedSeat);
            selectedSeat.Dispose();

            selectedSeat = null;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var list = new List<SeatData>();

            foreach (Control c in panelRoomLayout.Controls)
            {
                if (c is Guna2Button btn && btn.Tag is SeatData seat)
                {
                    // GHẾ: cập nhật toạ độ mới sau khi kéo
                    seat.X = btn.Left;
                    seat.Y = btn.Top;

                    // Nếu seat.Status null → gán Active
                    if (string.IsNullOrEmpty(seat.Status))
                        seat.Status = "Active";

                    // Nếu seat.Type null → gán Normal
                    if (string.IsNullOrEmpty(seat.Type))
                        seat.Type = "Normal";

                    list.Add(seat);
                }
            }

            // ====== LƯU ĐÚNG FILE THEO PHÒNG ======
            // Tạo file path theo phòng
            string filePath = Path.Combine(localRoomFolder, $"Room_{currentRoom}.json");

            // Tự tạo folder nếu chưa có
            Directory.CreateDirectory(localRoomFolder);

            File.WriteAllText(filePath, JsonConvert.SerializeObject(list, Formatting.Indented));

            MessageBox.Show(
                $"Đã lưu sơ đồ phòng {currentRoom}!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FormatSeatPositions();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (selectedSeat == null) return;
            SetEditMode(true);
        }

        private void txtMaGhe_TextChanged(object sender, EventArgs e)
        {
            if (!editMode || selectedSeat == null) return;

            var seat = (SeatData)selectedSeat.Tag;
            seat.SeatId = txtMaGhe.Text;
            selectedSeat.Text = seat.SeatId;
        }

        private void rdoVip_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || selectedSeat == null || !rdoVip.Checked) return;
            var seat = (SeatData)selectedSeat.Tag;
            seat.Type = "VIP";
            ApplySeatStyle(selectedSeat, seat);
        }

        private void rdoThuong_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || selectedSeat == null || !rdoThuong.Checked) return;
            var seat = (SeatData)selectedSeat.Tag;
            seat.Type = "Normal";
            ApplySeatStyle(selectedSeat, seat);
        }

        private void rdoBinhThuong_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || selectedSeat == null || !rdoBinhThuong.Checked) return;
            var seat = (SeatData)selectedSeat.Tag;
            seat.Status = "Active";
            ApplySeatStyle(selectedSeat, seat);
        }

        private void rdoBaoTri_CheckedChanged(object sender, EventArgs e)
        {
            if (!editMode || selectedSeat == null || !rdoBaoTri.Checked) return;
            var seat = (SeatData)selectedSeat.Tag;
            seat.Status = "Disabled";
            ApplySeatStyle(selectedSeat, seat);
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            // Reset lại seatMap từ bản gốc
            seatMap = initialSeatMap.ToDictionary(entry => entry.Key, entry => entry.Value);

            // Xóa ghế đang chọn
            selectedSeat = null;

            // Tắt chế độ edit
            SetEditMode(false);

            // Reset textbox + radio
            txtMaGhe.Text = "";
            rdoThuong.Checked = true;
            rdoBinhThuong.Checked = true;

            // Tạo lại layout ban đầu
            GenerateSeatLayout();
        }

        private void btnXoaHang_Click(object sender, EventArgs e)
        {

            if (selectedSeat == null)
            {
                MessageBox.Show("Hãy chọn 1 ghế trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seat = (SeatData)selectedSeat.Tag;
            string rowName = seat.Row;

            // XÁC NHẬN
            if (MessageBox.Show($"Xóa toàn bộ hàng {rowName}?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.No)
                return;

            // XÓA GHẾ TRÊN GIAO DIỆN
            var seatsToRemove = panelRoomLayout.Controls
                .OfType<Guna2Button>()
                .Where(btn => ((SeatData)btn.Tag).Row == rowName)
                .ToList();

            foreach (var btn in seatsToRemove)
            {
                panelRoomLayout.Controls.Remove(btn);
                btn.Dispose();
            }

            // XÓA KHỎI seatMap
            if (seatMap.ContainsKey(rowName))
                seatMap.Remove(rowName);

            // TẮT chọn ghế
            selectedSeat = null;
        }


        private void btnPhong1_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong1.Checked)
            { LoadRoom(1); txtDinhDang.Text = "2D"; }
        }
        private void btnPhong2_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong2.Checked)
            { LoadRoom(2); txtDinhDang.Text = "2D"; }
        }
        private void btnPhong3_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong3.Checked)
            { LoadRoom(3); txtDinhDang.Text = "2D"; }
        }
        private void btnPhong4_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong4.Checked)
            { LoadRoom(4); txtDinhDang.Text = "2D"; }
        }
        private void btnPhong5_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPhong5.Checked)
            { LoadRoom(5); txtDinhDang.Text = "3D"; }
        }

    }

}
