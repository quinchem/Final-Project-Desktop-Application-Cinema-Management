using System;
using System.Drawing;
using System.Windows.Forms;
using SharedData.Models;
using SharedData.Repositories;

namespace UserApp
{
    public partial class FormLogin : Form
    {
        private readonly AccountRepo AccountRepo = new AccountRepo();
        private Guna.UI2.WinForms.Guna2Button currentButton;

        public FormLogin()
        {
            InitializeComponent();
            this.Opacity = 0;   // fade-in
            this.KeyPreview = true;

            ShowLogin();
        }
        private Form currentChildForm;

        public void OpenChildForm(Form child)
        {
            if (currentChildForm != null)
                currentChildForm.Close();

            currentChildForm = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;

            panelDangNhap.Visible = false;
            panelDangKy.Visible = false;  
            panelLogin.Controls.Add(child);  
            panelLogin.Tag = child;

            child.BringToFront();
            child.Show();
        }

        // ================================
        // Fade-in animation
        // ================================
        private void FormLogin_Load(object sender, EventArgs e)
        {
            var t = new System.Windows.Forms.Timer();
            t.Interval = 10;

            t.Tick += (s, a) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    t.Stop();
            };

            t.Start();
            
        }

        // ================================
        // UI switching giữa Login/Register
        // ================================
        public void ShowLogin()
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                panelLogin.Controls.Remove(currentChildForm);
                currentChildForm = null;
            }

            panelDangNhap.Visible = true;
            panelDangNhap.Enabled = true;
            panelDangKy.Visible = false;

            panelDangNhap.BringToFront();
            if (btnDangNhap == null) return;
            ActivateButton(btnDangNhap);

        }

        public void ShowRegister()
        {
            panelDangNhap.Visible = false;
            panelDangKy.Visible = true;

            panelDangKy.BringToFront();
            if (btnDangKy == null) return;
            ActivateButton(btnDangKy);
        }


       
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            ShowRegister();
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ShowLogin();
        }

        // ================================
        // Button highlight
        // ================================
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
            currentButton.FillColor = Color.FromArgb(255, 128, 0);
            //currentButton.ForeColor = Color.FromArgb(255, 128, 0);
            currentButton.Font = new Font(currentButton.Font, FontStyle.Bold);

        }

        // ================================
        // REGISTER VALIDATION
        // ================================
        private bool ValidateRegistrationForm(out string msg)
        {
            msg = "";

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                msg = "Vui lòng nhập họ tên.";
                return false;
            }

            if (!txtEmailDK.Text.Contains("@"))
            {
                msg = "Email không hợp lệ.";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text ?? "", @"^\d{10}$"))
            {
                msg = "Số điện thoại phải gồm 10 chữ số.";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPassDK.Text, @"^(?=.*[A-Z])(?=.*\W).{8,}$"))
            {
                msg = "Mật khẩu phải có chữ hoa + ký tự đặc biệt và >= 8 ký tự.";
                return false;
            }

            if (txtPassDK.Text != txtPassCF.Text)
            {
                msg = "Mật khẩu xác nhận không khớp.";
                return false;
            }

            if (dtpNgaySinh.Value.Date > DateTime.Today)
            {
                msg = "Ngày sinh không hợp lệ.";
                return false;
            }

            if (!chkDieuKhoan.Checked)
            {
                msg = "Bạn phải đồng ý điều khoản.";
                return false;
            }

            return true;
        }

        // ================================
        // REGISTER new account
        // ================================
        private bool InsertNewAccount(out string message)
        {
            message = "";

            try
            {
                var customer = new Customer
                {
                    full_name = txtHoTen.Text.Trim(),
                    date_of_birth = dtpNgaySinh.Value.ToString("dd/MM/yyyy"),
                    gender = radNam.Checked ? "Nam" : "Nữ",
                    address = txtDiachi.Text.Trim(),
                    email = txtEmailDK.Text.Trim(),   // EMAIL -> lưu vào customer.email
                    phone_number = txtSDT.Text.Trim(),
                    create_date = DateTime.UtcNow.ToString("HH:mm:ss dd-MM-yyyy")
                };

                var account = new Account
                {
                    username = txtEmailDK.Text.Trim(),  // USERNAME = EMAIL
                    password = txtPassDK.Text,
                    role_account = "Khách hàng",
                    staff_id = null
                };

                return AccountRepo.Register(customer, account, out message);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
   

        private void btnminiDK_Click(object sender, EventArgs e)
        {
            if (!ValidateRegistrationForm(out string validateMsg))
            {
                MessageBox.Show(validateMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InsertNewAccount(out string msg))
            {
                MessageBox.Show("Đăng ký thành công!", "Thành công",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);

                ShowLogin();
                ClearRegisterFields();
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại: " + msg, "Lỗi",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearRegisterFields()
        {
            txtHoTen.Text = "";
            txtEmailDK.Text = "";
            txtSDT.Text = "";
            txtPassDK.Text = "";
            txtPassCF.Text = "";
            txtDiachi.Text = "";
            radNam.Checked = false;
            radNu.Checked = false;
            chkDieuKhoan.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
        }

        // ================================
        // LOGIN
        // ================================
        private void btnMiniDN_Click(object sender, EventArgs e)
        {
            string email = txtEmailDN.Text.Trim();
            string password = txtPassDN.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập email và mật khẩu.",
                                 "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AccountRepo.Login(email, password, out Customer customer, out string msg))
            {
                MessageBox.Show($"Đăng nhập thành công! Xin chào {customer.full_name}",
                                 "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // MỞ MAIN FORM
                // Lưu ý: Cần đảm bảo class UserMainForm có tồn tại và constructor nhận đối tượng Customer
                UserMainForm main = new UserMainForm(customer);
                main.Show();

                // ẨN LOGIN FORM 
                this.Hide();

                // Khi MAIN FORM đóng → đóng luôn LOGIN FORM → app tắt đúng cách
                main.FormClosed += (s, args) => this.Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại: " + msg,
                                 "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuenMk_Click(object sender, EventArgs e)
        {
            this.OpenChildForm(new FormForgetPassword(this));
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (panelDangNhap.Visible)
                {
                    btnMiniDN.PerformClick(); // ENTER -> Đăng nhập
                }
                else if (panelDangKy.Visible)
                {
                    btnminiDK.PerformClick(); // ENTER -> Đăng ký
                }
            }
        }

      
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (txtPassDN.UseSystemPasswordChar == true)
            {
                // Hiện lên + Đổi ảnh mở
                txtPassDN.UseSystemPasswordChar = false;
                txtPassDN.PasswordChar = '\0';
                guna2PictureBox1.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn đi + Đổi ảnh đóng
                txtPassDN.UseSystemPasswordChar = true;

                // Đổi ảnh đóng ở đây
                guna2PictureBox1.Image = Properties.Resources.hide; 
            }
        }
    }
}