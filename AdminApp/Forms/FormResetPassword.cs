using Microsoft.Data.Sqlite;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp.Forms
{
    public partial class FormResetPassword : Form
    {
        // Form cha (AdminMainForm) để quay về màn hình đăng nhập sau khi reset
        private AdminMainForm parentForm;
        // Email người dùng (được truyền từ bước trước – hiện chưa dùng)
        private string _userEmail;
        public FormResetPassword(AdminMainForm parent, string email)
        {
            InitializeComponent();
            parentForm = parent;
            this._userEmail = email;
        }

        private void btnDatLaiMK_Click(object sender, EventArgs e)
        {
            string username = "admin"; 
            string newPass = txtMKmoi.Text.Trim();
            string confirmPass = txtXacNhanMK.Text.Trim();
            // Kiểm tra nhập thiếu dữ liệu
            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            // Kiểm tra mật khẩu xác nhận
            if (newPass != confirmPass)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Mật khẩu xác nhận không khớp");
                return;
            }
            // Kết nối CSDL và cập nhật mật khẩu
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
                        SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                        player.Play();
                        MessageBox.Show("Đặt lại mật khẩu thành công!");
                        parentForm.ShowLoginPanel();
                        this.Close();
                    }
                    else
                    {
                        SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                        player.Play();
                        MessageBox.Show("Không tìm thấy tài khoản");
                    }
                }
            }
        }
         // Hiện / Ẩn mật khẩu mới
        private void picEye1_Click(object sender, EventArgs e)
        {
            if (txtMKmoi.UseSystemPasswordChar == true)
            {
                txtMKmoi.UseSystemPasswordChar = false;
                txtMKmoi.PasswordChar = '\0';
                picEye1.Image = Properties.Resources.view;
            }
            else
            {
                txtMKmoi.UseSystemPasswordChar = true;
                picEye1.Image = Properties.Resources.hide; 
            }
        }
        // Hiện / Ẩn mật khẩu xác nhận
        private void picEye2_Click(object sender, EventArgs e)
        {
            if (txtXacNhanMK.UseSystemPasswordChar == true)
            {
                txtXacNhanMK.UseSystemPasswordChar = false;
                txtXacNhanMK.PasswordChar = '\0';
                picEye2.Image = Properties.Resources.view;
            }
            else
            {
                txtXacNhanMK.UseSystemPasswordChar = true;
                picEye2.Image = Properties.Resources.hide; 
            }
        }
    }
}
