namespace UserApp
{
    public partial class UserMainForm : Form
    {
        public UserMainForm()
        {
            InitializeComponent();
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
    }
}

