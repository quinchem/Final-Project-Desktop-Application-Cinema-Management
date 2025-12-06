using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp.Forms
{
    public partial class FrmEditShowTime : Form
    {
        // Repo để lấy dữ liệu phim, phòng, loại phòng và ghế
        private readonly FilmRepo _filmRepo = new FilmRepo();
        private readonly AuditoriumRepo _audRepo = new AuditoriumRepo();
        private readonly AuditoriumTypeRepo _audTypeRepo = new AuditoriumTypeRepo();
        private readonly SeatRepo _seatRepo = new SeatRepo();

        // id suất chiếu cần chỉnh
        private string _showtimeId;
        // đối tượng suất chiếu đang chỉnh sửa
        private Showtime _currentShowtime;  

        // Nhận id suất chiếu cần chỉnh
        public FrmEditShowTime(string showtimeId)
        {
            InitializeComponent();
            _showtimeId = showtimeId;
        }

        private void FrmEditShowTime_Load(object sender, EventArgs e)
        {
            LoadFilms();
            LoadRooms();
            LoadAuditoriumTypes();

            // cấu hình DateTimePicker cho giờ bắt đầu
            dtpGioBD.Format = DateTimePickerFormat.Time;
            dtpGioBD.ShowUpDown = true;

            LoadShowtimeData();
        }

        // Nạp danh sách phim lên combobox phim
        private void LoadFilms()
        {
            try
            {
                var films = _filmRepo.GetAllFilms();
                cboChonPhim.DataSource = films;
                cboChonPhim.DisplayMember = "title";
                cboChonPhim.ValueMember = "movie_id";
            }
            catch (Exception ex)
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show($"Lỗi khi tải danh sách phim: {ex.Message}", "Lỗi");
            }
        }

        // Nạp danh sách phòng chiếu lên combobox phòng
        private void LoadRooms()
        {
            try
            {
                var rooms = _audRepo.GetAll();
                cboChonPhong.DataSource = rooms;
                cboChonPhong.DisplayMember = "name";
                cboChonPhong.ValueMember = "auditorium_id";
            }
            catch (Exception ex)
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show($"Lỗi khi tải danh sách phòng: {ex.Message}", "Lỗi");
            }
        }

        // Nạp danh sách loại phòng lên combobox định dạng
        private void LoadAuditoriumTypes()
        {
            try
            {
                var types = _audTypeRepo.GetAll();
                cboDinhDang.DataSource = types;
                cboDinhDang.DisplayMember = "auditorium_type";
                cboDinhDang.ValueMember = "auditorium_type_id";
            }
            catch (Exception ex)
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show($"Lỗi khi tải định dạng: {ex.Message}", "Lỗi");
            }
        }

        // Tải dữ liệu suất chiếu từ repo theo id, gán giá trị lên các control trên form
        private void LoadShowtimeData()
        {
            try
            {
                var showtimes = ShowtimeRepo.GetAll();
                _currentShowtime = showtimes.FirstOrDefault(s => s.showtime_id == _showtimeId);

                if (_currentShowtime == null)
                {
                    new SoundPlayer(Properties.Resources.fail_sound).Play();
                    MessageBox.Show("Không tìm thấy suất chiếu!", "Lỗi");
                    this.Close();
                    return;
                }

                // gán combobox phim và phòng theo id lưu trong suất chiếu
                cboChonPhim.SelectedValue = _currentShowtime.movie_id;
                cboChonPhong.SelectedValue = _currentShowtime.auditorium_id;

                // parse và gán ngày chiếu 
                if (DateTime.TryParseExact(_currentShowtime.show_date, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime showDate))
                {
                    dtpNgayChieu.Value = showDate;
                }

                // parse và gán giờ bắt đầu theo định dạng HH:mm:ss
                if (DateTime.TryParseExact(_currentShowtime.start_time, "HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    dtpGioBD.Value = startTime;
                }

                // cập nhật nhãn giá vé dựa theo định dạng phòng hiện tại
                UpdateTicketPrice();
            }
            catch (Exception ex)
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi");
            }
        }

        // Xử lý sự kiện khi chọn định dạng phòng, gọi cập nhật giá vé
        private void cboDinhDang_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTicketPrice();
        }

        // Lấy giá vé từ repo theo loại phòng và hiển thị lên nhãn
        private void UpdateTicketPrice()
        {
            if (cboDinhDang.SelectedValue == null)
            {
                lblGiaVe.Text = "0";
                return;
            }
            try
            {
                string typeId = cboDinhDang.SelectedValue.ToString();
                double price = _seatRepo.GetTicketPriceByAuditoriumType(typeId);
                lblGiaVe.Text = price.ToString("N0"); // format có phân tách hàng nghìn
            }
            catch
            {
                lblGiaVe.Text = "0";
            }
        }

        // Xử lý sự kiện khi user nhấn nút chỉnh sửa để lưu thay đổi suất chiếu
        private void btnChinh_Click(object sender, EventArgs e)
        {
            if (cboChonPhim.SelectedIndex == -1)
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show("Vui lòng chọn phim!", "Thông báo");
                return;
            }

            if (cboChonPhong.SelectedIndex == -1)
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show("Vui lòng chọn phòng!", "Thông báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(lblGiaVe.Text) || lblGiaVe.Text == "0")
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show("Giá vé chưa hợp lệ (bằng 0 hoặc rỗng)!", "Thông báo");
                return;
            }

            try
            {
                // cập nhật thuộc tính suất chiếu từ dữ liệu trên form
                _currentShowtime.movie_id = cboChonPhim.SelectedValue.ToString();
                _currentShowtime.auditorium_id = cboChonPhong.SelectedValue.ToString();
                _currentShowtime.show_date = dtpNgayChieu.Value.ToString("dd/MM/yyyy");
                _currentShowtime.start_time = dtpGioBD.Value.ToString("HH:mm:ss");
                _currentShowtime.end_time = CalculateEndTime(dtpGioBD.Value).ToString("HH:mm:ss");

                // gọi repo cập nhật vào database
                ShowtimeRepo.Update(_currentShowtime);

                new SoundPlayer(Properties.Resources.success_sound).Play();
                MessageBox.Show("Cập nhật suất chiếu thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                new SoundPlayer(Properties.Resources.fail_sound).Play();
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút đóng form
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Tính thời điểm kết thúc suất chiếu dựa trên thời lượng phim; nếu không có dữ liệu thì mặc định 120 phút
        private DateTime CalculateEndTime(DateTime startTime)
        {
            if (cboChonPhim.SelectedValue != null)
            {
                var film = _filmRepo.GetById(cboChonPhim.SelectedValue.ToString());
                if (film != null && film.duration > 0)
                {
                    return startTime.AddMinutes(film.duration);
                }
            }
            return startTime.AddMinutes(120);
        }

        private void cboChonPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
