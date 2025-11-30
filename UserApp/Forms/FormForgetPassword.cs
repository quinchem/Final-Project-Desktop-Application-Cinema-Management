using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormForgetPassword : Form
    {
        private readonly CustomerRepo _customerRepo = new CustomerRepo();
        public FormForgetPassword()
        {
            InitializeComponent();
        }

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
                bool isExist = _customerRepo.CheckEmailExist(emailInput);

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

        // Nút Quay Lại 
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            parentForm.OpenChildForm(new FormLogin());
        }
    }
}