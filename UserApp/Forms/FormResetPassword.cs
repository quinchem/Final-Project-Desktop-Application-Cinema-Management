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
        // Biến lưu email của người dùng cần đổi mật khẩu
        private string _userEmail;
        // Repo để thao tác với account
        private readonly AccountRepo _accountRepo = new AccountRepo();

        public FormResetPassword()
        {
            InitializeComponent();
        }

        // Lưu tham chiếu đến form cha (FormLogin) và email của người dùng
        private FormLogin parentForm;
        public FormResetPassword(FormLogin parent, string email)
        {
            InitializeComponent();
            parentForm = parent;
            this._userEmail = email;
        }

        // Xử lý khi người dùng bấm nút Gửi để cập nhật mật khẩu mới
        private void btnGui_Click(object sender, EventArgs e)
        {
            // Lấy mật khẩu mới và mật khẩu xác nhận từ textbox, loại bỏ khoảng trắng thừa
            string newPass = txtMKmoi.Text.Trim();
            string confirmPass = txtXacNhanMK.Text.Trim();

            // Kiểm tra đã nhập đầy đủ hay chưa
            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu.");
                return;
            }

            // Kiểm tra mật khẩu xác nhận có khớp không
            if (newPass != confirmPass)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Mật khẩu xác nhận không khớp.");
                return;
            }

            // Kiểm tra điều kiện mật khẩu: tối thiểu tám ký tự và có ký tự đặc biệt
            if (!System.Text.RegularExpressions.Regex.IsMatch(newPass, @"^(?=.{8,})(?=.*\W).*$"))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Mật khẩu phải >= 8 ký tự và có ký tự đặc biệt.");
                return;
            }

            // Gọi repo để đặt lại mật khẩu cho email đã lưu
            if (_accountRepo.ResetPassword(_userEmail, newPass, out string msg))
            {
                // Nếu thành công phát âm thanh và thông báo, quay về giao diện đăng nhập
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.");

                parentForm.ShowLogin();
                this.Close();
            }
            else
            {
                // Nếu thất bại phát âm thanh lỗi và hiển thị lý do
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi: " + msg);
            }
        }

        // Hiển thị/ẩn mật khẩu mới khi click vào icon mắt tương ứng với txtMKmoi
        private void picEye1_Click(object sender, EventArgs e)
        {
            if (txtMKmoi.UseSystemPasswordChar == true)
            {
                // Hiện mật khẩu và đổi icon sang trạng thái xem
                txtMKmoi.UseSystemPasswordChar = false;
                txtMKmoi.PasswordChar = '\0';
                picEye1.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn mật khẩu và đổi icon sang trạng thái ẩn
                txtMKmoi.UseSystemPasswordChar = true;
                picEye1.Image = Properties.Resources.hide;
            }
        }

        // Hiển thị/ẩn mật khẩu xác nhận khi click vào icon mắt tương ứng với txtXacNhanMK
        private void picEye2_Click(object sender, EventArgs e)
        {
            if (txtXacNhanMK.UseSystemPasswordChar == true)
            {
                // Hiện mật khẩu xác nhận và đổi icon sang trạng thái xem
                txtXacNhanMK.UseSystemPasswordChar = false;
                txtXacNhanMK.PasswordChar = '\0';
                picEye2.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn mật khẩu xác nhận và đổi icon sang trạng thái ẩn
                txtXacNhanMK.UseSystemPasswordChar = true;
                picEye2.Image = Properties.Resources.hide;
            }
        }

        // Xử lý khi người dùng bấm Quay lại
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormForgetPassword());
        }
    }
}
