using AdminApp.Forms;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            // 1. Lấy email từ TextBox (Giả sử tên control là txtEmail)
            string emailInput = txtEmail.Text.Trim();

            // 2. Kiểm tra rỗng
            if (string.IsNullOrEmpty(emailInput))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ Email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                // 3. Kiểm tra email có tồn tại trong Database không
                // Hàm CheckEmailExist cần trả về true nếu tìm thấy, false nếu không
                bool isExist = _staffRepo.CheckEmailExist(emailInput);

                if (!isExist)
                {
                    // Trường hợp Email KHÔNG tồn tại
                    MessageBox.Show("Email này không tồn tại trong hệ thống. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.SelectAll();
                    txtEmail.Focus();
                }
                else
                {
                    // Trường hợp Email Có tồn tại
                    MessageBox.Show("Email hợp lệ! Vui lòng đặt lại mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 4. Mở form Reset Password
                    FormResetPassword resetForm = new FormResetPassword(parentForm, emailInput);
                    parentForm.OpenChildForm(resetForm);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
