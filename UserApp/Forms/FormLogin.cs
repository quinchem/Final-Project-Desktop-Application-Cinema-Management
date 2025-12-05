using System;
using System.Drawing;
using System.Windows.Forms;
using SharedData.Models;
using SharedData.Repositories;
using System.Media;

namespace UserApp
{
    public partial class FormLogin : Form
    {
        // Repo để thao tác với thông tin tài khoản
        private readonly AccountRepo AccountRepo = new AccountRepo();

        // Lưu tham chiếu đến nút đang được kích hoạt để đổi giao diện
        private Guna.UI2.WinForms.Guna2Button currentButton;

        public FormLogin()
        {
            InitializeComponent();
            // Thiết lập opacity ban đầu để tạo hiệu ứng mờ dần
            this.Opacity = 0;   
            // Cho phép form nhận sự kiện phím trước khi điều khiển con nhận
            this.KeyPreview = true;

            // Hiển thị giao diện đăng nhập mặc định
            ShowLogin();
        }

        // Lưu form con hiện tại đang mở trong vùng panel
        private Form currentChildForm;

        // Hàm mở một form con trong vùng panel chính
        public void OpenChildForm(Form child)
        {
            // Nếu đã có form con thì đóng nó trước khi mở form mới
            if (currentChildForm != null)
                currentChildForm.Close();

            currentChildForm = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;

            // Ẩn các panel đăng nhập và đăng ký khi mở form con
            panelDangNhap.Visible = false;
            panelDangKy.Visible = false; 
            panelLogin.Controls.Add(child); 
            panelLogin.Tag = child;

            child.BringToFront();
            child.Show();
        }

        // Hiệu ứng fade in khi load form
        private void FormLogin_Load(object sender, EventArgs e)
        {
            var t = new System.Windows.Forms.Timer();
            t.Interval = 10;

            // Tăng dần opacity để hiện form mượt mà
            t.Tick += (s, a) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    t.Stop();
            };

            t.Start();
        }

        // Chuyển giao diện giữa Login va Register
        public void ShowLogin()
        {
            // Nếu có form con đang mở thì đóng và loại bỏ khỏi panel
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                panelLogin.Controls.Remove(currentChildForm);
                currentChildForm = null;
            }

            // Hiện panel đăng nhập, ẩn panel đăng ký
            panelDangNhap.Visible = true;
            panelDangNhap.Enabled = true;
            panelDangKy.Visible = false;

