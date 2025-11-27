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
using System.IO;
using SharedData.Repositories;

namespace AdminApp
{
    public partial class FormEditAccount : Form
    {
        private readonly string _staff_id;
        private readonly StaffRepo _staffRepo = new StaffRepo();
        private byte[] _selectedImageBytes;
        private ImageRepo _imageRepo = new ImageRepo();

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
                MessageBox.Show("Họ tên không được để trống");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtMKmoi.Text) ||
                !string.IsNullOrWhiteSpace(txtNhapLaiMK.Text))
            {
                if (txtMKmoi.Text != txtNhapLaiMK.Text)
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp");
                    return;
                }
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
                // ✅ Lưu ảnh nếu có chọn
                if (_selectedImageBytes != null)
                {
                    _imageRepo.SaveStaffImage(_staff_id, _selectedImageBytes);
                }

                MessageBox.Show("Cập nhật thành công");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
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

