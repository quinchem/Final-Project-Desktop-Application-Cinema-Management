using DocumentFormat.OpenXml.Office2010.Excel;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormViewDetailMovie : Form
    {
        private string movieId;
        private ImageRepo _imageRepo = new ImageRepo();

        public FormViewDetailMovie(string id)
        {
            InitializeComponent();
            movieId = id;
            LoadMovieDetails();
            LoadMoviePoster();
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

                    // --- Giới hạn xuống dòng cho diễn viên ---
                    lblActor.AutoSize = true;
                    lblActor.MaximumSize = new Size(350, 0); 
                    lblActor.Text = film.actor;

                    // --- Giới hạn xuống dòng cho mô tả ---
                    lblDescription.AutoSize = true;
                    lblDescription.MaximumSize = new Size(700, 0);
                    lblDescription.Text = film.description;
                }
                else
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Không tìm thấy thông tin phim!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi khi load thông tin phim: " + ex.Message);
            }
        }

        private void LoadMoviePoster()
        {
            try
            {
                byte[] imgData = _imageRepo.GetMoviePoster(movieId);
                if (imgData != null)
                {
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        picPoster.Image = Image.FromStream(ms);
                        picPoster.SizeMode = PictureBoxSizeMode.Zoom; // hiển thị vừa khung
                    }
                }
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi khi load poster: " + ex.Message);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
