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

namespace AdminApp
{
    public partial class FormEditAccount : Form
    {
        private readonly string _staff_id;
        private readonly StaffRepo _staffRepo = new StaffRepo();
        private byte[] _selectedImageBytes;
        private ImageRepo _imageRepo = new ImageRepo();
        private readonly AccountRepo _accountRepo = new AccountRepo();

        public FormEditAccount(string staff_id)
        {
            InitializeComponent();
            _staff_id = staff_id;
            LoadStaffInfo();
        }

        private void LoadStaffInfo()
        {
            txtMKcu.UseSystemPasswordChar = true;
            txtMKmoi.UseSystemPasswordChar = true;
            txtNhapLaiMK.UseSystemPasswordChar = true;

            Staff staff = _staffRepo.GetStaffById(_staff_id);
            if (staff == null)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không tìm thấy nhân viên");
                Close();
                return;
            }

            txtHoTen.Text = staff.full_name;
            txtNgaySinh.Text = staff.date_of_birth;
            cbGioiTinh.Text = staff.gender;
            txtEmail.Text = staff.email;
            txtSDT.Text = staff.phone_number;
            txtChucVu.Text = staff.role;
            txtChucVu.ReadOnly = true; 
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

            bool wantChangePassword =
                !string.IsNullOrWhiteSpace(txtMKcu.Text) ||
                !string.IsNullOrWhiteSpace(txtMKmoi.Text) ||
                !string.IsNullOrWhiteSpace(txtNhapLaiMK.Text);

            //Phần xử lý đổi mật khẩu
            if (wantChangePassword)
            {
                if (string.IsNullOrWhiteSpace(txtMKcu.Text))
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Vui lòng nhập mật khẩu cũ");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMKmoi.Text) ||
                    string.IsNullOrWhiteSpace(txtNhapLaiMK.Text))
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu mới");
                    return;
                }

                // Phần kiểm tra mật khẩu cũ
                bool correctOldPass =
                    _accountRepo.CheckOldPassword(_staff_id, txtMKcu.Text);

                if (!correctOldPass)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Mật khẩu cũ không đúng");
                    return;
                }

                // Phần kiểm tra nhập lại
                if (txtMKmoi.Text != txtNhapLaiMK.Text)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Mật khẩu mới nhập lại không khớp");
                    return;
                }

                _accountRepo.UpdatePassword(_staff_id, txtMKmoi.Text);
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
                {
                    _selectedImageBytes = File.ReadAllBytes(ofd.FileName);

                    using (MemoryStream ms = new MemoryStream(_selectedImageBytes))
                    {
                        picAvatar.Image = Image.FromStream(ms);
                    }
                }
            }
        }
    }
}

