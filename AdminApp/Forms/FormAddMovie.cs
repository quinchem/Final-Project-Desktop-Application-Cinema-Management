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
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AdminApp
{
    public partial class FormAddMovie : Form
    {
       
        private FilmRepo _filmRepo = new FilmRepo();
        private byte[] _posterImageData = null;
        public FormAddMovie()
        {
            InitializeComponent(); 
            LoadComboBoxData(); 
        }

        private void LoadComboBoxData()
        {
            try
            { 
                cboDoTuoi.Items.AddRange(new object[] { "P", "K", "T13", "T16", "T18" });
                cboTrangThai.Items.AddRange(new object[] { "Đang chiếu", "Sắp chiếu" });
                cboTrangThai.SelectedIndex = 0; 
            }
            catch (Exception ex)
            {
                // Giúp bắt lỗi và hiển thị thông báo nếu có lỗi xảy ra
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
                var lastId = movies.OrderByDescending(m => m.movie_id).First().movie_id; 
                int lastNum = int.Parse(lastId.Substring(1));
                nextNumber = lastNum + 1;
            }

            return "M" + nextNumber.ToString("D3"); 
        }
        public event EventHandler FilmAdded;

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Giúp bỏ ảnh cũ
                    if (picPoster.Image != null)
                    {
                        picPoster.Image.Dispose();
                        picPoster.Image = null;
                    }

                    // Giúp load ảnh mới
                    picPoster.Image = Image.FromFile(ofd.FileName);

                    // Giúp đọc file ảnh ở dạng byte
                    _posterImageData = File.ReadAllBytes(ofd.FileName);
                }
                catch (OutOfMemoryException)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Ảnh quá lớn hoặc hệ thống không đủ bộ nhớ. Vui lòng chọn ảnh nhỏ hơn.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    picPoster.Image = null;
                    _posterImageData = null;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                //  Giúp kiểm tra dữ liệu hợp lệ
                if (!ValidateInput())
                    return;
                // Giúp lấy dữ liệu từ form
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
                // Giúp lưu vào SQLite
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
                    if (_posterImageData != null)
                    {
                        string sqlImg = @"
                            INSERT INTO ImageStore (related_id, image_type, image_data)
                            VALUES (@related_id, @image_type, @image_data)
                        ";

                        using (var cmdImg = new SqliteCommand(sqlImg, conn))
                        {
                            cmdImg.Parameters.AddWithValue("@related_id", movieId);
                            cmdImg.Parameters.AddWithValue("@image_type", "poster");
                            cmdImg.Parameters.Add("@image_data", SqliteType.Blob).Value = _posterImageData;
                            cmdImg.ExecuteNonQuery();
                        }
                    }
                }

                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Thêm phim thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilmAdded?.Invoke(this, EventArgs.Empty); 
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
            // Tên phim
            if (string.IsNullOrWhiteSpace(txtTenPhim.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập tên phim!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhim.Focus();
                return false;
            }

            // Thể loại
            if (string.IsNullOrWhiteSpace(txtTheLoai.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập thể loại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTheLoai.Focus();
                return false;
            }

            if (!IsAlphabetic(txtTheLoai.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Thể loại chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTheLoai.Focus();
                return false;
            }

            // Ngôn ngữ
            if (string.IsNullOrWhiteSpace(txtNgonNgu.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập ngôn ngữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNgonNgu.Focus();
                return false;
            }

            if (!IsAlphabetic(txtNgonNgu.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Ngôn ngữ chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNgonNgu.Focus();
                return false;
            }

            // Ngôn ngữ
            if (string.IsNullOrWhiteSpace(txtDaoDien.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập tên đạo diễn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDaoDien.Focus();
                return false;
            }

            if (!IsAlphabetic(txtDaoDien.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Tên đạo diễn chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDaoDien.Focus();
                return false;
            }

            // Diễn viên
            if (string.IsNullOrWhiteSpace(txtDienVien.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập diễn viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDienVien.Focus();
                return false;
            }

            if (!IsAlphabetic(txtDienVien.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Tên diễn viên chỉ được chứa chữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDienVien.Focus();
                return false;
            }


            // Giá nhập
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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Giá nhập phim không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }

            // Độ tuổi
            if (cboDoTuoi.SelectedIndex == -1)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn độ tuổi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Thời lượng
            if (string.IsNullOrWhiteSpace(txtThoiLuong.Text))
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng nhập thời lượng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtThoiLuong.Focus();
                return false;
            }
            return true;
        }

        // Giúp xóa form sau khi thêm phim thành công để nhập phim mới
        private void ClearForm()
        {
            txtTenPhim.Clear();
            txtGiaNhap.Clear();
            txtNgonNgu.Clear();
            txtDaoDien.Clear();
            txtDienVien.Clear();
            txtThoiLuong.Clear();
            txtMoTa.Clear();
            txtTheLoai.Clear();
            if (picPoster.Image != null)
            {
                picPoster.Image.Dispose();
                picPoster.Image = null;
            }
            
            _posterImageData = null;
            dtNgayChieu.Value = DateTime.Now;
        }
        // Giúp hạn chế chỉ cho nhập số và dấu chấm cho giá nhập
        private void txtGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true; 
            // Giúp chặn nhập nhiều dấu chấm
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        // Giúp hạn chế chỉ cho phép nhập số nguyên cho thời lượng
        private void txtThoiLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
