using Guna.UI2.WinForms;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace UserApp
{
    public partial class FormShowtimeList : Form
    {
        // --- KHAI BÁO BIẾN ---
        private ShowtimeRepo repo = new ShowtimeRepo();
        private ImageRepo _imageRepo = new ImageRepo();
        private List<ShowtimeInfo> currentShowtimes;
        private UserMainForm parentForm;

        private DateTime currentStartDate;
        private DateTime selectedDate;

        private ShowtimeInfo _selectedShowtime = null;
        private Guna2Panel _selectedPanel = null;

        // --- KHỞI TẠO ---
        public FormShowtimeList(UserMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;

            // 1. Cấu hình Panel chứa phim
            InitializeFlowLayoutPanel();

            // 2. Khởi tạo ngày mặc định
            currentStartDate = GetMondayOfWeek(DateTime.Today);
            selectedDate = DateTime.Today;

            // 3. Load ComboBox tháng
            LoadMonthsComboBox();
        }

        // ✅ FIX LỖI GIAO DIỆN BỊ CHE (QUAN TRỌNG)
        private void InitializeFlowLayoutPanel()
        {
            if (flpShowtimes == null) return;

            flpShowtimes.Visible = true;
            flpShowtimes.AutoScroll = true;
            flpShowtimes.FlowDirection = FlowDirection.LeftToRight;
            flpShowtimes.WrapContents = true;
            flpShowtimes.BorderStyle = BorderStyle.None;

            // Màu nền trong suốt để thấy màu form
            flpShowtimes.BackColor = Color.Transparent;

            // 🔥 QUAN TRỌNG: Đẩy xuống dưới cùng để không che cái Lịch
            flpShowtimes.SendToBack();
        }

        private void LoadMonthsComboBox()
        {
            cboMonth.Items.Clear();
            for (int i = 1; i <= 12; i++) cboMonth.Items.Add($"THÁNG {i}");

            // Chọn tháng hiện tại -> Sự kiện OnMonthChanged sẽ tự chạy
            cboMonth.SelectedIndex = DateTime.Today.Month - 1;
        }

        // --- LOGIC XỬ LÝ NGÀY THÁNG ---

        private void OnMonthChanged(object sender, EventArgs e)
        {
            if (cboMonth.SelectedIndex < 0) return;

            int selectedMonth = cboMonth.SelectedIndex + 1;
            int currentYear = DateTime.Today.Year;
            DateTime targetDate;

            // Nếu chọn tháng hiện tại -> Chọn Hôm Nay. Tháng khác -> Chọn mùng 1
            if (selectedMonth == DateTime.Today.Month && currentYear == DateTime.Today.Year)
                targetDate = DateTime.Today;
            else
                targetDate = new DateTime(currentYear, selectedMonth, 1);

            // Cập nhật lại lịch
            currentStartDate = GetMondayOfWeek(targetDate);
            selectedDate = targetDate;

            // 🔥 Tải lại dữ liệu ngay lập tức
            LoadShowtimes();
        }

        private DateTime GetMondayOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private void ChangeWeek(int days)
        {
            currentStartDate = currentStartDate.AddDays(days);
            LoadShowtimes();
        }

        private void btnPrevWeek_Click(object sender, EventArgs e) => ChangeWeek(-7);
        private void btnNextWeek_Click(object sender, EventArgs e) => ChangeWeek(7);

        private void SelectDate(DateTime date)
        {
            selectedDate = date;
            ResetDateButtonStyles();

            Guna2CircleButton clickedButton = GetDateButton(date);
            if (clickedButton != null)
            {
                clickedButton.FillColor = Color.FromArgb(255, 140, 50); // Cam
                clickedButton.ForeColor = Color.White;
            }

            DisplayShowtimes();
        }

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
                btn.Click -= DateButton_Click; // Xóa cũ
                btn.Click += DateButton_Click; // Thêm mới
            }

            // Highlight lại nút đang chọn nếu nó nằm trong tuần này
            ResetDateButtonStyles();
            Guna2CircleButton selectedBtn = GetDateButton(selectedDate);
            if (selectedBtn != null)
            {
                selectedBtn.FillColor = Color.FromArgb(255, 140, 50);
                selectedBtn.ForeColor = Color.White;
            }
        }

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

        private void DateButton_Click(object sender, EventArgs e)
        {
            if (sender is Control ctrl && ctrl.Tag is DateTime dt)
            {
                SelectDate(dt);
            }
        }

        // --- TẢI VÀ HIỂN THỊ DỮ LIỆU ---

        private void LoadShowtimes()
        {
            UpdateDateButtons(); // Vẽ lại số ngày trên lịch

            try
            {
                // Lấy dữ liệu 7 ngày
                currentShowtimes = repo.GetShowtimesByDateRange(currentStartDate, 7);
                if (currentShowtimes == null) currentShowtimes = new List<ShowtimeInfo>();

                DisplayShowtimes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

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

                // Lọc theo ngày
                var filteredList = currentShowtimes
                    .Where(s => s.ParsedDate.Date == selectedDate.Date)
                    .ToList();

                if (filteredList.Count == 0)
                {
                    ShowEmptyMessage($"Ngày {selectedDate:dd/MM} không có suất chiếu nào.");
                    return;
                }

                // Vẽ giao diện từng phim
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

        // --- TẠO GIAO DIỆN PHIM ĐẸP (Xanh Hamster) ---
        private Guna2Panel CreateMoviePanel(string movieTitle, List<ShowtimeInfo> showtimes)
        {
            if (showtimes == null || showtimes.Count == 0) return null;

            string displayTitle = string.IsNullOrEmpty(movieTitle) ? "Tên phim đang cập nhật" : movieTitle.ToUpper();

            // Màu sắc
            Color mainBgColor = Color.FromArgb(92, 124, 150); // Nền Xanh
            Color titleColor = Color.White;
            Color timeBgColor = Color.FromArgb(236, 230, 224); // Nút Kem
            Color accentColor = Color.FromArgb(45, 87, 154);   // Chữ Xanh đậm

            int panelHeight = 280;

            // Panel Chính
            Guna2Panel mainPanel = new Guna2Panel
            {
                Width = flpShowtimes.ClientSize.Width - 25,
                Height = panelHeight,
                FillColor = mainBgColor,
                BackColor = Color.Transparent,
                UseTransparentBackground = true, // Fix góc trắng
                BorderRadius = 15,
                Margin = new Padding(5, 5, 5, 25)
            };
            mainPanel.ShadowDecoration.Enabled = true;
            mainPanel.ShadowDecoration.Depth = 10;
            mainPanel.ShadowDecoration.Color = Color.Black;
            mainPanel.ShadowDecoration.BorderRadius = 15;

            // Poster
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

            // Panel Phải (Chứa thông tin)
            Panel rightPanel = new Panel
            {
                Location = new Point(240, 15),
                Size = new Size(mainPanel.Width - 250, panelHeight - 30),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(rightPanel);

            // Tên Phim
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

            // Container Nút Giờ
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

            // Tạo Nút Giờ
            var sortedShowtimes = showtimes.OrderBy(s => s.StartTime).ToList();

            foreach (var s in sortedShowtimes)
            {
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

                // Giờ
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

                // Loại
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

                // Phòng
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

                timePanel.Click += Showtime_Click_Handler;
                lblTime.Click += (sender, e) => Showtime_Click_Handler(timePanel, e);
                lblType.Click += (sender, e) => Showtime_Click_Handler(timePanel, e);
                lblRoom.Click += (sender, e) => Showtime_Click_Handler(timePanel, e);

                timePanel.Controls.Add(lblRoom);
                timePanel.Controls.Add(lblType);
                timePanel.Controls.Add(lblTime);

                flpTimes.Controls.Add(timePanel);
            }

            return mainPanel;
        }

        private void Showtime_Click_Handler(object sender, EventArgs e)
        {
            Guna2Panel clickedPanel = null;
            if (sender is Guna2Panel p) clickedPanel = p;
            else if (sender is Control c && c.Parent is Guna2Panel p2) clickedPanel = p2;

            if (clickedPanel != null && clickedPanel.Tag is ShowtimeInfo info)
            {
                if (_selectedPanel != null)
                {
                    _selectedPanel.FillColor = Color.FromArgb(236, 230, 224); // Trả màu cũ
                    _selectedPanel.BorderThickness = 0;
                }

                clickedPanel.FillColor = Color.FromArgb(245, 131, 35); // Tô màu cam chọn
                clickedPanel.BorderThickness = 2;

                _selectedPanel = clickedPanel;
                _selectedShowtime = info;
            }
            string showtimeId = _selectedShowtime.showtime_id;
            string auditoriumId = _selectedShowtime.auditorium_id;

        }

        private void btnChonCho_Click(object sender, EventArgs e)
        {
            if (_selectedShowtime == null)
            {
                MessageBox.Show("Vui lòng chọn suất chiếu trước!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            parentForm.OpenChildForm(new FormSeatSelection(_selectedShowtime));

        }
    }
}