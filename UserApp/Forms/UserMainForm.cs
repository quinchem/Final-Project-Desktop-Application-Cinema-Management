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
        // Thông tin người dùng đang đăng nhập
        // Thuộc tính này được truyền từ FormLogin sau khi đăng nhập thành công.
        public Customer CurrentUser { get; private set; }

        // Form con hiện đang hiển thị bên trong mainpanel
        private Form currentFormChild;

        // Nút menu đang được chọn (để tạo hiệu ứng active)
        private Guna.UI2.WinForms.Guna2Button currentButton;

        // Ảnh nhỏ đang được chọn — dùng cho phần xem gallery ảnh phim
        Guna2PictureBox currentSelected = null;

        // Constructor chính: nhận Customer từ FormLogin
        public UserMainForm(Customer customer)
        {
            InitializeComponent();
            CurrentUser = customer; // Lưu user đang đăng nhập
            UpdateHeaderUI();       // Hiển thị tên user lên header
        }

        // Constructor phụ phòng hờ cho trường hợp cần khởi tạo trống (không dùng đến)
        public UserMainForm()
        {
            InitializeComponent();
        }

        // Cập nhật phần header gồm tên người dùng và nút Logout
        private void UpdateHeaderUI()
        {
            if (CurrentUser != null)
            {
                // Hiển thị tên user dạng chữ in hoa
                btnUserName.Text = CurrentUser.full_name.ToUpper();
                btnUserName.Visible = true;

                // Hiển thị nút đăng xuất khi đã có user
                btnLogout.Visible = true;
            }
        }

        // Mở form con trong mainpanel (áp dụng cho mọi form tính năng User)
        public void OpenChildForm(Form childForm)
        {
            // Nếu có form đang mở → đóng trước khi mở form mới
            if (currentFormChild != null)
                currentFormChild.Close();

            // Tắt AutoScroll để tránh lệch layout khi thay đổi form
            mainpanel.AutoScroll = false;

            currentFormChild = childForm;
            childForm.TopLevel = false;              // Form con không phải cửa sổ riêng
            childForm.FormBorderStyle = FormBorderStyle.None; // Gỡ viền form con
            childForm.Dock = DockStyle.Fill;         // Form con chiếm toàn bộ panel

            // Thêm form con vào mainpanel và hiển thị
            mainpanel.Controls.Add(childForm);
            mainpanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            // Khi form con đóng → bật lại AutoScroll
            childForm.FormClosed += (s, e) =>
            {
                mainpanel.AutoScroll = true;
            };
        }

        // Người dùng click vào tên → mở trang hồ sơ cá nhân
        private void btnUserName_Click(object sender, EventArgs e)
        {
            if (CurrentUser == null) return;

            OpenChildForm(new FormProfile(CurrentUser));
        }

        // Người dùng click nút Đăng Xuất
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Hủy user hiện tại
                CurrentUser = null;

                // Mở lại FormLogin
                FormLogin login = new FormLogin();
                login.Show();
                this.Hide();

                // Khi FormLogin đóng thìthì đóng luôn UserMainForm
                login.FormClosed += (s2, e2) => this.Close();
            }
        }

        // Quay về giao diện trang chủ (xóa form con)
        public void GoHome()
        {
            // Nếu có form con → đóng lại
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }

            // Bật lại AutoScroll cho giao diện chính
            this.AutoScroll = true;
        }
        
        // Sự kiện click vào logo → quay về Home
        private void logo_Click(object sender, EventArgs e)
        {
            GoHome();
        }

        // Nút mở danh sách suất chiếu
        private void btnLichChieu_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            // Tạo hiệu ứng nút được chọn
            ActivateButton(btn);

            // Mở form danh sách suất chiếu
            OpenChildForm(new FormShowtimeList(this));
        }

        // Nút mở danh sách phim
        private void btnPhim_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);

            OpenChildForm(new FormMovieList(this));
        }
        
        // Thay đổi hiệu ứng (màu, font) cho nút menu được chọn
        private void ActivateButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            if (btn == null) return;

            // Reset hiệu ứng của nút trước đó
            if (currentButton != null)
            {
                currentButton.FillColor = currentButton.Tag != null
                    ? (Color)currentButton.Tag   // Màu gốc lưu trong Tag
                    : Color.FromArgb(44, 84, 115);

                currentButton.ForeColor = Color.White;
                currentButton.Font = new Font(currentButton.Font, FontStyle.Regular);
            }

            // Lưu màu gốc của nút để khi bỏ chọn có thể khôi phục
            if (btn.Tag == null)
                btn.Tag = btn.FillColor;

            // Kích hoạt hiệu ứng cho nút hiện tại
            currentButton = btn;
            currentButton.FillColor = Color.FromArgb(44, 84, 115);  // Màu nền khi chọn
            currentButton.ForeColor = Color.FromArgb(255, 128, 0);  // Màu chữ khi chọn
            currentButton.Font = new Font(currentButton.Font, FontStyle.Bold);
        }
        
        // Mở chat bot hỗ trợ khách hàng
        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            FormChatbot f = new FormChatbot(this);
            f.Show();
        }

        // Hàm xử lý sự kiện chọn hình ảnh nhỏ trong gallery
        private void picSmall_Click(object sender, EventArgs e)
        {
            var clickedPic = (Guna2PictureBox)sender;

            // Tắt hiệu ứng shadow của hình trước đó (nếu có)
            if (currentSelected != null)
            {
                currentSelected.ShadowDecoration.Enabled = false;
            }

            // Bật hiệu ứng shadow cho hình vừa chọn
            clickedPic.ShadowDecoration.Enabled = true;
            clickedPic.ShadowDecoration.Color = Color.FromArgb(245, 131, 35);
            clickedPic.ShadowDecoration.Depth = 15;

            // Gán ảnh nhỏ sang khung ảnh lớn
            if (clickedPic.Image != null)
            {
                gunaPicBig.Image = clickedPic.Image;
                gunaPicBig.SizeMode = PictureBoxSizeMode.Zoom;
            }

            // Lưu lại ảnh đã chọn
            currentSelected = clickedPic;
        }
    }
}
