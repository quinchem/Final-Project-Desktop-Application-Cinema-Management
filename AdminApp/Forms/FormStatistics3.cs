using Guna.Charts.WinForms;
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

namespace AdminApp
{
    public partial class FormStatistics3 : Form
    {

        private StatisticRepo _statisticsRepo = new StatisticRepo();
        private bool isFilteringByYear = false;
        public FormStatistics3()
        {
            InitializeComponent();
            LoadYearCombo();
            LoadMovieCombo();

            // Thêm event handler cho MouseDown để catch khi user click vào DateTimePicker bị disabled
            dtpFrom.MouseDown += DateTimePicker_MouseDown;
            dtpTo.MouseDown += DateTimePicker_MouseDown;
        }

        private void FormStatistics3_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;
            ConfigurePieChartLegend();
            ReloadAll();
        }

        private void ConfigurePieChartLegend()
        {
            gunaChartPieTopMovies.Legend.Position = LegendPosition.Right;
            // Ẩn các trục tọa độ cho pie chart
            gunaChartPieTopMovies.XAxes.Display = false;
            gunaChartPieTopMovies.YAxes.Display = false;
        }

        private void LoadYearCombo()
        {
            comboYear.Items.Clear();
            comboYear.Items.Add("Không");
            comboYear.Items.Add("2023");
            comboYear.Items.Add("2024");
            comboYear.Items.Add("2025");
            comboYear.SelectedIndex = 0;
        }

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

        private string GetSelectedMovie()
        {
            if (MovieCombo.SelectedItem == null || MovieCombo.SelectedItem.ToString() == "Tất cả")
                return null;

            return MovieCombo.SelectedItem.ToString();
        }

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

        private void LoadTopMovieInfo()
        {
            string selectedMovie = GetSelectedMovie();
            var from = dtpFrom.Value;
            var to = dtpTo.Value;

            var (movieTitle, totalRevenue, totalTickets) = _statisticsRepo.GetTopMovie(from, to, selectedMovie);

            lblTopMovieName.Text = movieTitle;
        }

        private void LoadRevenueLineChart()
        {
            gunaChartRevenue.Datasets.Clear();
            string selectedMovie = GetSelectedMovie();

            var totalLine = new GunaLineDataset { Label = "Tổng doanh thu", BorderColor = Color.Blue };
            var topMovieLine = new GunaLineDataset { Label = "Doanh thu phim nổi bật", BorderColor = Color.Red };

            var data = _statisticsRepo.GetRevenueWithTopMovieByDay(dtpFrom.Value, dtpTo.Value, selectedMovie);

            foreach (var item in data)
            {
                totalLine.DataPoints.Add(item.date, (double)item.totalRevenue);

                string movieName = item.topMovieName ?? (selectedMovie ?? "N/A");
                string customLabel = $"{item.date} - {movieName}";
                topMovieLine.DataPoints.Add(customLabel, (double)item.topMovieRevenue);
            }

            gunaChartRevenue.Datasets.Add(totalLine);
            gunaChartRevenue.Datasets.Add(topMovieLine);
            gunaChartRevenue.Legend.Display = true;
            gunaChartRevenue.Update();
        }

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

        // Event handler khi user click vào DateTimePicker
        private void DateTimePicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (isFilteringByYear)
            {
                MessageBox.Show("Đang lọc theo năm. Vui lòng chọn 'Không' ở Năm để lọc theo ngày!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        // VALIDATE DATE: Ngày bắt đầu không được sau ngày kết thúc
        private bool ValidateDateRange()
        {
            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được sau ngày kết thúc!",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            // Chỉ validate và reload nếu đang không lọc theo năm
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

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            // Chỉ validate và reload nếu đang không lọc theo năm
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
                // Lọc theo năm - DISABLE DateTimePicker
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
    }
}