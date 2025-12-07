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
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace AdminApp
{
    public partial class FormStatistics4 : Form
    {
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

        // Hàm thêm các giá trị năm vào combobox lọc.
        private void LoadYearCombo()
        {
            comboYear.Items.Clear();
            comboYear.Items.Add("Không");
            int currentYear = DateTime.Today.Year;
            for (int y = currentYear - 2; y <= currentYear + 1; y++)
                comboYear.Items.Add(y.ToString());
            comboYear.SelectedIndex = 0;
        }

        // Hàm lấy danh sách phòng chiếu từ database, sử dụng _repo.GetRooms
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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi load phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm để gọi lại tất cả hàm load KPI và chart.
        private void ReloadAll()
        {
            if (!ValidateDateRange()) return;

            try
            {
                LoadKPIs();
                LoadRevenueByRoomColumnChart();
                LoadShowtimeLineChart();
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Hàm để trả về phòng được chọn trong combobox.
        private string GetSelectedRoom()
        {
            if (comboRoomFilter.SelectedItem == null || comboRoomFilter.SelectedItem.ToString() == "Tất cả")
                return null;
            return comboRoomFilter.SelectedItem.ToString();
        }

        // Hàm chặn người dùng thay đổi ngày khi đang lọc theo năm.
        private void DateTimePicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (isFilteringByYear)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Đang lọc theo năm. Vui lòng chọn 'Không' ở Năm để lọc theo ngày!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        //Hàm kiểm tra logic ngày bắt đầu và ngày kết thúc
        private bool ValidateDateRange()
        {
            if (dptdateFrom.Value.Date > dptdateTo.Value.Date)
            {
                return false;
            }
            return true;
        }

        // Hàm xử lý sự kiện thay đổi Ngày Bắt đầu
        private void dptdateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (isFilteringByYear) return;
            
            if (dptdateFrom.Value.Date > dptdateTo.Value.Date)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                dptdateFrom.Value = dptdateTo.Value;
                return; // Dừng lại, không ReloadAll
            }
            ReloadAll();
        }

         // Hàm xử lý sự kiện thay đổi Ngày Kết thúc
        private void dptdateTo_ValueChanged(object sender, EventArgs e)
        {
            if (isFilteringByYear) return;

            if (dptdateTo.Value.Date < dptdateFrom.Value.Date)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                dptdateTo.Value = dptdateFrom.Value;
                return; // Dừng lại, không ReloadAll
            }

            ReloadAll();
        }

        // Hàm để chuyển đổi chế độ lọc theo năm / lọc theo ngày.
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

        // Hàm để cập nhập toàn bộ thống kê khi chọn phòng khác
        private void comboRoomFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadAll();
        }

       // Hàm để lấy các chỉ số KPI từ Statistic Repo và hiển thị lên các label tương ứng
        private void LoadKPIs()
        {
            DateTime from = dptdateFrom.Value;
            DateTime to = dptdateTo.Value;
            string roomFilter = GetSelectedRoom();

            int totalRooms = _repo.GetActiveRoomCount(from, to);
            lblTotalRooms.Text = totalRooms.ToString();

            int totalShowtime = _repo.GetTotalShowtimes(from, to, roomFilter);
            lblTotalShowtime.Text = totalShowtime.ToString();

            var topRoom = _repo.GetTopRevenueRoom(from, to);
            if (!string.IsNullOrEmpty(topRoom.RoomName) && topRoom.RoomName != "N/A")
            {
                lblTopRoom.Text = $"{topRoom.RoomName} ({topRoom.Percentage:0.0}%)";
            }
            else
            {
                lblTopRoom.Text = "N/A";
            }
        }

        // Hàm để vẽ biểu đồ đường doanh thu theo ngày hoặc theo giờ.
        private void LoadShowtimeLineChart()
        {
            lineChartShowtime.Datasets.Clear();
            lineChartShowtime.XAxes.Display = true;
            lineChartShowtime.YAxes.Display = true;
            lineChartShowtime.Legend.Position = LegendPosition.Top;

            DateTime from = dptdateFrom.Value;
            DateTime to = dptdateTo.Value;
            string roomFilter = GetSelectedRoom();

            // Kiểm tra xem có phải đang chọn cùng 1 ngày không, nếu là 1 ngày thì sẽ hiện doanh thu theo giờ
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
                datasetRevenue.Label = $"Doanh thu theo giờ ({from:dd/MM/yyyy})";
                var revenueData = _repo.GetRevenueByHour(from, to, roomFilter);
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

        // Hàm để vẽ biểu đồ cột doanh thu theo phòng (có lọc theo phòng)
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
               
                dataset.DataPoints.Add(item.RoomName, (double)item.Revenue);
            }
            columnChartRevenue.Datasets.Add(dataset);
            columnChartRevenue.Legend.Position = LegendPosition.Top;
            columnChartRevenue.XAxes.Display = true;
            columnChartRevenue.YAxes.Display = true;
            columnChartRevenue.Update();
        }

        // Các hàm xử lý sự kiện chuyển form thống kê khi nhấn vào các button tương ứng
        private void btnTongQuan_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics1(_parent));
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics2(_parent));
        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics3(_parent));
        }

        private void btnPhongChieu_Click(object sender, EventArgs e)
        {

        }
    }
}
