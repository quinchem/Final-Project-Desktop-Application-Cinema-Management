using SharedData.Models;
using SharedData.Repositories;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        public FormEditMovie(string id)
        {
            InitializeComponent();
            movieId = id;
            LoadMovieInfo();
        }

        private void LoadMovieInfo()
        {
            try
            {
                var filmRepo = new FilmRepo();
                var film = filmRepo.GetById(movieId); // Phương thức trong FilmRepo
                if (film != null)
                {
                    string movieId = Guid.NewGuid().ToString();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var updatedFilm = new Film
                {
                    movie_id = Guid.NewGuid().ToString(),
                    title = txtTenPhim.Text,
                    genre = txtTheLoai.Text,
                    language = txtNgonNgu.Text,
                    director = txtDaoDien.Text,
                    actor = txtDienVien.Text,
                    description = txtMoTa.Text,
                    status = cboTrangThai.Text,
                    film_purchase_price = int.Parse(txtGiaNhap.Text),
                    duration = int.Parse(txtThoiLuong.Text),
                    age_restriction = cboDoTuoi.Text,
                    release_date = dtNgayChieu.Value.ToString("dd/MM/yyyy")
                };

                var filmRepo = new FilmRepo();
                filmRepo.UpdateFilm(updatedFilm);

                MessageBox.Show("Cập nhật phim thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating movie: " + ex.Message);
            }
        }

    }
}
