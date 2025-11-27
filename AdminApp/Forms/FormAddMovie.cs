using Microsoft.Data.Sqlite;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AdminApp
{
    public partial class FormAddMovie : Form
    {

        private FilmRepo _filmRepo = new FilmRepo();
        public FormAddMovie()
        {
            InitializeComponent(); // Khởi tạo các control trên form
            LoadComboBoxData(); // Load dữ liệu vào các ComboBox
        }

        private void LoadComboBoxData()
        {
            try
            {
                // 3.2: Load độ tuổi (dữ liệu cố định, không cần lấy từ DB)
                // ------------------------------------
                // P: Phổ thông, K: Trẻ em, T13: Trên 13 tuổi, T16: Trên 16, T18: Trên 18
                cboDoTuoi.Items.AddRange(new object[] { "P", "K", "T13", "T16", "T18" });

                // 3.3: Load trạng thái (dữ liệu cố định)
                // ------------------------------------
                cboTrangThai.Items.AddRange(new object[] { "Đang chiếu", "Sắp chiếu" });
                cboTrangThai.SelectedIndex = 0; // Mặc định chọn item đầu tiên
            }
            catch (Exception ex)
            {
                // Bắt lỗi và hiển thị thông báo nếu có lỗi xảy ra
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public string GenerateNextMovieId()
        {
            var movies = _filmRepo.GetAllFilms();
            int nextNumber = 1;

            if (movies.Count > 0)
            {
                // Lấy movie_id lớn nhất hiện tại
                var lastId = movies.OrderByDescending(m => m.movie_id).First().movie_id; // M001, M002…
                int lastNum = int.Parse(lastId.Substring(1)); // cắt chữ M ra
                nextNumber = lastNum + 1;
            }

            return "M" + nextNumber.ToString("D3"); // M001, M002, …
        }
        public event EventHandler FilmAdded;

    
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                //  Kiểm tra dữ liệu hợp lệ
                if (!ValidateInput())
                    return;


                // 3. Lấy dữ liệu từ form
                string movieId = GenerateNextMovieId();

                string title = txtTenPhim.Text;
                string genre = txtTheLoai.Text;
                string language = txtNgonNgu.Text;
                string director = txtDaoDien.Text;
                string actor = txtDienVien.Text;
                string description = txtMoTa.Text;
                string status = cboTrangThai.Text;
                int film_purchase_price = int.Parse(txtGiaNhap.Text);
                int duration = int.Parse(txtThoiLuong.Text);
                string age = cboDoTuoi.Text;
                string releaseDate = dtNgayChieu.Value.ToString("dd/MM/yyyy");


                // 4. Lưu vào SQLite
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = @"INSERT INTO movie 
(movie_id, title, description, genre, director, actor, release_date, language, age_restriction, duration, film_purchase_price, status)
VALUES
(@id, @title, @description, @genre, @director, @actor, @date, @language, @age, @duration, @film_purchase_price, @status)
";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        // Tạo id (GUID)
                        cmd.Parameters.AddWithValue("@id", movieId);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@genre", genre);
                        cmd.Parameters.AddWithValue("@film_purchase_price", film_purchase_price);
                        cmd.Parameters.AddWithValue("@date", releaseDate);
                        cmd.Parameters.AddWithValue("@director", director);
                        cmd.Parameters.AddWithValue("@language", language);
                        cmd.Parameters.AddWithValue("@actor", actor);
                        cmd.Parameters.AddWithValue("@age", cboDoTuoi.Text);
                        cmd.Parameters.AddWithValue("@duration", duration);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();
                    }

                }

                MessageBox.Show("Thêm phim thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilmAdded?.Invoke(this, EventArgs.Empty); // Báo cho FormMovieManagement
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsAlphabetic(string input)
        {
            // Chỉ cho phép chữ (kể cả tiếng Việt) và khoảng trắng
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[\p{L}\s]+$");
        }
        private bool ValidateInput()
        {
            // --- TÊN PHIM ---
            if (string.IsNullOrWhiteSpace(txtTenPhim.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phim!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhim.Focus();
                return false;
            }

            // --- THỂ LOẠI ---
            if (string.IsNullOrWhiteSpace(txtTheLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập thể loại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTheLoai.Focus();
                return false;
            }

            if (!IsAlphabetic(txtTheLoai.Text))
            {
                MessageBox.Show("Thể loại chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTheLoai.Focus();
                return false;
            }

            // --- NGÔN NGỮ ---
            if (string.IsNullOrWhiteSpace(txtNgonNgu.Text))
            {
                MessageBox.Show("Vui lòng nhập ngôn ngữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNgonNgu.Focus();
                return false;
            }

            if (!IsAlphabetic(txtNgonNgu.Text))
            {
                MessageBox.Show("Ngôn ngữ chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNgonNgu.Focus();
                return false;
            }

            // --- ĐẠO DIỄN ---
            if (string.IsNullOrWhiteSpace(txtDaoDien.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đạo diễn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDaoDien.Focus();
                return false;
            }

            if (!IsAlphabetic(txtDaoDien.Text))
            {
                MessageBox.Show("Tên đạo diễn chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDaoDien.Focus();
                return false;
            }

            // --- DIỄN VIÊN ---
            if (string.IsNullOrWhiteSpace(txtDienVien.Text))
            {
                MessageBox.Show("Vui lòng nhập diễn viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDienVien.Focus();
                return false;
            }

            if (!IsAlphabetic(txtDienVien.Text))
            {
                MessageBox.Show("Tên diễn viên chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDienVien.Focus();
                return false;
            }


            // --- GIÁ NHẬP ---
            if (string.IsNullOrWhiteSpace(txtGiaNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập giá nhập phim!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }

            decimal giaNhap;
            if (!decimal.TryParse(txtGiaNhap.Text, out giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập phim không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }

            // --- ĐỘ TUỔI ---
            if (cboDoTuoi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn độ tuổi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // --- THỜI LƯỢNG ---
            if (string.IsNullOrWhiteSpace(txtThoiLuong.Text))
            {
                MessageBox.Show("Vui lòng nhập thời lượng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtThoiLuong.Focus();
                return false;
            }
            return true;
        }




        // PHẦN 8: XÓA FORM (RESET)
        // =====================================================
        // Được gọi sau khi thêm phim thành công để nhập phim mới
        private void ClearForm()
        {
            // Xóa tất cả TextBox
            txtTenPhim.Clear();
            txtGiaNhap.Clear();
            txtNgonNgu.Clear();
            txtDaoDien.Clear();
            txtDienVien.Clear();
            txtThoiLuong.Clear();
            txtMoTa.Clear();
            txtTheLoai.Clear();

            // Reset DateTimePicker về ngày hiện tại
            dtNgayChieu.Value = DateTime.Now;

        }



        // PHẦN 9: VALIDATE INPUT TRONG TEXTBOX
        // =====================================================

        // 9.1: Chỉ cho phép nhập số và dấu chấm cho giá nhập
        // ------------------------------------
        // KeyPress: Sự kiện khi user nhấn phím
        private void txtGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // char.IsControl: Cho phép các phím điều khiển (Backspace, Delete...)
            // char.IsDigit: Kiểm tra có phải số (0-9)
            // e.KeyChar != '.': Cho phép dấu chấm
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true; // Chặn ký tự không hợp lệ

            // Chặn nhập nhiều dấu chấm
            // IndexOf('.'): Tìm vị trí dấu chấm, trả về -1 nếu không tìm thấy
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        // 9.2: Chỉ cho phép nhập số nguyên cho thời lượng
        // ------------------------------------
        private void txtThoiLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số và phím điều khiển
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
