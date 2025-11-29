using SharedData.Models;
using ClosedXML.Excel;
using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class AdminMainForm : Form
    {
        private string _staffId;
        private bool _isLoggedIn = false;

        public AdminMainForm(string staffId)
        {
            InitializeComponent();
            _staffId = staffId;

            panelDangNhap.Visible = true;      // hiện panel đăng nhập
            //btnDangNhap.Visible = true;
            btnDangXuat.Visible = false;

            // Ẩn thông tin nhân viên
            picAvatar.Visible = false;
            lblChucVu.Visible = false;
            //lblTen.Visible = false;
            this.KeyPreview = true;
            this.AcceptButton = btnDN;


        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            panelDangNhap.Visible = true;

            //btnDangNhap.Visible = true;
            btnDangXuat.Visible = false;

            picAvatar.Visible = false;
            //lblTen.Visible = false;
            lblChucVu.Visible = false;

            // CHƯA đăng nhập → khóa chức năng (logic), nhưng không đổi màu nút
            SetMenuEnabled(false);
        }

        private Form currentFormChild;
        private Guna.UI2.WinForms.Guna2Button currentButton;

        // Chỉ cập nhật trạng thái đăng nhập và hiển thị avatar.
        // KHÔNG set Enabled cho các nút để tránh "xám".
        private void SetMenuEnabled(bool enabled)
        {
            _isLoggedIn = enabled;
            picAvatar.Visible = enabled;
            // Nếu muốn hiển thị tên/role khi đăng nhập thì gán ở chỗ login thành công
        }

        public void OpenChildForm(Form childForm)
        {
            // Nếu có form con đang mở thì đóng
            if (currentFormChild != null)
                currentFormChild.Close();

            panelMain.AutoScroll = false;

            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            // Khi form con đóng, bật lại AutoScroll
            childForm.FormClosed += (s, e) =>
            {
                panelMain.AutoScroll = true;
            };
        }

        private void ActivateButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            if (btn == null) return;

            // Reset nút cũ về trạng thái Design
            if (currentButton != null)
            {
                // Reset về trạng thái mặc định trong Designer
                currentButton.FillColor = currentButton.Tag != null
                    ? (Color)currentButton.Tag
                    : Color.FromArgb(44, 84, 115); // fallback
                currentButton.ForeColor = Color.White;
                currentButton.Font = new Font(currentButton.Font, FontStyle.Regular);
            }

            // Lưu màu gốc của nút mới (nếu chưa lưu)
            if (btn.Tag == null)
                btn.Tag = btn.FillColor; // lưu FillColor gốc vào Tag

            // Set nút hiện tại active
            currentButton = btn;
            currentButton.FillColor = Color.FromArgb(44, 84, 115);
            currentButton.ForeColor = Color.FromArgb(255, 128, 0);
            currentButton.Font = new Font(currentButton.Font, FontStyle.Bold);
        }

        private bool CheckLogin()
        {
            if (!_isLoggedIn)
            {
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
            OpenChildForm(new FormStatistics1());
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

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            ActivateButton(btn);
            OpenChildForm(new FormProduct());
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            ActivateButton(btn);
            OpenChildForm(new FormCustomerManagement()); // sửa đúng form
        }

        private void picUserIcon_Click(object sender, EventArgs e)
        {
            if (!CheckLogin()) return;

            // picUserIcon là PictureBox (hoặc control khác) — không ActivateButton trên nó
            OpenChildForm(new FormAccountManagement(_staffId));
        }

        public void GoHome()
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }

            panelMain.AutoScroll = true;

            // Reset nút active
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

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            _staffId = null;
            _isLoggedIn = false;

            picAvatar.Visible = false;
            //lblTen.Visible = false;
            lblChucVu.Visible = false;

            //btnDangNhap.Visible = true;
            btnDangXuat.Visible = false;

            panelDangNhap.Visible = true;
            panelDangNhap.Enabled = true;
            panelDangNhap.BringToFront();

            // set menu state (logic)
            SetMenuEnabled(false);

            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();
            lblError.Visible = false;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                lblError.Text = "Vui lòng nhập đầy đủ tài khoản và mật khẩu";
                lblError.Visible = true;
                return;
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT account_id, role_account, staff_id
            FROM Account
            WHERE username = @user
              AND password = @pass
              AND role_account = 'Nhân viên (Admin)'";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", pass);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _staffId = reader["staff_id"].ToString();

                            //lblTen.Text = "Admin";
                            lblChucVu.Text = reader["role_account"].ToString();

                            picAvatar.Visible = true;
                            //lblTen.Visible = true;
                            lblChucVu.Visible = true;

                            //btnDangNhap.Visible = false;
                            btnDangXuat.Visible = true;

                            // ẨN & ĐẨY PANEL LOGIN XUỐNG
                            HideLoginPanel();

                            // MỞ MENU (logic) — không đổi màu nút
                            SetMenuEnabled(true);

                            // MỞ FORM MẶC ĐỊNH
                            ActivateButton(btnThongKe);
                            OpenChildForm(new FormStatistics1());
                        }
                        else
                        {
                            lblError.Text = "Sai tài khoản / mật khẩu hoặc không có quyền Admin";
                            lblError.Visible = true;
                        }
                    }
                }
            }
            btnDangXuat.Visible = true;
        }

        private void HideLoginPanel()
        {
            panelDangNhap.Visible = false;
            panelDangNhap.Enabled = false;
            panelDangNhap.SendToBack();
        }

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

            // Mở form Quên mật khẩu vào panelMain
            OpenChildForm(new FormForgetPassword(this));
        }
        public void ShowLoginPanel()
        {
            // đóng form con nếu còn
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }

            panelDangNhap.Visible = true;
            panelDangNhap.Enabled = true;
            panelDangNhap.BringToFront();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar == true)
            {
                // Hiện lên + Đổi ảnh mở
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.PasswordChar = '\0';
                picEye.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn đi + Đổi ảnh đóng
                txtPassword.UseSystemPasswordChar = true;

                // Đổi ảnh đóng ở đây
                picEye.Image = Properties.Resources.hide; // <--- THÊM DÒNG NÀY VÀO
            }
        }
    }
}


