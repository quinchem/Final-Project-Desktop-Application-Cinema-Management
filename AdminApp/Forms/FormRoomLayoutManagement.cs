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
        // Biến chứa đường dẫn thư mục lưu layout phòng (file JSON).
        // Thư mục đặt trong solution tại SharedData/RoomDesign.
        // Mục đích: mỗi phòng chiếu sẽ có 1 file JSON ghi lại tọa độ + thông tin ghế.
        private readonly string roomDesignFolder;

        // Lưu bản sao ban đầu của seatMap để so sánh hoặc khôi phục khi cần.
        private Dictionary<string, int> initialSeatMap;

        // Lưu ghế đang được kéo bằng chuột. Null nghĩa là không có ghế nào đang kéo.
        private Guna2Button draggingSeat = null;

        // Đánh dấu trạng thái đang kéo ghế. Dùng trong MouseMove để xác định có di chuyển ghế hay không.
        private bool dragging = false;

        // Lưu vị trí chuột ban đầu (khi bắt đầu kéo ghế).
        private Point dragCursorPoint;

        // Lưu vị trí ghế ban đầu trước khi kéo.
        private Point dragStartPoint;

        // Lưu số phòng hiện tại đang mở (phòng 1 → phòng 5).
        private int currentRoom = 1;

        // Đánh dấu trạng thái đang kéo thanh MÀN HÌNH (screen bar).
        private bool draggingScreen = false;

        // Lưu vị trí ban đầu của thanh MÀN HÌNH trước khi kéo.
        private Point screenDragStartPoint;

        // Cho biết hệ thống có đang ở chế độ chỉnh sửa ghế (edit mode) hay không.
        // Khi bật edit mode → người dùng có thể thay đổi mã ghế, loại ghế, trạng thái ghế.
        private bool editMode = false;

        // Cấu trúc lượng ghế theo từng hàng (logic).
        // Key = tên hàng (A–F), Value = số lượng ghế trong hàng.
        // Dùng để vẽ layout tự động và định dạng lại vị trí ghế.
        private Dictionary<string, int> seatMap = new Dictionary<string, int>
        {
            { "A", 15 },
            { "B", 15 },
            { "C", 15 },
            { "D", 15 },
            { "E", 15 },
            { "F", 15 }
        };

        // Danh sách các ghế đang được chọn ở UI để thao tác (đổi loại, đổi trạng thái, xóa, thêm).
        private readonly List<Guna2Button> selectedSeats = new List<Guna2Button>();

        // Constructor chính của form.
        // Khởi tạo layout ban đầu, sao chép seatMap để lưu cấu hình gốc
        // và xác định thư mục lưu file RoomDesign.
        public FormRoomLayoutManagement()
        {
            InitializeComponent();

            // Sao chép map ghế ban đầu để phục hồi khi cần.
            initialSeatMap = seatMap.ToDictionary(x => x.Key, x => x.Value);

            // Lấy đường dẫn đến thư mục RoomDesign (nằm trong solution).
            roomDesignFolder = GetRoomDesignFolder();
        }

        private string GetRoomDesignFolder()
        {
            // Lấy connection string từ DatabaseHelper.
            // Mục đích: dùng DataSource để suy ra thư mục gốc solution.
            var csb = new SqliteConnectionStringBuilder(DatabaseHelper.GetConnectionString());

            // dbPath là đường dẫn đến file CSDL, ví dụ: ...\SharedDatabase\Cinema.db
            string dbPath = csb.DataSource;

            // Lấy thư mục chứa database, ví dụ: ...\SharedDatabase
            string dbDir = Path.GetDirectoryName(dbPath);

            // solutionRoot là thư mục cha của SharedDatabase → thư mục gốc solution.
            string solutionRoot = Directory.GetParent(dbDir)?.FullName ?? dbDir;

            // Tạo đường dẫn đến thư mục RoomDesign trong solution.
            string folder = Path.Combine(solutionRoot, "SharedData", "RoomDesign");

            // Nếu thư mục chưa tồn tại → tự tạo.
            Directory.CreateDirectory(folder);

            return folder;
        }

        // Sự kiện load form — mặc định mở phòng 1 và set trạng thái nút Phòng 1.
        private void FormRoomLayoutManagement_Load(object sender, EventArgs e)
        {
            // Tải layout phòng 1 ngay khi mở form.
            LoadRoom(1);

            // Nếu nút phòng 1 tồn tại (để tránh null) thì set Checked = true.
            if (btnPhong1 != null) btnPhong1.Checked = true;
        }
                // Load thông tin một phòng chiếu dựa trên roomIndex (1 → 5).
        // Hàm này sẽ:
        // 1. Đọc thông tin phòng từ database (loại phòng + số ghế).
        // 2. Tải file JSON layout ghế (nếu có).
        // 3. Nếu chưa có file → tự sinh layout mặc định.
        // 4. Cập nhật UI và reset trạng thái chọn ghế.
        private void LoadRoom(int roomIndex)
        {
            currentRoom = roomIndex;
            string auditoriumId = $"R0{roomIndex}";  // Định danh phòng theo format R01, R02,...

            int seatCount = 0;               // Biến tạm để lưu số ghế đọc từ DB
            string auditoriumTypeName = "";  // Lưu kiểu phòng (2D, 3D, Deluxe,...)

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // Lệnh đọc thông tin phòng từ bảng auditorium + auditorium_type.
                // Mục đích: lấy số ghế + tên định dạng phòng.
                var cmdInfo = conn.CreateCommand();
                cmdInfo.CommandText = @"
                SELECT atype.auditorium_type, a.number_of_seats
                FROM auditorium a
                LEFT JOIN auditorium_type atype
                    ON a.auditorium_type_id = atype.auditorium_type_id
                WHERE a.auditorium_id = $id;
";
                cmdInfo.Parameters.AddWithValue("$id", auditoriumId);

                // Đọc kết quả từ database.
                using (var reader = cmdInfo.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Nếu dữ liệu không null → lấy giá trị.
                        auditoriumTypeName = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        seatCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    }
                }

                // Đẩy dữ liệu lên UI (loại phòng + số ghế).
                txtSoGhe.Text = seatCount.ToString();
                txtDinhDang.Text = auditoriumTypeName;

                // Chuẩn bị đường dẫn file JSON layout của phòng.
                Directory.CreateDirectory(roomDesignFolder);
                string jsonFile = Path.Combine(roomDesignFolder, $"Room_{roomIndex}.json");

                // Reset giao diện layout.
                panelRoomLayout.Controls.Clear();   // Xóa hết ghế cũ đang hiển thị
                CreateScreenBar();                  // Vẽ lại thanh “MÀN HÌNH”
                selectedSeats.Clear();              // Reset ghế đang chọn
                SetEditMode(false);                 // Tắt chế độ chỉnh sửa
                txtMaGhe.Text = "";                 // Xóa ô mã ghế

                // Nếu file JSON tồn tại → đọc layout từ file.
                if (File.Exists(jsonFile))
                {
                    // Đọc danh sách SeatData từ JSON.
                    var list = JsonConvert.DeserializeObject<List<SeatData>>(File.ReadAllText(jsonFile)) 
                               ?? new List<SeatData>();

                    // Chuẩn hóa dữ liệu trước khi hiển thị.
                    foreach (var seat in list)
                    {
                        // Chuẩn hóa loại ghế (Type)
                        string t = (seat.Type ?? "").Trim().ToLower();
                        if (t == "vip" || t == "st02")
                            seat.Type = "VIP";
                        else
                            seat.Type = "Thường";

                        // Chuẩn hóa trạng thái ghế
                        string st = (seat.Status ?? "").Trim().ToLower();
                        if (st == "bảo trì" || st == "bao tri")
                            seat.Status = "Bảo trì";
                        else
                            seat.Status = "Bình thường";

                        // Tạo button ghế và gán vào panel.
                        var btn = CreateSeat(seat);
                        btn.Width = 60;
                        btn.Height = 60;
                    }
                }
                else
                {
                    // Nếu không có file JSON → tạo layout mặc định bằng code.
                    GenerateSeatLayout();
                }

                // Sau khi load ghế → cập nhật số ghế.
                UpdateSeatCountUI();
            }
        }

        // Tạo và hiển thị thanh "MÀN HÌNH" phía trên layout ghế.
        // Đây chỉ là một panel trang trí, nhưng có thể kéo thả để chỉnh vị trí.
        private void CreateScreenBar()
        {
            var screen = new Guna2Panel
            {
                Name = "screenBar",
                FillColor = Color.WhiteSmoke,   // Nền xám nhạt
                BorderRadius = 0,
                Height = 50                     // Chiều cao thanh màn hình
            };

            // Tính toán chiều rộng panel màn hình cho cân đối với layout ghế.
            int width = panelRoomLayout.Width - 150;
            screen.Width = width;

            // Căn giữa panel màn hình.
            screen.Left = (panelRoomLayout.Width - width) / 2;
            screen.Top = 20;

            // Label chữ “MÀN HÌNH”.
            var lbl = new Label
            {
                Text = "MÀN HÌNH",
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                BackColor = Color.Transparent,
                AutoSize = true
            };

            // Đưa label vào panel screen.
            screen.Controls.Add(lbl);

            // Căn giữa label trong panel screen.
            lbl.Left = (screen.Width - lbl.Width) / 2;
            lbl.Top = (screen.Height - lbl.Height) / 2;

            // Các event để cho phép kéo thả thanh màn hình.
            screen.MouseDown += Screen_MouseDown;
            screen.MouseMove += Screen_MouseMove;
            screen.MouseUp += Screen_MouseUp;

            // Thêm panel screen vào layout và đưa lên trước cùng.
            panelRoomLayout.Controls.Add(screen);
            screen.BringToFront();
        }

        // Tạo một ghế (Guna2Button) từ dữ liệu SeatData.
        // Hàm sẽ:
        // - Tạo button
        // - Gán text (A01, A02,...)
        // - Gán style theo loại ghế
        // - Gán event (hover, click, drag)
        // - Thêm lên giao diện
        private Guna2Button CreateSeat(SeatData seat)
        {
            var btn = new Guna2Button
            {
                Size = new Size(60, 60),             // Kích thước ghế
                Location = new Point(seat.X, seat.Y), // Tọa độ ghế trong panel
                Text = $"{seat.Row}{seat.Col:00}",    // Tên ghế như A01, A02
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = seat                            // Gán SeatData vào Tag để xử lý sau này
            };

            // Áp dụng style đúng theo loại ghế (VIP/Thường) và trạng thái (Bình thường/Bảo trì).
            ApplySeatStyle(btn, seat);

            // Gán sự kiện chuột cho ghế:
            // Hover → đổi màu
            // MouseDown / Move / Up → hỗ trợ kéo ghế
            // Click → chọn ghế
            btn.MouseEnter += Seat_Hover;
            btn.MouseLeave += Seat_Unhover;
            btn.MouseDown += Seat_MouseDown;
            btn.MouseMove += Seat_MouseMove;
            btn.MouseUp += Seat_MouseUp;
            btn.Click += Seat_Select;

            // Thêm ghế vào panel layout.
            panelRoomLayout.Controls.Add(btn);
            return btn;
        }
                // Áp dụng style cho một ghế dựa theo SeatData:
        // - Nếu ghế Bảo trì: chuyển màu xám, khóa border.
        // - Nếu ghế VIP: có viền vàng.
        // - Nếu ghế Thường: viền xám.
        private void ApplySeatStyle(Guna2Button btn, SeatData seat)
        {
            btn.AutoRoundedCorners = false;
            btn.BorderRadius = 0;
            btn.ForeColor = Color.Black;

            // Nếu ghế đang ở trạng thái Bảo trì → luôn xám và không có viền.
            if (seat.Status == "Bảo trì")
            {
                btn.FillColor = Color.DimGray;
                btn.ForeColor = Color.White;
                btn.BorderThickness = 0;
                return;
            }

            // Nếu ghế VIP → border vàng.
            if (seat.Type == "VIP")
            {
                btn.FillColor = Color.White;
                btn.BorderColor = Color.FromArgb(255, 193, 7);
                btn.BorderThickness = 4;
            }
            else
            {
                // Ghế thường → border xám.
                btn.FillColor = Color.White;
                btn.BorderColor = Color.DimGray;
                btn.BorderThickness = 4;
            }
        }

        // Khi rê chuột vào một ghế chưa được chọn → đổi màu xanh nhạt để tạo hiệu ứng hover.
        private void Seat_Hover(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;

            // Không đổi màu nếu đang được chọn.
            if (selectedSeats.Contains(btn)) return;

            btn.FillColor = Color.FromArgb(167, 238, 250);
        }

        // Khi rời chuột → trả ghế về đúng màu theo loại/ trạng thái.
        private void Seat_Unhover(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;
            if (selectedSeats.Contains(btn)) return;

            var seat = (SeatData)btn.Tag;
            ApplySeatStyle(btn, seat);
        }

        // Căn chỉnh lại toàn bộ vị trí ghế dựa theo seatMap.
        // Hàm này đảm bảo rằng:
        // - Các ghế thẳng hàng
        // - Căn giữa theo chiều ngang
        // - Ghế giãn tỷ lệ nếu panel thay đổi kích thước
        private void FormatSeatPositions()
        {
            if (panelRoomLayout.Controls.Count == 0)
                return;

            int panelW = panelRoomLayout.Width;

            // Kích thước ghế cơ bản.
            int baseSeatW = 60;
            int baseSeatH = 60;
            int baseSpaceX = 10;
            int baseSpaceY = 10;

            int maxSeats = seatMap.Max(r => r.Value);

            // Tính tổng chiều rộng cần thiết cho hàng ghế trước khi scale.
            int wantedWidth = (maxSeats * baseSeatW) + ((maxSeats - 1) * baseSpaceX);

            // Tính hệ số scale để vừa panel.
            float scale = (float)(panelW - 40) / wantedWidth;
            if (scale > 1) scale = 1;

            // Tính kích thước ghế sau khi scale.
            int seatW = (int)(baseSeatW * scale);
            int seatH = (int)(baseSeatH * scale);
            int spaceX = (int)(baseSpaceX * scale);
            int spaceY = baseSpaceY;

            int startY = 90; // Khoảng cách từ màn hình đến hàng đầu tiên.

            // Duyệt từng hàng ghế (A, B, C, ...)
            foreach (var row in seatMap)
            {
                string rowName = row.Key;
                int count = row.Value;

                int rowWidth = (count * seatW) + ((count - 1) * spaceX);
                int startX = (panelW - rowWidth) / 2; // Căn giữa hàng

                // Lấy danh sách ghế thuộc hàng này, sắp theo cột.
                var rowSeats = panelRoomLayout.Controls
                                .OfType<Guna2Button>()
                                .Where(b => b.Tag is SeatData sd && sd.Row == rowName)
                                .OrderBy(b => ((SeatData)b.Tag).Col)
                                .ToList();

                // Đặt lại vị trí cho từng ghế
                foreach (var btn in rowSeats)
                {
                    var seat = (SeatData)btn.Tag;

                    btn.Width = seatW;
                    btn.Height = seatH;

                    btn.Left = startX;
                    btn.Top = startY;

                    // Lưu lại vào SeatData để đảm bảo lưu JSON đúng.
                    seat.X = btn.Left;
                    seat.Y = btn.Top;

                    startX += seatW + spaceX;
                }

                startY += seatH + spaceY;
            }
        }

        // Sự kiện bắt đầu kéo ghế bằng chuột.
        // Lưu vị trí chuột và vị trí ghế để tính toán khi di chuyển.
        private void Seat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            draggingSeat = (Guna2Button)sender;
            dragging = true;

            dragCursorPoint = Cursor.Position;       // Vị trí chuột
            dragStartPoint = draggingSeat.Location;  // Vị trí hiện tại của ghế
        }

        // Sự kiện kéo ghế: tính toán vị trí mới dựa trên độ lệch của chuột.
        private void Seat_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;

            var diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            draggingSeat.Location = Point.Add(dragStartPoint, new Size(diff));
        }

        // Khi thả chuột sau khi kéo → cập nhật lại X/Y vào SeatData.
        private void Seat_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            if (draggingSeat == null) return;

            var seat = (SeatData)draggingSeat.Tag;

            seat.X = draggingSeat.Location.X;
            seat.Y = draggingSeat.Location.Y;
        }

        // Tạo layout ghế tự động khi:
        // - Phòng chưa có file JSON
        // - Hoặc người dùng thêm hàng mới
        // - Hoặc thay đổi seatMap
        //
        // Hệ thống sẽ:
        // - Tính toán vị trí từng ghế theo hàng
        // - Tự phân loại ghế VIP/thường theo quy ước (A–C là Thường, D–F là VIP)
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
                        Type = (rowName == "A" || rowName == "B" || rowName == "C")
                                ? "Thường"
                                : "VIP",
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
                // Hàm này đếm số ghế hiện có trong panel và cập nhật lên textbox.
        // Việc đếm dựa trên số lượng nút Guna2Button có gắn Tag là SeatData.
        private void UpdateSeatCountUI()
        {
            int count = panelRoomLayout.Controls
                         .OfType<Guna2Button>()
                         .Count(b => b.Tag is SeatData);
            txtSoGhe.Text = count.ToString();
        }


        // Khi người dùng bấm thêm hàng, hệ thống sẽ tự sinh thêm một hàng mới.
        // Hàng mới dựa trên chữ cái cuối cùng trong seatMap. Ví dụ F → G.
        // Số ghế mặc định của hàng mới là 15.
        private void btnThemHang_Click(object sender, EventArgs e)
        {
            char last = seatMap.Keys.Last()[0];
            char next = (char)(last + 1);

            seatMap.Add(next.ToString(), 15);

            GenerateSeatLayout();
            UpdateSeatCountUI();
        }


        // Khi thêm ghế trong một hàng, hệ thống yêu cầu phải chọn một ghế làm mốc.
        // Ghế mới sẽ được thêm vào cuối hàng dựa trên tọa độ ghế cuối cùng hiện tại.
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

            // Lấy số ghế hiện tại của hàng
            int currentCount = seatMap[rowName];
            int newCol = currentCount + 1;
            seatMap[rowName] = newCol;

            // Xác định vị trí ghế cuối cùng trong hàng để đặt ghế mới bên cạnh
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

            // Tạo ghế mới theo mốc vừa tìm được
            var newSeat = new SeatData
            {
                SeatId = displayId,
                Row = rowName,
                Col = newCol,
                Type = seat.Type,
                Status = "Bình thường",
                X = lastX + seatW + spaceX,
                Y = lastY
            };

            CreateSeat(newSeat);
            UpdateSeatCountUI();
        }


        // Khi xóa ghế, hệ thống sẽ xóa toàn bộ ghế đang nằm trong danh sách selectedSeats.
        // Người dùng phải xác nhận trước khi xóa.
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


        // Xóa toàn bộ ghế trong một hàng dựa trên ghế được chọn làm mốc.
        // seatMap cũng phải xóa hàng đó để tránh chênh lệch dữ liệu.
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


        // Quy trình lưu sơ đồ ghế gồm:
        // 1. Thu thập toàn bộ dữ liệu ghế đang hiển thị trong panelRoomLayout.
        // 2. Ghi lại dưới dạng JSON để khi mở lại form có thể tái tạo layout tương tự.
        // 3. Xóa dữ liệu ghế cũ trong database và chèn dữ liệu mới hoàn toàn.
        // 4. Cập nhật lại tổng số ghế của phòng trong bảng auditorium.
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string auditoriumId = $"R0{currentRoom}";

            // Lấy dữ liệu ghế từ giao 
            var seats = new List<SeatData>();

            foreach (Control c in panelRoomLayout.Controls)
            {
                if (c is Guna2Button btn && btn.Tag is SeatData seat)
                {
                    // Lưu lại tọa độ hiện tại
                    seat.X = btn.Left;
                    seat.Y = btn.Top;

                    // Đồng bộ kiểu ghế
                    seat.Type = seat.Type.Trim().ToLower() == "vip" ? "VIP" : "Thường";

                    // Đồng bộ trạng thái ghế
                    seat.Status = seat.Status.Trim().ToLower() == "bảo trì" ? "Bảo trì" : "Bình thường";

                    seat.SeatId = $"{seat.Row}{seat.Col:00}";

                    seats.Add(seat);
                }
            }

            // Lưu JSON để phục vụ việc load lại UI
            Directory.CreateDirectory(roomDesignFolder);
            string jsonPath = Path.Combine(roomDesignFolder, $"Room_{currentRoom}.json");
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(seats, Formatting.Indented));

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // Xóa ghế cũ trong database
                    var delSeat = conn.CreateCommand();
                    delSeat.Transaction = tran;
                    delSeat.CommandText = "DELETE FROM seat WHERE auditorium_id = $aud";
                    delSeat.Parameters.AddWithValue("$aud", auditoriumId);
                    delSeat.ExecuteNonQuery();

                    // Đồng thời xóa liên kết ghế - suất chiếu
                    var delSfs = conn.CreateCommand();
                    delSfs.Transaction = tran;
                    delSfs.CommandText =
                        @"DELETE FROM seat_for_showtime WHERE seat_id LIKE '%' || $aud";
                    delSfs.Parameters.AddWithValue("$aud", auditoriumId);
                    delSfs.ExecuteNonQuery();

                    // Chèn lại toàn bộ ghế mới
                    foreach (var s in seats)
                    {
                        string logicalId = $"{s.Row}{s.Col:00}";
                        string dbSeatId = $"{logicalId}{auditoriumId}";

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
                        cmd.Parameters.AddWithValue("$status", s.Status);
                        cmd.Parameters.AddWithValue("$price", price);

                        cmd.ExecuteNonQuery();
                    }

                    // Cập nhật lại số ghế trong bảng auditorium
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
                // Làm mới vị trí các ghế theo đúng bố cục tính toán trong FormatSeatPositions.
        // Thường dùng khi thay đổi kích thước form hoặc muốn căn lại layout cho đều.
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FormatSeatPositions();
        }


        // Mở chế độ chỉnh sửa thông tin ghế.
        // Khi enable, các radio button và textbox sẽ được bật để cho phép thay đổi.
        // Nếu chỉ chọn một ghế thì điền thông tin ghế đó vào giao diện chỉnh sửa.
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


        // Cập nhật mã ghế khi chỉnh sửa bằng tay tại ô txtMaGhe.
        // Chỉ cho phép chỉnh khi đang ở chế độ edit và chỉ chọn đúng 1 ghế.
        // Hệ thống sẽ tự định dạng lại thành dạng chuẩn A01, B05,...
        private void txtMaGhe_TextChanged(object sender, EventArgs e)
        {
            if (!editMode || selectedSeats.Count != 1)
                return;

            var btn = selectedSeats[0];
            var seat = (SeatData)btn.Tag;

            string input = txtMaGhe.Text.Trim().ToUpper();

            // Tách chữ cái (hàng) và số ghế
            if (input.Length >= 2)
            {
                string row = new string(input.TakeWhile(char.IsLetter).ToArray());
                string numStr = new string(input.SkipWhile(char.IsLetter).ToArray());

                if (int.TryParse(numStr, out int num))
                {
                    input = $"{row}{num:00}";
                }
            }

            seat.SeatId = input;
            btn.Text = input;
        }


        // Khi đổi loại ghế sang VIP, áp dụng trực tiếp cho tất cả ghế được chọn.
        // Chỉ có tác dụng khi đang bật chế độ edit. Dữ liệu Tag của ghế sẽ được cập nhật ngay.
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


        // Tương tự phần VIP, nhưng đổi sang loại Thường.
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


        // Khi đổi trạng thái ghế về "Bình thường"
        // Hệ thống cần cập nhật cả dữ liệu trong DB:
        // - Update bảng seat
        // - Update bảng seat_for_showtime về trạng thái "Trống" cho tất cả suất chiếu của phòng
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
                    // Lấy danh sách showtime của phòng để đồng bộ trạng thái ghế
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

                        // Cập nhật trạng thái ghế trong bảng seat
                        var upSeat = conn.CreateCommand();
                        upSeat.Transaction = tran;
                        upSeat.CommandText =
                            @"UPDATE seat SET status = 'Bình thường' WHERE seat_id = $id";
                        upSeat.Parameters.AddWithValue("$id", seatId);
                        upSeat.ExecuteNonQuery();

                    }

                    tran.Commit();
                }
            }
        }


        // Khi đổi trạng thái ghế sang "Bảo trì"
        // Hệ thống thực hiện quy trình tương tự Bình thường:
        // - Cập nhật bảng seat
        // - Cập nhật bảng seat_for_showtime sang "Bảo trì" cho mọi suất chiếu
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

                        // Update bảng seat
                        var upSeat = conn.CreateCommand();
                        upSeat.Transaction = tran;
                        upSeat.CommandText =
                            @"UPDATE seat SET status = 'Bảo trì' WHERE seat_id = $id";
                        upSeat.Parameters.AddWithValue("$id", seatId);
                        upSeat.ExecuteNonQuery();

                        // Đồng bộ tất cả seat_for_showtime
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


        // Khôi phục dữ liệu phòng về trạng thái ban đầu bằng cách load lại JSON cũ.
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            LoadRoom(currentRoom);
            SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
            player.Play();
            MessageBox.Show("Đã khôi phục về ban đầu!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Chuyển đổi giữa các phòng chiếu bằng cách gọi LoadRoom.
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



