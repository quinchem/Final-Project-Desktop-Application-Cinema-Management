using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserApp.Models;
using UserApp.Repositories;
using static UserApp.Repositories.CustomerRepo;

namespace UserApp
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            this.Opacity = 0;
        }
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
        public void ShowLogin()
        {
            panelDangNhap.Visible = true;
            panelDangKy.Visible = false;
            panelDangNhap.BringToFront();
        }

        public void ShowRegister()
        {
            panelDangNhap.Visible = false;
            panelDangKy.Visible = true;
            panelDangKy.BringToFront();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ShowLogin();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            ShowRegister();
        }
        public void SwitchToLogin()
        {
            ShowLogin();
        }

        public void SwitchToRegister()
        {
            ShowRegister();
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
                msg = "Số điện thoại phải gồm đúng 10 chữ số (ví dụ: 0123456789).";
                return false;
            }

            string password = txtPassDK.Text ?? "";
            string confirm = txtPassCF.Text ?? "";
            if (string.IsNullOrWhiteSpace(password))
            {
                msg = "Vui lòng nhập mật khẩu.";
                return false;
            }

            // mật khẩu tối thiểu 8 ký tự, ít nhất 1 ký tự hoa và 1 ký tự đặc biệt
            var pwdPattern = new System.Text.RegularExpressions.Regex(@"^(?=.*[A-Z])(?=.*\W).{8,}$");
            if (!pwdPattern.IsMatch(password))
            {
                msg = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm ít nhất 1 chữ hoa và 1 ký tự đặc biệt.";
                return false;
            }

            if (password != confirm)
            {
                msg = "Xác nhận mật khẩu không khớp.";
                return false;
            }

            // ngày sinh không được ở tương lai
            if (dtpNgaySinh.Value.Date > DateTime.Today)
            {
                msg = "Ngày sinh không hợp lệ.";
                return false;
            }

            // optional: check terms checkbox
            if (!chkDieuKhoan.Checked)
            {
                msg = "Bạn phải chấp nhận điều khoản để đăng ký.";
                return false;
            }

            return true;
        }

        // ================================
        // INSERT NEW ACCOUNT (calls AccountRepo.Register which handles hashing & transaction)
        // ================================
        private bool InsertNewAccount(out string message)
        {
            message = "";

            try
            {
                // Build Customer (dates as strings)
                var customer = new Customer
                {
                    // CustomerId will be generated in repository
                    full_name = txtHoTen.Text.Trim(),
                    date_of_birth = dtpNgaySinh.Value.ToString("dd-MM-yyyy"), // string format
                    gender = radNam.Checked ? "Nam" : "Nữ",
                    address = txtDiachi.Text.Trim(),
                    email = txtEmailDK.Text.Trim(),
                    phone_number = txtSDT.Text.Trim(),
                    create_date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") // string
                };

                // Build Account (use email as username if you don't have a username field)
                var account = new Account
                {
                    // AccountId will be generated in repository
                    username = txtEmailDK.Text.Trim(),
                    password = txtPassDK.Text, // pass plaintext — AccountRepo.Register will hash
                    role_account = "customer", // default role
                    staff_id = null // no staff
                };

                // Call repository register (which does transaction, duplication checks, hashing)
                bool ok = AccountRepo.Register(customer, account, out message);
                return ok;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        // ================================
        // REGISTER BUTTON CLICK
        // ================================
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
                ShowLogin();  // về lại login
                ClearRegisterFields();
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại: " + msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearRegisterFields()
        {
            try
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
            catch
            {
                // ignore if some control not found
            }
        }

        private void btnMiniDN_Click(object sender, EventArgs e)
        {
            string username = txtEmailDN.Text.Trim();
            string password = txtPassDN.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập username và password.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AccountRepo.Login(username, password, out Customer customer, out string msg))
            {
                MessageBox.Show($"Đăng nhập thành công! Xin chào {customer.full_name}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // TODO: mở form chính hoặc lưu thông tin user đã đăng nhập
                parentForm?.SetCurrentUser(customer);
                this.Close(); // đóng form login
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại: " + msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
