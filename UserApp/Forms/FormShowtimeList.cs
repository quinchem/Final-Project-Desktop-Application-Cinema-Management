using Guna.UI2.WinForms;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO; // Thêm thư viện này để check file ảnh
using System.Linq;
using System.Windows.Forms;
using UserApp.Models;

namespace UserApp
{
    public partial class FormShowtimeList : Form
    {
        private ShowtimeRepo repo = new ShowtimeRepo();
        private List<ShowtimeInfo> currentShowtimes;
        private DateTime currentStartDate;
        private DateTime selectedDate;

        public FormShowtimeList()
        {
            InitializeComponent();

            currentStartDate = GetMondayOfWeek(DateTime.Today);
            selectedDate = DateTime.Today;

            // Load dữ liệu
            LoadMonthsComboBox();
            LoadShowtimes();
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
            if (cboMonth.SelectedIndex < 0) return;
            int selectedMonth = cboMonth.SelectedIndex + 1;
            int currentYear = DateTime.Today.Year;
            DateTime firstDayOfMonth = new DateTime(currentYear, selectedMonth, 1);
            currentStartDate = GetMondayOfWeek(firstDayOfMonth);
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
            UpdateDateButtons();
            try
            {
                currentShowtimes = repo.GetShowtimesByDateRange(currentStartDate, 7);

                // Update combobox tháng
                if (currentShowtimes.Any())
                {
                    if (int.TryParse(currentShowtimes.First().Month, out int month))
                        cboMonth.SelectedIndex = Math.Max(0, month - 1);
                    else
                        cboMonth.SelectedIndex = DateTime.Today.Month - 1;
                }
                else
                {
                    cboMonth.SelectedIndex = DateTime.Today.Month - 1;
                }

                UpdateMovieList();
                SelectDate(currentStartDate);

                // --- DEBUG: Nếu số này > 0 mà không hiện phim là do lỗi ở hàm DisplayShowtimes
                // MessageBox.Show("Số lượng phim lấy được: " + currentShowtimes.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void UpdateMovieList()
        {
            if (currentShowtimes == null) return;
            var movies = currentShowtimes.Select(s => s.title).Distinct().OrderBy(t => t).ToList();
        }

        private void FilterChanged_Event(object sender, EventArgs e)
        {
            DisplayShowtimes();
        }

        // --- HÀM QUAN TRỌNG: HIỂN THỊ PHIM ---
        private void DisplayShowtimes()
        {
            // <--- SỬA 1: Đảm bảo tên biến đúng là 'flpShowtimes' (có s)
            flpShowtimes.Controls.Clear();

            // <--- SỬA 2: Gọi hàm GetFilteredShowtimes thay vì viết lại code lọc
            var filteredList = GetFilteredShowtimes();

            // Nhóm phim theo tiêu đề
            var grouped = filteredList.GroupBy(s => s.title);

            foreach (var group in grouped)
            {
                // Gọi hàm tạo Panel
                flpShowtimes.Controls.Add(CreateMoviePanel(group.Key, group.ToList()));
            }

            // Nếu không có phim nào
            if (!filteredList.Any())
            {
                Label lblNoData = new Label
                {
                    Text = "Không có suất chiếu nào cho ngày này.",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.DimGray,
                    AutoSize = true,
                    Padding = new Padding(20)
                };
                flpShowtimes.Controls.Add(lblNoData);
            }
        }

        // --- HÀM LỌC DỮ LIỆU ---
        private List<ShowtimeInfo> GetFilteredShowtimes()
        {
            if (currentShowtimes == null) return new List<ShowtimeInfo>();

            // <--- SỬA 3: Dùng s.ParsedDate thay vì DateTime.Parse(s.show_date) để an toàn
            var filtered = currentShowtimes
                .Where(s => s.ParsedDate.Date == selectedDate.Date)
                .ToList();

            // Lọc theo loại phòng
            if (rad2D.Checked)
                filtered = filtered.Where(s => s.auditorium_type == "2D").ToList();
            else if (rad3D.Checked)
                filtered = filtered.Where(s => s.auditorium_type == "3D").ToList();

            return filtered;
        }

        // --- HÀM TẠO GIAO DIỆN TỪNG PHIM ---
        private Panel CreateMoviePanel(string movieTitle, List<ShowtimeInfo> showtimes)
        {
            Panel panel = new Panel
            {
                Width = flpShowtimes.Width - 30,
                Height = 230,
                Margin = new Padding(0, 0, 0, 20),
                BackColor = Color.Transparent
            };

            var firstInfo = showtimes.First();

            Label lblTitle = new Label
            {
                Text = $"{movieTitle}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Orange,
                Location = new Point(160, 10),
                AutoSize = true,
                Parent = panel
            };

            PictureBox picPoster = new PictureBox
            {
                Size = new Size(140, 210),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(50, 50, 50),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Parent = panel
            };

            // <--- SỬA 4: Mở khóa tính năng Poster (Lấy từ DB) ---
            //if (!string.IsNullOrEmpty(firstInfo.poster_path) && File.Exists(firstInfo.poster_path))
            //{
            //    try { picPoster.Image = Image.FromFile(firstInfo.poster_path); }
            //    catch { /* Nếu ảnh lỗi thì thôi, để màu xám */ }
            //}
            // ----------------------------------------------------

            int xPos = 160;
            int yPos = 50;
            int count = 0;

            foreach (var showtime in showtimes.OrderBy(s => s.start_time))
            {
                Panel btnShowtime = new Panel
                {
                    Size = new Size(120, 60),
                    Location = new Point(xPos, yPos),
                    BackColor = Color.WhiteSmoke,
                    Cursor = Cursors.Hand,
                    Tag = showtime, // <--- ĐÂY LÀ CÁCH LẤY DỮ LIỆU TỪ DB (Gắn vào Tag)
                    Parent = panel
                };

                btnShowtime.Paint += (s, e) =>
                {
                    ControlPaint.DrawBorder(e.Graphics, btnShowtime.ClientRectangle,
                        Color.Silver, ButtonBorderStyle.Solid);
                };

                Label lblTime = new Label
                {
                    Text = showtime.TimeRange.Split('-')[0].Trim(),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(10, 10),
                    AutoSize = true,
                    Parent = btnShowtime
                };

                Label lblType = new Label
                {
                    Text = $"{showtime.auditorium_type} • {showtime.name}",
                    Font = new Font("Segoe UI", 8, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    Location = new Point(10, 35),
                    AutoSize = true,
                    Parent = btnShowtime
                };

                // Gắn sự kiện click
                btnShowtime.Click += Showtime_Click_Handler;
                lblTime.Click += (s, e) => Showtime_Click_Handler(btnShowtime, e);
                lblType.Click += (s, e) => Showtime_Click_Handler(btnShowtime, e);

                xPos += 130;
                count++;

                if (count % 4 == 0)
                {
                    xPos = 160;
                    yPos += 70;
                }
            }
            return panel;
        }

        private void Showtime_Click_Handler(object sender, EventArgs e)
        {
            if (sender is Panel panel && panel.Tag is ShowtimeInfo showtime)
            {
                // <--- LẤY THÔNG TIN TỪ DB Ở ĐÂY (thông qua biến showtime)
                MessageBox.Show(
                    $"Bạn chọn phim: {showtime.title}\n" +
                    $"Ngày: {showtime.show_date}\n" +
                    $"Giờ: {showtime.start_time}\n" +
                    $"Phòng: {showtime.name}",
                    "Xác nhận");

                // Code mở form chọn ghế của bạn:
                // var frm = new FormChonGhe(showtime);
                // frm.ShowDialog();
            }
        }
    }
}