using AdminApp.Models;

namespace AdminApp
{
    public partial class AdminMainForm : Form
    {
        private readonly string _staffId;
        public AdminMainForm(string staffId)
        {
            InitializeComponent();
            _staffId = staffId;
        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            ActivateButton(btnThongKe);
            OpenChildForm(new FormStatistics1());
        }
        private Form currentFormChild;
        private Guna.UI2.WinForms.Guna2Button currentButton;

        public void OpenChildForm(Form childForm)
        {
            // Nếu có form con đang mở thì đóng
            if (currentFormChild != null)
                currentFormChild.Close();

            panelMain.AutoScroll = false;

            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            // Khi form con đóng, bật lại AutoScroll
            childForm.FormClosed += (s, e) =>
            {
                panelMain.AutoScroll = true;
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


        private void btnSuatChieu_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);
            OpenChildForm(new FormShowManagement());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);
            OpenChildForm(new FormStatistics1());
        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);
            OpenChildForm(new FormMovieManagement());
        }

        private void btnSoDoGhe_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);
            OpenChildForm(new FormRoomLayoutManagement());
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);
            OpenChildForm(new FormProduct());
        }


        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn == null) return;

            ActivateButton(btn);
            OpenChildForm(new FormCustomerManagement());
        }

        private void picUserIcon_Click(object sender, EventArgs e)
        {
            
            OpenChildForm(new FormAccountManagement(_staffId));

        }
        public void GoHome()
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }

            panelMain.AutoScroll = true;

            // Reset nút active
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(51, 51, 76);
                currentButton = null;
            }
        }

        private void logo_Click(object sender, EventArgs e)
        {
            GoHome();
        }
    }
}
