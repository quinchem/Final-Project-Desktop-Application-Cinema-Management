using AdminApp.Models;
using AdminApp.Properties;
using AdminApp.Repositories;
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

namespace AdminApp
{
    public partial class FormMovieManagement : Form
    {
        private FilmRepo _filmRepo = new FilmRepo();

        public FormMovieManagement()
        {
            InitializeComponent();
            this.Load += FormMovieManagement_Load;
            dgvMovies.AutoGenerateColumns = false;
        }

        private void FormMovieManagement_Load(object sender, EventArgs e)
        {
            LoadFilmData();
        }

        private void LoadFilmData()
        {
            try
            {
                var films = _filmRepo.GetAllFilms();
                BindDataToGrid(films);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading films: " + ex.Message);
            }
        }
        private void BindDataToGrid(List<Film> films)
        {
            dgvMovies.DataSource = films;

            dgvMovies.Columns["title"].DisplayIndex = 0;
            dgvMovies.Columns["release_date"].DisplayIndex = 1;
            dgvMovies.Columns["status"].DisplayIndex = 2;
            dgvMovies.Columns["duration"].DisplayIndex = 3;
            dgvMovies.Columns["colEdit"].DisplayIndex = 4;
            dgvMovies.Columns["colDelete"].DisplayIndex = 5;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            List<Film> results;

            if (string.IsNullOrEmpty(keyword))
            {
                // Nếu textbox rỗng -> load tất cả phim
                results = _filmRepo.GetAllFilms();
            }
            else
            {
                results = _filmRepo.SearchFilmByName1(keyword);
            }

            BindDataToGrid(results);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var f = new FormAddMovie();
            f.Show();
        }


        private void dgvMovies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua header
            if (e.RowIndex < 0) return;

            // Lấy ID của dòng được click
           Film film = (Film)dgvMovies.Rows[e.RowIndex].DataBoundItem;
            string movie_id = film.movie_id;

            // 1) Click icon chỉnh sửa
            if (dgvMovies.Columns[e.ColumnIndex].Name == "colEdit")
            {
                FormEditMovie f = new FormEditMovie(movie_id);
                f.ShowDialog();
                LoadFilmData();   // Reload lại bảng sau khi sửa
                return;
            }

            // 2) Click icon xóa
            if (dgvMovies.Columns[e.ColumnIndex].Name == "colDelete")
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn xóa phim này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    DeleteMovie(movie_id);
                    LoadFilmData();
                }
                return;
            }
        }
        private void DeleteMovie(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM movie WHERE movie_id = @id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void dgvMovie_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Film film = (Film)dgvMovies.Rows[e.RowIndex].DataBoundItem;
            string movieId = film.movie_id;

            var f = new FormViewDetailMovie(movieId);
            f.Show();
        }


    }
}


