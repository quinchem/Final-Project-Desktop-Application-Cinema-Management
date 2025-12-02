using DocumentFormat.OpenXml.Spreadsheet;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp
{
    public partial class FormPayment1 : Form
    {
        private ShowtimeInfo _showtime;
        private List<SeatUser> _selectedSeats;
        private Customer _customer;
        private double _total;

        public FormPayment1(ShowtimeInfo showtime, List<SeatUser> seats, Customer customer)
        {
            InitializeComponent();

            _showtime = showtime;
            _selectedSeats = seats;
            _customer = customer;

            LoadPaymentInfo();
        }
        private void LoadPaymentInfo()
        {
            try
            {
                // Lấy thông tin phim
                var filmRepo = new FilmRepo();
                var film = filmRepo.GetById(_showtime.movie_id);

                //Hiển thị thông tin đơn đặt vé
                if (film != null)
                    lblTenPhim.Text = $"{film.title}";
                else
                    lblTenPhim.Text = _showtime.title;

                lblLoaiRap.Text = $"{_showtime.auditorium_type} - {_showtime.name}";


                lblNgay.Text = _showtime.show_date;
                lblGio.Text = $"{_showtime.start_time} - {_showtime.end_time}";
                lblGhe.Text = string.Join(", ",
                    _selectedSeats.OrderBy(s => s.Row)
                                  .ThenBy(s => s.Col)
                                  .Select(s => $"{s.Row}{s.Col:00}"));

                _total = _selectedSeats.Sum(s => s.Price);
                lblTong.Text = _total.ToString("N0") + " VND";
                lblKhachHang.Text = _customer.full_name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị giao diện: " + ex.Message);
            }
        }

        private void FormPayment1_Load(object sender, EventArgs e)
        {


        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                var parent = this.ParentForm as UserMainForm;

                parent.OpenChildForm(new FormPayment2(
                    _showtime,
                    _selectedSeats,
                    _customer,
                    _total
                ));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chuyển sang giao diện thanh toán: " + ex.Message);
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            try
            {
                var parent = this.ParentForm as UserMainForm;
                parent.OpenChildForm(new FormSeatSelection(parent, _showtime));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể quay lại: " + ex.Message);
            }
        }
    }
}
