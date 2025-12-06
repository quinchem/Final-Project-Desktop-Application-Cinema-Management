using AdminApp.Forms;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AdminApp
{
    public partial class FormForgetPassword : Form
    {
        private readonly StaffRepo _staffRepo = new StaffRepo();
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
            // Lấy email từ TextBox 
            string emailInput = txtEmail.Text.Trim();

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(emailInput))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập địa chỉ Email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                // Kiểm tra email có tồn tại trong Database không
                bool isExist = _staffRepo.CheckEmailExist(emailInput);

                if (!isExist)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Email này không tồn tại trong hệ thống. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.SelectAll();
                    txtEmail.Focus();
                }
                else
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                    player.Play();
                    MessageBox.Show("Email hợp lệ! Vui lòng đặt lại mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FormResetPassword resetForm = new FormResetPassword(parentForm, emailInput);
                    parentForm.OpenChildForm(resetForm);
                }
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Đã xảy ra lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        // Giúp quay lại login
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
            parentForm.ShowLoginPanel();
        }
    }
}
