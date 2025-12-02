using ClosedXML.Excel;
using Microsoft.Data.Sqlite;
using SharedData.Models;
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

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            panelDangNhap.Visible = true;
            btnDangXuat.Visible = false;
            picAvatar.Visible = false;
            lblChucVu.Visible = false;
            SetMenuEnabled(false);
        }

        private Form currentFormChild;
        private Guna.UI2.WinForms.Guna2Button currentButton;
        
        private void SetMenuEnabled(bool enabled)
        {
            _isLoggedIn = enabled;
            picAvatar.Visible = enabled;
        }

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

        private void ActivateButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            if (btn == null) return;
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
                            lblChucVu.Text = reader["role_account"].ToString();
                            picAvatar.Visible = true;
                            lblChucVu.Visible = true;
                            btnDangXuat.Visible = true;
                            // Giúp ẩn và đẩy panel xuống
                            HideLoginPanel();
                            // Giúp mở Menu
                            SetMenuEnabled(true);
                            // Giúp mở form mặc định
                            ActivateButton(btnThongKe);
                            OpenChildForm(new FormStatistics1(this));
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
            // Đóng form con nếu còn
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


