using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using Guna.UI2.WinForms;
using Guna.Charts.WinForms;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AdminApp
{
    public partial class FormStatistics2 : Form
    {
        public FormStatistics2()
        {
            InitializeComponent();
        }

        private void FormStatistics2_Load(object sender, EventArgs e)
        {
            gunaTable.AutoGenerateColumns = false;

            SetupCustomerTableColumns();

            StartTime.Value = DateTime.Now.AddDays(-7);
            EndTime.Value = DateTime.Now;

            LoadYearCombo();
            ReloadAll();
        }
        private void SetupCustomerTableColumns()
        {
            HoTen.DataPropertyName = "HoTen";
            Email.DataPropertyName = "Email";
            SDT.DataPropertyName = "SDT";
            DateOfBirth.DataPropertyName = "DateOfBirth";
            address.DataPropertyName = "Address";
        }


        private void ReloadAll()
        {
            LoadTotalCustomer();
            LoadCustomerByDayChart();
            LoadGenderPie();
            LoadCustomerRanking();
        }
        private void LoadYearCombo()
        {
            YearSort.Items.Clear();
            YearSort.Items.Add("Tất cả");

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
        SELECT DISTINCT strftime('%Y', create_date)
        FROM customer
        WHERE create_date IS NOT NULL
        ORDER BY 1 DESC"
            ;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))   // ✅ chống crash
                {
                    YearSort.Items.Add(reader.GetString(0));
                }
            }

            YearSort.SelectedIndex = 0;
        }


        // =======================
        // 1. Khách mới theo ngày
        // =======================
        private void LoadCustomerByDayChart()
        {
            gunaChartCustomer.Datasets.Clear();
            var bar = new GunaBarDataset { Label = "Khách mới" };

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
    SELECT 
        date(
            CASE 
                WHEN length(create_date) >= 10
                     AND substr(create_date,1,4) BETWEEN '1900' AND '2100'
                THEN create_date
                ELSE NULL
            END
        ) AS Ngay,
        COUNT(*) AS Total
    FROM customer
    WHERE create_date IS NOT NULL
    GROUP BY Ngay
    HAVING Ngay IS NOT NULL
    ORDER BY Ngay";

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;

                bar.DataPoints.Add(
                    rd.GetString(0),
                    rd.GetInt32(1)
                );
            }

            gunaChartCustomer.Datasets.Add(bar);
            gunaChartCustomer.Update();
        }






        // =======================
        // 2. Biểu đồ giới tính
        // =======================
        private void LoadGenderPie()
        {
            gunaChartGender.Datasets.Clear();
            var pie = new GunaPieDataset();
            string yearFilter = YearSort.SelectedItem?.ToString() ?? "Tất cả";

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            if (yearFilter == "Tất cả")
            {
                cmd.CommandText = @"
        SELECT 
            CASE
                WHEN lower(trim(gender)) IN ('nam','male') THEN 'Nam'
                WHEN lower(trim(gender)) IN ('nữ','nu','female') THEN 'Nữ'
                ELSE 'Không rõ'
            END,
            COUNT(*)
        FROM customer
        GROUP BY 1";
            }
            else
            {
                cmd.CommandText = @"
        SELECT 
            CASE
                WHEN lower(trim(gender)) IN ('nam','male') THEN 'Nam'
                WHEN lower(trim(gender)) IN ('nữ','nu','female') THEN 'Nữ'
                ELSE 'Không rõ'
            END,
            COUNT(*)
        FROM customer
        WHERE create_date IS NOT NULL
          AND date(create_date) BETWEEN date(@from) AND date(@to)
          AND strftime('%Y', create_date) = @year
        GROUP BY 1";

                cmd.Parameters.AddWithValue("@from", StartTime.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", EndTime.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@year", yearFilter);
            }

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                pie.DataPoints.Add(rd.GetString(0), rd.GetInt32(1));

            gunaChartGender.Datasets.Add(pie);
            gunaChartGender.Update();
        }




        // =======================
        // 3. Xếp hạng chi tiêu
        // =======================
        private void LoadCustomerRanking()
        {
            var dt = new DataTable();
            string yearFilter = YearSort.SelectedItem?.ToString() ?? "Tất cả";

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();

            // ✅ QUERY CHUNG – đủ cột cho DataGridView
            cmd.CommandText = @"
SELECT 
    c.full_name     AS HoTen,
    c.email         AS Email,
    c.phone_number  AS SDT,
    c.date_of_birth AS DateOfBirth,
    c.address       AS Address,
    SUM(b.total + IFNULL(bd.total,0)) AS ChiTieu
FROM customer c
LEFT JOIN bill b ON c.customer_id = b.customer_id
LEFT JOIN bill_detail bd ON b.bill_id = bd.bill_id
WHERE
    (@year = 'Tất cả'
     OR (
         b.bill_date IS NOT NULL
         AND date(b.bill_date) BETWEEN date(@from) AND date(@to)
         AND strftime('%Y', b.bill_date) = @year
     ))
GROUP BY c.customer_id
ORDER BY ChiTieu DESC";

            cmd.Parameters.AddWithValue("@from", StartTime.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", EndTime.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@year", yearFilter);

            using var reader = cmd.ExecuteReader();
            dt.Load(reader);

            gunaTable.DataSource = dt;
        }
        // =======================
        // 0. Tổng số khách hàng
        // =======================
        private void LoadTotalCustomer()
        {
            string yearFilter = YearSort.SelectedItem?.ToString() ?? "Tất cả";

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            if (yearFilter == "Tất cả")
            {
                cmd.CommandText = "SELECT COUNT(*) FROM customer";
            }
            else
            {
                cmd.CommandText = @"
        SELECT COUNT(*) 
        FROM customer
        WHERE create_date IS NOT NULL
          AND date(create_date) BETWEEN date(@from) AND date(@to)
          AND strftime('%Y', create_date) = @year";

                cmd.Parameters.AddWithValue("@from", StartTime.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", EndTime.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@year", yearFilter);
            }

            lbTongKhachHang.Text = cmd.ExecuteScalar().ToString();
        }


        private void StartTime_ValueChanged(object sender, EventArgs e)
        {
            ReloadAll();
        }

        private void EndTime_ValueChanged(object sender, EventArgs e)
        {
            ReloadAll();
        }

        private void YearSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadAll();
        }
    }
}


