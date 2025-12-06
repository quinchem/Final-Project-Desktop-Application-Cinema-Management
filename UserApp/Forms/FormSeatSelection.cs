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
using System.Media;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormSeatSelection : Form
    {
        private UserMainForm parentForm;

        // Lưu thông tin suất chiếu và phòng chiếu được truyền từ màn hình trước
        private ShowtimeInfo _showtime;
        private string _auditoriumId;
        private string _showtimeId;

        // Repository dùng để lấy hình ảnh poster phim từ database
        private ImageRepo _imageRepo = new ImageRepo();

        // Folder chứa file JSON layout phòng được tạo bởi bên ứng dụng của Admin
        private string _roomJsonFolder;

        // Danh sách tất cả ghế sau khi load layout và dữ liệu database
        private List<SeatUser> _allSeats = new();

        // Danh sách ghế mà người dùng đang chọn
        private List<SeatUser> _selectedSeats = new();

        // Bộ đếm giữ ghế 10 phút
        private int countdown = 600;

        // Cờ để xác định timer có đang chạy hay không
        private bool isCounting = false;

        // Các constructor
        public FormSeatSelection()
        {
            InitializeComponent();
            _roomJsonFolder = GetRoomFolder();
        }

        
        public FormSeatSelection(UserMainForm parent, ShowtimeInfo showtime) : this()
        {
            parentForm = parent;
            _showtime = showtime;
            _auditoriumId = showtime.auditorium_id;
            _showtimeId = showtime.showtime_id;
        }

        // Khi tạo form, hệ thống tự xác định đường dẫn folder lưu layout phòng
        private string GetRoomFolder()
        {
            var csb = new SqliteConnectionStringBuilder(DatabaseHelper.GetConnectionString());
            string db = csb.DataSource;
            string root = Directory.GetParent(Path.GetDirectoryName(db)).FullName;
            return Path.Combine(root, "SharedData", "RoomDesign");
        }

        
        // Khi load form: hiển thị thông tin suất chiếu và load sơ đồ ghế
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
            countdown = 600;
            lblTime.Text = "10:00";
        }
        
        // Load hình poster phim từ database
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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi khi load poster: " + ex.Message);
            }
        }
        // Load sơ đồ phòng
        private void LoadRoom(string auditoriumId, string showtimeId)
        {
            _allSeats.Clear();
            _selectedSeats.Clear();
            panelRoom.Controls.Clear();
            CreateScreenBar();
            UpdateTotal();
            UpdateSelectedSeatLabel();

            // Tìm file JSON layout phòng theo số phòng
            string digits = new string(auditoriumId.Where(char.IsDigit).ToArray());
            int roomNumber = int.Parse(digits);
            string jsonPath = Path.Combine(_roomJsonFolder, $"Room_{roomNumber}.json");

            if (!File.Exists(jsonPath))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không tìm thấy layout phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Đọc tọa độ ghế (X,Y) từ file JSON 
            var jsonSeats = JsonConvert.DeserializeObject<List<SeatData>>(File.ReadAllText(jsonPath));

             // Lấy loại ghế, trạng thái, giá từ database
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

            // Lấy danh sách ghế FULL (đã có người đặt)
            var fullSeats = SeatForShowtimeRepo.GetSeatStatus(showtimeId);

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

                // Còn lại là trống
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

       // Tạo nút ghế hiển thị trên giao diện
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
        
        // Định dạng màu ghế theo loại và trạng thái
       private void ApplySeatStyle(Guna2Button btn, SeatUser seat)
        {
            if (seat.Status == "Bảo trì" )
            {
                btn.FillColor = Color.Gray;
                btn.DisabledState.FillColor = Color.Gray;
                btn.DisabledState.ForeColor = Color.Black;
                btn.Enabled = false;
                return;
            }
            if (seat.Status == "Full")
            {
                btn.FillColor = Color.LightCoral;
                btn.DisabledState.FillColor = Color.LightCoral;
                btn.DisabledState.ForeColor = Color.Black;
                btn.Enabled = false;
                return;
            }
        
            btn.FillColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.BorderColor = seat.Type == "VIP" ? Color.Gold : Color.DimGray;
            btn.BorderThickness = 3;
        }

        // Hàm xử lý sự kiện khi user click chọn ghế
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

            // Bắt đầu đếm thời gian giữ ghế khi chọn ghế đầu tiên
            if (!isCounting && _selectedSeats.Count > 0)
            {
                countdown = 600;
                isCounting = true;
                timer1.Start();
            }
            // Nếu bỏ hết ghế → dừng đếm
            else if (_selectedSeats.Count == 0)
            {
                isCounting = false;
                timer1.Stop();
                countdown = 600;
                lblTime.Text = "10:00";
            }
        }

        // Cập nhật nhãn ghế đang chọn
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

        // Tính tổng tiền ghế đang chọn
        private void UpdateTotal()
        {
            double total = _selectedSeats.Sum(s => s.Price);
            lblSotien.Text = total.ToString("N0") + " VND";
        }

        // Tạo thanh “MÀN HÌNH” để định hướng người xem
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

        // Chuyển sang form thanh toán
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
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
        
        // Timer giữ ghế
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (countdown <= 0)
            {
                timer1.Stop();
                isCounting = false;
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
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
