using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
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

            LoadUserInfo();
        }

        private void pctAvatar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pctAvatar.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void LoadUserInfo()
        {
            if (currentUser == null) return;

            txtHoTen.Text = currentUser.full_name;
            txtEmail.Text = currentUser.email;
            txtSDT.Text = currentUser.phone_number;
            dtpNgaysinh.Value = DateTime.ParseExact(currentUser.date_of_birth, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            txtDiachi.Text = currentUser.address;

            // --- Set RadioButton ---
            if ((currentUser.gender + "").Trim().ToLower() == "nam")
            {
                radNam.Checked = true;
            }
            else if ((currentUser.gender + "").Trim().ToLower() == "nữ"
                  || (currentUser.gender + "").Trim().ToLower() == "nu")
            {
                radNu.Checked = true;
            }
            else
            {
                // Không rõ giới tính → bỏ check cả 2 (nếu muốn)
                radNam.Checked = false;
                radNu.Checked = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateProfile();
        }

        private void UpdateProfile()
        {
            if (currentUser == null)
            {
                MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy dữ liệu mới từ UI
            string newName = txtHoTen.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string newPhone = txtSDT.Text.Trim();
            string newAddress = txtDiachi.Text.Trim();
            string newDob = dtpNgaysinh.Value.ToString("dd-MM-yyyy");
            string newGender = radNam.Checked ? "Nam" : radNu.Checked ? "Nữ" : "";

            // Validate cơ bản
            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newEmail) || string.IsNullOrWhiteSpace(newPhone))
            {
                MessageBox.Show("Họ tên, Email và SĐT không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPhone.Length != 10 || !newPhone.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải đúng 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = DatabaseHelper2.GetConnection())
                {
                    conn.Open();

                    using (var tx = conn.BeginTransaction())
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = @"UPDATE customer
                                        SET full_name = @name,
                                            email = @email,
                                            phone_number = @phone,
                                            gender = @gender,
                                            date_of_birth = @dob,
                                            address = @address
                                        WHERE customer_id = @id";

                            cmd.Parameters.AddWithValue("@name", newName);
                            cmd.Parameters.AddWithValue("@email", newEmail);
                            cmd.Parameters.AddWithValue("@phone", newPhone);
                            cmd.Parameters.AddWithValue("@gender", newGender);
                            cmd.Parameters.AddWithValue("@dob", newDob);
                            cmd.Parameters.AddWithValue("@address", newAddress);
                            cmd.Parameters.AddWithValue("@id", currentUser.customer_id);

                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                }

                // Cập nhật lại currentUser trong RAM
                currentUser.full_name = newName;
                currentUser.email = newEmail;
                currentUser.phone_number = newPhone;
                currentUser.gender = newGender;
                currentUser.date_of_birth = newDob;
                currentUser.address = newAddress;

                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
