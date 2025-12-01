using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using Guna.UI2.WinForms;
using Guna.Charts.WinForms;

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
            YearSort.BeginUpdate();
            YearSort.Items.Clear();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();

                // ✅ Query các năm từ create_date (DATETIME)
                cmd.CommandText = @"
SELECT DISTINCT strftime('%Y', create_date) AS Y
FROM customer
WHERE create_date IS NOT NULL
ORDER BY Y DESC";

                using var rd = cmd.ExecuteReader();

                YearSort.Items.Add("Tất cả");

                while (rd.Read())
                {
                    if (!rd.IsDBNull(0))
                    {
                        string year = rd.GetString(0);
                        YearSort.Items.Add(year);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load năm: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                YearSort.EndUpdate();
            }

            if (YearSort.Items.Count > 0)
                YearSort.SelectedIndex = 0;
        }




        // =======================
        // 1. Khách mới theo tháng (Biểu đồ cột)
        // =======================
        private void LoadCustomerByDayChart()
        {
            gunaChartCustomer.Datasets.Clear();
            GetCreateDateRange(out DateTime? from, out DateTime? to);

            var bar = new GunaBarDataset
            {
                Label = "Khách đăng ký"
            };

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            if (from == null)
            {
                // ✅ TẤT CẢ - Hiển thị theo tháng/năm (MM/yyyy)
                cmd.CommandText = @"
SELECT 
    strftime('%Y-%m', create_date) AS Thang,
    COUNT(*) AS Total
FROM customer
WHERE create_date IS NOT NULL
GROUP BY strftime('%Y-%m', create_date)
ORDER BY Thang";
            }
            else
            {
                // ✅ THEO NĂM - Hiển thị 12 tháng trong năm (01, 02, ..., 12)
                cmd.CommandText = @"
SELECT 
    strftime('%m', create_date) AS Thang,
    COUNT(*) AS Total
FROM customer
WHERE create_date IS NOT NULL
  AND date(create_date) BETWEEN date(@from) AND date(@to)
GROUP BY strftime('%m', create_date)
ORDER BY Thang";

                cmd.Parameters.AddWithValue("@from", from.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", to.Value.ToString("yyyy-MM-dd"));
            }

            // ✅ Đọc dữ liệu từ database
            var monthData = new System.Collections.Generic.Dictionary<string, int>();
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    if (rd.IsDBNull(0) || rd.IsDBNull(1)) continue;
                    monthData[rd.GetString(0)] = rd.GetInt32(1);
                }
            }

            // ✅ Hiển thị dữ liệu lên chart
            if (from == null)
            {
                // Hiển thị theo format "MM/yyyy"
                foreach (var item in monthData)
                {
                    // Convert "2024-01" thành "T1/2024"
                    string[] parts = item.Key.Split('-');
                    string label = $"T{int.Parse(parts[1])}/{parts[0]}";
                    bar.DataPoints.Add(label, item.Value);
                }
            }
            else
            {
                // Hiển thị 12 tháng đầy đủ (01 -> 12)
                string[] monthNames = { "T1", "T2", "T3", "T4", "T5", "T6",
                                       "T7", "T8", "T9", "T10", "T11", "T12" };

                for (int i = 1; i <= 12; i++)
                {
                    string monthKey = i.ToString("00"); // 01, 02, ..., 12
                    int count = monthData.ContainsKey(monthKey) ? monthData[monthKey] : 0;
                    bar.DataPoints.Add(monthNames[i - 1], count);
                }
            }

            // ✅ ĐẶT MÀU SAU KHI THÊM DATAPOINTS
            bar.FillColors.Clear();
            bar.FillColors.Add(Color.FromArgb(94, 114, 228)); // Xanh primary

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

            GetCreateDateRange(out DateTime? from, out DateTime? to);

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            if (from == null)
            {
                cmd.CommandText = @"
SELECT 
    CASE
        WHEN lower(trim(gender)) IN ('nam','male') THEN 'Nam'
        WHEN lower(trim(gender)) IN ('nữ','nu','female') THEN 'Nữ'
        ELSE 'Không rõ'
    END AS GioiTinh,
    COUNT(*) AS Total
FROM customer
WHERE create_date IS NOT NULL
GROUP BY GioiTinh";
            }
            else
            {
                cmd.CommandText = @"
SELECT 
    CASE
        WHEN lower(trim(gender)) IN ('nam','male') THEN 'Nam'
        WHEN lower(trim(gender)) IN ('nữ','nu','female') THEN 'Nữ'
        ELSE 'Không rõ'
    END AS GioiTinh,
    COUNT(*) AS Total
FROM customer
WHERE create_date IS NOT NULL
  AND date(create_date) BETWEEN date(@from) AND date(@to)
GROUP BY GioiTinh";

                cmd.Parameters.AddWithValue("@from", from.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", to.Value.ToString("yyyy-MM-dd"));
            }

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (rd.IsDBNull(0) || rd.IsDBNull(1)) continue;
                pie.DataPoints.Add(rd.GetString(0), rd.GetInt32(1));
            }

            // ✅ ĐẶT MÀU SAU KHI THÊM DATAPOINTS
            pie.FillColors.Clear();
            pie.FillColors.Add(Color.FromArgb(94, 114, 228));   // Nam - Xanh primary
            pie.FillColors.Add(Color.FromArgb(255, 107, 129));  // Nữ - Hồng
            pie.FillColors.Add(Color.FromArgb(189, 195, 199));  // Không rõ - Xám

            gunaChartGender.Datasets.Add(pie);
            gunaChartGender.Update();
        }


        // =======================
        // 3. Xếp hạng chi tiêu
        // =======================
        private void LoadCustomerRanking()
        {
            var dt = new DataTable();
            GetCreateDateRange(out DateTime? from, out DateTime? to);

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            if (from == null)
            {
                cmd.CommandText = @"
SELECT 
    c.full_name     AS HoTen,
    c.email         AS Email,
    c.phone_number  AS SDT,
    c.date_of_birth AS DateOfBirth,
    c.address       AS Address,
    IFNULL(SUM(b.total),0) AS ChiTieu
FROM customer c
LEFT JOIN bill b ON c.customer_id = b.customer_id
WHERE c.create_date IS NOT NULL
GROUP BY c.customer_id
ORDER BY ChiTieu DESC";
            }
            else
            {
                cmd.CommandText = @"
SELECT 
    c.full_name     AS HoTen,
    c.email         AS Email,
    c.phone_number  AS SDT,
    c.date_of_birth AS DateOfBirth,
    c.address       AS Address,
    IFNULL(SUM(b.total),0) AS ChiTieu
FROM customer c
LEFT JOIN bill b ON c.customer_id = b.customer_id
WHERE c.create_date IS NOT NULL
  AND date(c.create_date) BETWEEN date(@from) AND date(@to)
GROUP BY c.customer_id
ORDER BY ChiTieu DESC";

                cmd.Parameters.AddWithValue("@from", from.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", to.Value.ToString("yyyy-MM-dd"));
            }

            using var reader = cmd.ExecuteReader();
            dt.Load(reader);
            gunaTable.DataSource = dt;
        }


        // =======================
        // 0. Tổng số khách hàng
        // =======================
        private void LoadTotalCustomer()
        {
            GetCreateDateRange(out DateTime? from, out DateTime? to);

            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            if (from == null)
            {
                cmd.CommandText = @"
SELECT COUNT(*) 
FROM customer 
WHERE create_date IS NOT NULL";
            }
            else
            {
                cmd.CommandText = @"
SELECT COUNT(*) 
FROM customer
WHERE create_date IS NOT NULL
  AND date(create_date) BETWEEN date(@from) AND date(@to)";

                cmd.Parameters.AddWithValue("@from", from.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", to.Value.ToString("yyyy-MM-dd"));
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

        private void GetCreateDateRange(out DateTime? from, out DateTime? to)
        {
            string year = YearSort.SelectedItem?.ToString() ?? "Tất cả";

            if (year == "Tất cả")
            {
                from = null;
                to = null;
            }
            else
            {
                if (int.TryParse(year, out int y))
                {
                    from = new DateTime(y, 1, 1);
                    to = new DateTime(y, 12, 31);
                }
                else
                {
                    from = null;
                    to = null;
                }
            }
        }
    }
}