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

namespace UserApp.Forms
{
    public partial class FormComingMovieDetail : Form
    {
        private string movieId;
        private FilmRepo _filmRepo = new FilmRepo();
        private ImageRepo _imageRepo = new ImageRepo();
        private UserMainForm _parentForm;

        //Hàm Khởi tạo form chi tiết phim, lưu lại ID phim được chọn ở danh sách phim,
        // sau đó load thông tin phim và poster
        public FormComingMovieDetail(string id, UserMainForm parentForm)
        {
            InitializeComponent();
            movieId = id;
            _parentForm = parentForm;
            LoadMovieDetails();
            LoadMoviePoster();
        }

        // Hàm lấy thông tin chi tiết phim từ CSDL và gán vào các label tương ứng, sử dụng Film Repo
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

        // Hàm lấy poster phim từ CSDL dưới dạng bype và hiển thị vào pictureBox, sử dụng image Repo
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
                        picPoster.SizeMode = PictureBoxSizeMode.Zoom;
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


        // Hàm xử lý sự kiện nhi nhấn nút quay lại, sẽ quay về danh sách phim
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            if (_parentForm != null)
            {
                _parentForm.OpenChildForm(new FormComingMovieList(_parentForm));
            }
        }
    }
}
