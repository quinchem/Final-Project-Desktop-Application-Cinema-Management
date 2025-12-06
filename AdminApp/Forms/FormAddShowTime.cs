using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class FormAddShowTime : Form
    {
        // Repo lấy dữ liệu phim
        private readonly FilmRepo _filmRepo = new FilmRepo();
        // Repo lấy dữ liệu phòng chiếu
        private readonly AuditoriumRepo _audRepo = new AuditoriumRepo();
        // Repo lấy dữ liệu loại phòng
        private readonly AuditoriumTypeRepo _audTypeRepo = new AuditoriumTypeRepo();
        // Repo lấy thông tin ghế và giá vé theo loại phòng
        private readonly SeatRepo _seatRepo = new SeatRepo();

        public FormAddShowTime()
        {
            InitializeComponent();
        }

        // Xử lý khi form được load lần đầu
        private void FormAddShowTime_Load(object sender, EventArgs e)
        {
            // Nạp danh sách phim, phòng, định dạng lên các combobox
            LoadFilms();
            LoadRooms();
            LoadAuditoriumTypes();

            // Thiết lập ngày giờ mặc định
            dtpNgayChieu.Value = DateTime.Now;
            dtpGioBD.Value = DateTime.Now;
            dtpGioBD.Format = DateTimePickerFormat.Time;
            dtpGioBD.ShowUpDown = true;

            // Cập nhật hiển thị giá vé theo định dạng đang chọn
            UpdateTicketPrice();
        }

        // Nạp danh sách phim lên combobox
        private void LoadFilms()
        {
            var films = _filmRepo.GetAllFilms();
            cboChonPhim.DataSource = films;
            cboChonPhim.DisplayMember = "title";
            cboChonPhim.ValueMember = "movie_id";
            cboChonPhim.SelectedIndex = -1;
        }

        // Nạp danh sách phòng chiếu lên combobox
        private void LoadRooms()
        {
            var rooms = _audRepo.GetAll();
            cboChonPhong.DataSource = rooms;
            cboChonPhong.DisplayMember = "name";
            cboChonPhong.ValueMember = "auditorium_id";
            cboChonPhong.SelectedIndex = -1;
        }

        // Nạp danh sách định dạng phòng lên combobox
        private void LoadAuditoriumTypes()
        {
            var types = _audTypeRepo.GetAll();
            cboDinhDang.DataSource = types;
            cboDinhDang.DisplayMember = "auditorium_type";
            cboDinhDang.ValueMember = "auditorium_type_id";
            cboDinhDang.SelectedIndex = -1;
        }

        // Khi thay đổi định dạng phòng thì cập nhật giá vé tương ứng
        private void cboDinhDang_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTicketPrice();
        }

        // Hàm cập nhật nhãn giá vé dựa trên định dạng phòng đang chọn
        private void UpdateTicketPrice()
        {
            // Nếu chưa có dữ liệu hoặc chưa chọn thì đặt giá về 0
            if (cboDinhDang.DataSource == null || cboDinhDang.SelectedIndex == -1 || cboDinhDang.SelectedValue == null)
            {
                lblGiaVe.Text = "0";
                return;
            }
            try
            {
                string auditoriumTypeId = cboDinhDang.SelectedValue.ToString().Trim();
                if (string.IsNullOrEmpty(auditoriumTypeId))
                {
                    lblGiaVe.Text = "0";
                    return;
                }

                // Lấy giá vé theo loại phòng từ repo
                double price = _seatRepo.GetTicketPriceByAuditoriumType(auditoriumTypeId);
                lblGiaVe.Text = price > 0 ? price.ToString("N0") : "0";
            }
            catch (Exception ex)
            {
                // Nếu có lỗi khi lấy giá thì hiển thị 0
                lblGiaVe.Text = "0";
            }
        }

        // Xử lý khi bấm nút Thêm suất chiếu
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường bắt buộc
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
            if (cboDinhDang.SelectedIndex == -1)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Vui lòng chọn định dạng phòng!", "Thông báo");
                return;
            }
            if (string.IsNullOrWhiteSpace(lblGiaVe.Text) || lblGiaVe.Text == "0")
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Giá vé chưa hợp lệ!", "Thông báo");
                return;
            }

            // Chuyển nhãn giá về số double
            double currentPrice = 0;
            double.TryParse(lblGiaVe.Text.Replace(",", ""), out currentPrice);

            try
            {
                // Tạo object Showtime từ dữ liệu nhập
                var showtime = new Showtime
                {
                    showtime_id = GenerateShowtimeId(),
                    movie_id = cboChonPhim.SelectedValue.ToString(),
                    auditorium_id = cboChonPhong.SelectedValue.ToString(),
                    show_date = dtpNgayChieu.Value.ToString("dd/MM/yyyy"),
                    start_time = dtpGioBD.Value.ToString("HH:mm:ss"),
                    end_time = CalculateEndTime(dtpGioBD.Value).ToString("HH:mm:ss")
                };

                // Ghi vào database
                ShowtimeRepo.Insert(showtime);

                // Thông báo thành công và đóng form
                SoundPlayer player = new SoundPlayer(Properties.Resources.success_sound);
                player.Play();
                MessageBox.Show("Thêm suất chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Thông báo lỗi khi thêm thất bại
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý khi bấm đóng form
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Sinh id cho suất chiếu mới dựa trên danh sách hiện có
        private string GenerateShowtimeId()
        {
            var all = ShowtimeRepo.GetAll();
            int maxNum = 0;
            foreach (var s in all)
            {
                // Kiểm tra tiền tố ro và lấy phần số để so sánh
                if (s.showtime_id.StartsWith("ro"))
                {
                    string numPart = s.showtime_id.Substring(2);
                    if (int.TryParse(numPart, out int num) && num > maxNum)
                        maxNum = num;
                }
            }
            // Trả về id mới có dạng roxxx
            return $"ro{(maxNum + 1):D3}";
        }

        // Tính thời gian kết thúc suất chiếu dựa trên thời lượng phim nếu có
        private DateTime CalculateEndTime(DateTime startTime)
        {
            if (cboChonPhim.SelectedValue != null)
            {
                var film = _filmRepo.GetById(cboChonPhim.SelectedValue.ToString());
                if (film != null && film.duration > 0)
                    return startTime.AddMinutes(film.duration);
            }
            // Nếu không có thông tin phim thì mặc định cộng 120 phút
            return startTime.AddMinutes(120);
        }
    }
}
