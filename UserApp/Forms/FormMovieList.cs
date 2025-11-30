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

namespace UserApp
{
    public partial class FormMovieList : Form
    {
        private FilmRepo _filmRepo = new FilmRepo();
        private ImageRepo _imageRepo = new ImageRepo();
        private UserMainForm _parentForm;
        string movieId;

        public FormMovieList(UserMainForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            LoadMovies();
        }

        private void LoadMovies()
        {
            FilmRepo repo = new FilmRepo();
            ImageRepo imgRepo = new ImageRepo();

            var films = _filmRepo.GetCurrentlyShowingFilms1();
            flowLayoutPanel1.Controls.Clear();
            foreach (var film in films)
            {
                var posterBytes = _imageRepo.GetMoviePoster(film.movie_id);
                var card = CreateFilmCard(film, posterBytes);
                flowLayoutPanel1.Controls.Add(card);
            }
        }

        private Panel CreateFilmCard(Film film, byte[] posterBytes)
        {
            Panel panel = new Panel();
            panel.Size = new Size(250, 420);
            panel.BackColor = Color.FromArgb(92, 124, 150);
            panel.Margin = new Padding(20);

            // ---------------- POSTER ----------------
            PictureBox poster = new PictureBox();
            poster.Size = new Size(180, 230);
            poster.SizeMode = PictureBoxSizeMode.Zoom;
            poster.Location = new Point((panel.Width - poster.Width) / 2, 10);
            poster.Cursor = Cursors.Hand; // đổi con trỏ chuột khi hover

            if (posterBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(posterBytes))
                    poster.Image = Image.FromStream(ms);
            }
            else poster.BackColor = Color.Gray;

            // Thêm sự kiện click mở FormMovieDetail theo movie_id
            poster.Click += (s, e) =>
            {
                if (_parentForm != null)
                {
                    _parentForm.OpenChildForm(new FormMovieDetail(film.movie_id, _parentForm));

                }
            };

            panel.Controls.Add(poster);

            // ---------------- TÊN PHIM ----------------
            Label lblTitle = new Label();
            lblTitle.AutoSize = false;
            lblTitle.Width = panel.Width - 20;
            lblTitle.Height = 32;
            lblTitle.Location = new Point(10, poster.Bottom + 10);
            lblTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Hiển thị tên phim 1 dòng, có dấu "..." nếu quá dài
            lblTitle.Text = film.title;
            lblTitle.AutoEllipsis = true;

            // Tooltip hiển thị full title khi hover
            ToolTip tt = new ToolTip();
            tt.SetToolTip(lblTitle, film.title);

            panel.Controls.Add(lblTitle);

            panel.Controls.Add(lblTitle);

            // ---------------- THỜI LƯỢNG | TUỔI ----------------
            FlowLayoutPanel infoPanel = new FlowLayoutPanel();
            infoPanel.AutoSize = true;
            infoPanel.FlowDirection = FlowDirection.LeftToRight;
            infoPanel.WrapContents = false;
            infoPanel.Location = new Point(10, lblTitle.Bottom + 6);
            infoPanel.BackColor = Color.Transparent;

            // Label thời lượng
            Label lblDuration = new Label();
            lblDuration.AutoSize = true;
            lblDuration.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblDuration.ForeColor = Color.White;
            lblDuration.BackColor = Color.Transparent;
            lblDuration.Text = $"{film.duration} PHÚT";

            // Label dấu |
            Label lblSeparator = new Label();
            lblSeparator.AutoSize = true;
            lblSeparator.Font = new Font("Segoe UI ", 10F, FontStyle.Regular);
            lblSeparator.ForeColor = Color.White;
            lblSeparator.BackColor = Color.Transparent;
            lblSeparator.Text = " | ";

            // Label tuổi với màu
            Label lblAge = new Label();
            lblAge.AutoSize = true;
            lblAge.Font = new Font("Segoe UI Black", 10F, FontStyle.Bold);
            lblAge.TextAlign = ContentAlignment.MiddleCenter;
            lblAge.BackColor = Color.Transparent;
            lblAge.Text = film.age_restriction;
            lblAge.ForeColor = film.age_restriction switch
            {
                "P" => Color.LimeGreen,
                "K" => Color.HotPink,
                "T13" => Color.Yellow,
                "T16" => Color.Orange,
                "T18" => Color.FromArgb(232, 81, 81),
                _ => Color.White
            };

            infoPanel.Controls.Add(lblDuration);
            infoPanel.Controls.Add(lblSeparator);
            infoPanel.Controls.Add(lblAge);

            // Canh giữa infoPanel
            infoPanel.Left = (panel.Width - infoPanel.PreferredSize.Width) / 2;
            infoPanel.Height = infoPanel.PreferredSize.Height;

            panel.Controls.Add(infoPanel);

            // ---------------- KHỞI CHIẾU ----------------
            Label lblDate = new Label();
            lblDate.AutoSize = false;
            lblDate.Width = panel.Width - 20;
            lblDate.Height = 20;
            lblDate.Location = new Point(10, infoPanel.Bottom + 6);
            lblDate.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblDate.ForeColor = Color.White;
            lblDate.BackColor = Color.Transparent;
            lblDate.TextAlign = ContentAlignment.MiddleCenter;
            lblDate.Text = $"KHỞI CHIẾU: {film.release_date}";
            panel.Controls.Add(lblDate);

            // ---------------- BUTTON ĐẶT VÉ ----------------
            Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();
            btn.Text = "ĐẶT VÉ";
            btn.FillColor = Color.FromArgb(245, 131, 35);
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BorderRadius = 10;
            btn.Size = new Size(120, 38);
            btn.Location = new Point((panel.Width - btn.Width) / 2, panel.Height - 55);
            btn.Tag = film.movie_id;
            btn.Click += (s, e) =>
            {
                if (_parentForm != null)
                {
                    btn.Click += (s, e) =>
                    {
                        if (_parentForm != null)
                        {
                            _parentForm.OpenChildForm(new FormShowtimeDetail(_parentForm, film.movie_id));
                        }
                    };

                }
            };

            panel.Controls.Add(btn);

            return panel;
        }



    }
}


