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
    public partial class FormForgetPassword : Form
    {
        public FormForgetPassword()
        {
            InitializeComponent();
        }
        private UserMainForm parentForm;

        public FormForgetPassword(UserMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormResetPassword(parentForm));
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            var loginForm = new FormLogin(parentForm);
            parentForm.OpenChildForm(loginForm);
            loginForm.ShowLogin();
        }
    }
}
