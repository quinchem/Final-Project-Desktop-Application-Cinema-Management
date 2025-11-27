using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Data.Sqlite;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormEditMovie : Form
    {
        private string movieId;
        private byte[] _posterImageBytes; // lưu ảnh được chọn
        private ImageRepo _imageRepo = new ImageRepo();

        public FormEditMovie(string id)
        {
            InitializeComponent();
            movieId = id;
            LoadComboBoxData();
            LoadMovieInfo();
            LoadMoviePoster();
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

        private void LoadMovieInfo()
        {
            try
            {
                var filmRepo = new FilmRepo();
                var film = filmRepo.GetById2(movieId); // Lấy film theo ID
                if (film != null)
                {
                    // TextBox
                    txtTenPhim.Text = film.title;
                    txtTheLoai.Text = film.genre;
                    txtNgonNgu.Text = film.language;
                    txtDaoDien.Text = film.director;
                    txtDienVien.Text = film.actor;
                    txtMoTa.Text = film.description;
                    txtGiaNhap.Text = film.film_purchase_price?.ToString() ?? "";
                    txtThoiLuong.Text = film.duration.ToString();

                    // ComboBox: trạng thái
                    if (!string.IsNullOrEmpty(film.status))
                    {
                        if (cboTrangThai.Items.Contains(film.status))
                            cboTrangThai.SelectedItem = film.status;
                        else
                            cboTrangThai.Text = film.status; // fallback
                    }

                    // ComboBox: độ tuổi
                    if (!string.IsNullOrEmpty(film.age_restriction))
                    {
                        if (cboDoTuoi.Items.Contains(film.age_restriction))
                            cboDoTuoi.SelectedItem = film.age_restriction;
                        else
                            cboDoTuoi.Text = film.age_restriction;
                    }

                    // DateTimePicker: release_date
                    DateTime parsedDate;
                    if (!string.IsNullOrEmpty(film.release_date) &&
                        DateTime.TryParseExact(film.release_date, "dd/MM/yyyy", null,
                                               System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        dtNgayChieu.Value = parsedDate;
                    }
                    else
                    {
                        dtNgayChieu.Value = DateTime.Today;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin phim!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading movie: " + ex.Message);
            }
        }

        private void LoadMoviePoster()
        {
            try
            {
                byte[] imgData = _imageRepo.GetMoviePoster(movieId);
                if (imgData != null)
                {
                    _posterImageBytes = imgData;
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        // Đảm bảo PictureBox hiển thị hình gốc
                        picPoster.Image = Image.FromStream(ms);
                        picPoster.SizeMode = PictureBoxSizeMode.Zoom; // hiển thị vừa khung
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load poster: " + ex.Message);
            }
        }


        private void btnUploadPoster_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn poster";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _posterImageBytes = File.ReadAllBytes(ofd.FileName);
                        using (MemoryStream ms = new MemoryStream(_posterImageBytes))
                        {
                            picPoster.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tải ảnh: " + ex.Message);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var updatedFilm = new Film
                {
                    movie_id = movieId,
                    title = txtTenPhim.Text,
                    genre = txtTheLoai.Text,
                    language = txtNgonNgu.Text,
                    director = txtDaoDien.Text,
                    actor = txtDienVien.Text,
                    description = txtMoTa.Text,
                    status = cboTrangThai.Text,
                    film_purchase_price = string.IsNullOrEmpty(txtGiaNhap.Text) ? null : int.Parse(txtGiaNhap.Text),
                    duration = int.Parse(txtThoiLuong.Text),
                    age_restriction = cboDoTuoi.Text,
                    release_date = dtNgayChieu.Value.ToString("dd/MM/yyyy")
                };

                var filmRepo = new FilmRepo();
                filmRepo.UpdateFilm(updatedFilm);

                // ✅ Lưu poster nếu đã chọn
                if (_posterImageBytes != null)
                {
                    _imageRepo.SaveMoviePoster(movieId, _posterImageBytes);
                }

                MessageBox.Show("Cập nhật phim thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật phim: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
