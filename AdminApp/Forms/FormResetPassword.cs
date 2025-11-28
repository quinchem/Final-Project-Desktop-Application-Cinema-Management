using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp.Forms
{
    public partial class FormResetPassword : Form
    {
        private AdminMainForm parentForm;

        public FormResetPassword(AdminMainForm parent)
        {
            InitializeComponent();
            parentForm = parent;

            txtMKmoi.UseSystemPasswordChar = true;
            txtXacNhanMK.UseSystemPasswordChar = true;
        }

        private void btnDatLaiMK_Click(object sender, EventArgs e)
        {
            string username = "admin"; // hoặc truyền từ form Quên MK qua
            string newPass = txtMKmoi.Text.Trim();
            string confirmPass = txtXacNhanMK.Text.Trim();

            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp");
                return;
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE Account
                       SET password = @pass
                       WHERE username = @username";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pass", newPass);
                    cmd.Parameters.AddWithValue("@username", username);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Đặt lại mật khẩu thành công!");
                        parentForm.ShowLoginPanel();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản");
                    }
                }
            }
        }
    }
}
