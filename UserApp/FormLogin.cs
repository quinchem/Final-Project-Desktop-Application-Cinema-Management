using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            this.Opacity = 0;
        }
        private void FormLogin_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 10;
            t.Tick += (s, a) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    t.Stop();
            };
            t.Start();
        }
        public void ShowLogin()
        {
            panelDangNhap.Visible = true;
            panelDangKy.Visible = false;
            panelDangNhap.BringToFront();
        }

        public void ShowRegister()
        {
            panelDangNhap.Visible = false;
            panelDangKy.Visible = true;
            panelDangKy.BringToFront();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            ShowLogin();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            ShowRegister();
        }
        public void SwitchToLogin()
        {
            ShowLogin();
        }

        public void SwitchToRegister()
        {
            ShowRegister();
        }
        private UserMainForm parentForm;

        public FormLogin(UserMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void btnQuenMk_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormForgetPassword(parentForm));
        }
    }
}
