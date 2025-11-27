using AdminApp.Properties;
using ClosedXML.Excel;
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
using System.IO;

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
            f.FilmAdded += (s, ev) => LoadFilmData(); // Khi có phim mới, load lại DGV
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
           

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = new FilmRepo();
                var movies = repo.GetAllFilms(); // Lấy tất cả phim từ DB

                if (movies == null || movies.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog()
                { Filter = "Excel Workbook|*.xlsx", FileName = "Movies.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Movies");

                            // Header
                            worksheet.Cell(1, 1).Value = "STT";
                            worksheet.Cell(1, 2).Value = "Title";
                            worksheet.Cell(1, 3).Value = "Genre";
                            worksheet.Cell(1, 4).Value = "Language";
                            worksheet.Cell(1, 5).Value = "Director";
                            worksheet.Cell(1, 6).Value = "Actor";
                            worksheet.Cell(1, 7).Value = "Description";
                            worksheet.Cell(1, 8).Value = "Status";
                            worksheet.Cell(1, 9).Value = "Age Restriction";
                            worksheet.Cell(1, 10).Value = "Duration";
                            worksheet.Cell(1, 11).Value = "Purchase Price";
                            worksheet.Cell(1, 12).Value = "Release Date";
                            worksheet.Cell(1, 13).Value = "id";

                            // Dữ liệu
                            int row = 2;
                            int stt = 1;
                            foreach (var film in movies)
                            { 
                                
                                worksheet.Cell(row, 1).Value = stt++;
                                worksheet.Cell(row, 2).Value = film.title;
                                worksheet.Cell(row, 3).Value = film.genre;
                                worksheet.Cell(row, 4).Value = film.language;
                                worksheet.Cell(row, 5).Value = film.director;
                                worksheet.Cell(row, 6).Value = film.actor;
                                worksheet.Cell(row, 7).Value = film.description;
                                worksheet.Cell(row, 8).Value = film.status;
                                worksheet.Cell(row, 9).Value = film.age_restriction;
                                worksheet.Cell(row, 10).Value = film.duration;
                                worksheet.Cell(row, 11).Value = film.film_purchase_price;
                                worksheet.Cell(row, 12).Value = film.release_date;
                                worksheet.Cell(row, 13).Value = film.movie_id;
                                row++;
                            }

                            worksheet.Columns().AdjustToContents();

                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show("Xuất file Excel thành công!", "Thông báo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file Excel: " + ex.Message, "Lỗi");
            }
        }


        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = new FilmRepo();
                var movies = repo.GetAllFilms(); // Lấy tất cả phim từ DB

                if (movies == null || movies.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog()
                { Filter = "Excel Workbook|*.xlsx", FileName = "Movies.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Movies");

                            // Header
                            worksheet.Cell(1, 1).Value = "STT";
                            worksheet.Cell(1, 2).Value = "Title";
                            worksheet.Cell(1, 3).Value = "Genre";
                            worksheet.Cell(1, 4).Value = "Language";
                            worksheet.Cell(1, 5).Value = "Director";
                            worksheet.Cell(1, 6).Value = "Actor";
                            worksheet.Cell(1, 7).Value = "Description";
                            worksheet.Cell(1, 8).Value = "Status";
                            worksheet.Cell(1, 9).Value = "Age Restriction";
                            worksheet.Cell(1, 10).Value = "Duration";
                            worksheet.Cell(1, 11).Value = "Purchase Price";
                            worksheet.Cell(1, 12).Value = "Release Date";
                            worksheet.Cell(1, 13).Value = "id";

                            // Dữ liệu
                            int row = 2;
                            int stt = 1;
                            foreach (var film in movies)
                            { 
                                
                                worksheet.Cell(row, 1).Value = stt++;
                                worksheet.Cell(row, 2).Value = film.title;
                                worksheet.Cell(row, 3).Value = film.genre;
                                worksheet.Cell(row, 4).Value = film.language;
                                worksheet.Cell(row, 5).Value = film.director;
                                worksheet.Cell(row, 6).Value = film.actor;
                                worksheet.Cell(row, 7).Value = film.description;
                                worksheet.Cell(row, 8).Value = film.status;
                                worksheet.Cell(row, 9).Value = film.age_restriction;
                                worksheet.Cell(row, 10).Value = film.duration;
                                worksheet.Cell(row, 11).Value = film.film_purchase_price;
                                worksheet.Cell(row, 12).Value = film.release_date;
                                worksheet.Cell(row, 13).Value = film.movie_id;
                                row++;
                            }

                            worksheet.Columns().AdjustToContents();

                            workbook.SaveAs(sfd.FileName);
                            MessageBox.Show("Xuất file Excel thành công!", "Thông báo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file Excel: " + ex.Message, "Lỗi");
            }
        }

    }
}


