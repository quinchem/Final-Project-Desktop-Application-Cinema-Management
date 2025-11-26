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
using SharedData;

namespace AdminApp
{
    public partial class FormRoomLayoutManagement : Form
    {
        private Guna2Button selectedSeat = null;
        public FormRoomLayoutManagement()
        {
            InitializeComponent();
        }

        private void FormRoomLayoutManagement_Load(object sender, EventArgs e)
        {
            GenerateSeatLayout();
        }

        // Danh sách số ghế mỗi hàng
        private Dictionary<string, int> seatMap = new Dictionary<string, int>
        {
            { "A", 15 },
            { "B", 15 },
            { "C", 15 },
            { "D", 15 },
            { "E", 15 },
            { "F", 15 }
        };

        bool dragging = false;
        Point dragCursorPoint;
        Point dragButtonPoint;

        //Tạo nút ghế dựa trên dữ liệu ghế
        private Guna2Button CreateSeat(SeatData seat)
        {
            var btn = new Guna2Button();
            btn.Text = seat.SeatId;
            btn.Size = new Size(55, 55);
            btn.Location = new Point(seat.X, seat.Y);
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            ApplySeatStyle(btn, seat);

            btn.Tag = seat;

            // CRUD events
            btn.MouseDown += Seat_MouseDown;
            btn.MouseMove += Seat_MouseMove;
            btn.MouseUp += Seat_MouseUp;
            btn.DoubleClick += Seat_DoubleClick;
            btn.KeyDown += Seat_KeyDown;
            btn.TabStop = true;

            btn.ContextMenuStrip = cmsSeat;   // menu chuột phải

            panelRoomLayout.Controls.Add(btn);
            return btn;
        }
        //Áp dụng kiểu dáng cho ghế dựa trên loại ghế
        private void ApplySeatStyle(Guna2Button btn, SeatData seat)
        {
            btn.AutoRoundedCorners = false;
            btn.AutoSize = false;

            btn.BorderRadius = 4;
            btn.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            btn.TextAlign = HorizontalAlignment.Center;

            btn.ForeColor = Color.FromArgb(30, 30, 30);
            btn.ShadowDecoration.Enabled = false;

            // style theo loại ghế
            if (seat.Type == "VIP")
            {
                btn.FillColor = Color.White;
                btn.BorderColor = Color.FromArgb(255, 193, 7);
                btn.BorderThickness = 3;

                btn.HoverState.FillColor = Color.FromArgb(240, 255, 240);
                btn.HoverState.BorderColor = Color.LimeGreen;
            }
            else if (seat.Type == "Disabled")
            {
                btn.FillColor = Color.Gray;
                btn.BorderThickness = 0;
                btn.ForeColor = Color.White;
            }
            else // NORMAL
            {
                btn.FillColor = Color.White;
                btn.BorderColor = Color.Silver;
                btn.BorderThickness = 1;

                btn.HoverState.FillColor = Color.FromArgb(235, 243, 255);
                btn.HoverState.BorderColor = Color.Green;
            }

            // KHÔNG cho Guna tự scale text
            btn.TextFormatNoPrefix = true;
            btn.TextOffset = new Point(0, 0);
        }
       
        //Tạo bố cục sơ đồ ghế
        private void GenerateSeatLayout()
        {
            panelRoomLayout.Controls.Clear();

            int seatWidth = 70;
            int seatHeight = 55;
            int spacingX = 12;
            int spacingY = 15;

            int panelW = panelRoomLayout.Width;
            int panelH = panelRoomLayout.Height;

            int maxSeats = seatMap.Max(r => r.Value);
            int totalRows = seatMap.Count;

            // kích thước layout gốc
            int layoutWidth = maxSeats * (seatWidth + spacingX);
            int layoutHeight = totalRows * (seatHeight + spacingY);

            // scale để vừa panel
            float scaleX = (float)panelW / layoutWidth;
            float scaleY = (float)panelH / layoutHeight;
            float scale = Math.Min(scaleX, scaleY);

            // kích thước mới sau scale
            int W = (int)(seatWidth * scale);
            int H = (int)(seatHeight * scale);
            int SX = (int)(spacingX * scale);
            int SY = (int)(spacingY * scale);

            // canh giữa layout
            int totalWidth = maxSeats * (W + SX);
            int startX = (panelW - totalWidth) / 2;

            int totalHeight = totalRows * (H + SY);
            int startY = (panelH - totalHeight) / 2;

            foreach (var row in seatMap)
            {
                string rowName = row.Key;
                int seatCount = row.Value;

                int rowStartX = startX;

                for (int col = 1; col <= seatCount; col++)
                {
                    var seat = new SeatData
                    {
                        SeatId = $"{rowName}{col}",
                        Row = rowName,
                        Col = col,
                        Type = (rowName == "A" || rowName == "B" || rowName == "C") ? "Normal" : "VIP",
                        X = rowStartX,
                        Y = startY
                    };

                    var btn = CreateSeat(seat);
                    btn.Width = W;
                    btn.Height = H;

                    rowStartX += W + SX;
                }

                startY += H + SY;
            }
        }
        

        private void Seat_MouseDown(object sender, MouseEventArgs e)
        {
            selectedSeat = (Guna2Button)sender;

            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragButtonPoint = selectedSeat.Location;
                selectedSeat.Focus();
            }
        }
        private void Seat_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;

            var diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            selectedSeat.Location = Point.Add(dragButtonPoint, new Size(diff));
        }
        private void Seat_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;

            var seat = (SeatData)selectedSeat.Tag;
            seat.X = selectedSeat.Location.X;
            seat.Y = selectedSeat.Location.Y;
            selectedSeat.Tag = seat;
        }

        private void Seat_DoubleClick(object sender, EventArgs e)
        {
            var seat = (SeatData)selectedSeat.Tag;

            string newId = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập mã ghế mới:",
                "Chỉnh sửa ghế",
                seat.SeatId);

            if (!string.IsNullOrWhiteSpace(newId))
            {
                seat.SeatId = newId;
                selectedSeat.Text = newId;
                selectedSeat.Tag = seat;
            }
        }

        private void Seat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                panelRoomLayout.Controls.Remove(selectedSeat);
                selectedSeat.Dispose();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var list = new List<SeatData>();

            foreach (Control c in panelRoomLayout.Controls)
            {
                if (c is Guna2Button btn)
                {
                    var seat = (SeatData)btn.Tag;
                    seat.X = btn.Location.X;
                    seat.Y = btn.Location.Y;
                    list.Add(seat);
                }
            }

            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText("Room_1.json", json);

            MessageBox.Show("Đã lưu layout!");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("Room_1.json"))
            {
                MessageBox.Show("Chưa có layout để load!");
                return;
            }

            var json = File.ReadAllText("Room_1.json");
            var list = JsonConvert.DeserializeObject<List<SeatData>>(json);

            panelRoomLayout.Controls.Clear();

            foreach (var seat in list)
                CreateSeat(seat);
        }
    }
}
