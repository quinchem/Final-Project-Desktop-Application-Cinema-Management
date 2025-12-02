using Microsoft.Data.Sqlite;
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

namespace UserApp
{
    public partial class FormResetPassword : Form
    {
        private string _userEmail; 
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
            this._userEmail = email; 
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            string newPass = txtMKmoi.Text.Trim();
            string confirmPass = txtXacNhanMK.Text.Trim();

            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu.");
                return;
            }

            if (newPass != confirmPass)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Mật khẩu xác nhận không khớp.");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(newPass, @"^(?=.{8,})(?=.*\W).*$"))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Mật khẩu phải >= 8 ký tự và có ký tự đặc biệt.");
                return;
            }
            if (_accountRepo.ResetPassword(_userEmail, newPass, out string msg))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.");

                parentForm.ShowLogin();
                this.Close();
            }
            else
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi: " + msg);
            }
        }

        private void picEye1_Click(object sender, EventArgs e)
        {
            if (txtMKmoi.UseSystemPasswordChar == true)
            {
                // Hiện lên và đổi ảnh mở
                txtMKmoi.UseSystemPasswordChar = false;
                txtMKmoi.PasswordChar = '\0';
                picEye1.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn đi và đổi ảnh đóng
                txtMKmoi.UseSystemPasswordChar = true;

                // Đổi ảnh đóng ở đây
                picEye1.Image = Properties.Resources.hide; 
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
                // Ẩn đi và đổi ảnh đóng
                txtXacNhanMK.UseSystemPasswordChar = true;

                // Đổi ảnh đóng ở đây
                picEye2.Image = Properties.Resources.hide; 
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormForgetPassword());
        }
    }
}
