using DocumentFormat.OpenXml.Spreadsheet;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormPayment1 : Form
    {
        // Lưu thông tin suất chiếu mà người dùng đã chọn
        private ShowtimeInfo _showtime;

        // Danh sách ghế người dùng đã chọn ở màn hình trước
        private List<SeatUser> _selectedSeats;

        // Thông tin khách hàng đang đăng nhập
        private Customer _customer;

        // Tổng số tiền phải thanh toán
        private double _total;

        public FormPayment1(ShowtimeInfo showtime, List<SeatUser> seats, Customer customer)
        {
            InitializeComponent();

            _showtime = showtime;   // Gán thông tin suất chiếu
            _selectedSeats = seats; // Gán danh sách ghế đã chọn
            _customer = customer;   // Gán thông tin khách hàng

            LoadPaymentInfo();      // Tải thông tin hiển thị lên giao diện
        }

        // Hàm hiển thị toàn bộ thông tin thanh toán cho người dùng xem lại
        private void LoadPaymentInfo()
        {
            try
            {
                // Lấy thông tin phim từ database
                var filmRepo = new FilmRepo();
                var film = filmRepo.GetById(_showtime.movie_id);

                // Nếu tìm được phim trong database thì hiển thị tên phim theo DB
                // Nếu không tìm thấy thì dùng tên trong thông tin suất chiếu
                if (film != null)
                    lblTenPhim.Text = $"{film.title}";
                else
                    lblTenPhim.Text = _showtime.title;

                // Hiển thị loại phòng chiếu và tên phòng
                lblLoaiRap.Text = $"{_showtime.auditorium_type} - {_showtime.name}";
                
                 // Hiển thị ngày chiếu, giờ chiếu và giờ kết thúc
                lblNgay.Text = _showtime.show_date;
                lblGio.Text = $"{_showtime.start_time} - {_showtime.end_time}";
                
                // Liệt kê danh sách ghế theo đúng thứ tự hàng và cột
                lblGhe.Text = string.Join(", ",
                    _selectedSeats.OrderBy(s => s.Row)
                                  .ThenBy(s => s.Col)
                                  .Select(s => $"{s.Row}{s.Col:00}"));

                _total = _selectedSeats.Sum(s => s.Price);
                
                // Tính tổng tiền vé dựa vào danh sách ghế
                lblTong.Text = _total.ToString("N0") + " VND";

                // Hiển thị tên khách hàng
                lblKhachHang.Text = _customer.full_name;
            }
            catch (Exception ex)
            {
                // Hiển thị âm thanh báo lỗi và popup khi dữ liệu không load được
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi hiển thị giao diện: " + ex.Message);
            }
        }
        
        // Hàm xử lý sự kiện khi người dùng nhấn nút “Thanh toán”
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                 // Lấy form cha để mở form mới bên trong panelMain
                var parent = this.ParentForm as UserMainForm;

                // Chuyển sang màn hình FormPayment2, truyền đầy đủ dữ liệu
                parent.OpenChildForm(new FormPayment2(
                    _showtime,
                    _selectedSeats,
                    _customer,
                    _total
                ));
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không thể chuyển sang giao diện thanh toán: " + ex.Message);
            }
        }
        
        // Hàm xử lý sự kiện khi người dùng chọn “Quay lại”
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            try
            {
                var parent = this.ParentForm as UserMainForm;
                parent.OpenChildForm(new FormSeatSelection(parent, _showtime));
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không thể quay lại: " + ex.Message);
            }
        }
    }
}
