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
    public partial class FormViewDetailMovie : Form
    {
        private string movieId;

        public FormViewDetailMovie(string id)
        {
            InitializeComponent();
            movieId = id;

            LoadMovieDetails();
        }

        private void LoadMovieDetails()
        {
            try
            {
                var repo = new FilmRepo();
                var film = repo.GetById2(movieId);

                if (film != null)
                {
                    lblTitle.Text = film.title;
                    lblGenre.Text = film.genre;
                    lblLanguage.Text = film.language;
                    lblDirector.Text = film.director;
                    lblActor.Text = film.actor;
                    lblDescription.Text = film.description;
                    lblStatus.Text = film.status;
                    lblDuration.Text = film.duration.ToString() + " phút";
                    lblPrice.Text = film.film_purchase_price.HasValue ? film.film_purchase_price.Value.ToString() : "N/A";
                    lblAge.Text = film.age_restriction;
                    lblReleaseDate.Text = film.release_date;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin phim!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load thông tin phim: " + ex.Message);
            }
        }
    }
}