            panelDangNhap.BringToFront();
            if (btnDangNhap == null) return;
            // Kích hoạt hiệu ứng cho nút đăng nhập
            ActivateButton(btnDangNhap);
        }

        public void ShowRegister()
        {
            // Hiện panel đăng ký, ẩn panel đăng nhập
            panelDangNhap.Visible = false;
            panelDangKy.Visible = true;

            panelDangKy.BringToFront();
            if (btnDangKy == null) return;
            // Kích hoạt hiệu ứng cho nút đăng ký
            ActivateButton(btnDangKy);
        }

        // Xử lý sự kiện nhấn nút chuyển sang đăng ký
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            ShowRegister();
        }

        // Xử lý sự kiện nhấn nút chuyển sang đăng nhập
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ShowLogin();
        }

        // Hiệu ứng tô sáng nút
        private void ActivateButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            if (btn == null) return;

            // Reset trạng thái nút trước đó về màu và font ban đầu
            if (currentButton != null)
            {
                currentButton.FillColor = currentButton.Tag != null
                    ? (Color)currentButton.Tag
                    : Color.FromArgb(44, 84, 115); // phương án dự phòng
                currentButton.ForeColor = Color.White;
                currentButton.Font = new Font(currentButton.Font, FontStyle.Regular);
            }

            // Lưu màu gốc của nút mới vào Tag nếu chưa lưu
            if (btn.Tag == null)
                btn.Tag = btn.FillColor;

            // Đặt nút hiện tại là nút được chọn và đổi giao diện để nổi bật
            currentButton = btn;
            currentButton.FillColor = Color.FromArgb(255, 128, 0);
            currentButton.Font = new Font(currentButton.Font, FontStyle.Bold);
        }

        // Kiểm tra hợp lệ cho form đăng ký
        private bool ValidateRegistrationForm(out string msg)
        {
            msg = "";

            // Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                msg = "Vui lòng nhập họ tên.";
                return false;
            }

            // Kiểm tra email dạng cơ bản
            if (!txtEmailDK.Text.Contains("@"))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                msg = "Email không hợp lệ.";
                return false;
            }

            // Kiểm tra số điện thoại gồm mười chữ số
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text ?? "", @"^\d{10}$"))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                msg = "Số điện thoại phải gồm 10 chữ số.";
                return false;
            }

            // Kiểm tra mật khẩu có chữ hoa và ký tự đặc biệt và độ dài tối thiểu
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPassDK.Text, @"^(?=.*[A-Z])(?=.*\W).{8,}$"))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                msg = "Mật khẩu phải có chữ hoa và ký tự đặc biệt và tối thiểu tám ký tự.";
                return false;
            }

            // Kiểm tra mật khẩu xác nhận
            if (txtPassDK.Text != txtPassCF.Text)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                msg = "Mật khẩu xác nhận không khớp.";
                return false;
            }

            // Kiểm tra ngày sinh không vượt quá ngày hiện tại
            if (dtpNgaySinh.Value.Date > DateTime.Today)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                msg = "Ngày sinh không hợp lệ.";
                return false;
            }

            // Kiểm tra người dùng đã đồng ý điều khoản
            if (!chkDieuKhoan.Checked)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                msg = "Bạn phải đồng ý điều khoản.";
                return false;
            }

            return true;
        }

        // Chèn tài khoản mới vào hệ thống
        private bool InsertNewAccount(out string message)
        {
            message = "";

            try
            {
                // Tạo đối tượng khách hàng từ dữ liệu nhập
                var customer = new Customer
                {
                    full_name = txtHoTen.Text.Trim(),
                    date_of_birth = dtpNgaySinh.Value.ToString("dd/MM/yyyy"),
                    gender = radNam.Checked ? "Nam" : "Nữ",
                    address = txtDiachi.Text.Trim(),
                    email = txtEmailDK.Text.Trim(),   
                    phone_number = txtSDT.Text.Trim(),
                    create_date = DateTime.UtcNow.ToString("HH:mm:ss dd-MM-yyyy")
                };

                // Tạo đối tượng tài khoản kèm vai trò
                var account = new Account
                {
                    username = txtEmailDK.Text.Trim(),
                    password = txtPassDK.Text,
                    role_account = "Khách hàng",
                    staff_id = null
                };

                // Gọi repo để đăng ký và trả về kết quả
                return AccountRepo.Register(customer, account, out message);
            }
            catch (Exception ex)
            {
                // Nếu xảy ra lỗi thì trả về thông báo lỗi
                message = ex.Message;
                return false;
            }
        }

        // Xử lý khi nhấn nút đăng ký trên giao diện đăng ký
        private void btnminiDK_Click(object sender, EventArgs e)
        {
            if (!ValidateRegistrationForm(out string validateMsg))
            {
                MessageBox.Show(validateMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (InsertNewAccount(out string msg))
            {
                // Phát âm thanh và thông báo đăng ký thành công
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Đăng ký thành công!", "Thành công",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trở về giao diện đăng nhập và xóa dữ liệu trong form đăng ký
                ShowLogin();
                ClearRegisterFields();
            }
            else
            {
                // Thông báo lỗi khi đăng ký thất bại
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Đăng ký thất bại: " + msg, "Lỗi",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa các trường dữ liệu trên form đăng ký
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

        // Xử lý đăng nhập
        private void btnMiniDN_Click(object sender, EventArgs e)
        {
            string email = txtEmailDN.Text.Trim();
            string password = txtPassDN.Text;

            // Kiểm tra nhập đầy đủ thông tin
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập email và mật khẩu.",
                                 "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi repo đăng nhập và nếu thành công thì mở form chính người dùng
            if (AccountRepo.Login(email, password, out Customer customer, out string msg))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show($"Đăng nhập thành công! Xin chào {customer.full_name}",
                                 "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UserMainForm main = new UserMainForm(customer);
                main.Show();
                this.Hide();
                // Đóng form đăng nhập khi form chính đóng
                main.FormClosed += (s, args) => this.Close();
            }
            else
            {
                // Thông báo khi đăng nhập thất bại
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Đăng nhập thất bại: " + msg,
                                 "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mở form quên mật khẩu khi người dùng chọn
        private void btnQuenMk_Click(object sender, EventArgs e)
        {
            this.OpenChildForm(new FormForgetPassword(this));
        }

        // Bắt phím Enter để submit form phù hợp với panel hiện tại
        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (panelDangNhap.Visible)
                {
                    btnMiniDN.PerformClick(); 
                }
                else if (panelDangKy.Visible)
                {
                    btnminiDK.PerformClick(); 
                }
            }
        }

        // Hiện an hien mat khau khi click vao icon
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (txtPassDN.UseSystemPasswordChar == true)
            {
                // Hiện mật khẩu và đổi icon sang trạng thái xem
                txtPassDN.UseSystemPasswordChar = false;
                txtPassDN.PasswordChar = '\0';
                guna2PictureBox1.Image = Properties.Resources.view;
            }
            else
            {
                // Ẩn mật khẩu và đổi icon sang trạng thái ẩn
                txtPassDN.UseSystemPasswordChar = true;
                guna2PictureBox1.Image = Properties.Resources.hide; 
            }
        }
    }
}
