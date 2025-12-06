using ClosedXML.Excel;
using Microsoft.Data.Sqlite;
using SharedData.Models;
using SharedData.Repositories; // THÊM DÒNG NÀY
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class AdminMainForm : Form
    {
        private string _staffId;
        private bool _isLoggedIn = false;
        // Repository xử lý đăng nhập tài khoản
        private readonly AccountRepo _accountRepo = new AccountRepo(); 
        
        
        public AdminMainForm(string staffId)
        {
            InitializeComponent();
            _staffId = staffId;
            panelDangNhap.Visible = true;
            btnDangXuat.Visible = false;
            picAvatar.Visible = false;
            lblChucVu.Visible = false;
            this.KeyPreview = true;
            this.AcceptButton = btnDN;
        }
        // Load form: mặc định chưa đăng nhập
        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            panelDangNhap.Visible = true;
            btnDangXuat.Visible = false;
            picAvatar.Visible = false;
            lblChucVu.Visible = false;
            SetMenuEnabled(false);
        }
        // Lưu form con đang mở
        private Form currentFormChild;
        private Guna.UI2.WinForms.Guna2Button currentButton;
        
        // Bật / tắt menu theo trạng thái đăng nhập
        private void SetMenuEnabled(bool enabled)
        {
            _isLoggedIn = enabled;
            picAvatar.Visible = enabled;
        }
         // Mở form con trong panel chính
        public void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
                currentFormChild.Close();

            panelMain.AutoScroll = false;
            panelMain.HorizontalScroll.Enabled = false;
            panelMain.HorizontalScroll.Visible = false;

            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        // Hàm dùng để đổi trạng thái nút menu đang active thành màu cam
        private void ActivateButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            if (btn == null) return;

            //Thực hiện reset nút trước đó thành màu trắng
            if (currentButton != null)
            {
                currentButton.FillColor = currentButton.Tag != null
                    ? (Color)currentButton.Tag
                    : Color.FromArgb(44, 84, 115);
                currentButton.ForeColor = Color.White;
                currentButton.Font = new Font(currentButton.Font, FontStyle.Regular);
            }
            if (btn.Tag == null)
                btn.Tag = btn.FillColor;
            currentButton = btn;
            currentButton.FillColor = Color.FromArgb(44, 84, 115);
            currentButton.ForeColor = Color.FromArgb(255, 128, 0);
            currentButton.Font = new Font(currentButton.Font, FontStyle.Bold);
        }

        // Hàm dùng để kiểm tra người dùng đã đăng nhập hay chưa, nếu chưa thì sẽ hiện thông báo
        private bool CheckLogin()
        {
            if (!_isLoggedIn)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show(
                    "Vui lòng đăng nhập để sử dụng chức năng này",
                    "Chưa đăng nhập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }
            return true;
        }

        // Các hàm xử lý sự kiện khi bấm vào các chức năng trên menu thì sẽ hiện form con
        private void btnSuatChieu_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            ActivateButton(btn);
            OpenChildForm(new FormShowManagement());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            ActivateButton(btn);
            OpenChildForm(new FormStatistics1(this));
        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            ActivateButton(btn);
            OpenChildForm(new FormMovieManagement());
        }

        private void btnSoDoGhe_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            ActivateButton(btn);
            OpenChildForm(new FormRoomLayoutManagement());
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            ActivateButton(btn);
            OpenChildForm(new FormCustomerManagement());
        }

        // Hàm xử lý sự kiện, nếu nhấn vào Avatar thì sẽ hiện form quản lý tài khoản với đúng staff_id
        private void picUserIcon_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;
            OpenChildForm(new FormAccountManagement(_staffId));
        }

        
        public void GoHome()
        {
            if (currentFormChild != null)
            {
                panelMain.Controls.Remove(currentFormChild);
                currentFormChild.Close();
                currentFormChild = null;
            }

            panelMain.AutoScroll = true;
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(51, 51, 76);
                currentButton = null;
            }
        }

        private void logo_Click(object sender, EventArgs e)
        {
            GoHome();
        }

        // Hàm xử lý sự kiện khi nhấn đăng xuất, lúc này sẽ quay lại form đăng nhập 
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            _staffId = null;
            _isLoggedIn = false;

            picAvatar.Visible = false;
            lblChucVu.Visible = false;
            btnDangXuat.Visible = false;
            panelDangNhap.Visible = true;
            panelDangNhap.Enabled = true;
            panelDangNhap.BringToFront();
            SetMenuEnabled(false); 

            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }
        }

        // Xử lý đăng nhập
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();
            lblError.Visible = false;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                lblError.Text = "Vui lòng nhập đầy đủ tài khoản và mật khẩu";
                lblError.Visible = true;
                return;
            }

            // Gọi AccountRepo để xác thực tài khoản
            string staffId, role, msg;
            if (_accountRepo.LoginStaff(user, pass, out staffId, out role, out msg))
            {
                _staffId = staffId;
                lblChucVu.Text = role;
                picAvatar.Visible = true;
                lblChucVu.Visible = true;
                btnDangXuat.Visible = true;

                HideLoginPanel();
                SetMenuEnabled(true);

                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();

                ActivateButton(btnThongKe);
                OpenChildForm(new FormStatistics1(this));
            }
            else
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                lblError.Text = msg;
                lblError.Visible = true;
            }
        }

        private void HideLoginPanel()
        {
            panelDangNhap.Visible = false;
            panelDangNhap.Enabled = false;
            panelDangNhap.SendToBack();
        }
        
        // Nhấn Enter để đăng nhập
        private void btnDN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDN.PerformClick();
            }
        }

        private void btnQuenMk_Click(object sender, EventArgs e)
        {
            panelDangNhap.Visible = false;
            panelDangNhap.Enabled = false;
            OpenChildForm(new FormForgetPassword(this));
        }
        // Hiện lại panel đăng nhập
        public void ShowLoginPanel()
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }
            panelDangNhap.Visible = true;
            panelDangNhap.Enabled = true;
            panelDangNhap.BringToFront();
        }
         // Ẩn / hiện mật khẩu
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar == true)
            {
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.PasswordChar = '\0';
                picEye.Image = Properties.Resources.view;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
                picEye.Image = Properties.Resources.hide;
            }
        }
    }
}
