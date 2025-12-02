using Guna.Charts.WinForms;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using SharedData.Models;
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


namespace AdminApp
{

    public partial class FormStatistics1 : Form
    {
        private bool isFilteringByYear = false;
        private AdminMainForm _parent;

        public FormStatistics1(AdminMainForm parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void FormStatistics1_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;

            LoadYearCombo();
            ReloadAll();

            dtpFrom.MouseDown += DatePicker_MouseDown;
            dtpTo.MouseDown += DatePicker_MouseDown;
        }

        private void DatePicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (isFilteringByYear)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Bạn đang lọc theo năm. Hãy chọn 'Tất cả' để dùng lọc theo ngày!",
                    "Không thể chọn ngày", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadYearCombo()
        {
            YearCombo.Items.Clear();

            // Thêm mục "Không" để được phép chọn DateTimePicker
            YearCombo.Items.Add("Không");

            // Các năm cố định theo yêu cầu
            YearCombo.Items.Add("2023");
            YearCombo.Items.Add("2024");
            YearCombo.Items.Add("2025");
            YearCombo.Items.Add("2026");

            YearCombo.SelectedIndex = 0;
        }



        private void ReloadAll()
        {
            if (isFilteringByYear && YearCombo.SelectedIndex > 0)
            {
                int year = int.Parse(YearCombo.SelectedItem.ToString());
                dtpFrom.Value = new DateTime(year, 1, 1);
                dtpTo.Value = new DateTime(year, 12, 31);
            }

            LoadRevenueKPI();
            LoadRevenueByDayChart();
            LoadTopMoviesChart();
        }

        // ==============================================
        // KPI
        // ==============================================
        private void LoadRevenueKPI()
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT 
                        SUM(total) AS TotalRevenue,
                        COUNT(bill_id) AS TotalTickets,
                        SUM(total) * 1.0 / 
                        (julianday(@to) - julianday(@from) + 1) AS AvgRevenuePerDay
                    FROM bill
                    WHERE bill_date IS NOT NULL
                        AND substr(bill_date, 7, 4) || '-' || 
                            substr(bill_date, 4, 2) || '-' || 
                            substr(bill_date, 1, 2) 
                        BETWEEN @from AND @to";

                cmd.Parameters.AddWithValue("@from", dtpFrom.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", dtpTo.Value.ToString("yyyy-MM-dd"));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTotalRevenue.Text =
                            reader["TotalRevenue"] != DBNull.Value ?
                            Convert.ToDecimal(reader["TotalRevenue"]).ToString("N0") + " VND" :
                            "0 VND";

                        lblTotalTickets.Text =
                            reader["TotalTickets"] != DBNull.Value ?
                            reader["TotalTickets"].ToString() : "0";

                        lblAvgRevenue.Text =
                            reader["AvgRevenuePerDay"] != DBNull.Value ?
                            Convert.ToDecimal(reader["AvgRevenuePerDay"]).ToString("N0") + " VND" :
                            "0 VND";
                    }
                }

                // Top movie
                cmd.CommandText = @"
                    SELECT m.title
                    FROM bill b
                    JOIN showtime s ON b.showtime_id = s.showtime_id
                    JOIN movie m ON s.movie_id = m.movie_id
                    WHERE substr(b.bill_date, 7, 4) || '-' || 
                          substr(b.bill_date, 4, 2) || '-' || 
                          substr(b.bill_date, 1, 2) 
                          BETWEEN @from AND @to
                    GROUP BY m.movie_id
                    ORDER BY SUM(b.total) DESC
                    LIMIT 1";

                using (var reader = cmd.ExecuteReader())
                {
                    lblTopMovie.Text = reader.Read() ? reader.GetString(0) : "-";
                    lblTopMovie.AutoSize = true;
                    lblTopMovie.MaximumSize = new Size(250, 0);
                }

