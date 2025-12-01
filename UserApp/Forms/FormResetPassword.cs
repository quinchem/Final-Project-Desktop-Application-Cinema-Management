using Microsoft.Data.Sqlite;
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

namespace UserApp
{
    public partial class FormResetPassword : Form
    {
        private string _userEmail; // Biến lưu email để dùng lúc đổi pass
        private readonly AccountRepo _accountRepo = new AccountRepo();
        public FormResetPassword()
        {
            InitializeComponent();
        }
        private FormLogin parentForm;
        public FormResetPassword(FormLogin parent, string email)
        {
            InitializeComponent();
            parentForm = parent;
            this._userEmail = email; // Lưu lại email
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào
            string newPass = txtMKmoi.Text.Trim();
            string confirmPass = txtXacNhanMK.Text.Trim();

            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu.");
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.");
                return;
            }

            // Validate độ mạnh mật khẩu (nếu cần giống lúc đăng ký)
            if (!System.Text.RegularExpressions.Regex.IsMatch(newPass, @"^(?=.{8,})(?=.*\W).*$"))
            {
                MessageBox.Show("Mật khẩu phải >= 8 ký tự và có ký tự đặc biệt.");
                return;
            }

            // --- GỌI HÀM MỚI VIẾT TRONG REPO ---
            // Truyền _userEmail (biến lưu email từ form trước) và mật khẩu mới
            if (_accountRepo.ResetPassword(_userEmail, newPass, out string msg))
            {
                MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.");

                parentForm.ShowLogin(); // Quay về login
                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi: " + msg);
            }
        }

        private void picEye1_Click(object sender, EventArgs e)
        {
            if (txtMKmoi.UseSystemPasswordChar == true)
            {
                // Hiện lên + Đổi ảnh mở
                txtMKmoi.UseSystemPasswordChar = false;
                txtMKmoi.PasswordChar = '\0';
                picEye1.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn đi + Đổi ảnh đóng
                txtMKmoi.UseSystemPasswordChar = true;

                // Đổi ảnh đóng ở đây
                picEye1.Image = Properties.Resources.hide; // <--- THÊM DÒNG NÀY VÀO
            }
        }

        private void picEye2_Click(object sender, EventArgs e)
        {
            if (txtXacNhanMK.UseSystemPasswordChar == true)
            {
                // Hiện lên + Đổi ảnh mở
                txtXacNhanMK.UseSystemPasswordChar = false;
                txtXacNhanMK.PasswordChar = '\0';
                picEye2.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn đi + Đổi ảnh đóng
                txtXacNhanMK.UseSystemPasswordChar = true;

                // Đổi ảnh đóng ở đây
                picEye2.Image = Properties.Resources.hide; // <--- THÊM DÒNG NÀY VÀO
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormForgetPassword());
        }
    }
}
