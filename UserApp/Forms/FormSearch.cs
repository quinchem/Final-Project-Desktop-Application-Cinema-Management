using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UserApp.Forms
{
    public partial class FormSearch : Form
    {
        // Form cha để bật form con bằng OpenChildForm
        private UserMainForm _parentForm;

        // Repo tìm kiếm phim
        private SearchRepo _searchRepo = new SearchRepo();

        // Repo lấy ảnh poster
        private ImageRepo _imageRepo = new ImageRepo();

        // Lưu từ khóa đang tìm
        private string _keyword;


        // Constructor: nhận form cha và từ khóa
        public FormSearch(UserMainForm parentForm, string keyword)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _keyword = keyword;

            txtTimKiem.Text = keyword;       // Hiển thị keyword lên ô tìm kiếm
            LoadMovies();                    // Tải danh sách phim tìm được
        }


        // Hàm tải danh sách phim theo từ khóa
        private void LoadMovies()
        {
            flowLayoutPanel1.Controls.Clear();      // Xóa kết quả cũ

            var films = _searchRepo.SearchFilms(_keyword);   // Tìm phim theo từ khóa

            foreach (var film in films)
            {
                var poster = _imageRepo.GetMoviePoster(film.movie_id);  // Lấy ảnh poster
                var card = CreateFilmCard(film, poster);               // Tạo card phim
                flowLayoutPanel1.Controls.Add(card);                   // Thêm vào giao diện
            }
        }


        // Sự kiện nhấn nút tìm kiếm
        private void btnSearch_Click(object sender, EventArgs e)
        {
            _keyword = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(_keyword))
                LoadMovies();
        }


        // Sự kiện nhấn Enter trong ô tìm kiếm
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnTimKiem.PerformClick();      // Gọi nút tìm
            }
        }


        // Hàm tạo 1 card phim hoàn chỉnh
        private Panel CreateFilmCard(Film film, byte[] posterBytes)
        {
            Panel card = new Panel();
            card.AutoSize = true;
            card.Width = 300;
            card.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            card.BackColor = Color.Transparent;
            card.Margin = new Padding(20);


            // Poster phim
            PictureBox poster = new PictureBox();
            poster.Size = new Size(250, 300);
            poster.SizeMode = PictureBoxSizeMode.Zoom;
            poster.Location = new Point((card.Width - poster.Width) / 2, 10);
            poster.Cursor = Cursors.Hand;

            // Gán ảnh poster
            if (posterBytes != null)
            {
                using MemoryStream ms = new MemoryStream(posterBytes);
                poster.Image = Image.FromStream(ms);
            }
            else
                poster.BackColor = Color.Gray;

            // Mở chi tiết phim khi nhấn vào poster
            poster.Click += (s, e) =>
            {
                _parentForm.OpenChildForm(new FormMovieDetail(film.movie_id, _parentForm));
            };

            card.Controls.Add(poster);



            // Tên phim
            Label lblTitle = new Label();
            lblTitle.AutoSize = false;
            lblTitle.Width = card.Width - 20;
            lblTitle.Height = 45;
            lblTitle.Location = new Point(10, poster.Bottom + 10);
            lblTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Text = film.title;
            lblTitle.AutoEllipsis = true;
            card.Controls.Add(lblTitle);


            // Panel chứa độ tuổi và thời lượng
            FlowLayoutPanel infoPanel = new FlowLayoutPanel();
            infoPanel.FlowDirection = FlowDirection.LeftToRight;
            infoPanel.AutoSize = true;
            infoPanel.WrapContents = false;
            infoPanel.BackColor = Color.Transparent;

            // Đặt infoPanel dưới dòng ngày chiếu
            infoPanel.Location = new Point((card.Width - 200) / 2, lblTitle.Bottom + 6);



            // Thời lượng
            Label lblDuration = new Label();
            lblDuration.AutoSize = true;
            lblDuration.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDuration.ForeColor = Color.White;
            lblDuration.Text = $"{film.duration} PHÚT".ToUpper();

            // Dấu phân cách
            Label lblDot = new Label();
            lblDot.AutoSize = true;
            lblDot.ForeColor = Color.White;
            lblDot.Text = "  |  ";

            // Độ tuổi
            Label lblAge = new Label();
            lblAge.AutoSize = true;
            lblAge.Font = new Font("Segoe UI Black", 10F, FontStyle.Bold);
            lblAge.Text = film.age_restriction;
            lblAge.ForeColor = film.age_restriction switch
            {
                "P" => Color.LimeGreen,
                "K" => Color.Pink,
                "T13" => Color.Yellow,
                "T16" => Color.Orange,
                "T18" => Color.Red,
                _ => Color.White
            };

            infoPanel.Controls.Add(lblDuration);
            infoPanel.Controls.Add(lblDot);
            infoPanel.Controls.Add(lblAge);

            card.Controls.Add(infoPanel);

            // Ngày khởi chiếu
            Label lblDate = new Label();
            lblDate.AutoSize = false;
            lblDate.Width = card.Width - 20;
            lblDate.Height = 20;
            lblDate.Location = new Point(10, infoPanel.Bottom + 6);
            lblDate.Font = new Font("Segoe UI", 10F);
            lblDate.ForeColor = Color.White;
            lblDate.TextAlign = ContentAlignment.MiddleCenter;
            lblDate.Text = "KHỞI CHIẾU: " + film.release_date;
            card.Controls.Add(lblDate);


            // Nút đặt vé
            var btn = new Guna.UI2.WinForms.Guna2Button();
            btn.Text = "ĐẶT VÉ";
            btn.FillColor = Color.FromArgb(245, 131, 35);
            btn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btn.BorderRadius = 12;
            btn.ForeColor = Color.White;
            btn.Size = new Size(140, 40);

            // Nút nằm dưới ngày chiếu
            btn.Location = new Point((card.Width - btn.Width) / 2, lblDate.Bottom + 12);


            // Mở form suất chiếu khi nhấn nút
            btn.Click += (s, e) =>
            {
                _parentForm.OpenChildForm(new FormShowtimeDetail(_parentForm, film.movie_id));
            };

            // Nếu phim chưa chiếu thì ẩn nút
            if (DateTime.TryParse(film.release_date, out DateTime release) &&
                release > DateTime.Now.Date)
            {
                btn.Visible = false;
            }

            card.Controls.Add(btn);


            return card;
        }
    }
}
