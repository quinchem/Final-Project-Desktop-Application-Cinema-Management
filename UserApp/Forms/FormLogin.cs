using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharedData.Models;
using SharedData.Repositories;

namespace UserApp
{
    public partial class FormLogin : Form
    {
        private readonly AccountRepo AccountRepo = new AccountRepo();

        public FormLogin()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        private Guna.UI2.WinForms.Guna2Button currentButton;

        private void FormLogin_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
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
            currentButton.FillColor = Color.FromArgb(255, 128, 0);
            currentButton.Font = new Font(currentButton.Font, FontStyle.Bold);
        }

        public void ShowLogin()
        {
            panelDangNhap.Visible = true;
            panelDangKy.Visible = false;
            panelDangNhap.BringToFront();

            if (btnDangNhap != null)
                ActivateButton(btnDangNhap);
        }

        public void ShowRegister()
        {
            panelDangNhap.Visible = false;
            panelDangKy.Visible = true;
            panelDangKy.BringToFront();

            if (btnDangKy != null)
                ActivateButton(btnDangKy);
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ShowLogin();
            ActivateButton((Guna.UI2.WinForms.Guna2Button)sender);
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            ShowRegister();
            ActivateButton((Guna.UI2.WinForms.Guna2Button)sender);
        }

        private UserMainForm parentForm;

        public FormLogin(UserMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void btnQuenMk_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormForgetPassword(parentForm));
        }

        private bool ValidateRegistrationForm(out string msg)
        {
            msg = "";

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                msg = "Vui lòng nhập họ tên.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmailDK.Text) || !txtEmailDK.Text.Contains("@"))
            {
                msg = "Email không hợp lệ.";
                return false;
            }

            string phone = txtSDT.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(phone) || !System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{10}$"))
            {
                msg = "Số điện thoại phải gồm đúng 10 chữ số.";
                return false;
            }

            string password = txtPassDK.Text ?? "";
            string confirm = txtPassCF.Text ?? "";
            if (string.IsNullOrWhiteSpace(password))
            {
                msg = "Vui lòng nhập mật khẩu.";
                return false;
            }

            var pwdPattern = new System.Text.RegularExpressions.Regex(@"^(?=.*[A-Z])(?=.*\W).{8,}$");
            if (!pwdPattern.IsMatch(password))
            {
                msg = "Mật khẩu cần 8 ký tự gồm chữ hoa + ký tự đặc biệt.";
                return false;
            }

            if (password != confirm)
            {
                msg = "Xác nhận mật khẩu không khớp.";
                return false;
            }

            if (dtpNgaySinh.Value.Date > DateTime.Today)
            {
                msg = "Ngày sinh không hợp lệ.";
                return false;
            }

            if (!chkDieuKhoan.Checked)
            {
                msg = "Bạn phải chấp nhận điều khoản.";
                return false;
            }

            return true;
        }

        // ================================
        // BUILD Customer + Account và REGISTER
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
                    create_date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                };

                var account = new Account
                {
                    username = txtEmailDK.Text.Trim(),  // USERNAME = EMAIL
                    password = txtPassDK.Text,
                    role_account = "customer",
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

            bool ok = InsertNewAccount(out string msg);
            if (ok)
            {
                MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowLogin();
                ClearRegisterFields();
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại: " + msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearRegisterFields()
        {
            txtHoTen.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            radNam.Checked = false;
            radNu.Checked = false;
            txtDiachi.Text = "";
            txtEmailDK.Text = "";
            txtSDT.Text = "";
            txtPassDK.Text = "";
            txtPassCF.Text = "";
            chkDieuKhoan.Checked = false;
        }

        // ================================
        // LOGIN (email + password)
        // ================================
        private void btnMiniDN_Click(object sender, EventArgs e)
        {
            string email = txtEmailDN.Text.Trim();
            string password = txtPassDN.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập email và password.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AccountRepo.Login(email, password, out Customer customer, out string msg))
            {
                MessageBox.Show($"Đăng nhập thành công! Xin chào {customer.full_name}",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                parentForm?.SetCurrentUser(customer);
                this.Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại: " + msg, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
