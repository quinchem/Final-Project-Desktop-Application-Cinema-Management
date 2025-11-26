using AdminApp.Forms;
using AdminApp.Models;
using AdminApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormShowManagement : Form
    {
        private readonly FilmRepo _filmRepo = new FilmRepo();
        private readonly AuditoriumRepo _audRepo = new AuditoriumRepo();
        private readonly AuditoriumTypeRepo _audTypeRepo = new AuditoriumTypeRepo();
        private readonly SeatRepo _seatRepo = new SeatRepo();   // ⭐ Thêm repo giá vé

        private string _filterFilmId = null;
        private string _filterRoomId = null;
        private DateTime? _filterDate = null;

        public FormShowManagement()
        {
            InitializeComponent();
            dgvShowtime.AutoGenerateColumns = false;
        }

        private void FormShowManagement_Load(object sender, EventArgs e)
        {
            LoadShow();
        }

        // =====================================================
        // LOAD SHOWTIME
        // =====================================================
        private void LoadShow()
        {
            List<Showtime> shows;

            if (_filterFilmId != null && _filterDate != null)
                shows = ShowtimeRepo.GetByFilmAndDate(_filterFilmId, _filterDate.Value);
            else if (_filterFilmId != null)
                shows = ShowtimeRepo.GetByFilm(_filterFilmId);
            else if (_filterDate != null)
                shows = ShowtimeRepo.GetByDate(_filterDate.Value);
            else
                shows = ShowtimeRepo.GetAll();

            if (_filterRoomId != null)
                shows = shows.Where(s => s.auditorium_id == _filterRoomId).ToList();

            var display = new List<ShowtimeDisplay>();

            foreach (var s in shows)
            {
                var film = _filmRepo.GetById(s.movie_id);
                var room = _audRepo.GetById(s.auditorium_id);
                var type = room != null ? _audTypeRepo.GetById(room.auditorium_type_id) : null;

                // ⭐ Lấy giá vé theo phòng chiếu
                double price = _seatRepo.GetTicketPriceByAuditorium(s.auditorium_id);

                display.Add(new ShowtimeDisplay
                {
                    showtime_id = s.showtime_id,
                    title = film?.title ?? "",
                    name = room?.name ?? "",
                    auditorium_type = type?.auditorium_type ?? "",
                    per_seat_ticket_price = price.ToString("N0"),
                    show_date = s.show_date,
                    start_time = $"{s.start_time} - {s.end_time}"
                });
            }

            dgvShowtime.DataSource = null;
            dgvShowtime.DataSource = display;
        }

        // ================== BUTTON EVENTS ==================

        private void btnTim_Click(object sender, EventArgs e)
        {
            string search = txtTenPhim.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                _filterFilmId = null;
                LoadShow();
                return;
            }

            // Tìm kiếm không phân biệt hoa thường
            var allFilms = _filmRepo.GetAllFilms();
            var film = allFilms.FirstOrDefault(f =>
                f.title.ToLower().Contains(search.ToLower())
            );

            if (film != null)
            {
                _filterFilmId = film.movie_id;
                LoadShow();
            }
            else
            {
                MessageBox.Show($"Không tìm thấy phim có tên: '{search}'", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _filterFilmId = null;
                LoadShow();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var f = new FormAddShowTime();
            if (f.ShowDialog() == DialogResult.OK)
                LoadShow();
        }

        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            if (dgvShowtime.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn suất chiếu cần sửa!");
                return;
            }

            string id = dgvShowtime.SelectedRows[0].Cells["ShowtimeId"].Value.ToString();

            var f = new FrmEditShowTime(id);
            if (f.ShowDialog() == DialogResult.OK)
                LoadShow();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvShowtime.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn suất chiếu để xóa!");
                return;
            }

            string id = dgvShowtime.SelectedRows[0].Cells["ShowtimeId"].Value.ToString();

            if (MessageBox.Show("Xóa suất chiếu này?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ShowtimeRepo.Delete(id);
            LoadShow();
        }

        private void FilterRoom(string auditorium_id)
        {
            _filterRoomId = auditorium_id;
            LoadShow();
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            _filterRoomId = null;
            LoadShow();
        }

        private void btnPhong1_Click(object sender, EventArgs e) => FilterRoom("RO1");
        private void btnPhong2_Click(object sender, EventArgs e) => FilterRoom("RO2");
        private void btnPhong3_Click(object sender, EventArgs e) => FilterRoom("RO3");
        private void btnPhong4_Click(object sender, EventArgs e) => FilterRoom("RO4");
        private void btnPhong5_Click(object sender, EventArgs e) => FilterRoom("RO5");

        // ================== LỌC THEO NGÀY ==================

        private void dtpNgayChieu_ValueChanged(object sender, EventArgs e)
        {
            _filterDate = dtpNgayChieu.Value.Date;
            LoadShow();
        }

        public void RefreshData()
        {
            LoadShow();
        }
    }
}
