using System;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormForgetPassword : Form
    {
        private FormLogin parentForm;

        // Constructor cha-truy-con
        public FormForgetPassword(FormLogin parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        // Nút Gửi → mở FormResetPassword trong FormLogin (cha)
        private void btnGui_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormResetPassword());
        }

        // Nút Quay Lại → quay về panel Đăng nhập trong FormLogin (cha)
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            parentForm.ShowLogin();   
            this.Close();        
        }
    }
}