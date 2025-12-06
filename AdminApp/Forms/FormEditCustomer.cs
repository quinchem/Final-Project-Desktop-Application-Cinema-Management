using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormEditCustomer : Form
    {
        private readonly CustomerRepo repo = new CustomerRepo();
        private readonly Customer customer;
        public FormEditCustomer(Customer c)
        {
            InitializeComponent();
            CbGioiTinh.Items.Clear();
            CbGioiTinh.Items.AddRange(new string[]
            {
                    "Nam",
                    "Nữ",
            });
            CbGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            customer = c;
            LoadCustomerToForm();
        }

        // Phần này giúp load dữ liệu lên form con
        private void LoadCustomerToForm()
        {
            if (customer == null) return;

            txtTenKH.Text = customer.full_name;
            txtEmail.Text = customer.email;
            txtSDT.Text = customer.phone_number;
            txtDiaChi.Text = customer.address;
            txtThoiGian.Text = customer.create_date;

            if (!string.IsNullOrWhiteSpace(customer.gender))
            {
                CbGioiTinh.SelectedItem = customer.gender;
            }

            if (!string.IsNullOrWhiteSpace(customer.date_of_birth))
            {
                string[] formats = new[] { "dd/MM/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" };
                if (DateTime.TryParseExact(customer.date_of_birth, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                {
                    dtpNgaySinh.Value = dob;
                }
                else
                {
                    
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Họ tên không được để trống");
                return;
            }

            if (CbGioiTinh.SelectedIndex == -1)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn giới tính");
                return;
            }

            Customer updatedCustomer = new Customer
            {
                customer_id = customer.customer_id,          
                full_name = txtTenKH.Text.Trim(),
                gender = CbGioiTinh.SelectedItem.ToString(),
                date_of_birth = dtpNgaySinh.Value.ToString("dd/MM/yyyy"),
                phone_number = txtSDT.Text.Trim(),
                email = txtEmail.Text.Trim(),
                address = txtDiaChi.Text.Trim(),
                create_date = customer.create_date            
            };
            bool ok = repo.Update(updatedCustomer);         
            if (ok)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Cập nhật thông tin khách hàng thành công!");
                DialogResult = DialogResult.OK; 
                Close();
            }
            else
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không thể cập nhật khách hàng!");
            }
        }


    }
}


