using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedData.Models;
using SharedData.Repositories;

namespace AdminApp.Forms
{
    public partial class FormResetPassword : Form
    {
        private AdminMainForm parentForm;
        private string _userEmail; // Biến lưu email để dùng lúc đổi pass
        public FormResetPassword(AdminMainForm parent, string email)
        {
            InitializeComponent();
            parentForm = parent;
            this._userEmail = email; // Lưu lại email
        }

        private void btnDatLaiMK_Click(object sender, EventArgs e)
        {
            string username = "admin"; // hoặc truyền từ form Quên MK qua
            string newPass = txtMKmoi.Text.Trim();
            string confirmPass = txtXacNhanMK.Text.Trim();

            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp");
                return;
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE Account
                       SET password = @pass
                       WHERE username = @username";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pass", newPass);
                    cmd.Parameters.AddWithValue("@username", username);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Đặt lại mật khẩu thành công!");
                        parentForm.ShowLoginPanel();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản");
                    }
                }
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
    }
}
