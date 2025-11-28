using System;
using System.Drawing;
using System.Windows.Forms;
using SharedData.Models;

namespace UserApp
{
    public partial class UserMainForm : Form
    {
        // Lưu thông tin user hiện tại
        public Customer CurrentUser { get; private set; }

        private Form currentFormChild;
        private Guna.UI2.WinForms.Guna2Button currentButton;

        // Nhận user từ FormLogin
        public UserMainForm(Customer customer)
        {
            InitializeComponent();
            CurrentUser = customer;
            UpdateHeaderUI();
        }

        // Giữ lại cho các form khác cần khởi tạo mặc định (nếu có)
        public UserMainForm()
        {
            InitializeComponent();
        }

        // Cập nhật giao diện header
        private void UpdateHeaderUI()
        {
            if (CurrentUser != null)
            {
                btnUserName.Text = CurrentUser.full_name.ToUpper();
                btnUserName.Visible = true;

                btnLogout.Visible = true;
            }
        }

        // Load form con
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

        // Nút xem trang cá nhân
        private void btnUserName_Click(object sender, EventArgs e)
        {
            if (CurrentUser == null) return;

            OpenChildForm(new FormProfile(CurrentUser));
        }

        // Nút đăng xuất
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                CurrentUser = null;

                // Quay về FormLogin
                // Mở lại FormLogin
                FormLogin login = new FormLogin();
                login.Show();

                // Ẩn UserMainForm (không đóng ngay để tránh tắt app)
                this.Hide();

                // Khi FormLogin đóng → đóng luôn UserMainForm
                login.FormClosed += (s2, e2) => this.Close();
            }
        }

        // Quay về home
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

        // Mở form tìm kiếm
        private void txtTimKiem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormSearch());
        }

        // Mở chi tiết phim
        //private void guna2PictureBox1_Click(object sender, EventArgs e)
        //{
        //    OpenChildForm(new FormMovieDetail());
        //}

        //private void Poster_Click(object sender, EventArgs e)
        //{
        //    OpenChildForm(new FormMovieDetail());
        //}

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

            OpenChildForm(new FormMovieList(this));

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
    }
}
