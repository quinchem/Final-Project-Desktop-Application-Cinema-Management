using Microsoft.VisualBasic.ApplicationServices;
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
using SharedData.Models;

namespace UserApp
{
    public partial class FormMovieList : Form
    {
        private FilmRepo _filmRepo = new FilmRepo();
        private ImageRepo _imageRepo = new ImageRepo();

        public FormMovieList()
        {
            InitializeComponent();
            LoadMovies();
        }

        private void LoadMovies()
        {
            flowLayoutPanel1.Controls.Clear(); // xóa cũ
            var films = _filmRepo.GetAllFilms();

            foreach (var film in films)
            {
                // clone panel template
                Guna.UI2.WinForms.Guna2Panel panel = new Guna.UI2.WinForms.Guna2Panel();
                panel.Size = panelTemplate.Size;
                panel.BackColor = Color.FromArgb(92, 124, 150); // set nền đúng màu
                panel.ShadowDecoration.CustomizableEdges = panelTemplate.ShadowDecoration.CustomizableEdges;

                // Clone poster
                Guna.UI2.WinForms.Guna2PictureBox poster = new Guna.UI2.WinForms.Guna2PictureBox();
                poster.Size = poster1.Size;
                poster.Location = poster1.Location;
                poster.SizeMode = poster1.SizeMode;
                poster.BackColor = Color.Transparent;

                // load ảnh từ db
                byte[] posterBytes = _imageRepo.GetMoviePoster(film.movie_id);
                if (posterBytes != null)
                {
                    using (MemoryStream ms = new MemoryStream(posterBytes))
                    {
                        poster.Image = Image.FromStream(ms);
                    }
                }

                panel.Controls.Add(poster);

                // Tên phim
                Guna.UI2.WinForms.Guna2HtmlLabel lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
                lblTitle.Size = guna2HtmlLabel2.Size;
                lblTitle.Location = guna2HtmlLabel2.Location;
                lblTitle.Font = guna2HtmlLabel2.Font;
                lblTitle.ForeColor = Color.White;
                lblTitle.BackColor = Color.Transparent;
                lblTitle.Text = film.title;
                panel.Controls.Add(lblTitle);

                // Thời lượng
                Guna.UI2.WinForms.Guna2HtmlLabel lblDuration = new Guna.UI2.WinForms.Guna2HtmlLabel();
                lblDuration.Location = guna2HtmlLabel10.Location;
                lblDuration.Size = guna2HtmlLabel10.Size;
                lblDuration.ForeColor = Color.White;
                lblDuration.BackColor = Color.Transparent;
                lblDuration.Text = $"{film.duration} phút";
                panel.Controls.Add(lblDuration);

                // Độ tuổi
                Label lblAge = new Label();
                lblAge.Location = label2.Location;
                lblAge.Size = label2.Size;
                lblAge.ForeColor = Color.White;
                lblAge.BackColor = Color.Transparent;
                lblAge.Text = film.age_restriction;
                panel.Controls.Add(lblAge);

                // Ngày khởi chiếu
                Guna.UI2.WinForms.Guna2HtmlLabel lblRelease = new Guna.UI2.WinForms.Guna2HtmlLabel();
                lblRelease.Location = guna2HtmlLabel12.Location;
                lblRelease.Size = guna2HtmlLabel12.Size;
                lblRelease.ForeColor = Color.White;
                lblRelease.BackColor = Color.Transparent;
                lblRelease.Text = film.release_date;
                panel.Controls.Add(lblRelease);

                // Nút Đặt vé
                Guna.UI2.WinForms.Guna2Button btnBook = new Guna.UI2.WinForms.Guna2Button();
                btnBook.Size = guna2Button1.Size;
                btnBook.Location = guna2Button1.Location;
                btnBook.FillColor = guna2Button1.FillColor;
                btnBook.ForeColor = guna2Button1.ForeColor;
                btnBook.Font = guna2Button1.Font;
                btnBook.Text = "ĐẶT VÉ";
                btnBook.BorderRadius = guna2Button1.BorderRadius;
                btnBook.Click += (s, e) =>
                {
                    MessageBox.Show($"Bạn chọn đặt vé cho phim: {film.title}");
                    // ở đây gọi form đặt vé
                };
                panel.Controls.Add(btnBook);

                // Icon 1st prize (nếu muốn hiển thị)
                Guna.UI2.WinForms.Guna2PictureBox icon = new Guna.UI2.WinForms.Guna2PictureBox();
                icon.Size = icon1st.Size;
                icon.Location = icon1st.Location;
                icon.Image = icon1st.Image;
                icon.BackColor = Color.Transparent;
                panel.Controls.Add(icon);

                flowLayoutPanel1.Controls.Add(panel);
            }
        }
    }
}
