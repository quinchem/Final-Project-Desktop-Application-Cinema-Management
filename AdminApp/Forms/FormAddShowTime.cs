using AdminApp.Models;
using AdminApp.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormAddShowTime : Form
    {
        private readonly FilmRepo _filmRepo = new FilmRepo();
        private readonly AuditoriumRepo _audRepo = new AuditoriumRepo();
        private readonly AuditoriumTypeRepo _audTypeRepo = new AuditoriumTypeRepo();
        private readonly SeatRepo _seatRepo = new SeatRepo();

        public FormAddShowTime()
        {
            InitializeComponent();
        }

        private void FormAddShowTime_Load(object sender, EventArgs e)
        {
            LoadFilms();
            LoadRooms();
            LoadAuditoriumTypes();

            dtpNgayChieu.Value = DateTime.Now;
            dtpGioBD.Value = DateTime.Now;
            dtpGioBD.Format = DateTimePickerFormat.Time;
            dtpGioBD.ShowUpDown = true;

            // Cập nhật giá vé mặc định lúc load (nếu có chọn sẵn)
            UpdateTicketPrice();
        }

        private void LoadFilms()
        {
            var films = _filmRepo.GetAllFilms();
            cboChonPhim.DataSource = films;
            cboChonPhim.DisplayMember = "title";
            cboChonPhim.ValueMember = "movie_id";
            cboChonPhim.SelectedIndex = -1;
        }

        private void LoadRooms()
        {
            var rooms = _audRepo.GetAll();
            cboChonPhong.DataSource = rooms;
            cboChonPhong.DisplayMember = "name";
            cboChonPhong.ValueMember = "auditorium_id";
            cboChonPhong.SelectedIndex = -1;
        }

        private void LoadAuditoriumTypes()
        {
            var types = _audTypeRepo.GetAll();
            cboDinhDang.DataSource = types;
            cboDinhDang.DisplayMember = "auditorium_type";
            cboDinhDang.ValueMember = "auditorium_type_id";
            cboDinhDang.SelectedIndex = -1;
        }

        // Sự kiện khi chọn thay đổi định dạng
        private void cboDinhDang_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTicketPrice();
        }

        // --- HÀM CẬP NHẬT GIÁ VÉ ---
        // Hàm cập nhật giá vé
        private void UpdateTicketPrice()
        {
            // 1. Kiểm tra nếu chưa chọn định dạng thì giá = 0
            if (cboDinhDang.SelectedValue == null)
            {
                lblGiaVe.Text = "0";
                return;
            }

            try
            {
                // 2. Lấy ID định dạng (VD: 'type01', '2D'...)
                string typeId = cboDinhDang.SelectedValue.ToString();

                // --- SỬA Ở ĐÂY ---
                // Lúc nãy gọi nhầm hàm GetTicketPriceByAuditorium -> Sửa thành GetTicketPriceByAuditoriumType
                double price = _seatRepo.GetTicketPriceByAuditoriumType(typeId);

                // 3. Hiển thị lên Label
                lblGiaVe.Text = price.ToString("N0");
            }
            catch (Exception ex)
            {
                // Debug lỗi nếu cần
                // MessageBox.Show(ex.Message);
                lblGiaVe.Text = "0";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboChonPhim.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phim!", "Thông báo");
                return;
            }

            if (cboChonPhong.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng!", "Thông báo");
                return;
            }

            if (cboDinhDang.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn định dạng phòng!", "Thông báo");
                return;
            }

            // SỬA: Kiểm tra thuộc tính .Text của Label
            if (string.IsNullOrWhiteSpace(lblGiaVe.Text) || lblGiaVe.Text == "0")
            {
                MessageBox.Show("Giá vé chưa hợp lệ!", "Thông báo");
                return;
            }

            try
            {
                var showtime = new Showtime
                {
                    showtime_id = GenerateShowtimeId(),
                    movie_id = cboChonPhim.SelectedValue.ToString(),
                    auditorium_id = cboChonPhong.SelectedValue.ToString(),
                    show_date = dtpNgayChieu.Value.ToString("dd/MM/yyyy"),
                    start_time = dtpGioBD.Value.ToString("HH:mm:ss"),
                    end_time = CalculateEndTime(dtpGioBD.Value).ToString("HH:mm:ss")
                    // Giá vé (price) thường sẽ được lấy từ bảng seat/type chứ không lưu cứng vào showtime (tùy DB của bạn)
                };

                ShowtimeRepo.Insert(showtime);

                MessageBox.Show("Thêm suất chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private string GenerateShowtimeId()
        {
            // (Code cũ của bạn giữ nguyên)
            var all = ShowtimeRepo.GetAll();
            int maxNum = 0;
            foreach (var s in all)
            {
                if (s.showtime_id.StartsWith("ro"))
                {
                    string numPart = s.showtime_id.Substring(2);
                    if (int.TryParse(numPart, out int num) && num > maxNum)
                        maxNum = num;
                }
            }
            return $"ro{(maxNum + 1):D3}";
        }

        private DateTime CalculateEndTime(DateTime startTime)
        {
            // (Code cũ của bạn giữ nguyên)
            if (cboChonPhim.SelectedValue != null)
            {
                var film = _filmRepo.GetById(cboChonPhim.SelectedValue.ToString());
                if (film != null && film.duration > 0)
                    return startTime.AddMinutes(film.duration);
            }
            return startTime.AddMinutes(120);
        }
    }
}