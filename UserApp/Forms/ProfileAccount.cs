using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using UserApp.Models;

namespace UserApp
{
    public partial class ProfileAccount : UserControl
    {
        private Customer currentUser;

        public ProfileAccount(Customer user)
        {
            InitializeComponent();
            currentUser = user;

            InitGenderComboBox();
            LoadUserInfo();
        }

        // ===================== INIT =====================
        private void InitGenderComboBox()
        {
            CbGioiTinh.Items.Clear();
            CbGioiTinh.Items.AddRange(new[] { "Nam", "Nữ", "Khác" });
            CbGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // ===================== LOAD DATA =====================
        private void LoadUserInfo()
        {
            if (currentUser == null) return;

            txtHoTen.Text = currentUser.full_name ?? "";
            txtEmail.Text = currentUser.email ?? "";
            txtSDT.Text = currentUser.phone_number ?? "";
            txtDiachi.Text = currentUser.address ?? "";

            // Ngày sinh – chấp nhận nhiều format
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

            // Giới tính
            if (!string.IsNullOrWhiteSpace(currentUser.gender))
            {
                CbGioiTinh.SelectedItem = currentUser.gender;
            }
        }

        // ===================== SAVE =====================
        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateProfile();
        }

        private void UpdateProfile()
        {
            if (currentUser == null || string.IsNullOrWhiteSpace(currentUser.customer_id))
            {
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

            // ===================== VALIDATE =====================
            if (string.IsNullOrWhiteSpace(newName) ||
                string.IsNullOrWhiteSpace(newEmail) ||
                string.IsNullOrWhiteSpace(newPhone))
            {
                MessageBox.Show("Họ tên, Email và SĐT không được để trống!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPhone.Length != 10 || !newPhone.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải đúng 10 chữ số!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper2.GetConnection())
                {
                    conn.Open();

                    // ✅ Bật foreign key
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

                // ✅ Update object trong RAM
                currentUser.full_name = newName;
                currentUser.email = newEmail;
                currentUser.phone_number = newPhone;
                currentUser.gender = newGender;
                currentUser.date_of_birth = newDob;
                currentUser.address = newAddress;

                MessageBox.Show("✅ Cập nhật thông tin thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ===================== UI BO GÓC =====================
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
            Invalidate(); // vẽ lại UI
        }

        
    }
}
