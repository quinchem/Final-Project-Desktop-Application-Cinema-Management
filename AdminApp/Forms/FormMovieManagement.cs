using AdminApp.Repositories;
using AdminApp.Properties;
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
        MovieRepository _movieRepo = new MovieRepository();

        public FormMovieManagement()
        {
            InitializeComponent();
            this.Load += FormMovieManagement_Load;
            dgvMovies.AutoGenerateColumns = false;
        }

        private void FormMovieManagement_Load(object sender, EventArgs e)
        {
            LoadMovieData();
        }

        private void LoadMovieData()
        {
            try
            {
                var movies = _movieRepo.GetAllMovies();

                // Gán dữ liệu theo đúng DataPropertyName của từng cột
                dgvMovies.DataSource = movies;

                // Đảm bảo thứ tự cột đúng Designer
                dgvMovies.Columns["title"].DisplayIndex = 0;
                dgvMovies.Columns["release_date"].DisplayIndex = 1;
                dgvMovies.Columns["status"].DisplayIndex = 2;
                dgvMovies.Columns["duration"].DisplayIndex = 3;
                dgvMovies.Columns["ChinhSua"].DisplayIndex = 4;
                dgvMovies.Columns["Xoa"].DisplayIndex = 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading movies: " + ex.Message);

            }
        }
    }
}
        namespace AdminApp.Models
    {
        public class Movie
        {
            public string MovieId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Genre { get; set; }
            public string Director { get; set; }
            public string Actor { get; set; }
            public string ReleaseDate { get; set; }
            public string Language { get; set; }
            public string AgeRestriction { get; set; }
            public int Duration { get; set; }
            public int FilmPurchasePrice { get; set; }
            public string Status { get; set; }
        }
    }


