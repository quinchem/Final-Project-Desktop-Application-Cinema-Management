using Guna.Charts.WinForms;
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
    public partial class FormStatistics3 : Form
    {
        private AdminMainForm _parent;
        private StatisticRepo _statisticsRepo = new StatisticRepo();
        private bool isFilteringByYear = false;
        
        public FormStatistics3(AdminMainForm parent)
        {
            InitializeComponent();
            LoadYearCombo();
            LoadMovieCombo();
            this.AutoScaleMode = AutoScaleMode.None;   
            this.AutoScroll = true;                  
            this.Dock = DockStyle.Top;            

            // Thêm event handler cho MouseDown để catch khi user click vào DateTimePicker bị disabled
            dtpFrom.MouseDown += DateTimePicker_MouseDown;
            dtpTo.MouseDown += DateTimePicker_MouseDown;
            _parent = parent;
        }

        private void FormStatistics3_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;
            ConfigurePieChartLegend();
            ReloadAll();
        }

        // Hàm để chỉnh sửa trục của biểu đồ tròn
        private void ConfigurePieChartLegend()
        {
            gunaChartPieTopMovies.Legend.Position = LegendPosition.Right;
            // Ẩn các trục tọa độ cho pie chart
            gunaChartPieTopMovies.XAxes.Display = false;
            gunaChartPieTopMovies.YAxes.Display = false;
        }

        // Hàm load các giá trị năm cho Combo Box năm
        private void LoadYearCombo()
        {
            comboYear.Items.Clear();
            comboYear.Items.Add("Không");
            comboYear.Items.Add("2023");
            comboYear.Items.Add("2024");
            comboYear.Items.Add("2025");
            comboYear.SelectedIndex = 0;
        }

        // Hàm load các danh sách phim đang có vào Combo Box phim, sử dụng Statistics Repo
        private void LoadMovieCombo()
        {
            MovieCombo.Items.Clear();
            MovieCombo.Items.Add("Tất cả");
            var movies = _statisticsRepo.GetMoviesCurrentlyShowing();
            MovieCombo.Items.AddRange(movies.ToArray());

            if (MovieCombo.Items.Count > 0)
                MovieCombo.SelectedIndex = 0;
        }

        private void ReloadAll()
        {
            LoadRevenueKPI();
            LoadTopMovieInfo();
            LoadRevenueLineChart();
            LoadTopMoviesPieChart();
            LoadRevenueBarChart();
        }

        // Hàm chọn phim 
        private string GetSelectedMovie()
        {
            if (MovieCombo.SelectedItem == null || MovieCombo.SelectedItem.ToString() == "Tất cả")
                return null;

            return MovieCombo.SelectedItem.ToString();
        }

        // Hàm lấy thông tin KPI doanh thu, sau đó đưa vào label tương ứng, sử dụng Statistic Repo
        private void LoadRevenueKPI()
        {
            string selectedMovie = GetSelectedMovie();
            var from = dtpFrom.Value;
            var to = dtpTo.Value;

            var (totalRevenue, totalTickets, avgRevenue) = _statisticsRepo.GetRevenueKPI(from, to, selectedMovie);

            lblTotalRevenue.Text = totalRevenue.ToString("N0") + " VND";
            lblTotalTickets.Text = totalTickets.ToString();
            lblAvgRevenue.Text = avgRevenue.ToString("N0") + " VND";
        }

        // Hàm để lấy thông tin phim có doanh thu cao nhất, sử dụng Statistic Repo
        private void LoadTopMovieInfo()
        {
            string selectedMovie = GetSelectedMovie();
            var from = dtpFrom.Value;
            var to = dtpTo.Value;

            var (movieTitle, totalRevenue, totalTickets) = _statisticsRepo.GetTopMovie(from, to, selectedMovie);

            lblTopMovieName.Text = movieTitle;
        }

        //Hàm lấy thông tin doanh thu theo ngày của tất cả phim
        // hoặc doanh thu theo ngày của phim được chọn vào biểu đồ đường,  sử dụng Statistic Repo
        private void LoadRevenueLineChart()
        {
            gunaChartRevenue.Datasets.Clear();
            string selectedMovie = GetSelectedMovie();

            var revenueLine = new GunaLineDataset
            {
                Label = selectedMovie == null ? "Tổng doanh thu tất cả phim" : $"Doanh thu phim: {selectedMovie}",
                BorderColor = Color.Blue,
                BorderWidth = 2,
            };

            var data = _statisticsRepo.GetRevenueByDay(dtpFrom.Value, dtpTo.Value, selectedMovie);

            foreach (var item in data)
            {
                revenueLine.DataPoints.Add(item.date, (double)item.revenue);
            }

            gunaChartRevenue.Datasets.Add(revenueLine);
            gunaChartRevenue.Legend.Display = true;
            gunaChartRevenue.Update();
        }

        //Hàm lấy top 5 phim có doanh thu cao nhất cùng doanh thu tương ứng vào biểu đồ tròn,  sử dụng Statistic Repo
        private void LoadTopMoviesPieChart()
        {
            gunaChartPieTopMovies.Datasets.Clear();
            string selectedMovie = GetSelectedMovie();

            var data = _statisticsRepo.GetRevenueForPie(dtpFrom.Value, dtpTo.Value, selectedMovie);

            var top5 = data.Take(5).ToList();
            var othersTotal = data.Skip(5).Sum(x => x.total);

            var grandTotal = top5.Sum(x => x.total) + othersTotal;

            var pieDataset = new GunaPieDataset();

            foreach (var item in top5)
            {
                var percentage = (double)item.total / (double)grandTotal * 100;
                pieDataset.DataPoints.Add($"{item.movieTitle}: {percentage:F1}%", (double)item.total);
            }

            if (othersTotal > 0)
            {
                var percentage = (double)othersTotal / (double)grandTotal * 100;
                pieDataset.DataPoints.Add($"Khác: {percentage:F1}%", (double)othersTotal);
            }

            gunaChartPieTopMovies.Datasets.Add(pieDataset);
            gunaChartPieTopMovies.Update();
        }

        // Hàm lấy doanh thu từng phim đang có vào biểu đồ cột,  sử dụng Statistic Repo
        private void LoadRevenueBarChart()
        {
            gunaChartBar.Datasets.Clear();
            string selectedMovie = GetSelectedMovie();

            var data = _statisticsRepo.GetRevenueBar(dtpFrom.Value, dtpTo.Value, selectedMovie);

            var bar = new GunaBarDataset { Label = "Doanh thu từng phim" };
            foreach (var item in data)
                bar.DataPoints.Add(item.movieTitle, (double)item.total);

            gunaChartBar.Datasets.Add(bar);
            gunaChartBar.Update();
        }

        // Hàm xử lý sự kiện chặn click vào DateTimePicker khi đang lọc theo năm
        private void DateTimePicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (isFilteringByYear)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Đang lọc theo năm. Vui lòng chọn 'Không' ở Năm để lọc theo ngày!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        // Hàm validate ngày: Ngày bắt đầu không được sau ngày kết thúc
        private bool ValidateDateRange()
        {
            if (dtpFrom.Value > dtpTo.Value)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Ngày bắt đầu không được sau ngày kết thúc!",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Hàm để cho phép validate và reload nếu đang không lọc theo năm
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (!isFilteringByYear)
            {
                if (ValidateDateRange())
                {
                    ReloadAll();
                }
                else
                {
                    dtpFrom.Value = dtpTo.Value.AddDays(-1);
                }
            }
        }

        // Hàm để cho phép validate và reload nếu đang không lọc theo năm
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (!isFilteringByYear)
            {
                if (ValidateDateRange())
                {
                    ReloadAll();
                }
                else
                {
                    dtpTo.Value = dtpFrom.Value.AddDays(1);
                }
            }
        }

        // Hàm để cho phép người dùng chọn theo năm hoặc theo ngày 
        private void comboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedYear = comboYear.SelectedItem?.ToString();
            if (selectedYear == "Không")
            {
                // Cho phép chọn ngày
                isFilteringByYear = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;

                ReloadAll();
            }
            else
            {
                isFilteringByYear = true;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;

                int year = int.Parse(selectedYear);
                dtpFrom.Value = new DateTime(year, 1, 1);
                dtpTo.Value = new DateTime(year, 12, 31);
                ReloadAll();
            }
        }

        private void MovieCombo_SelectedIndexChanged(object sender, EventArgs e) => ReloadAll();

        // Hàm xử lý sự kiện để chuyển form thống kê tương ứng với nút chọn
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

        }
        private void btnPhongChieu_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics4(_parent));
        }
    }
}
