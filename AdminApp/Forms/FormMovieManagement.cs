using AdminApp.Models;
using AdminApp.Properties;
using AdminApp.Repositories;
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

            // Đảm bảo thứ tự cột đúng Designer
            dgvMovies.Columns["title"].DisplayIndex = 0;
            dgvMovies.Columns["release_date"].DisplayIndex = 1;
            dgvMovies.Columns["status"].DisplayIndex = 2;
            dgvMovies.Columns["duration"].DisplayIndex = 3;
            dgvMovies.Columns["ChinhSua"].DisplayIndex = 4;
            dgvMovies.Columns["Xoa"].DisplayIndex = 5;
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
            var f = new FormAddShowTime();
            f.Show();
        }

    }
}


