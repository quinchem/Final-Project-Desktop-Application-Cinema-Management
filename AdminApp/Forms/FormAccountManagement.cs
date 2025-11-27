using AdminApp.Models;
using AdminApp.Repositories;
using SharedData.Repositories;
using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace AdminApp
{
    public partial class FormAccountManagement : Form
    {
        private readonly string _staff_id;
        private readonly StaffRepo _staffRepo = new StaffRepo();
        private bool _isEditing = false;
        private readonly ImageRepo _imageRepo = new ImageRepo();

        public FormAccountManagement(string staff_id)
        {
            InitializeComponent();
            _staff_id = staff_id;

            SetReadOnly(true);
            LoadStaffInfo();
        }

        private void LoadStaffInfo()
        {
            Staff staff = _staffRepo.GetStaffById(_staff_id);

            if (staff == null)
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên.");
                Close();
                return;
            }

            txtHoTen.Text = staff.full_name;
            txtNgaySinh.Text = staff.date_of_birth;
            txtGioiTinh.Text = staff.gender;
            txtEmail.Text = staff.email;
            txtSDT.Text = staff.phone_number;
            txtChucVu.Text = staff.role;

            // ✅ LOAD AVATAR
            byte[] img = _imageRepo.GetStaffImage(_staff_id);
            if (img != null)
            {
                using (MemoryStream ms = new MemoryStream(img))
                {
                    picAvatar.Image = Image.FromStream(ms);
                }
            }
            else
            {
                picAvatar.Image = null; // hoặc ảnh mặc định
            }
        }


        private void SetReadOnly(bool value)
        {
            txtHoTen.ReadOnly = value;
            txtNgaySinh.ReadOnly = value;
            txtGioiTinh.ReadOnly = value;
            txtEmail.ReadOnly = value;
            txtSDT.ReadOnly = value;
            txtChucVu.ReadOnly = true;
        }
        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            FormEditAccount formEdit = new FormEditAccount(_staff_id);

            // mở dạng modal
            if (formEdit.ShowDialog() == DialogResult.OK)
            {
                LoadStaffInfo(); // ✅ reload sau khi chỉnh
            }
        }
        private bool SaveStaffInfo()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống");
                return false;
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

            bool success = _staffRepo.UpdateStaff(staff);

            if (success)
            {
                MessageBox.Show("Cập nhật thông tin thành công!");
                return true;
            }

            MessageBox.Show("Cập nhật thất bại!");
            return false;
        }
    }
}


