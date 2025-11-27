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

namespace UserApp
{
    public partial class FormShowtimeList : Form
    {
        private ShowtimeRepo repo = new ShowtimeRepo();
        private List<ShowtimeInfo> currentShowtimes;
        private DateTime currentStartDate;
        private DateTime selectedDate;
        private ShowtimeInfo _selectedShowtime = null;
        // Biến lưu cái panel đang chọn (để đổi màu)
        private Guna2Panel _selectedPanel = null;

        public FormShowtimeList()
        {
            InitializeComponent();

            // ✅ Khởi tạo FlowLayoutPanel
            InitializeFlowLayoutPanel();

            currentStartDate = GetMondayOfWeek(DateTime.Today);
            selectedDate = DateTime.Today;

            LoadMonthsComboBox();

        }

        // ✅ MỚI: Đảm bảo FlowLayoutPanel có thuộc tính đúng
        private void InitializeFlowLayoutPanel()
        {
            if (flpShowtimes == null)
            {
                MessageBox.Show("FlowLayoutPanel 'flpShowtimes' chưa được tạo trong Designer!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Đảm bảo visible, kích thước hợp lý và dễ debug
            flpShowtimes.Visible = true;
            flpShowtimes.AutoScroll = true;
            flpShowtimes.FlowDirection = FlowDirection.LeftToRight;
            flpShowtimes.WrapContents = true;

            flpShowtimes.BorderStyle = BorderStyle.FixedSingle;
            flpShowtimes.BringToFront();

        }

        private void LoadMonthsComboBox()
        {
            cboMonth.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cboMonth.Items.Add($"THÁNG {i}");
            }
            cboMonth.SelectedIndex = DateTime.Today.Month - 1;
        }

        private void OnMonthChanged(object sender, EventArgs e)
        {
            // Chặn sự kiện chạy khi chưa load xong form
            if (cboMonth.SelectedIndex < 0) return;

            int selectedMonth = cboMonth.SelectedIndex + 1;
            int currentYear = DateTime.Today.Year;

            // Lấy ngày đầu tiên của tháng được chọn
            DateTime firstDayOfMonth = new DateTime(currentYear, selectedMonth, 1);

            // Cập nhật lại ngày bắt đầu tuần (Thứ 2)
            currentStartDate = GetMondayOfWeek(firstDayOfMonth);

            // 🔥 QUAN TRỌNG: Reset ngày đang chọn về đầu tuần đó luôn để tránh lỗi lệch ngày
            selectedDate = firstDayOfMonth; ;

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

        private void btnPrevWeek_Click(object sender, EventArgs e)
        {
            ChangeWeek(-7);
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            ChangeWeek(7);
        }

        private void SelectDate(DateTime date)
        {
            selectedDate = date;
            ResetDateButtonStyles();
            Guna2CircleButton clickedButton = GetDateButton(date);
            if (clickedButton != null)
            {
                clickedButton.FillColor = Color.FromArgb(255, 140, 50);
                clickedButton.ForeColor = Color.White;
            }

            DisplayShowtimes();
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
            int dayIndex = (int)date.DayOfWeek;
            dayIndex = (dayIndex == 0) ? 6 : dayIndex - 1;
            Guna2CircleButton[] dateButtons = { btnMon, btnTue, btnWed, btnThu, btnFri, btnSat, btnSun };
            return dayIndex >= 0 && dayIndex < 7 ? dateButtons[dayIndex] : null;
        }
        private void LoadShowtimes()
        {
            // Cập nhật nhãn ngày trên các nút (Thứ 2, Thứ 3...)
            UpdateDateButtons();

            try
            {
                // 1️⃣ Lấy dữ liệu từ repo
                currentShowtimes = repo.GetShowtimesByDateRange(currentStartDate, 7);

                // Debug log
                Console.WriteLine($"📊 Loaded {currentShowtimes?.Count ?? 0} showtimes from repo");

                // Đảm bảo list không bao giờ null để tránh lỗi crash sau này
                if (currentShowtimes == null)
                    currentShowtimes = new List<ShowtimeInfo>();

                /* * ❌ ĐÃ XÓA: Phần code tự động set lại cboMonth ở đây.
                 * Lý do: Việc chọn tháng là do người dùng quyết định ở sự kiện OnMonthChanged.
                 * Nếu để lại, nó sẽ gây xung đột (User chọn tháng 2, code tự nhảy về tháng 11).
                 */

                // 2️⃣ Hiển thị dữ liệu lên giao diện
                // Phải gọi hàm này thì Panel phim mới hiện ra
                SelectDate(selectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch chiếu: " + ex.Message, "Lỗi Nghiêm Trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Reset list và xóa giao diện để tránh hiển thị sai
                currentShowtimes = new List<ShowtimeInfo>();
                if (flpShowtimes != null) flpShowtimes.Controls.Clear();
            }
        }

        private void DisplayShowtimes()
        {
            flpShowtimes.SuspendLayout();
            flpShowtimes.Controls.Clear();

            try
            {
                // 1. Kiểm tra dữ liệu đầu vào
                if (currentShowtimes == null || currentShowtimes.Count == 0)
                {
                    ShowEmptyMessage("Chưa có dữ liệu nào trong Database.");
                    return;
                }

                var listByDate = currentShowtimes
                    .Where(s => s.ParsedDate.Date == selectedDate.Date)
                    .ToList();

                var filteredList = listByDate;

                // 4. Group phim và vẽ Panel
                var grouped = filteredList.GroupBy(s => s.title);

                foreach (var group in grouped)
                {
                    var panel = CreateMoviePanel(group.Key, group.ToList());
                    if (panel != null)
                    {
                        panel.BackColor = Color.White;
                        flpShowtimes.Controls.Add(panel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị: " + ex.Message);
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
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.DimGray,
                AutoSize = false, // Để mình tự chỉnh kích thước
                TextAlign = ContentAlignment.MiddleCenter, // Căn giữa chữ
                Width = flpShowtimes.ClientSize.Width - 10, // Rộng bằng panel
                Height = 50,
                Margin = new Padding(0, 20, 0, 0) // Cách lề trên một chút
            };
            flpShowtimes.Controls.Add(lbl);
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
                btn.Click -= DateButton_Click;
                btn.Click += DateButton_Click;
            }
        }

        private void DateButton_Click(object sender, EventArgs e)
        {
            if (sender is Control ctrl && ctrl.Tag is DateTime dt)
            {
                SelectDate(dt);
            }
        }

        private void FilterChanged_Event(object sender, EventArgs e)
        {
            DisplayShowtimes();
        }

        // ✅ HÀM TẠO GIAO DIỆN TỪNG PHIM
        private Guna2Panel CreateMoviePanel(string movieTitle, List<ShowtimeInfo> showtimes)
        {
            if (showtimes == null || showtimes.Count == 0) return null;

            string displayTitle = string.IsNullOrEmpty(movieTitle) ? "Tên phim đang cập nhật" : movieTitle.ToUpper();

            // --- MÀU SẮC ---
            Color mainBgColor = Color.FromArgb(92, 124, 150); // Xanh Hamster
            Color titleColor = Color.White;
            Color timeBgColor = Color.FromArgb(236, 230, 224); // Kem
            Color accentColor = Color.FromArgb(45, 87, 154);   // Xanh đậm cho chữ phụ

            int panelHeight = 280;

            // 1. Panel Chính
            Guna2Panel mainPanel = new Guna2Panel
            {
                Width = flpShowtimes.ClientSize.Width - 25,
                Height = panelHeight,
                FillColor = mainBgColor,
                BackColor = Color.Transparent,
                BorderRadius = 15,
                UseTransparentBackground = true,
                Margin = new Padding(5, 5, 5, 25)
            };

            //Shadow
            mainPanel.ShadowDecoration.Enabled = true;
            mainPanel.ShadowDecoration.Depth = 10;
            mainPanel.ShadowDecoration.Color = Color.Black;
            mainPanel.ShadowDecoration.BorderRadius = 15;

            // 2. Poster (Bên trái)
            Guna2PictureBox picPoster = new Guna2PictureBox
            {
                Size = new Size(175, 250),
                Location = new Point(20, 15),
                BackColor = Color.Silver,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderRadius = 12
            };
            mainPanel.Controls.Add(picPoster);

            Panel rightPanel = new Panel
            {
                Location = new Point(280, 15),
                Size = new Size(mainPanel.Width - 290, panelHeight - 30),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(rightPanel);

            // 3.1 Tên Phim
            Label lblTitle = new Label
            {
                Text = displayTitle,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
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
                Padding = new Padding(0, 20, 0, 0),
                AutoScroll = true
            };
            rightPanel.Controls.Add(flpTimes);
            lblTitle.SendToBack();

            // 4. Tạo Nút Giờ
            var sortedShowtimes = showtimes.OrderBy(s => s.StartTime).ToList();

            foreach (var s in sortedShowtimes)
            {
                Guna2Panel timePanel = new Guna2Panel
                {
                    Size = new Size(300, 150),
                    FillColor = timeBgColor,
                    BorderThickness = 0,
                    BorderRadius = 15,
                    Margin = new Padding(0, 0, 25, 25),
                    Cursor = Cursors.Hand,
                    Tag = s
                };

                // Shadow cho nút giờ
                timePanel.ShadowDecoration.Enabled = true;
                timePanel.ShadowDecoration.Depth = 4;
                timePanel.ShadowDecoration.Color = Color.Gray;

                // Giờ chiếu
                Label lblTime = new Label
                {
                    Text = $"{s.StartTime:hh\\:mm} - {s.EndTime:hh\\:mm}",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.Black,
                    AutoSize = false,
                    TextAlign = ContentAlignment.BottomCenter,
                    Dock = DockStyle.Top,
                    Height = 45,
                    BackColor = Color.Transparent
                };

                // Loại (2D/3D)
                Label lblType = new Label
                {
                    Text = s.auditorium_type ?? "2D",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold | FontStyle.Italic),
                    ForeColor = accentColor,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 50,
                    Padding = new Padding(0, 5, 0, 0),
                    BackColor = Color.Transparent
                };

                // Tên phòng
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

                // Sự kiện Click
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
            // Lấy cái Panel vừa bị click
            Guna2Panel clickedPanel = null;
            if (sender is Guna2Panel p) clickedPanel = p;
            else if (sender is Control c && c.Parent is Guna2Panel p2) clickedPanel = p2;

            if (clickedPanel != null && clickedPanel.Tag is ShowtimeInfo info)
            {
                // 1. Trả lại màu cũ cho panel trước đó (nếu có)
                if (_selectedPanel != null)
                {
                    _selectedPanel.FillColor = Color.FromArgb(236, 230, 224); // Màu kem bình thường
                    _selectedPanel.BorderThickness = 0;
                }

                // 2. Tô màu mới cho panel vừa chọn (Highlight)
                clickedPanel.FillColor = Color.FromArgb(245, 131, 35); 
                clickedPanel.BorderThickness = 2;

                // 3. Lưu lại vào biến để tí nữa nút Button dùng
                _selectedPanel = clickedPanel;
                _selectedShowtime = info;
            }
        }

        private void FormShowtimeList_Load(object sender, EventArgs e)
        {
            LoadShowtimes();

        }

        private void btnChonCho_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem đã chọn suất nào chưa
    if (_selectedShowtime == null)
    {
        MessageBox.Show("Vui lòng chọn suất chiếu trước khi tiếp tục!", "Chưa chọn", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // 2. Mở form chọn ghế
    // (Đảm bảo bên FormSeatSelection đã có hàm khởi tạo nhận tham số nhé)
    //var frm = new FormSeatSelection(_selectedShowtime);
    var frm = new FormSeatSelection();
    
    this.Hide();      // Ẩn form hiện tại
    frm.ShowDialog(); // Hiện form chọn ghế
    this.Show();      // Hiện lại form này khi form kia đóng
        }
    }
}