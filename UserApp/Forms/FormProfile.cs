using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
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
            LoadUserControl(new ProfileAccount(currentUser));   
        }

        // Nút Thông tin
        private void btnInformation_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProfileAccount(currentUser));
        }

        // Nút Đổi mật khẩu
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            LoadUserControl(new ProfileChangePassword(currentUser));

        }

        // Nút Lịch sử
        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (currentUser == null)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng đăng nhập trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //string customer_id;
            // Tạo instance HistoryTicket với customerId
            HistoryTicket HistoryTicket = new HistoryTicket(currentUser.customer_id);
            //string customer_id = "C021";
            //HistoryTicket HistoryTicket = new HistoryTicket(customer_id);

            HistoryTicket.OnViewBillDetail += (billId) =>
            {
                // Khi bấm "Xem" → Hiển thị UserControl Chi Tiết
                HistoryTicketDetail detailUC = new HistoryTicketDetail(billId);

                // Đăng ký event "Quay lại"
                detailUC.BackToHistory += (s, e) =>
                {
                    // Khi bấm "Quay lại" → Quay về UserControl Lịch Sử Vé
                    LoadUserControl(HistoryTicket);
                };

                // Load UserControl Chi Tiết
                LoadUserControl(detailUC);
            };

            // Load UserControl Lịch Sử Vé lần đầu
            LoadUserControl(HistoryTicket);
        }
    }
}
