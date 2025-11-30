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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AdminApp
{
    public partial class FormStatistics1 : Form
    {
        public FormStatistics1()
        {
            InitializeComponent();
        }

        private void FormStatistics1_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;

            LoadYearCombo();
            ReloadAll();
        }

        private void LoadYearCombo()
        {
            YearCombo.Items.Clear();
            YearCombo.Items.Add("Tất cả");

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT DISTINCT strftime('%Y', bill_date) AS Year
                FROM bill
                WHERE bill_date IS NOT NULL
                ORDER BY Year DESC";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    YearCombo.Items.Add(reader.GetString(0));
            }

            YearCombo.SelectedIndex = 0;
        }

        private void ReloadAll()
        {
            LoadRevenueKPI();
            LoadRevenueByDayChart();
            LoadTopMoviesChart();
        }

        // =========================
        // KPI
        // =========================
        private void LoadRevenueKPI()
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                // Tổng doanh thu, số vé, doanh thu trung bình/ngày (theo khoảng thời gian from-to)
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
                        lblTotalRevenue.Text = reader["TotalRevenue"] != DBNull.Value
                            ? Convert.ToDecimal(reader["TotalRevenue"]).ToString("N0") + " VND"
                            : "0 VND";

                        lblTotalTickets.Text = reader["TotalTickets"] != DBNull.Value
                            ? reader["TotalTickets"].ToString()
                            : "0";

                        lblAvgRevenue.Text = reader["AvgRevenuePerDay"] != DBNull.Value
                            ? Convert.ToDecimal(reader["AvgRevenuePerDay"]).ToString("N0") + " VND"
                            : "0 VND";
                    }
                }

                // Phim nổi trội nhất
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
                    lblTopMovie.MaximumSize = new Size(200, 0); // 400 là bề rộng label, có thể chỉnh
                }

                // Khách hàng mới
                cmd.CommandText = @"
            SELECT COUNT(*)
            FROM customer
            WHERE create_date IS NOT NULL
                AND substr(create_date, 7, 4) || '-' || 
                    substr(create_date, 4, 2) || '-' || 
                    substr(create_date, 1, 2) 
                BETWEEN @from AND @to";

                using (var reader = cmd.ExecuteReader())
                {
                    lblNewCustomer.Text = reader.Read() ? reader.GetInt32(0).ToString() : "0";
                }
            }
        }

        // =========================
        // Line chart: doanh thu theo ngày
        // =========================
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
        SELECT 
            bill_date AS Ngay, 
            SUM(total) AS Total
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
                string date = reader.GetString(0);
                decimal total = reader.GetDecimal(1);
                line.DataPoints.Add(date, (double)total);
            }

            gunaChartRevenue.Datasets.Add(line);
            gunaChartRevenue.Update();
        }

        // =========================
        // Bar chart: top 5 phim
        // =========================
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
                string movie = reader.GetString(0);
                decimal total = reader.GetDecimal(1);
                bar.DataPoints.Add(movie, (double)total);
            }

            gunaChartTopMovies.Datasets.Add(bar);
            gunaChartTopMovies.Update();
        }



        // =========================
        // Event: thay đổi thời gian / năm
        // =========================
        private void dtpFrom_ValueChanged(object sender, EventArgs e) => ReloadAll();
        private void dtpTo_ValueChanged(object sender, EventArgs e) => ReloadAll();
        private void YearCombo_SelectedIndexChanged(object sender, EventArgs e) => ReloadAll();
    }
}

