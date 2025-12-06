using Microsoft.Data.Sqlite;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace UserApp
{
    public partial class ProfileAccount : UserControl
    {
        private Customer currentUser;
        private readonly CustomerRepo _customerRepo;
        private readonly ImageRepo _imageRepo;
        private readonly string _customerId; 
        private readonly int _avatarSize = 200;

        public ProfileAccount(Customer user)
        {
            InitializeComponent();
            currentUser = user;
            _customerRepo = new CustomerRepo();
            _imageRepo = new ImageRepo();
            _customerId = user.customer_id;  // tự lấy từ object user

            InitGenderComboBox();
            LoadUserInfo();
        }

       
        private void InitGenderComboBox()
        {
            CbGioiTinh.Items.Clear();
            CbGioiTinh.Items.AddRange(new[] { "Nam", "Nữ", "Khác" });
            CbGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Load dữ liệu
        private void LoadUserInfo()
        {
            if (currentUser == null) return;

            txtHoTen.Text = currentUser.full_name ?? "";
            txtEmail.Text = currentUser.email ?? "";
            txtSDT.Text = currentUser.phone_number ?? "";
            txtDiachi.Text = currentUser.address ?? "";

            // Định dạng lại format ngày sinh
            if (!string.IsNullOrWhiteSpace(currentUser.date_of_birth))
            {
                string[] formats =
                {
                    "dd-MM-yyyy",
                    "dd/MM/yyyy",
                    "yyyy-MM-dd",
                    "yyyy-MM-dd HH:mm:ss"
                };

                if (DateTime.TryParseExact(
                    currentUser.date_of_birth,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dob))
                {
                    dtpNgaySinh.Value = dob;
                }
            }

            
            if (!string.IsNullOrWhiteSpace(currentUser.gender))
            {
                CbGioiTinh.SelectedItem = currentUser.gender;
            }
            try
            {
                byte[] imgBytes = _imageRepo.GetCustomerAvatar(_customerId);
                if (imgBytes != null && imgBytes.Length > 0)
                {
                    using (var ms = new MemoryStream(imgBytes))
                    {
                        pctAvatar.Image = Image.FromStream(ms);
                    }
                }
            }
            catch { /* Bỏ qua lỗi load ảnh nếu muốn */ }
        }

        // Lưu avatar
        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateProfile();
        }

        private void UpdateProfile()
        {
            if (currentUser == null || string.IsNullOrWhiteSpace(currentUser.customer_id))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string newName = txtHoTen.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string newPhone = txtSDT.Text.Trim();
            string newAddress = txtDiachi.Text.Trim();
            string newDob = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
            string newGender = CbGioiTinh.SelectedItem?.ToString() ?? "";

            // Kiểm tra tính hợp lệ
            if (string.IsNullOrWhiteSpace(newName) ||
                string.IsNullOrWhiteSpace(newEmail) ||
                string.IsNullOrWhiteSpace(newPhone))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Họ tên, Email và SĐT không được để trống!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPhone.Length != 10 || !newPhone.All(char.IsDigit))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Số điện thoại phải đúng 10 chữ số!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                   
                    using (var pragma = conn.CreateCommand())
                    {
                        pragma.CommandText = "PRAGMA foreign_keys = ON;";
                        pragma.ExecuteNonQuery();
                    }

                    using (var tx = conn.BeginTransaction())
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tx;
                        cmd.CommandText = @"
                    UPDATE customer
                    SET full_name     = @name,
                        email         = @email,
                        phone_number  = @phone,
                        gender        = @gender,
                        date_of_birth = @dob,
                        address       = @address
                    WHERE customer_id = @id
                ";

                        cmd.Parameters.AddWithValue("@name", newName);
                        cmd.Parameters.AddWithValue("@email", newEmail);
                        cmd.Parameters.AddWithValue("@phone", newPhone);
                        cmd.Parameters.AddWithValue("@gender", newGender);
                        cmd.Parameters.AddWithValue("@dob", newDob);
                        cmd.Parameters.AddWithValue("@address", newAddress);
                        cmd.Parameters.AddWithValue("@id", currentUser.customer_id);

                        int affected = cmd.ExecuteNonQuery();

                        if (affected <= 0)
                        {
                            tx.Rollback();
                            MessageBox.Show("Không có dữ liệu nào được cập nhật.\nVui lòng thử đăng nhập lại!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        tx.Commit();
                    }
                }

                // Update object trong RAM
                currentUser.full_name = newName;
                currentUser.email = newEmail;
                currentUser.phone_number = newPhone;
                currentUser.gender = newGender;
                currentUser.date_of_birth = newDob;
                currentUser.address = newAddress;

                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Cập nhật thông tin thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi khi cập nhật dữ liệu:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int radius = 20;
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(Width - d, 0, d, d, 270, 90);
            path.AddArc(Width - d, Height - d, d, d, 0, 90);
            path.AddArc(0, Height - d, d, d, 90, 90);

            path.CloseFigure();
            Region = new Region(path);

            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void ProfileAccount_Load(object sender, EventArgs e)
        {
            Invalidate(); 
        }

        private void pctAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "Chọn ảnh avatar";
                    ofd.Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                    if (ofd.ShowDialog() != DialogResult.OK) return;

                    // Resize ảnh avatar
                    using (var src = Image.FromFile(ofd.FileName))
                    using (var resized = ResizeImageToSquare(src, _avatarSize))
                    {
                        // Hiển thị lên UI ngay lập tức
                        // (Clone ra bitmap mới để tránh lỗi stream đóng)
                        pctAvatar.Image = new Bitmap(resized);

                        // Lưu ảnh avatar vào database
                        byte[] imageBytes;
                        using (var ms = new MemoryStream())
                        {
                            resized.Save(ms, ImageFormat.Png);
                            imageBytes = ms.ToArray();
                        }

                        bool success = _imageRepo.SaveCustomerAvatar(_customerId, imageBytes);

                        if (success)
                        {
                            //MessageBox.Show("Cập nhật avatar thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                            player.Play();
                            MessageBox.Show("Lưu vào CSDL thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

       
        private Image ResizeImageToSquare(Image src, int size)
        {
            // tính crop trung tâm vuông
            int srcW = src.Width;
            int srcH = src.Height;
            int side = Math.Min(srcW, srcH);
            int x = (srcW - side) / 2;
            int y = (srcH - side) / 2;

            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(src, new Rectangle(0, 0, size, size), new Rectangle(x, y, side, side), GraphicsUnit.Pixel);
            }
            return bmp;
        }
    }
}
