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
        private Guna.UI2.WinForms.Guna2Button currentButton;

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
            OpenChildForm(new FormMovieDetail());
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

        private void Poster_Click(object sender, EventArgs e)
        {
            PictureBox poster = sender as PictureBox;

            // (OPTIONAL) Lấy thông tin phim từ Tag nếu có
            // var movieId = poster.Tag.ToString();

            OpenChildForm(new FormMovieDetail());
        }

        private void btnLichChieu_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);

            OpenChildForm(new FormShowtimeList());
        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);

            OpenChildForm(new FormMovieList());
        }

        
    }
}


