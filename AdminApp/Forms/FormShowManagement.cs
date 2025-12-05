using AdminApp.Forms;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormShowManagement : Form
    {
        // Repo để lấy dữ liệu phim, phòng, loại phòng và thông tin ghế/giá vé
        private readonly FilmRepo _filmRepo = new FilmRepo();
        private readonly AuditoriumRepo _audRepo = new AuditoriumRepo();
        private readonly AuditoriumTypeRepo _audTypeRepo = new AuditoriumTypeRepo();
        private readonly SeatRepo _seatRepo = new SeatRepo(); 

        // Các biến lưu bộ lọc hiện tại
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

        // Nạp danh sách suất chiếu dựa trên các bộ lọc hiện tại và gán DataSource cho dgv
        private void LoadShow()
        {
            List<Showtime> shows;

            // Lấy danh sách theo tổ hợp bộ lọc film và ngày nếu có
            if (_filterFilmId != null && _filterDate != null)
                shows = ShowtimeRepo.GetByFilmAndDate(_filterFilmId, _filterDate.Value);
            else if (_filterFilmId != null)
                shows = ShowtimeRepo.GetByFilm(_filterFilmId);
            else if (_filterDate != null)
                shows = ShowtimeRepo.GetByDate(_filterDate.Value);
            else
                shows = ShowtimeRepo.GetAll();

            // Nếu lọc theo phòng thì filter thêm
            if (_filterRoomId != null)
                shows = shows.Where(s => s.auditorium_id == _filterRoomId).ToList();

            // Sắp xếp danh sách theo ngày rồi theo giờ bắt đầu
            shows = shows.OrderBy(s =>
            {
                DateTime dt;
                string[] formats = { "dd/MM/yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };

                if (DateTime.TryParseExact(s.show_date, formats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dt))
                {
                    return dt;
                }
                // Nếu không parse được thì đưa về cuối cùng
                return DateTime.MaxValue; 
            })
            .ThenBy(s => s.start_time)
            .ToList();
            
            // Chuyển đổi sang model hiển thị để dgv dễ bind và có format giá
            var display = new List<ShowtimeDisplay>();

            foreach (var s in shows)
            {
                var film = _filmRepo.GetById(s.movie_id);
                var room = _audRepo.GetById(s.auditorium_id);
                var type = room != null ? _audTypeRepo.GetById(room.auditorium_type_id) : null;
                double price = 0;

                // Lấy giá vé theo loại phòng; nếu room là null thì cần xử lý tránh lỗi
                if (room != null)
                {
                    price = _seatRepo.GetTicketPriceByAuditoriumType(room.auditorium_type_id);
                }

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

            // Bind dữ liệu lên DataGridView
            dgvShowtime.DataSource = null;
            dgvShowtime.DataSource = display;
        }

        // Xử lý khi bấm nút tìm phim theo tên
        private void btnTim_Click(object sender, EventArgs e)
        {
            string search = txtTenPhim.Text.Trim();

            // Nếu ô tìm rỗng thì xóa bộ lọc film và load lại
            if (string.IsNullOrEmpty(search))
            {
                _filterFilmId = null;
                LoadShow();
                return;
            }

            // Tìm phim phù hợp theo tên (so sánh không phân biệt hoa thường)
            var allFilms = _filmRepo.GetAllFilms();
            var film = allFilms.FirstOrDefault(f =>
                f.title.ToLower().Contains(search.ToLower())
            );

            if (film != null)
            {
                // Nếu tìm thấy phim thì gán filter và load lại
                _filterFilmId = film.movie_id;
                LoadShow();
            }
            else
            {
                // Nếu không tìm thấy thì báo và giữ trạng thái không lọc theo film
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Không tìm thấy phim có tên: '{search}'", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _filterFilmId = null;
                LoadShow();
            }
        }

        // Mở form thêm suất chiếu, nếu thêm thành công thì refresh dữ liệu
        private void btnThem_Click(object sender, EventArgs e)
        {
            var f = new FormAddShowTime();
            if (f.ShowDialog() == DialogResult.OK)
                LoadShow();
        }

        // Chỉnh sửa suất chiếu: lấy id từ hàng đang chọn, mở form chỉnh sửa
        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            if (dgvShowtime.SelectedRows.Count == 0)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn suất chiếu cần chỉnh sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string id = dgvShowtime.SelectedRows[0].Cells["showtime_id"].Value.ToString();

                var f = new FrmEditShowTime(id);
                if (f.ShowDialog() == DialogResult.OK)
                    LoadShow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        // Xóa suất chiếu: xác nhận rồi gọi repo xóa, sau đó refresh
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvShowtime.SelectedRows.Count == 0)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn suất chiếu cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string id = dgvShowtime.SelectedRows[0].Cells["showtime_id"].Value.ToString();

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa suất chiếu này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                ShowtimeRepo.Delete(id);
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Xóa suất chiếu thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadShow();
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Gán filter theo id phòng rồi load dữ liệu
        private void FilterRoom(string auditorium_id)
        {
            _filterRoomId = auditorium_id;
            LoadShow();
        }

        // Xóa filter phòng và load lại tất cả
        private void btnTatCa_Click(object sender, EventArgs e)
        {
            _filterRoomId = null;
            LoadShow();
        }

        // Các nút nhanh để lọc theo tên phòng, tìm phòng theo tên
        private void btnPhong1_Click(object sender, EventArgs e) => FilterRoomByName("Phòng 1");
        private void btnPhong2_Click(object sender, EventArgs e) => FilterRoomByName("Phòng 2");
        private void btnPhong3_Click(object sender, EventArgs e) => FilterRoomByName("Phòng 3");
        private void btnPhong4_Click(object sender, EventArgs e) => FilterRoomByName("Phòng 4");
        private void btnPhong5_Click(object sender, EventArgs e) => FilterRoomByName("Phòng 5");

        // Tìm phòng theo tên, nếu có thì gọi FilterRoom, nếu không tìm thấy thì thông báo
        private void FilterRoomByName(string roomName)
        {
            var rooms = _audRepo.GetAll();
            var room = rooms.FirstOrDefault(r => r.name == roomName);

            if (room != null)
            {
                FilterRoom(room.auditorium_id);
            }
            else
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Không tìm thấy {roomName} trong database!", "Lỗi");
            }
        }

        // Xử lý khi thay đổi ngày lọc
        private void dtpNgayChieu_ValueChanged(object sender, EventArgs e)
        {
            _filterDate = dtpNgayChieu.Value.Date;
            LoadShow();
        }

        // Hàm public để refresh dữ liệu từ nơi khác gọi
        public void RefreshData()
        {
            LoadShow();
        }
    }
}