                // New customers
                // 3️⃣ Khách hàng mới
                // 3️⃣ Khách hàng mới - fix lỗi định dạng create_date
                cmd.CommandText = @"
                SELECT COUNT(*)
                FROM customer
                WHERE create_date IS NOT NULL
                  AND (date(substr(create_date, -10, 10), 'DD-MM-YYYY') BETWEEN @from AND @to
                       OR date(create_date) BETWEEN @from AND @to)";
                using (var reader = cmd.ExecuteReader())
                {
                    lblNewCustomer.Text = reader.Read() ? reader.GetInt32(0).ToString() : "0";
                }
                //cmd.CommandText = @"
                //    SELECT COUNT(*)
                //    FROM customer
                //    WHERE create_date IS NOT NULL
                //        AND substr(create_date, 7, 4) || '-' || 
                //            substr(create_date, 4, 2) || '-' || 
                //            substr(create_date, 1, 2) 
                //            BETWEEN @from AND @to";

                //using (var reader = cmd.ExecuteReader())
                //{
                //    lblNewCustomer.Text = reader.Read()
                //        ? reader.GetInt32(0).ToString()
                //        : "0";
            }
        }


        // ==============================================
        // Line chart: doanh thu theo ngày
        // ==============================================
        private void LoadRevenueByDayChart()
        {
            gunaChartRevenue.Datasets.Clear();

            var line = new GunaLineDataset
            {
                Label = "Doanh thu",
                BorderColor = Color.Blue
            };

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT bill_date AS Ngay, SUM(total) AS Total
                FROM bill
                WHERE substr(bill_date, 7, 4) || '-' || 
                      substr(bill_date, 4, 2) || '-' || 
                      substr(bill_date, 1, 2) 
                      BETWEEN @from AND @to
                GROUP BY bill_date
                ORDER BY substr(bill_date, 7, 4) || '-' || 
                         substr(bill_date, 4, 2) || '-' || 
                         substr(bill_date, 1, 2)";

            cmd.Parameters.AddWithValue("@from", dtpFrom.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", dtpTo.Value.ToString("yyyy-MM-dd"));

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                line.DataPoints.Add(reader.GetString(0), (double)reader.GetDecimal(1));
            }

            gunaChartRevenue.Datasets.Add(line);
            gunaChartRevenue.Update();
        }

        // ==============================================
        // Bar chart: top 5 movie
        // ==============================================
        private void LoadTopMoviesChart()
        {
            gunaChartTopMovies.Datasets.Clear();

            var bar = new GunaBarDataset
            {
                Label = "Top 5 phim"
            };

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT m.title, SUM(b.total) AS Total
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date, 7, 4) || '-' || 
                      substr(b.bill_date, 4, 2) || '-' || 
                      substr(b.bill_date, 1, 2) 
                      BETWEEN @from AND @to
                GROUP BY m.movie_id
                ORDER BY Total DESC
                LIMIT 5";

            cmd.Parameters.AddWithValue("@from", dtpFrom.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", dtpTo.Value.ToString("yyyy-MM-dd"));

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bar.DataPoints.Add(reader.GetString(0), (double)reader.GetDecimal(1));
            }

            gunaChartTopMovies.Datasets.Add(bar);
            gunaChartTopMovies.Update();
        }

        // ==============================================
        // EVENTS
        // ==============================================
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (isFilteringByYear) return;

            if (dtpFrom.Value > dtpTo.Value)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc!");
                dtpFrom.Value = dtpTo.Value;
            }

            ReloadAll();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (isFilteringByYear) return;

            if (dtpTo.Value < dtpFrom.Value)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!");
                dtpTo.Value = dtpFrom.Value;
            }

            ReloadAll();
        }

        // ================================
        // ⭐ EVENT COMBOBOX NĂM — QUAN TRỌNG
        // ================================
        private void YearCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (YearCombo.SelectedItem.ToString() == "Không")
            {
                // Cho phép chọn ngày
                isFilteringByYear = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
            }
            else
            {
                // Khóa DatePicker và tự set theo năm
                isFilteringByYear = true;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;

                int year = int.Parse(YearCombo.SelectedItem.ToString());
                dtpFrom.Value = new DateTime(year, 1, 1);
                dtpTo.Value = new DateTime(year, 12, 31);
            }

            ReloadAll();
        }


        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics2(_parent));
        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics3(_parent));
        }

        private void btnSuatChieu_Click(object sender, EventArgs e)
        {
            _parent.OpenChildForm(new FormStatistics4(_parent));
        }
    }
}


