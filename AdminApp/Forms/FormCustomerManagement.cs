using ClosedXML.Excel;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Media;
using System.Windows.Forms;


namespace AdminApp
{
    public partial class FormCustomerManagement : Form
    {
        // Repository xử lý dữ liệu khách hàng
        private CustomerRepo repo = new CustomerRepo();
        // Danh sách khách hàng đang load
        private List<Customer> customerList = new List<Customer>();

        public FormCustomerManagement()
        {
            InitializeComponent();

            DataGridViewCustomerManagement.AutoGenerateColumns = false;
            DataGridViewCustomerManagement.ReadOnly = true;
            DataGridViewCustomerManagement.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewCustomerManagement.AllowUserToAddRows = false;
            
            // Thêm cột ID (ẩn) nếu chưa tồn tại
            if (DataGridViewCustomerManagement.Columns["customer_id"] == null)
            {
                DataGridViewCustomerManagement.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    DataPropertyName = "customer_id",
                    Name = "customer_id",
                    HeaderText = "ID",
                    Visible = false
                });
            }
            SetupDateColumns();
            LoadCustomers();
        }

        // Thiết lập format hiển thị cho các cột ngày tháng
        private void SetupDateColumns()
        {
            if (DataGridViewCustomerManagement.Columns["date_of_birth"] != null)
            {
                DataGridViewCustomerManagement.Columns["date_of_birth"].DefaultCellStyle.Format = "dd/MM/yyyy";
                DataGridViewCustomerManagement.Columns["date_of_birth"].DefaultCellStyle.NullValue = "";
            }
            if (DataGridViewCustomerManagement.Columns["create_date"] != null)
            {
                DataGridViewCustomerManagement.Columns["create_date"].DefaultCellStyle.Format = "dd/MM/yyyy";
                DataGridViewCustomerManagement.Columns["create_date"].DefaultCellStyle.NullValue = "";
            }
        }
        // Reload dữ liệu khi form được focus lại
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            LoadCustomers();
        }
        // Load danh sách khách hàng từ CSDL
        private void LoadCustomers()
        {
            customerList = repo.GetAll();
            // Chuẩn hóa định dạng ngày sinh – ngày tạo
            foreach (var customer in customerList)
            {
                if (!string.IsNullOrEmpty(customer.date_of_birth))
                {
                    if (DateTime.TryParse(customer.date_of_birth, out DateTime dob))
                    {
                        customer.date_of_birth = dob.ToString("dd/MM/yyyy");
                    }
                }
                if (!string.IsNullOrEmpty(customer.create_date))
                {
                    if (DateTime.TryParse(customer.create_date, out DateTime cd))
                    {
                        customer.create_date = cd.ToString("dd/MM/yyyy");
                    }
                }
            }

            DataGridViewCustomerManagement.DataSource = null;
            DataGridViewCustomerManagement.DataSource = customerList;
            SetupDateColumns();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            SearchCustomers();
        }
        // Tìm kiếm khách hàng theo từ khóa
        private void SearchCustomers()
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            // Nếu không nhập từ khóa -> hiển thị toàn bộ
            if (string.IsNullOrEmpty(keyword))
            {
                DataGridViewCustomerManagement.DataSource = null;
                DataGridViewCustomerManagement.DataSource = customerList;
                SetupDateColumns();
                return;
            }
            // Lọc theo tên, email, số điện thoại, địa chỉ
            var filtered = customerList.FindAll(c =>
                (c.full_name != null && c.full_name.ToLower().Contains(keyword)) ||
                (c.email != null && c.email.ToLower().Contains(keyword)) ||
                (c.phone_number != null && c.phone_number.ToLower().Contains(keyword)) ||
                (c.address != null && c.address.ToLower().Contains(keyword))
            );

            DataGridViewCustomerManagement.DataSource = null;
            DataGridViewCustomerManagement.DataSource = filtered;
            SetupDateColumns();
            if (filtered.Count == 0)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không tìm thấy khách hàng nào!", "Kết quả tìm kiếm",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchCustomers();
                e.SuppressKeyPress = true; 
                e.Handled = true;          
            }
        }
        // Reset danh sách khi xóa ô tìm kiếm
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                DataGridViewCustomerManagement.DataSource = null;
                DataGridViewCustomerManagement.DataSource = customerList;
                SetupDateColumns();
            }
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            if (customerList == null || customerList.Count == 0)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không có dữ liệu để xuất!");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel File|*.xlsx";
            save.FileName = "DanhSachKhachHang.xlsx";

            if (save.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("KhachHang");

                    sheet.Cell(1, 1).Value = "Mã KH";
                    sheet.Cell(1, 2).Value = "Họ tên";
                    sheet.Cell(1, 3).Value = "Email";
                    sheet.Cell(1, 4).Value = "SĐT";
                    sheet.Cell(1, 5).Value = "Giới tính";
                    sheet.Cell(1, 6).Value = "Ngày sinh";
                    sheet.Cell(1, 7).Value = "Địa chỉ";
                    sheet.Cell(1, 8).Value = "Ngày tạo tài khoản";

                    int row = 2;
                    foreach (var c in customerList)
                    {
                        sheet.Cell(row, 1).Value = c.customer_id;
                        sheet.Cell(row, 2).Value = c.full_name;
                        sheet.Cell(row, 3).Value = c.email;
                        sheet.Cell(row, 4).Value = c.phone_number;
                        sheet.Cell(row, 5).Value = c.gender;
                        sheet.Cell(row, 6).Value = FormatDateForExcel(c.date_of_birth);
                        sheet.Cell(row, 7).Value = c.address;
                        sheet.Cell(row, 8).Value = FormatDateForExcel(c.create_date);

                        row++;
                    }

                    sheet.Columns().AdjustToContents();
                    workbook.SaveAs(save.FileName);
                }

                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Xuất file thành công!");
            }
        }
         // Chuẩn hóa ngày tháng khi ghi Excel
        private string FormatDateForExcel(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
                return "";

            if (DateTime.TryParse(dateStr, out DateTime dt))
            {
                return dt.ToString("dd/MM/yyyy");
            }

            return dateStr;
        }
        // Mở form chỉnh sửa khách hàng
        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            if (DataGridViewCustomerManagement.SelectedRows.Count == 0)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn 1 khách hàng!");
                return;
            }

            var row = DataGridViewCustomerManagement.SelectedRows[0];

            Customer c = new Customer
            {
                customer_id = Convert.ToString(row.Cells["customer_id"].Value),
                full_name = Convert.ToString(row.Cells["full_name"].Value),
                gender = Convert.ToString(row.Cells["gender"].Value),
                date_of_birth = Convert.ToString(row.Cells["date_of_birth"].Value),
                phone_number = Convert.ToString(row.Cells["phone_number"].Value),
                email = Convert.ToString(row.Cells["email"].Value),
                address = Convert.ToString(row.Cells["address"].Value),
                create_date = Convert.ToString(row.Cells["create_date"].Value)
            };

            using (FormEditCustomer f = new FormEditCustomer(c))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers();
                }
            }
        }

        private void DataGridViewCustomerManagement_CellEndEdit(
            object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewCustomerManagement.Columns[e.ColumnIndex].Name == "customer_id")
                return;

            try
            {
                var row = DataGridViewCustomerManagement.Rows[e.RowIndex];
                if (row.Cells["customer_id"].Value == null) return;

                string id = Convert.ToString(row.Cells["customer_id"].Value);
                if (string.IsNullOrWhiteSpace(id)) return;
                string fullName = row.Cells["full_name"].Value?.ToString() ?? "";
                string gender = row.Cells["gender"].Value?.ToString() ?? "";
                string phone = row.Cells["phone_number"].Value?.ToString() ?? "";
                string email = row.Cells["email"].Value?.ToString() ?? "";
                string address = row.Cells["address"].Value?.ToString() ?? "";
                string birth = row.Cells["date_of_birth"].Value?.ToString() ?? "";
                if (!TryNormalizeDate(birth, out string birthFormatted))
                {
                    MessageBox.Show("Ngày sinh không đúng định dạng dd/MM/yyyy");
                    LoadCustomers();
                    return;
                }

                string createDate = row.Cells["create_date"].Value?.ToString() ?? "";
                if (!TryNormalizeDate(createDate, out string createFormatted))
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Ngày tạo không đúng định dạng dd/MM/yyyy");
                    LoadCustomers();
                    return;
                }
                Customer c = new Customer
                {
                    customer_id = id,
                    full_name = fullName,
                    gender = gender,
                    phone_number = phone,
                    email = email,
                    address = address,
                    date_of_birth = birthFormatted,
                    create_date = createFormatted
                };

                bool ok = repo.Update(c);

                if (!ok)

                    MessageBox.Show("Cập nhật thất bại!");
                else
                    LoadCustomers(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        // Kiểm tra và chuẩn hóa định dạng ngày tháng
        private bool TryNormalizeDate(string input, out string output)
        {
            output = "";

            if (string.IsNullOrWhiteSpace(input))
                return true;

            string[] formats = new[] {
                "dd/MM/yyyy",
                "dd-MM-yyyy",
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd"
            };

            if (DateTime.TryParseExact(
                input,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime d))
            {
                output = d.ToString("dd/MM/yyyy");
                return true;
            }

            if (DateTime.TryParse(input, out d))
            {
                output = d.ToString("dd/MM/yyyy");
                return true;
            }

            return false;
        }
    }
}
