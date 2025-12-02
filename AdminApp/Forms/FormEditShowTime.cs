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
        private readonly FilmRepo _filmRepo = new FilmRepo();
        private readonly AuditoriumRepo _audRepo = new AuditoriumRepo();
        private readonly AuditoriumTypeRepo _audTypeRepo = new AuditoriumTypeRepo();
        private readonly SeatRepo _seatRepo = new SeatRepo();

        private string _showtimeId;
        private Showtime _currentShowtime;

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
            dtpGioBD.Format = DateTimePickerFormat.Time;
            dtpGioBD.ShowUpDown = true;
            LoadShowtimeData();
        }

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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi tải danh sách phim: {ex.Message}", "Lỗi");
            }
        }

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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi tải danh sách phòng: {ex.Message}", "Lỗi");
            }
        }

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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi tải định dạng: {ex.Message}", "Lỗi");
            }
        }

        private void LoadShowtimeData()
        {
            try
            {
                var showtimes = ShowtimeRepo.GetAll();
                _currentShowtime = showtimes.FirstOrDefault(s => s.showtime_id == _showtimeId);

                if (_currentShowtime == null)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Không tìm thấy suất chiếu!", "Lỗi");
                    this.Close();
                    return;
                }
                cboChonPhim.SelectedValue = _currentShowtime.movie_id;
                cboChonPhong.SelectedValue = _currentShowtime.auditorium_id;

                if (DateTime.TryParseExact(_currentShowtime.show_date, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime showDate))
                {
                    dtpNgayChieu.Value = showDate;
                }

                if (DateTime.TryParseExact(_currentShowtime.start_time, "HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    dtpGioBD.Value = startTime;
                }
                UpdateTicketPrice();
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi");
            }
        }

        private void cboDinhDang_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTicketPrice();
        }

        // Hàm cập nhật giá vé riêng để tái sử dụng
        // Hàm cập nhật giá vé
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
                lblGiaVe.Text = price.ToString("N0");
            }
            catch
            {
                lblGiaVe.Text = "0";
            }
        }

        private void btnChinh_Click(object sender, EventArgs e)
        {
            if (cboChonPhim.SelectedIndex == -1)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn phim!", "Thông báo");
                return;
            }

            if (cboChonPhong.SelectedIndex == -1)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn phòng!", "Thông báo");
                return;
            }
            if (string.IsNullOrWhiteSpace(lblGiaVe.Text) || lblGiaVe.Text == "0")
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Giá vé chưa hợp lệ (bằng 0 hoặc rỗng)!", "Thông báo");
                return;
            }

            try
            {
                _currentShowtime.movie_id = cboChonPhim.SelectedValue.ToString();
                _currentShowtime.auditorium_id = cboChonPhong.SelectedValue.ToString();
                _currentShowtime.show_date = dtpNgayChieu.Value.ToString("dd/MM/yyyy");
                _currentShowtime.start_time = dtpGioBD.Value.ToString("HH:mm:ss");
                _currentShowtime.end_time = CalculateEndTime(dtpGioBD.Value).ToString("HH:mm:ss");

                ShowtimeRepo.Update(_currentShowtime);
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Cập nhật suất chiếu thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

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