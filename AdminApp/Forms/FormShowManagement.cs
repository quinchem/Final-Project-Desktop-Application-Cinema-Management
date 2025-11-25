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
        private readonly ShowtimeRepo _showtimeRepo;
        private readonly FilmRepo _filmRepo;
        private readonly AuditoriumRepo _auditoriumRepo;
        public FormShowManagement()
        {
            InitializeComponent();
            _showtimeRepo = new ShowtimeRepo();
            _filmRepo = new FilmRepo();
            _auditoriumRepo = new AuditoriumRepo();

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var f = new FormAddShowTime();
            f.Show();
        }

        private void FormShowManagement_Load(object sender, EventArgs e)
        {
            InitializeComboBoxes();
            LoadShow();
        }

        // Load dữ liệu lên DataGridView
        private void LoadShow(string searchText = "")
        {
            try
            {
                var showtimes = _showtimeRepo.GetAll();

                // Lọc theo tên phim nếu có tìm kiếm
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    showtimes = showtimes.Where(s =>
                        s.Film.TenPhim.ToLower().Contains(searchText.ToLower())
                    ).ToList();
                }

                // Tạo dữ liệu hiển thị
                var displayData = showtimes.Select(s => new
                {
                    Id = s.Id,
                    TenPhim = s.Film.TenPhim,
                    NgayChieu = s.NgayChieu.ToString("dd/MM/yyyy"),
                    TrangThai = s.TrangThai,
                    ThoiLuong = s.Film.ThoiLuong + " phút",
                    ChinhXoa = "Chỉnh sửa / Xóa"
                }).ToList();

                dgvShowtime.DataSource = displayData;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Khởi tạo ComboBox cho phòng chiếu
        private void InitializeComboBoxes()
        {
            try
            {
                // Load danh sách phòng chiếu
                var auditoriums = _auditoriumRepo.GetAll();
                // Thêm logic để bind vào ComboBox các phòng (Phòng 1, 2, 3, 4, 5)

                // Load danh sách phim vào ComboBox (nếu có)
                var films = _filmRepo.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện nút Tìm
        private void btnTim_Click(object sender, EventArgs e)
        {
            string searchText = txtTimKiem.Text.Trim();
            LoadShow(searchText);
        }



        // Sự kiện click vào cell trong DataGridView
        private void dgvShowtimes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Nếu click vào cột "Chỉnh, Xóa"
            if (dgvShowtime.Columns[e.ColumnIndex].HeaderText == "Chỉnh, Xóa")
            {
                // Hiển thị context menu hoặc dialog để chọn chỉnh sửa/xóa
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("Chỉnh sửa", null, (s, ev) => btnSua_Click(s, ev));
                contextMenu.Items.Add("Xóa", null, (s, ev) => btnXoa_Click(s, ev));
                contextMenu.Show(Cursor.Position);
            }
        }

        // Lọc theo phòng chiếu
        private void btnPhong_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string phongText = btn.Text; // "Phòng 1", "Phòng 2", etc.

            try
            {
                var showtimes = _showtimeRepo.GetAll();

                if (phongText != "Tất cả")
                {
                    showtimes = showtimes.Where(s =>
                        s.Auditorium.TenPhong == phongText
                    ).ToList();
                }

                var displayData = showtimes.Select(s => new
                {
                    Id = s.Id,
                    TenPhim = s.Film.TenPhim,
                    NgayChieu = s.NgayChieu.ToString("dd/MM/yyyy"),
                    TrangThai = s.TrangThai,
                    ThoiLuong = s.Film.ThoiLuong + " phút",
                    ChinhXoa = "Chỉnh sửa / Xóa"
                }).ToList();

                dgvShowtime.DataSource = displayData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lọc theo ngày
        private void dtpNgayChieu_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dtpNgayChieu.Value.Date;
                var showtimes = _showtimeRepo.GetAll()
                    .Where(s => s.NgayChieu.Date == selectedDate)
                    .ToList();

                var displayData = showtimes.Select(s => new
                {
                    Id = s.Id,
                    TenPhim = s.Film.TenPhim,
                    NgayChieu = s.NgayChieu.ToString("dd/MM/yyyy HH:mm"),
                    TrangThai = s.TrangThai,
                    ThoiLuong = s.Film.ThoiLuong + " phút",
                    ChinhXoa = "Chỉnh sửa / Xóa"
                }).ToList();

                dgvShowtime.DataSource = displayData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc theo ngày: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refresh dữ liệu
        public void RefreshData()
        {
            LoadShow();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

        }
    }
}

