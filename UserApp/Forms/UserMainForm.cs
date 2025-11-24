using UserApp.Models;

namespace UserApp
{
    public partial class UserMainForm : Form
    {
        public UserMainForm()
        {
            InitializeComponent();
            UpdateHeaderUI();
        }

        private Form currentFormChild;
        public void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
                currentFormChild.Close();
            mainpanel.AutoScroll = false;

            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(childForm);
            mainpanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            childForm.FormClosed += (s, e) =>
            {
                mainpanel.AutoScroll = true;
            };
        }
        private FormLogin loginForm;

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            loginForm = new FormLogin(this);
            OpenChildForm(loginForm);
            loginForm.ShowLogin();
        }

        // Biến lưu thông tin user đã login
        public Customer CurrentUser { get; private set; }

        // Method để set thông tin user khi login thành công
        public void SetCurrentUser(Customer customer)
        {
            CurrentUser = customer;
            UpdateHeaderUI();
        }

        private void UpdateHeaderUI()
        {
            if (CurrentUser != null)
            {
                // Đã login
                btnUserName.Text = CurrentUser.full_name.ToUpper();
                btnUserName.Visible = true;
                btnLogout.Visible = true;

                btnDangNhap.Visible = false;
                btnDangKy.Visible = false;
            }
            else
            {
                // Chưa login
                btnUserName.Visible = false;
                btnLogout.Visible = false;

                btnDangNhap.Visible = true;
                btnDangKy.Visible = true;
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {

            loginForm = new FormLogin(this);
            OpenChildForm(loginForm);
            loginForm.ShowRegister();
        }
        public void GoHome()
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }

            this.AutoScroll = true;
        }

        private void logo_Click(object sender, EventArgs e)
        {
            GoHome();
        }

        private void txtTimKiem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormSearch());
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormFilmDetail());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 1️⃣ Xóa thông tin user hiện tại
                CurrentUser = null;

                // 2️⃣ Reset header về trạng thái chưa login
                UpdateHeaderUI();

                // 3️⃣ Đóng child form hiện tại (nếu có) và trở về trang chủ
                GoHome();
            }
        }
        private void btnUserName_Click(object sender, EventArgs e)
        {
            if (CurrentUser == null) return;

            FormProfile profileForm = new FormProfile(CurrentUser);
            OpenChildForm(profileForm);
        }
    }
}


