using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Media;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormForgetPassword : Form
    {
        // Tạo đối tượng repo để làm việc với dữ liệu khách hàng
        private readonly CustomerRepo _customerRepo = new CustomerRepo();
        public FormForgetPassword()
        {
            InitializeComponent();
        }
        // Biến lưu form đăng nhập để dùng khi quay lại hoặc mở form mới
        private FormLogin parentForm;
        
        // Hàm tạo nhận vào form cha để có thể thao tác điều hướng
        public FormForgetPassword(FormLogin parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        // Hàm xử lý khi nhấn nút Gửi
        private void btnGui_Click(object sender, EventArgs e)
        {
            // Lấy nội dung email người dùng nhập vào rồi loại khoảng trắn
            string emailInput = txtEmail.Text.Trim();

            // Kiểm tra nếu người dùng chưa nhập email thì báo lỗi và yêu cầu nhập lại
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
                // Kiểm tra xem email có tồn tại trong cơ sở dữ liệu không
                bool isExist = _customerRepo.CheckEmailExist(emailInput);

                // Nếu email không tồn tại thì báo lỗi cho người dùng
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
                    // Nếu email tồn tại thì báo thành công
                    SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                    player.Play();
                    MessageBox.Show("Email hợp lệ! Vui lòng đặt lại mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở form đặt lại mật khẩu và truyền email sang form đó
                    parentForm.OpenChildForm(new FormResetPassword(parentForm, emailInput));
                }
            }
            catch (Exception ex)
            {
                // Nếu xảy ra lỗi hệ thống hoặc lỗi kết nối thì báo cho người dùng
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Đã xảy ra lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Hàm xử lý khi nhấn nút Quay lại
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            parentForm.ShowLogin();
        }
    }
}
