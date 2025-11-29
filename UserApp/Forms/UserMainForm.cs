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

            OpenChildForm(new FormShowtimeList(this));

        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);
            
            OpenChildForm(new FormShowtimeList());
        }

        private void ActivateButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            if (btn == null) return;

            ActivateButton(btn);
           
            OpenChildForm(new FormMovieList());
        }

        
    }
}
