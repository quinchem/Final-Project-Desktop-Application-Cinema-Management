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

namespace UserApp
{
    public partial class FormMovieDetail : Form
    {
        private string movieId;
        private FilmRepo _filmRepo = new FilmRepo();
        private ImageRepo _imageRepo = new ImageRepo();
        private UserMainForm _parentForm;
        public FormMovieDetail(string id, UserMainForm parentForm)
        {
            InitializeComponent();
            movieId = id;
            _parentForm = parentForm;
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
                    //lblStatus.Text = film.status;
                    lblDuration.Text = film.duration.ToString() + " phút";
                    //lblPrice.Text = film.film_purchase_price.HasValue ? film.film_purchase_price.Value.ToString() : "N/A";
                    lblAge.Text = film.age_restriction;
                    lblReleaseDate.Text = film.release_date;

                    // --- Giới hạn xuống dòng cho diễn viên ---
                    lblActor.AutoSize = true;
                    lblActor.MaximumSize = new Size(350, 0); // 400 là bề rộng label, có thể chỉnh
                    lblActor.Text = film.actor;

                    // --- Giới hạn xuống dòng cho mô tả ---
                    lblDescription.AutoSize = true;
                    lblDescription.MaximumSize = new Size(800, 0); // 400 là bề rộng label, có thể chỉnh
                    lblDescription.Text = film.description;
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
                MessageBox.Show("Lỗi khi load poster: " + ex.Message);
            }
        }
        
        
        private void btnDatVe_Click(object sender, EventArgs e)
        {
            // Tìm MainForm để gọi OpenChildForm()
            UserMainForm parent = this.ParentForm as UserMainForm;

            if (parent != null)
            {
                _parentForm.OpenChildForm(new FormShowtimeDetail(_parentForm, movieId));
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
            {
                _parentForm.OpenChildForm(new FormMovieList(_parentForm));
            }
        }
    }
}


