using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormAccountManagement : Form
    {
        // ID nhân viên đang đăng nhập
        private readonly string _staff_id;
         // Repository xử lý dữ liệu nhân viên
        private readonly StaffRepo _staffRepo = new StaffRepo();
        // Kiểm soát trạng thái chỉnh sửa
        private bool _isEditing = false;
        // Repository xử lý ảnh đại diện
        private readonly ImageRepo _imageRepo = new ImageRepo();

        public FormAccountManagement(string staff_id)
        {
            InitializeComponent();
            _staff_id = staff_id;
            SetLockControls(true);
            LoadStaffInfo();
        }
        // Load thông tin nhân viên từ CSDL
        private void LoadStaffInfo()
        {
            Staff staff = _staffRepo.GetStaffById(_staff_id);

            if (staff == null)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không tìm thấy thông tin nhân viên.");
                Close();
                return;
            }
            // Gán thông tin cá nhân lên giao diện
            txtHoTen.Text = staff.full_name;
            txtNgaySinh.Text = staff.date_of_birth;
            txtGioiTinh.Text = staff.gender;
            txtEmail.Text = staff.email;
            txtSDT.Text = staff.phone_number;
            txtChucVu.Text = staff.role;

            // Load ảnh đại diện nhân viên
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
                picAvatar.Image = null; 
            }
        }

        // Khóa / mở các ô nhập liệu
        private void SetLockControls(bool isLocked)
        {
            // Thông tin cá nhân
            txtHoTen.Enabled = !isLocked;
            txtNgaySinh.Enabled = !isLocked;
            txtGioiTinh.Enabled = !isLocked;
            txtEmail.Enabled = !isLocked;
            txtSDT.Enabled = !isLocked;
            txtChucVu.Enabled = false;

            // Khóa mật khẩu
            txtMKcu.Enabled = !isLocked;
            txtMKmoi.Enabled = !isLocked;
            txtNhapLaiMK.Enabled = !isLocked;
        }
        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            FormEditAccount formEdit = new FormEditAccount(_staff_id);
            if (formEdit.ShowDialog() == DialogResult.OK)
            {
                LoadStaffInfo();
            }
        }
    }
}


