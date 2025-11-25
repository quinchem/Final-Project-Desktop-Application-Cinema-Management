using AdminApp.Forms;
using AdminApp.Models;
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
    public partial class FormShowManagement : Form
    {
        private readonly FilmRepo _filmRepo = new FilmRepo();
        private readonly AuditoriumRepo _audRepo = new AuditoriumRepo();
        private readonly AuditoriumTypeRepo _audTypeRepo = new AuditoriumTypeRepo();

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
            // LOAD SHOWTIME
            // =====================================================
        private void LoadShow(string search = null)
        {
            List<Showtime> shows = null;

            if (_filterFilmId != null && _filterDate != null)
                shows = ShowtimeRepo.GetShowByFilmAndDate(_filterFilmId, _filterDate.Value);
            else if (_filterFilmId != null)
                shows = ShowtimeRepo.GetShowByFilm(_filterFilmId);
            else if (_filterDate != null)
                shows = ShowtimeRepo.GetShowByDate(_filterDate.Value);
            else
                shows = ShowtimeRepo.GetAll();

            if (_filterRoomId != null)
                shows = shows.Where(s => s.auditorium_id == _filterRoomId).ToList();

            // Build list of ShowtimeDisplay by joining repo results
            var displayList = shows.Select(s =>
            {
                var film = _filmRepo.GetById(s.movie_id);
                var auditorium_id = _audRepo.GetById(s.auditorium_id);
                var audType = auditorium_id != null ? _audTypeRepo.GetById(auditorium_id.auditorium_type_id) : null;

                // build gio chieu string
                string gio = null;
                try
                {
                    // nếu start_time/end_time là "HH:mm" hoặc "HH:mm:ss"
                    var st = s.start_time ?? "";
                    var et = s.end_time ?? "";
                    if (!string.IsNullOrWhiteSpace(st) && !string.IsNullOrWhiteSpace(et))
                        gio = $"{st} - {et}";
                    else if (!string.IsNullOrWhiteSpace(st))
                        gio = st;
                    else
                        gio = et;
                }
                catch
                {
                    gio = $"{s.start_time} - {s.end_time}";
                }

                return new ShowtimeDisplay
                {
                    ShowtimeId = s.showtime_id,
                    TenPhim = film?.title ?? "(Không tìm thấy)",
                    Phong = auditorium_id?.name ?? s.auditorium_id,
                    LoaiPhong = audType?.auditorium_type ?? "",
                    NgayChieu = s.show_date,
                    GioChieu = gio
                };
            }).ToList();

            // bind giữ cột Designer -> reset datasource trước
            dgvShowtime.DataSource = null;
            dgvShowtime.DataSource = displayList;
        }

       
        // Sự kiện nút Tìm
        private void btnTim_Click(object sender, EventArgs e)
        {
            string search = txtTenPhim.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                _filterFilmId = null;
            }
            else
            {
                var films = _filmRepo.SearchFilmByName(search);
                _filterFilmId = films.FirstOrDefault()?.movie_id;
            }

            LoadShow();
        }

        // Sự kiện nút Thêm - Mở form thêm mới
        private void btnThem_Click(object sender, EventArgs e)
        {
            var f = new FormAddShowTime();
            if (f.ShowDialog() == DialogResult.OK)
                LoadShow();
        }

        // Sự kiện nút Chỉnh sửa - Mở form chỉnh sửa
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

        // Sự kiện nút Xóa
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
            => FilterRoom(null);

        private void btnPhong1_Click(object sender, EventArgs e)
            => FilterRoom("RO1");

        private void btnPhong2_Click(object sender, EventArgs e)
            => FilterRoom("RO2");

        private void btnPhong3_Click(object sender, EventArgs e)
            => FilterRoom("RO3");

        private void btnPhong4_Click(object sender, EventArgs e)
            => FilterRoom("RO4");

        private void btnPhong5_Click(object sender, EventArgs e)
            => FilterRoom("RO5");

        // Lọc theo ngày chọn
        private void dtpChonNgay_ValueChanged(object sender, EventArgs e)
        {
            _filterDate = dtpNgayChieu.Value.Date;
            LoadShow();
        }

        
        // Refresh dữ liệu
        public void RefreshData()
        {
            LoadShow();
        }
    }
}
