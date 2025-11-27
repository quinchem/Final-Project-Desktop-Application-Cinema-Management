using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdminApp.Forms;


namespace AdminApp
{
    public partial class FormForgetPassword : Form
    {
        public FormForgetPassword()
        {
            InitializeComponent();
        }
        private AdminMainForm parentForm;

        public FormForgetPassword(AdminMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            FormResetPassword resetForm = new FormResetPassword(parentForm);
            parentForm.OpenChildForm(resetForm);
        }

        // QUAY LẠI LOGIN
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();

            // Hiện lại panel đăng nhập
            parentForm.ShowLoginPanel();
        }
    }
}
