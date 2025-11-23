using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class AdminCustomerManagement : Form
    {
        public AdminCustomerManagement()
        {
            InitializeComponent();
            DataGridViewCustomerManagement.AutoGenerateColumns = false;

            HoTen.DataPropertyName = "full_name";
            GioiTinh.DataPropertyName = "gender";
            NgaySinh.DataPropertyName = "date_of_birth";
            SĐT.DataPropertyName = "phone_number";
            Email.DataPropertyName = "email";
            DiaChi.DataPropertyName = "address";
            ThoiGianTaoTK.DataPropertyName = "create_date";
        }

        private void LoadCustomerData()
        {
            try
            {
                using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    var test = new SqliteCommand("SELECT COUNT(*) FROM customer", conn);
                    int count = Convert.ToInt32(test.ExecuteScalar());
                    MessageBox.Show("Số dòng trong bảng customer = " + count);

                    string query = @"SELECT 
                    full_name,
                    gender,
                    date_of_birth,
                    phone_number,
                    email,
                    address,
                    create_date
                 FROM customer";


                    var cmd = new SqliteCommand(query, conn);
                    var reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    DataGridViewCustomerManagement.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu khách hàng: " + ex.Message);
            }
            MessageBox.Show("Đang mở file DB:\n" + DatabaseHelper.GetConnectionString());



        }

        private void AdminCustomerManagement_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
            

        }
    }
}
