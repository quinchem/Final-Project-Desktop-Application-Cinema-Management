using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserApp.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace UserApp
{
    public partial class FormProfile : Form
    {
        public FormProfile()
        {
            InitializeComponent();
        }

        // Hàm load UserControl vào panelContainer
        private void LoadUserControl(UserControl uc)
        {
            panelContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelContent.Controls.Add(uc);
            uc.BringToFront();
        }
        private Customer currentUser;

        public FormProfile(Customer user)
        {
            InitializeComponent();
            currentUser = user;
        }
        private void FormProfile_Load(object sender, EventArgs e)
        {
            LoadUserControl(new ProfileAccount(currentUser));   // Load mặc định khi mở form
        }

        // Nút Thông tin
        private void btnInformation_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProfileAccount(currentUser));
        }

        // Nút Đổi mật khẩu
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProfileChangePassword());
        }

        // Nút Lịch sử
        private void btnHistory_Click(object sender, EventArgs e)
        {
            LoadUserControl(new HistoryTicket());
        }

    }
}
