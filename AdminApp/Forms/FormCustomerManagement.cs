using AdminApp.Models;
using AdminApp.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;

namespace AdminApp
{
    public partial class FormCustomerManagement : Form
    {
        private CustomerRepository repo = new CustomerRepository();
        private List<Customer> customerList = new List<Customer>();

        public FormCustomerManagement()
        {
            InitializeComponent();
            DataGridViewCustomerManagement.Columns.Add("customer_id", "ID");
            DataGridViewCustomerManagement.Columns["customer_id"].Visible = false;
            DataGridViewCustomerManagement.AutoGenerateColumns = false;
            LoadCustomers();

        }

        private void LoadCustomers()
        {
            customerList = repo.GetAllCustomers();
            DataGridViewCustomerManagement.DataSource = customerList;
            if (DataGridViewCustomerManagement.Columns["date_of_birth"] != null)
                DataGridViewCustomerManagement.Columns["date_of_birth"].DefaultCellStyle.Format = "dd/MM/yyyy";

            if (DataGridViewCustomerManagement.Columns["create_date"] != null)
                DataGridViewCustomerManagement.Columns["create_date"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }
        private void SearchCustomers()
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                DataGridViewCustomerManagement.DataSource = customerList;
                return;
            }

            var filtered = customerList.FindAll(c =>
                c.full_name.ToLower().Contains(keyword) ||
                c.email.ToLower().Contains(keyword) ||
                c.phone_number.ToLower().Contains(keyword) ||
                c.address.ToLower().Contains(keyword)
            );

            DataGridViewCustomerManagement.DataSource = filtered;
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            SearchCustomers();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (DataGridViewCustomerManagement.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            string id = DataGridViewCustomerManagement.SelectedRows[0].Cells["customer_id"].Value.ToString();

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa khách hàng này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                bool success = repo.DeleteCustomer(id);
                if (success)
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadCustomers();     // load lại danh sách
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!");
                }
            }
        }

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            if (customerList == null || customerList.Count == 0)
            {
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
                        sheet.Cell(row, 6).Value = c.date_of_birth;
                        sheet.Cell(row, 7).Value = c.address;
                        sheet.Cell(row, 8).Value = c.create_date;
                        row++;
                    }

                    sheet.Columns().AdjustToContents();

                    workbook.SaveAs(save.FileName);
                }

                MessageBox.Show("Xuất file thành công!");
            }
        }

        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            DataGridViewCustomerManagement.ReadOnly = false;

            MessageBox.Show(
                "Bạn đã bật chế độ chỉnh sửa.\n" +
                "Giờ bạn có thể sửa nhiều dòng cùng lúc.\n" +
                "Dữ liệu sẽ tự động lưu khi bạn rời ô hoặc nhấn Enter."
            );
        }

        private void DataGridViewCustomerManagement_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var row = DataGridViewCustomerManagement.Rows[e.RowIndex];

                // Nếu ID null => không update được
                if (row.Cells["customer_id"].Value == null)
                    return;
                if (e.ColumnIndex == DataGridViewCustomerManagement.Columns["date_of_birth"].Index)
                {
                    if (DateTime.TryParse(row.Cells["date_of_birth"].Value?.ToString(), out DateTime d))
                        row.Cells["date_of_birth"].Value = d.ToString("dd/MM/yyyy");
                }

                // ✔ Dòng bạn hỏi nằm ở đây
                string id = row.Cells["customer_id"].Value.ToString();

                string fullName = row.Cells["full_name"].Value?.ToString() ?? "";
                string gender = row.Cells["gender"].Value?.ToString() ?? "";
                string birth = row.Cells["date_of_birth"].Value?.ToString() ?? "";
                string phone = row.Cells["phone_number"].Value?.ToString() ?? "";
                string email = row.Cells["email"].Value?.ToString() ?? "";
                string address = row.Cells["address"].Value?.ToString() ?? "";
                string createDate = row.Cells["create_date"].Value?.ToString() ?? "";

                bool ok = repo.UpdateCustomer(id, fullName, gender, birth, phone, email, address, createDate);

                if (!ok)
                    MessageBox.Show("Cập nhật thất bại!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

    }
}