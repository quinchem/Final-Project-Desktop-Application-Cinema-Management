using Guna.UI2.WinForms;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace UserApp
{
    public partial class FormShowtimeList : Form
    {
        // Repo lấy dữ liệu suất chiếu
        private ShowtimeRepo repo = new ShowtimeRepo();
        // Repo lấy ảnh poster phim
        private ImageRepo _imageRepo = new ImageRepo();
        // Danh sách suất chiếu đang được load
        private List<ShowtimeInfo> currentShowtimes;
        // Tham chiếu đến form cha để mở các form con như chọn ghế
        private UserMainForm parentForm;

        // Ngày bắt đầu của tuần hiển thị
        private DateTime currentStartDate;
        // Ngày đang được chọn
        private DateTime selectedDate;
        // Tháng hiển thị trên label
        private int selectedMonth = DateTime.Today.Month;

        // Suất chiếu được chọn và panel tương ứng
        private ShowtimeInfo _selectedShowtime = null;
        private Guna2Panel _selectedPanel = null;

        public FormShowtimeList(UserMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;

            // Cấu hình flow layout và giá trị mặc định về ngày
            InitializeFlowLayoutPanel();
            selectedMonth = DateTime.Today.Month;
            currentStartDate = GetMondayOfWeek(DateTime.Today);
            selectedDate = DateTime.Today;
            LoadMonthsLabel();

            // Khi form được hiển thị thì khởi tạo calendar và load dữ liệu
            this.Shown += (s, e) => InitCalendar();
        }

        // Thiết lập ban đầu cho flow layout panel chứa các card suất chiếu
        private void InitializeFlowLayoutPanel()
        {
            if (flpShowtimes == null) return;

            flpShowtimes.Visible = true;
            flpShowtimes.AutoScroll = true;
            flpShowtimes.FlowDirection = FlowDirection.LeftToRight;
            flpShowtimes.WrapContents = true;
            flpShowtimes.BorderStyle = BorderStyle.None;

            // Đặt nền trong suốt để nhìn thấy màu nền form
            flpShowtimes.BackColor = Color.Transparent;

            // Đẩy panel xuống dưới để không che phần lịch ở trên
            flpShowtimes.SendToBack();
        }

        // Cập nhật label tháng
        private void LoadMonthsLabel()
        {
            lblMonth.Text = $"THÁNG {selectedMonth}";
        }

        // Khởi tạo calendar, cập nhật các nút ngày và load dữ liệu
        private void InitCalendar()
        {
            if (currentStartDate == default || currentStartDate == DateTime.MinValue)
                currentStartDate = GetMondayOfWeek(DateTime.Today);
            else
                currentStartDate = GetMondayOfWeek(currentStartDate);

            if (selectedDate == default || selectedDate == DateTime.MinValue)
                selectedDate = DateTime.Today;
            selectedDate = selectedDate.Date;

            UpdateMonthLabelByWeek();
            UpdateDateButtons();
            LoadShowtimes();
            SelectDate(selectedDate);
        }

        // Trả về ngày thứ hai (Monday) của tuần chứa ngày truyền vào
        private DateTime GetMondayOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        // Cập nhật label tháng theo tuần hiện tại (dựa trên ngày giữa tuần)
        private void UpdateMonthLabelByWeek()
        {
            DateTime midWeek = currentStartDate.AddDays(3);

            int month = midWeek.Month;
            int year = midWeek.Year;

            lblMonth.Text = $"THÁNG {month} - {year}";
        }

        // Xử lý nút lùi một tuần
        private void btnPrevWeek_Click(object sender, EventArgs e)
        {
            currentStartDate = currentStartDate.AddDays(-7);
            currentStartDate = GetMondayOfWeek(currentStartDate);
            selectedDate = currentStartDate;

            UpdateMonthLabelByWeek();
            LoadShowtimes();
        }

        // Xử lý nút tiến một tuần
        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            currentStartDate = currentStartDate.AddDays(7);
            currentStartDate = GetMondayOfWeek(currentStartDate);
            selectedDate = currentStartDate;

            UpdateMonthLabelByWeek();
            LoadShowtimes();
        }

        // Chọn một ngày và cập nhật giao diện
        private void SelectDate(DateTime date)
        {
            selectedDate = date;
            ResetDateButtonStyles();

            Guna2CircleButton clickedButton = GetDateButton(date);
            if (clickedButton != null)
            {
                clickedButton.FillColor = Color.FromArgb(255, 140, 50); // Màu cam cho ngày chọn
                clickedButton.ForeColor = Color.White;
            }

            DisplayShowtimes();
        }

        // Cập nhật nội dung và tag cho các nút ngày trong tuần
        private void UpdateDateButtons()
        {
            Guna2CircleButton[] dateButtons = { btnMon, btnTue, btnWed, btnThu, btnFri, btnSat, btnSun };
            for (int i = 0; i < 7; i++)
            {
                DateTime date = currentStartDate.AddDays(i);
                var btn = dateButtons[i];
                if (btn == null) continue;

                btn.Text = date.Day.ToString("00");
                btn.Tag = date;
                btn.Click -= DateButton_Click; // Xóa handler cũ để tránh đăng ký nhiều lần
                btn.Click += DateButton_Click; // Thêm handler mới
            }

            // Tô lại nút đang chọn nếu nằm trong tuần hiện tại
            ResetDateButtonStyles();
            Guna2CircleButton selectedBtn = GetDateButton(selectedDate);
            if (selectedBtn != null)
            {
                selectedBtn.FillColor = Color.FromArgb(255, 140, 50);
                selectedBtn.ForeColor = Color.White;
            }
        }

        // Reset style cho các nút ngày về mặc định
        private void ResetDateButtonStyles()
        {
            Guna2CircleButton[] dateButtons = { btnMon, btnTue, btnWed, btnThu, btnFri, btnSat, btnSun };
            foreach (var btn in dateButtons)
            {
                if (btn == null) continue;
                btn.FillColor = SystemColors.Control;
                btn.ForeColor = Color.Black;
            }
        }

        // Lấy nút tương ứng với ngày trong tuần hiện tại
        private Guna2CircleButton GetDateButton(DateTime date)
        {
            TimeSpan diff = date.Date - currentStartDate.Date;
            int daysDiff = diff.Days;
            if (daysDiff >= 0 && daysDiff < 7)
            {
                Guna2CircleButton[] dateButtons = { btnMon, btnTue, btnWed, btnThu, btnFri, btnSat, btnSun };
                return dateButtons[daysDiff];
            }
            return null;
        }

        // Handler khi click vào nút ngày
        private void DateButton_Click(object sender, EventArgs e)
        {
            if (sender is Control ctrl && ctrl.Tag is DateTime dt)
            {
                SelectDate(dt);
            }
        }

        // Lấy danh sách suất chiếu cho tuần hiện tại
        private void LoadShowtimes()
        {
            UpdateDateButtons();

            try
            {
                currentShowtimes = repo.GetShowtimesByDateRange(currentStartDate, 7);
                if (currentShowtimes == null) currentShowtimes = new List<ShowtimeInfo>();

                DisplayShowtimes();
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // Hiển thị các suất chiếu lên flow layout theo ngày được chọn
        private void DisplayShowtimes()
        {
            flpShowtimes.SuspendLayout();
            flpShowtimes.Controls.Clear();

            try
            {
                if (currentShowtimes == null || currentShowtimes.Count == 0)
                {
                    ShowEmptyMessage("Chưa có lịch chiếu nào trong tuần này.");
                    return;
                }

                // Lọc danh sách theo ngày đang chọn
                var filteredList = currentShowtimes
                    .Where(s => s.ParsedDate.Date == selectedDate.Date)
                    .ToList();

                if (filteredList.Count == 0)
                {
                    ShowEmptyMessage($"Ngày {selectedDate:dd/MM} không có suất chiếu nào.");
                    return;
                }

                // Gom nhóm theo tiêu đề phim để tạo từng panel riêng cho mỗi phim
                var grouped = filteredList.GroupBy(s => s.title);
                foreach (var group in grouped)
                {
                    var panel = CreateMoviePanel(group.Key, group.ToList());
                    if (panel != null) flpShowtimes.Controls.Add(panel);
                }
            }
            finally
            {
                flpShowtimes.ResumeLayout();
            }
        }

        // Hiển thị label thông báo khi không có suất chiếu
        private void ShowEmptyMessage(string message)
        {
            Label lbl = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 14, FontStyle.Italic),
                ForeColor = Color.WhiteSmoke,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = flpShowtimes.ClientSize.Width - 10,
                Height = 60,
                Margin = new Padding(0, 50, 0, 0)
            };
            flpShowtimes.Controls.Add(lbl);
        }

        // Tạo panel hiển thị thông tin phim và các nút giờ chiếu
        private Guna2Panel CreateMoviePanel(string movieTitle, List<ShowtimeInfo> showtimes)
        {
            if (showtimes == null || showtimes.Count == 0) return null;

            string displayTitle = string.IsNullOrEmpty(movieTitle) ? "Tên phim đang cập nhật" : movieTitle.ToUpper();
            Color mainBgColor = Color.FromArgb(92, 124, 150);
            Color titleColor = Color.White;
            Color timeBgColor = Color.FromArgb(236, 230, 224);
            Color accentColor = Color.FromArgb(45, 87, 154);
            int panelHeight = 280;

            // Panel chính cho từng phim
            Guna2Panel mainPanel = new Guna2Panel
            {
                Width = flpShowtimes.ClientSize.Width - 25,
                Height = panelHeight,
                FillColor = mainBgColor,
                BackColor = Color.Transparent,
                UseTransparentBackground = true,
                BorderRadius = 15,
                Margin = new Padding(5, 5, 5, 25)
            };
            mainPanel.ShadowDecoration.Enabled = true;
            mainPanel.ShadowDecoration.Depth = 10;
            mainPanel.ShadowDecoration.Color = Color.Black;
            mainPanel.ShadowDecoration.BorderRadius = 15;

            // Tải poster phim từ repo ảnh nếu có
            Image moviePoster = null;
            try
            {
                string movieId = showtimes[0].movie_id;
                byte[] imgBytes = _imageRepo.GetMoviePoster(movieId);
                if (imgBytes != null && imgBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        moviePoster = new Bitmap(Image.FromStream(ms));
                    }
                }
            }
            catch { }

            // Picture box hiển thị poster
            Guna2PictureBox picPoster = new Guna2PictureBox
            {
                Size = new Size(175, 250),
                Location = new Point(20, 15),
                BackColor = Color.Silver,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderRadius = 12,
                UseTransparentBackground = true,
                Image = moviePoster
            };
            mainPanel.Controls.Add(picPoster);

            // Panel bên phải chứa tên phim và các nút giờ
            Panel rightPanel = new Panel
            {
                Location = new Point(240, 15),
                Size = new Size(mainPanel.Width - 250, panelHeight - 30),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(rightPanel);

            // Label tiêu đề phim
            Label lblTitle = new Label
            {
                Text = displayTitle,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = titleColor,
                Width = rightPanel.Width,
                Height = 70,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.BottomLeft,
                AutoEllipsis = true
            };
            rightPanel.Controls.Add(lblTitle);

            // FlowLayoutPanel chứa các nút giờ
            FlowLayoutPanel flpTimes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(0, 15, 0, 0),
                AutoScroll = true
            };
            rightPanel.Controls.Add(flpTimes);
            lblTitle.SendToBack();

            // Sắp xếp suất chiếu theo thời gian bắt đầu
            var sortedShowtimes = showtimes.OrderBy(s => s.StartTime).ToList();

            foreach (var s in sortedShowtimes)
            {
                // Tạo panel nhỏ cho từng suất chiếu
                Guna2Panel timePanel = new Guna2Panel
                {
                    Size = new Size(170, 100),
                    FillColor = timeBgColor,
                    BorderThickness = 0,
                    BorderRadius = 15,
                    Margin = new Padding(0, 0, 20, 20),
                    Cursor = Cursors.Hand,
                    Tag = s,
                    UseTransparentBackground = true,
                    BackColor = Color.Transparent
                };

                timePanel.ShadowDecoration.Enabled = true;
                timePanel.ShadowDecoration.Depth = 3;
                timePanel.ShadowDecoration.Color = Color.Gray;

                // Label hiển thị giờ bắt đầu và kết thúc
                Label lblTime = new Label
                {
                    Text = $"{s.StartTime:hh\\:mm} - {s.EndTime:hh\\:mm}",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.Black,
                    AutoSize = false,
                    TextAlign = ContentAlignment.BottomCenter,
                    Dock = DockStyle.Top,
                    Height = 40,
                    BackColor = Color.Transparent
                };

                // Label hiển thị loại phòng hoặc định dạng
                Label lblType = new Label
                {
                    Text = s.auditorium_type ?? "2D",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold | FontStyle.Italic),
                    ForeColor = accentColor,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 30,
                    Padding = new Padding(0, 5, 0, 0),
                    BackColor = Color.Transparent
                };

                // Label hiển thị tên phòng
                Label lblRoom = new Label
                {
                    Text = s.name ?? "P.?",
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    ForeColor = Color.DimGray,
                    AutoSize = false,
                    TextAlign = ContentAlignment.TopCenter,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(0, 5, 0, 0),
                    BackColor = Color.Transparent
                };

                // Đăng ký handler click để chọn suất chiếu
                timePanel.Click += Showtime_Click_Handler;
                lblTime.Click += (sender, e) => Showtime_Click_Handler(timePanel, e);
                lblType.Click += (sender, e) => Showtime_Click_Handler(timePanel, e);
                lblRoom.Click += (sender, e) => Showtime_Click_Handler(timePanel, e);

                // Thêm control vào panel thời gian
                timePanel.Controls.Add(lblRoom);
                timePanel.Controls.Add(lblType);
                timePanel.Controls.Add(lblTime);

                // Thêm panel giờ vào flow layout
                flpTimes.Controls.Add(timePanel);
            }

            return mainPanel;
        }

        // Xử lý khi người dùng click chọn một suất chiếu
        private void Showtime_Click_Handler(object sender, EventArgs e)
        {
            Guna2Panel clickedPanel = null;
            if (sender is Guna2Panel p) clickedPanel = p;
            else if (sender is Control c && c.Parent is Guna2Panel p2) clickedPanel = p2;

            if (clickedPanel != null && clickedPanel.Tag is ShowtimeInfo info)
            {
                // Nếu đã có panel chọn trước đó thì reset style về mặc định
                if (_selectedPanel != null)
                {
                    _selectedPanel.FillColor = Color.FromArgb(236, 230, 224);
                    _selectedPanel.BorderThickness = 0;
                }

                // Đổi style panel vừa click để thể hiện là đang chọn
                clickedPanel.FillColor = Color.FromArgb(245, 131, 35);
                clickedPanel.BorderThickness = 2;

                // Cập nhật biến lưu suất chiếu và panel đang chọn
                _selectedPanel = clickedPanel;
                _selectedShowtime = info;
            }

            if (_selectedShowtime != null)
            {
                string showtimeId = _selectedShowtime.showtime_id;
                string auditoriumId = _selectedShowtime.auditorium_id;
            }
        }

        // Xử lý khi người dùng bấm nút Chọn ghế
        private void btnChonCho_Click(object sender, EventArgs e)
        {
            if (_selectedShowtime == null)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn suất chiếu trước!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form chọn ghế và truyền thông tin suất chiếu đã chọn
            parentForm.OpenChildForm(new FormSeatSelection(parentForm, _selectedShowtime));
        }
    }
}
