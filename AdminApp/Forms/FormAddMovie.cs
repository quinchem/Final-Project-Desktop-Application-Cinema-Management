using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.Devices;
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
using System.Text.RegularExpressions;
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
            rbActive.Checked = true;
        }

        // Hàm load dữ liệu vào Combo Box của độ tuổi
        private void LoadComboBoxData()
        {
            try
            {
                cboDoTuoi.Items.AddRange(new object[] { "P", "K", "T13", "T16", "T18" });
                
            }
            catch (Exception ex)
            {
                // Giúp bắt lỗi và hiển thị thông báo nếu có lỗi xảy ra
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm tự sinh ID mới cho phim với định dạng M00x bằng cách lấy ID cuối cùng trong database +1
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

        // Hàm cập nhật trạng thái phim dựa trên ngày chiếu và trạng thái radio button:
        // Nếu tick ngừng chiếu → luôn là ngừng chiếu
        // Nếu tick đang hoạt động → tự động xét ngày để ra đang chiếu / sắp chiếu
        private void UpdateStatus()
        {
            if (rbStopped.Checked)
            {
                lblTrangThai.Text = "Ngừng chiếu";
                return;
            }

            DateTime release = dtNgayChieu.Value;
            DateTime today = DateTime.Today;

            if (release > today)
                lblTrangThai.Text = "Sắp chiếu";
            else
                lblTrangThai.Text = "Đang chiếu";
        }


        // Hàm xử lý sự kiện thêm Poster phim, sau đó chuyển ảnh thành dạng byte để lưu vào CSDL
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

        // Hàm lưu thông tin phim, poster vào CSDL khi nhấn nút Thêm cùng với hiệu ứng âm thanh
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;
                    
                string movieId = GenerateNextMovieId();
                string title = txtTenPhim.Text;
                string genre = txtTheLoai.Text;
                string language = txtNgonNgu.Text;
                string director = txtDaoDien.Text;
                string actor = txtDienVien.Text;
                string description = txtMoTa.Text;
                string status = lblTrangThai.Text;
                int film_purchase_price = int.Parse(txtGiaNhap.Text);
                int duration = int.Parse(txtThoiLuong.Text);
                string age = cboDoTuoi.Text;
                string releaseDate = dtNgayChieu.Value.ToString("dd/MM/yyyy");
                
                // Lưu vào SQLite
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

     
        // Hàm validate các thông tin phim được nhập vào textbox và combobox
        private bool ValidateInput()
        {
           
            // Hàm check mềm cho các field text
            bool IsNameValid(string text)
            {
                // Cho phép chữ, khoảng trắng, dấu ', -, .
                return Regex.IsMatch(text, @"^[\p{L} .'-]+$");
            }

            bool IsGenreValid(string text)
            {
                // Cho phép chữ, số, dấu phẩy, gạch ngang
                return Regex.IsMatch(text, @"^[\p{L}0-9 ,'-]+$");
            }

            // Shortcut phát âm
            void PlayFail()
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
            }

            // TÊN PHIM
            if (string.IsNullOrWhiteSpace(txtTenPhim.Text))
            {
                PlayFail();
                MessageBox.Show("Vui lòng nhập tên phim!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhim.Focus();
                return false;
            }

            // THỂ LOẠI
            if (string.IsNullOrWhiteSpace(txtTheLoai.Text))
            {
                PlayFail();
                MessageBox.Show("Vui lòng nhập thể loại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTheLoai.Focus();
                return false;
            }

            if (!IsGenreValid(txtTheLoai.Text))
            {
                PlayFail();
                MessageBox.Show("Thể loại chỉ được chứa chữ, số hoặc dấu phẩy/gạch!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTheLoai.Focus();
                return false;
            }

            // NGÔN NGỮ
            if (string.IsNullOrWhiteSpace(txtNgonNgu.Text))
            {
                PlayFail();
                MessageBox.Show("Vui lòng nhập ngôn ngữ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNgonNgu.Focus();
                return false;
            }

            if (!IsNameValid(txtNgonNgu.Text))
            {
                PlayFail();
                MessageBox.Show("Ngôn ngữ không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNgonNgu.Focus();
                return false;
            }

            // ĐẠO DIỄN
            if (string.IsNullOrWhiteSpace(txtDaoDien.Text))
            {
                PlayFail();
                MessageBox.Show("Vui lòng nhập tên đạo diễn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDaoDien.Focus();
                return false;
            }

            if (!IsNameValid(txtDaoDien.Text))
            {
                PlayFail();
                MessageBox.Show("Tên đạo diễn không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDaoDien.Focus();
                return false;
            }

            // DIỄN VIÊN
            if (string.IsNullOrWhiteSpace(txtDienVien.Text))
            {
                PlayFail();
                MessageBox.Show("Vui lòng nhập diễn viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDienVien.Focus();
                return false;
            }

            // Cho phép nhiều tên cách nhau bằng dấu phẩy
            foreach (var name in txtDienVien.Text.Split(','))
            {
                if (!IsNameValid(name.Trim()))
                {
                    PlayFail();
                    MessageBox.Show("Danh sách diễn viên chứa tên không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDienVien.Focus();
                    return false;
                }
            }

            // GIÁ NHẬP
            if (string.IsNullOrWhiteSpace(txtGiaNhap.Text))
            {
                PlayFail();
                MessageBox.Show("Vui lòng nhập giá nhập phim!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }

            decimal giaNhap;
            if (!decimal.TryParse(txtGiaNhap.Text, out giaNhap) || giaNhap < 0)
            {
                PlayFail();
                MessageBox.Show("Giá nhập phim không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return false;
            }

            // ĐỘ TUỔI
            if (cboDoTuoi.SelectedIndex == -1)
            {
                PlayFail();
                MessageBox.Show("Vui lòng chọn độ tuổi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // THỜI LƯỢNG (phải là số)
            if (string.IsNullOrWhiteSpace(txtThoiLuong.Text))
            {
                PlayFail();
                MessageBox.Show("Vui lòng nhập thời lượng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtThoiLuong.Focus();
                return false;
            }

            int thoiLuong;
            if (!int.TryParse(txtThoiLuong.Text, out thoiLuong) || thoiLuong <= 0)
            {
                PlayFail();
                MessageBox.Show("Thời lượng phải là số phút hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtThoiLuong.Focus();
                return false;
            }

            // TRẠNG THÁI PHIM (Radio)
            if (!rbActive.Checked && !rbStopped.Checked)
            {
                PlayFail();
                MessageBox.Show("Vui lòng chọn trạng thái phim!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //NGÀY CHIẾU
            if (dtNgayChieu.Value.Year < 1900)
            {
                PlayFail();
                MessageBox.Show("Ngày chiếu không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        // Hàm xóa các thông tin được nhập trong textboxsau khi thêm phim thành công để nhập phim mới
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
        // Hàm xử lý sự kiện xóa Poster
        private void btnXoaPoster_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem hiện tại có ảnh hay không:
                // Nếu có thì giải phóng dung lượng ảnh cũ, để trống Picturebox, xóa dữ liệu byte của ảnh
                if (picPoster.Image != null)
                {
                    picPoster.Image.Dispose();

                    picPoster.Image = null;

                    _posterImageData = null;

                    SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                    player.Play();

                    MessageBox.Show("Đã xóa poster hiện tại.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Chưa có poster nào để xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa poster: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xử lý khi nhấn phím xuống (Dùng chung cho các TextBox 1 dòng)
        private void MoveFocusOnEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                SendKeys.Send("{TAB}");
            }
        }

        // Hàm xử lý sự kiện khi thay đổi trạng thái radio button hoặc ngày chiếu để cập nhật trạng thái phim
        private void rbActive_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void rbStopped_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void dtNgayChieu_ValueChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2CustomRadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

}
