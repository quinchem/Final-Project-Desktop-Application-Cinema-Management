using Guna.UI2.WinForms;
using Microsoft.VisualBasic.Devices;
using SharedData.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using UserApp.Forms;

namespace UserApp
{
    public partial class UserMainForm : Form
    {
        // Lưu thông tin user hiện tại
        public Customer CurrentUser { get; private set; }

        private Form currentFormChild;
        private Guna.UI2.WinForms.Guna2Button currentButton;
        Guna2PictureBox currentSelected = null;

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

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            FormChatbot f = new FormChatbot(this);
            f.Show();
        }

        private void LoadDanhSachPhim(List<Film> listPhim)
        {
            flowPanelList.Controls.Clear();
            foreach (var phim in listPhim)
            {
                Guna2PictureBox picSmall = new Guna2PictureBox();

                picSmall.Size = new Size(200, 110);
                picSmall.SizeMode = PictureBoxSizeMode.Zoom;
                picSmall.BorderRadius = 15;
                picSmall.Cursor = Cursors.Hand;
                picSmall.Click += picSmall_Click;

                // --- 4. THÊM VÀO DANH SÁCH ---
                flowPanelList.Controls.Add(picSmall);
            }
        }

        private void picSmall_Click(object sender, EventArgs e)
        {
            var clickedPic = (Guna2PictureBox)sender;

            // A. Tắt hiệu ứng của cái cũ (nếu có)
            if (currentSelected != null)
            {
                currentSelected.ShadowDecoration.Enabled = false;
            }

            // B. Bật hiệu ứng cho cái mới vừa click
            clickedPic.ShadowDecoration.Enabled = true;
            clickedPic.ShadowDecoration.Color = Color.FromArgb(245, 131, 35); 
            clickedPic.ShadowDecoration.Depth = 15;        

            // C. LẤY ẢNH TỪ NHỎ -> GÁN LÊN TO
            // Logic: Cái nhỏ đang hiện hình gì thì gán y chang lên trên
            if (clickedPic.Image != null)
            {
                gunaPicBig.Image = clickedPic.Image;
                gunaPicBig.SizeMode = PictureBoxSizeMode.Zoom;
            }

            // D. Lưu lại cái này để lần sau click cái khác thì biết đường tắt
            currentSelected = clickedPic;
        }
    }
}
