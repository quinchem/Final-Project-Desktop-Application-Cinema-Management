using AdminApp.Repositories;
using AdminApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormEditCustomer : Form
    {
        private readonly CustomerRepository repo = new CustomerRepository();
        private readonly Customer customer;

        // ✅ Constructor nhận khách hàng từ form cha
        public FormEditCustomer(Customer c)
        {
            InitializeComponent();
            CbGioiTinh.Items.Clear();
            CbGioiTinh.Items.AddRange(new string[]
            {
        "Nam",
        "Nữ",
        "Khác"
            });
            CbGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            customer = c;
            LoadCustomerToForm();
        }

        // ✅ Load dữ liệu lên form con
        private void LoadCustomerToForm()
        {
            if (customer == null) return;

            txtTenKH.Text = customer.full_name;
            txtEmail.Text = customer.email;
            txtSDT.Text = customer.phone_number;
            txtDiaChi.Text = customer.address;
            txtThoiGian.Text = customer.create_date;

            if (!string.IsNullOrWhiteSpace(customer.gender))
            {
                CbGioiTinh.SelectedItem = customer.gender;
            }

            // xử lý ngày sinh - chấp nhận nhiều format
            if (!string.IsNullOrWhiteSpace(customer.date_of_birth))
            {
                string[] formats = new[] { "dd/MM/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" };
                if (DateTime.TryParseExact(customer.date_of_birth, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                {
                    dtpNgaySinh.Value = dob;
                }
                else
                {
                    // nếu không parse được thì giữ giá trị mặc định (hoặc log)
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // ✅ Validate
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Họ tên không được để trống");
                return;
            }

            if (CbGioiTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giới tính");
                return;
            }

            // ✅ Lấy dữ liệu
            string fullName = txtTenKH.Text.Trim();
            string gender = CbGioiTinh.SelectedItem.ToString();
            string ngaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            string phone = txtSDT.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtDiaChi.Text.Trim();

            // ✅ Update DB
            bool ok = repo.UpdateCustomer(
                customer.customer_id,
                fullName,
                gender,
                ngaySinh,
                phone,
                email,
                address,
                customer.create_date // giữ nguyên ngày tạo
            );

            if (ok)
            {
                MessageBox.Show("✅ Cập nhật thông tin khách hàng thành công!");
                DialogResult = DialogResult.OK; // báo cho form cha reload
                Close();
            }
            else
            {
                MessageBox.Show("❌ Không thể cập nhật khách hàng!");
            }
        }

    }
}

