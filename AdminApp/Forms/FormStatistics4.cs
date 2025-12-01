using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Guna.Charts.WinForms;
using Microsoft.Data.Sqlite;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace AdminApp
{
    public partial class FormStatistics4 : Form
    {
        // Lưu ý: Đảm bảo tên class Repo khớp với file StatisticsRepo.cs của bạn (có 's' hay không)
        private StatisticRepo _repo = new StatisticRepo();
        private bool isFilteringByYear = false;
        private AdminMainForm _parent;

        public FormStatistics4(AdminMainForm parent)
        {
            InitializeComponent();
            LoadYearCombo();
            LoadRoomFilter();
            _parent = parent;
        }

        private void FormStatistics4_Load(object sender, EventArgs e)
        {
            dptdateFrom.Value = DateTime.Today.AddDays(-30);
            dptdateTo.Value = DateTime.Today;

            dptdateFrom.MouseDown += DateTimePicker_MouseDown;
            dptdateTo.MouseDown += DateTimePicker_MouseDown;

            ReloadAll();
        }

        private void LoadYearCombo()
        {
            comboYear.Items.Clear();
            comboYear.Items.Add("Không");
            int currentYear = DateTime.Today.Year;
            for (int y = currentYear - 2; y <= currentYear + 1; y++)
                comboYear.Items.Add(y.ToString());

            comboYear.SelectedIndex = 0;
        }

        private void LoadRoomFilter()
        {
            try
            {
                var rooms = _repo.GetRooms();
                comboRoomFilter.Items.Clear();
                comboRoomFilter.Items.Add("Tất cả");
                comboRoomFilter.Items.AddRange(rooms.ToArray());
                comboRoomFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadAll()
        {
            // Kiểm tra lần cuối, nếu sai thì thoát luôn không tải dữ liệu
            if (!ValidateDateRange()) return;

            try
            {
                LoadKPIs();
                LoadRevenueByRoomColumnChart();
                LoadShowtimeLineChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSelectedRoom()
        {
            if (comboRoomFilter.SelectedItem == null || comboRoomFilter.SelectedItem.ToString() == "Tất cả")
                return null;
            return comboRoomFilter.SelectedItem.ToString();
        }

        private void DateTimePicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (isFilteringByYear)
            {
                MessageBox.Show("Đang lọc theo năm. Vui lòng chọn 'Không' ở Năm để lọc theo ngày!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // 1. Hàm kiểm tra logic (So sánh .Date để bỏ qua giờ phút)
        private bool ValidateDateRange()
        {
            if (dptdateFrom.Value.Date > dptdateTo.Value.Date)
            {
                return false;
            }
            return true;
        }

        // 2. Sự kiện thay đổi Ngày Bắt Đầu
        private void dptdateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (isFilteringByYear) return;

            // Nếu Ngày Bắt Đầu lớn hơn Ngày Kết Thúc -> Báo lỗi và Reset ngay
            if (dptdateFrom.Value.Date > dptdateTo.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Đặt lại ngày bắt đầu bằng ngày kết thúc (để hợp lệ)
                // Gán cờ hoặc unsubscribe event nếu cần, nhưng gán bằng nhau là an toàn nhất
                dptdateFrom.Value = dptdateTo.Value;
                return; // Dừng lại, không ReloadAll
            }

            ReloadAll();
        }

        // 3. Sự kiện thay đổi Ngày Kết Thúc
        private void dptdateTo_ValueChanged(object sender, EventArgs e)
        {
            if (isFilteringByYear) return;

            // Nếu Ngày Kết Thúc nhỏ hơn Ngày Bắt Đầu -> Báo lỗi và Reset ngay
            if (dptdateTo.Value.Date < dptdateFrom.Value.Date)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Đặt lại ngày kết thúc bằng ngày bắt đầu
                dptdateTo.Value = dptdateFrom.Value;
                return; // Dừng lại, không ReloadAll
            }

            ReloadAll();
        }

        private void comboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedYear = comboYear.SelectedItem?.ToString();
            if (selectedYear == "Không")
            {
                isFilteringByYear = false;
                dptdateFrom.Enabled = true;
                dptdateTo.Enabled = true;
                dptdateFrom.Value = DateTime.Today.AddDays(-30);
                dptdateTo.Value = DateTime.Today;
            }
            else
            {
                isFilteringByYear = true;
                dptdateFrom.Enabled = false;
                dptdateTo.Enabled = false;
                int year = int.Parse(selectedYear);
                dptdateFrom.Value = new DateTime(year, 1, 1);
                dptdateTo.Value = (year == DateTime.Today.Year) ? DateTime.Today : new DateTime(year, 12, 31);
            }
            ReloadAll();
        }

        private void comboRoomFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadAll();
        }

        // ===================== KPIs =====================
        private void LoadKPIs()
        {
            DateTime from = dptdateFrom.Value;
            DateTime to = dptdateTo.Value;
            string roomFilter = GetSelectedRoom();

            // 1. Tổng số phòng hoạt động
            int totalRooms = _repo.GetActiveRoomCount(from, to);
            lblTotalRooms.Text = totalRooms.ToString();

            // 2. Tổng số suất chiếu
            int totalShowtime = _repo.GetTotalShowtimes(from, to, roomFilter);
            lblTotalShowtime.Text = totalShowtime.ToString();

            // 3. Phòng có hiệu suất cao nhất (dựa trên doanh thu)
            var topRoom = _repo.GetTopRevenueRoom(from, to);
            if (!string.IsNullOrEmpty(topRoom.RoomName) && topRoom.RoomName != "N/A")
            {
                // Sửa: Dùng topRoom.RoomName và topRoom.Percentage (viết hoa chữ đầu)
                lblTopRoom.Text = $"{topRoom.RoomName} ({topRoom.Percentage:0.0}%)";
            }
            else
            {
                lblTopRoom.Text = "N/A";
            }
        }

        // ===================== Chart 2: Line Chart - Doanh thu (theo giờ hoặc theo ngày) =====================
        private void LoadShowtimeLineChart()
        {
            lineChartShowtime.Datasets.Clear();
            lineChartShowtime.XAxes.Display = true;
            lineChartShowtime.YAxes.Display = true;
            lineChartShowtime.Legend.Position = LegendPosition.Top;

            DateTime from = dptdateFrom.Value;
            DateTime to = dptdateTo.Value;
            string roomFilter = GetSelectedRoom();

            // Kiểm tra xem có phải đang chọn cùng 1 ngày không
            bool isSingleDay = from.ToString("yyyyMMdd") == to.ToString("yyyyMMdd");

            var datasetRevenue = new GunaLineDataset
            {
                BorderColor = Color.Green,
                BorderWidth = 2,
                PointRadius = 5,
                PointStyle = PointStyle.Circle
            };

            if (isSingleDay)
            {
                // === TRƯỜNG HỢP 1 NGÀY: Hiện theo Giờ ===
                datasetRevenue.Label = $"Doanh thu theo giờ ({from:dd/MM/yyyy})";
                var revenueData = _repo.GetRevenueByHour(from, to, roomFilter);

                // Mẹo: Nếu dữ liệu trống, thêm điểm 0 tại giờ mở cửa để biểu đồ không bị trắng trơn
                if (revenueData.Count == 0)
                {
                    datasetRevenue.DataPoints.Add("08:00", 0);
                    datasetRevenue.DataPoints.Add("22:00", 0);
                }
                else
                {
                    foreach (var item in revenueData)
                    {
                        string hourLabel = item.hour.ToString("D2") + ":00"; // VD: 09:00
                        datasetRevenue.DataPoints.Add(hourLabel, (double)item.revenue);
                    }
                }
            }
            else
            {
                // === TRƯỜNG HỢP NHIỀU NGÀY: Hiện theo Ngày ===
                datasetRevenue.Label = "Doanh thu theo ngày (VNĐ)";
                var revenueData = _repo.GetRevenueShowTimeByDay(from, to, roomFilter);

                foreach (var item in revenueData)
                {
                    datasetRevenue.DataPoints.Add(item.date, (double)item.revenue);
                }
            }

            lineChartShowtime.Datasets.Add(datasetRevenue);
            lineChartShowtime.Update();
        }

        // ===================== Chart 3: Column Chart - Doanh thu theo phòng =====================
        private void LoadRevenueByRoomColumnChart()
        {
            columnChartRevenue.Datasets.Clear();

            DateTime from = dptdateFrom.Value;
            DateTime to = dptdateTo.Value;
            string roomFilter = GetSelectedRoom();

            var data = _repo.GetRevenueByRoom(from, to, roomFilter);

            if (data.Count == 0)
            {
                columnChartRevenue.Update();
                return;
            }

            var dataset = new GunaBarDataset
            {
                Label = "Doanh thu (VNĐ)"
            };

            foreach (var item in data)
            {
                // SỬA LỖI TẠI ĐÂY: item.RoomName (thay vì roomId) và item.Revenue (Viết hoa R)
                dataset.DataPoints.Add(item.RoomName, (double)item.Revenue);
            }

            columnChartRevenue.Datasets.Add(dataset);
            columnChartRevenue.Legend.Position = LegendPosition.Top;
            columnChartRevenue.XAxes.Display = true;
            columnChartRevenue.YAxes.Display = true;
            columnChartRevenue.Update();
        }

        // 1. Nút Tổng quan (Quay lại FormStatistics1)
        private void btnTongQuan_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics1(_parent));
        }

        // 2. Nút Khách hàng (Đang ở đây rồi thì không làm gì hoặc reload)
        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics2(_parent));
        }

        // 3. Nút Phim (Chuyển sang FormStatistics3)
        private void btnPhim_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics3(_parent));
        }

        // 4. Nút Phòng chiếu (Chuyển sang FormStatistics4)
        private void btnPhongChieu_Click(object sender, EventArgs e)
        {

        }


    }
}
