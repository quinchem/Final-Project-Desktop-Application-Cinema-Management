using SharedData.Models;
using SharedData.Repositories;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace AdminApp
{
    public partial class FormEditAccount : Form
    {
        // Lưu staff_id của nhân viên đang đăng nhập
        private readonly string _staff_id;
        // Lưu account_id tương ứng với staff_id (phục vụ đổi mật khẩu)
        private string _account_id; 
        // Repository thao tác dữ liệu nhân viên
        private readonly StaffRepo _staffRepo = new StaffRepo();
        // Lưu ảnh đại diện mới được chọn
        private byte[] _selectedImageBytes;
        // Repository thao tác hình ảnh nhân viên
        private ImageRepo _imageRepo = new ImageRepo();
        // Repository thao tác dữ liệu tài khoản (mật khẩu)
        private readonly AccountRepo _accountRepo = new AccountRepo();

        public FormEditAccount(string staff_id)
        {
            InitializeComponent();
            _staff_id = staff_id;
            LoadStaffInfo();
        }

        private void LoadStaffInfo()
        {    // Ẩn ký tự nhập mật khẩu
            txtMKcu.UseSystemPasswordChar = true;
            txtMKmoi.UseSystemPasswordChar = true;
            txtNhapLaiMK.UseSystemPasswordChar = true;

            // Lấy account_id tương ứng với staff_id
            using var conn = new SqliteConnection(DatabaseHelper.GetConnectionString());
            conn.Open();
            using var cmd = new SqliteCommand(
                "SELECT account_id FROM account WHERE staff_id=@sid", conn);
            cmd.Parameters.AddWithValue("@sid", _staff_id);
            _account_id = cmd.ExecuteScalar()?.ToString();
            // Nếu không tìm thấy tài khoản
            if (string.IsNullOrEmpty(_account_id))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không tìm thấy thông tin tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
             // Lấy thông tin nhân viên
            Staff staff = _staffRepo.GetStaffById(_staff_id);
            if (staff == null)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không tìm thấy nhân viên");
                Close();
                return;
            }
            // Hiển thị thông tin lên giao diện
            txtHoTen.Text = staff.full_name;
            txtNgaySinh.Text = staff.date_of_birth;
            cbGioiTinh.Text = staff.gender;
            txtEmail.Text = staff.email;
            txtSDT.Text = staff.phone_number;
            txtChucVu.Text = staff.role;
            txtChucVu.ReadOnly = true;
            // Load ảnh đại diện (nếu có)
            byte[] img = _imageRepo.GetStaffImage(_staff_id);
            if (img != null)
            {
                using (MemoryStream ms = new MemoryStream(img))
                {
                    picAvatar.Image = Image.FromStream(ms);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Họ tên không được để trống");
                return;
            }
            // Kiểm tra người dùng có muốn đổi mật khẩu hay không
            bool wantChangePassword =
                !string.IsNullOrWhiteSpace(txtMKcu.Text) ||
                !string.IsNullOrWhiteSpace(txtMKmoi.Text) ||
                !string.IsNullOrWhiteSpace(txtNhapLaiMK.Text);

            //Phần xử lý đổi mật khẩu
            if (wantChangePassword)
            {   // Kiểm tra mật khẩu cũ
                if (string.IsNullOrWhiteSpace(txtMKcu.Text))
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Vui lòng nhập mật khẩu cũ");
                    return;
                }
                 // Kiểm tra mật khẩu mới
                if (string.IsNullOrWhiteSpace(txtMKmoi.Text) ||
                    string.IsNullOrWhiteSpace(txtNhapLaiMK.Text))
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu mới");
                    return;
                }

                // Kiểm tra tính đúng đắn của mật khẩu cũ
                bool correctOldPass = _accountRepo.CheckOldPassword(_account_id, txtMKcu.Text);

                if (!correctOldPass)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Mật khẩu cũ không đúng");
                    return;
                }

                // Kiểm tra nhập lại mật khẩu
                if (txtMKmoi.Text != txtNhapLaiMK.Text)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Mật khẩu mới nhập lại không khớp");
                    return;
                }

                // Cập nhật mật khẩu mới
                _accountRepo.UpdatePassword(_account_id, txtMKmoi.Text);
            }

            Staff staff = new Staff
            {
                staff_id = _staff_id,
                full_name = txtHoTen.Text.Trim(),
                date_of_birth = txtNgaySinh.Text.Trim(),
                gender = cbGioiTinh.Text.Trim(),
                email = txtEmail.Text.Trim(),
                phone_number = txtSDT.Text.Trim(),
                role = txtChucVu.Text.Trim()
            };
             // Thực hiện cập nhật
            if (_staffRepo.UpdateStaff(staff))
            {
                if (_selectedImageBytes != null)
                {
                    _imageRepo.SaveStaffImage(_staff_id, _selectedImageBytes);
                }

                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Cập nhật thành công");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Cập nhật thất bại");
            }
        }


        private void btnTaiAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {   // Đọc ảnh thành mảng byte
                    _selectedImageBytes = File.ReadAllBytes(ofd.FileName);
                    // Hiển thị ảnh lên giao diện
                    using (MemoryStream ms = new MemoryStream(_selectedImageBytes))
                    {
                        picAvatar.Image = Image.FromStream(ms);
                    }
                }
            }
        }
    }
}
