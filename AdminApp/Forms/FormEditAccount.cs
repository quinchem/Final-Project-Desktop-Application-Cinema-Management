using AdminApp.Models;
using AdminApp.Repositories;
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
    public partial class FormEditAccount : Form
    {
        private readonly string _staff_id;
        private readonly StaffRepo _staffRepo = new StaffRepo();

        public FormEditAccount(string staff_id)
        {
            InitializeComponent();
            _staff_id = staff_id;
            LoadStaffInfo();
        }

        private void LoadStaffInfo()
        {
            Staff staff = _staffRepo.GetStaffById(_staff_id);
            if (staff == null)
            {
                MessageBox.Show("Không tìm thấy nhân viên");
                Close();
                return;
            }

            txtHoTen.Text = staff.full_name;
            txtNgaySinh.Text = staff.date_of_birth;
            txtGioiTinh.Text = staff.gender;
            txtEmail.Text = staff.email;
            txtSDT.Text = staff.phone_number;
            txtChucVu.Text = staff.role;

            txtChucVu.ReadOnly = true; // không cho sửa chức vụ
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống");
                return;
            }

            // ✅ kiểm tra mật khẩu nếu có nhập
            if (!string.IsNullOrWhiteSpace(txtMKmoi.Text) ||
                !string.IsNullOrWhiteSpace(txtNhapLaiMK.Text))
            {
                if (txtMKmoi.Text != txtNhapLaiMK.Text)
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp");
                    return;
                }

                // TODO: verify mật khẩu cũ nếu có bảng Account
            }

            Staff staff = new Staff
            {
                staff_id = _staff_id,
                full_name = txtHoTen.Text.Trim(),
                date_of_birth = txtNgaySinh.Text.Trim(),
                gender = txtGioiTinh.Text.Trim(),
                email = txtEmail.Text.Trim(),
                phone_number = txtSDT.Text.Trim(),
                role = txtChucVu.Text.Trim()
            };

            if (_staffRepo.UpdateStaff(staff))
            {
                MessageBox.Show("Cập nhật thành công");
                DialogResult = DialogResult.OK; // ✅ báo cho form cha
                Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }
    }
}