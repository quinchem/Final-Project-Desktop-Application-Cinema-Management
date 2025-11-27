using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp
{
    public partial class HistoryTicketDetail : UserControl
    {
        private string _billId;
        private TicketPrintData _printData;
        public event EventHandler BackToHistory;


        public HistoryTicketDetail(string billId)
        {
            InitializeComponent();
            _billId = billId;
            LoadDetail();
        }

        private void LoadDetail()
        {
            try
            {
                // Kết nối DB và load thông tin bill chi tiết
                using (var conn = new SqliteConnection(DatabaseHelper2.GetConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            b.bill_id,
                            b.bill_date,
                            b.quantity_ticket,
                            b.Total,
                            m.title AS movie_title,
                            s.show_date,
                            s.start_time,
                            s.end_time,
                            a.name AS auditorium_name,
                            at.auditorium_type,
                            GROUP_CONCAT(DISTINCT se.location) AS seats
                        FROM Bill b
                        INNER JOIN Showtime s ON b.showtime_id = s.showtime_id
                        INNER JOIN Movie m ON s.movie_id = m.movie_id
                        INNER JOIN Auditorium a ON s.auditorium_id = a.auditorium_id
                        INNER JOIN Auditorium_type at ON a.auditorium_type_id = at.auditorium_type_id
                        LEFT JOIN Bill_seat bs ON b.bill_id = bs.bill_id
                        LEFT JOIN Seat se ON bs.seat_id = se.seat_id
                        WHERE b.bill_id = @billId
                        GROUP BY b.bill_id";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", _billId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _printData = new TicketPrintData
                                {
                                    MaPhieu = "TT" + DateTime.Now.ToString("ddMMyyyy") + "-" + _billId.Substring(Math.Max(0, _billId.Length - 5)),
                                    MaDonDatVe = reader["bill_id"].ToString(),
                                    //HoTen = reader["full_name"].ToString(),
                                    //Email = reader["email"].ToString(),
                                    NgayDatVe = Convert.ToDateTime(reader["bill_date"]).ToString("dd/MM/yyyy"),
                                    TenPhim = reader["movie_title"].ToString(),
                                    SuatChieu = $"{reader["start_time"]} - {reader["end_time"]}, {reader["show_date"]}",
                                    Ghe = reader["seats"].ToString(),
                                    PhongChieu = $"{reader["auditorium_type"]} - {reader["auditorium_name"]}",
                                    DichVu = GetProducts(_billId),
                                    TongTien = Convert.ToDecimal(reader["Total"])
                                };

                                // Gán dữ liệu vào các label trong form của bạn
                                txtMaDatVe.Text = reader["bill_id"].ToString();
                                txtTenPhim.Text = reader["movie_title"].ToString();

                                string showtime = $"{reader["start_time"]} - {reader["end_time"]}, {reader["show_date"]}";
                                txtSuatChieu.Text = showtime;

                                txtGhe.Text = reader["seats"].ToString();
                                txtTinhTrang.Text = "Thành công";

                                string auditorium = $"{reader["auditorium_type"]} - {reader["auditorium_name"]}";
                                txtPhongChieu.Text = auditorium;

                                txtNgayDatVe.Text = Convert.ToDateTime(reader["bill_date"]).ToString("dd/MM/yyyy");

                                // Lấy sản phẩm/dịch vụ
                                txtDichVu.Text = GetProducts(_billId);

                                decimal total = Convert.ToDecimal(reader["Total"]);
                                txtTongTien.Text = $"{total:N0} VND";
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy thông tin đặt vé!", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetProducts(string billId)
        {
            string products = "";
            try
            {
                using (var conn = new SqliteConnection(DatabaseHelper2.GetConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            p.name,
                            bd.quantity
                        FROM Bill_detail bd
                        INNER JOIN Product p ON bd.product_id = p.product_id
                        WHERE bd.bill_id = @billId";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", billId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products += $"{reader["quantity"]} {reader["name"]}\n";
                            }
                        }
                    }
                }

                return string.IsNullOrEmpty(products) ? "Không có" : products.TrimEnd('\n');
            }
            catch
            {
                return "Không có";
            }
        }

        // Sự kiện nút In phiếu (nếu có)
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_printData == null)
            {
                MessageBox.Show("Không có dữ liệu để in!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;

            // Hiển thị hộp thoại xem trước và in
            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDoc;
            previewDialog.Width = 800;
            previewDialog.Height = 1000;

            if (previewDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font normalFont = new Font("Segoe UI", 10);
            Font totalFont = new Font("Segoe UI", 12, FontStyle.Bold);

            Brush blackBrush = Brushes.Black;
            Brush grayBrush = Brushes.Gray;
            Brush redBrush = Brushes.Red;

            int leftMargin = 50;
            int topMargin = 50;
            int yPos = topMargin;

            // Header - Logo và Công ty
            g.DrawString("CÔNG TY TNHH HAMSTER", headerFont, blackBrush, leftMargin, yPos);
            yPos += 60;

            // Tiêu đề
            g.DrawString("PHIẾU ĐẶT VÉ", titleFont, blackBrush,
                e.PageBounds.Width / 2 - 150, yPos);

            // Thời gian in
            string printTime = $"Thời gian in: {DateTime.Now:dd/MM/yyyy}";
            g.DrawString(printTime, new Font("Segoe UI", 9, FontStyle.Italic),
                grayBrush, e.PageBounds.Width - 250, yPos + 5);
            yPos += 80;

            // Vẽ box thông tin
            Rectangle infoBox = new Rectangle(leftMargin, yPos,
                e.PageBounds.Width - 100, 500);
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), infoBox);
            g.DrawRectangle(Pens.Black, infoBox);

            yPos += 30;

            // Thông tin chi tiết
            DrawInfoLine(g, "Mã phiếu:", _printData.MaPhieu, leftMargin + 30, yPos, normalFont);
            yPos += 35;

            DrawInfoLine(g, "Mã đơn đặt vé:", _printData.MaDonDatVe, leftMargin + 30, yPos, normalFont);
            yPos += 35;

            //DrawInfoLine(g, "Họ và tên:", _printData.HoTen, leftMargin + 30, yPos, normalFont);
            //yPos += 35;

            //DrawInfoLine(g, "Email:", _printData.Email, leftMargin + 30, yPos, normalFont);
            //yPos += 35;

            DrawInfoLine(g, "Ngày đặt vé:", _printData.NgayDatVe, leftMargin + 30, yPos, normalFont);
            yPos += 35;

            DrawInfoLine(g, "Tên phim:", _printData.TenPhim, leftMargin + 30, yPos, normalFont);
            yPos += 35;

            DrawInfoLine(g, "Suất chiếu:", _printData.SuatChieu, leftMargin + 30, yPos, normalFont);
            yPos += 35;

            DrawInfoLine(g, "Ghế:", _printData.Ghe, leftMargin + 30, yPos, normalFont);
            yPos += 35;

            DrawInfoLine(g, "Phòng chiếu:", _printData.PhongChieu, leftMargin + 30, yPos, normalFont);
            yPos += 35;

            DrawInfoLine(g, "Dịch vụ:", _printData.DichVu, leftMargin + 30, yPos, normalFont);
            yPos += 50;

            // Đường kẻ
            g.DrawLine(Pens.Gray, leftMargin + 30, yPos,
                e.PageBounds.Width - 70, yPos);
            yPos += 30;

            // Tổng tiền
            DrawInfoLine(g, "Tổng tiền:", $"{_printData.TongTien:N0} VND",
                leftMargin + 30, yPos, totalFont, redBrush);
            yPos += 40;

            DrawInfoLine(g, "Tổng tiền (bằng chữ):",
                NumberToVietnameseWords(_printData.TongTien),
                leftMargin + 30, yPos, normalFont);
            yPos += 40;

            DrawInfoLine(g, "Tình trạng thanh toán:", "Thành công",
                leftMargin + 30, yPos, normalFont, Brushes.Green);
        }

        private void DrawInfoLine(Graphics g, string label, string value,
            int x, int y, Font font, Brush valueBrush = null)
        {
            g.DrawString(label, font, Brushes.Black, x, y);
            g.DrawString(value, font, valueBrush ?? Brushes.Black, x + 200, y);
        }

        private string NumberToVietnameseWords(decimal number)
        {
            if (number == 0) return "Không đồng";

            string[] ones = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] thousands = { "", "nghìn", "triệu", "tỷ" };

            long num = (long)number;
            string result = "";
            int groupIndex = 0;

            while (num > 0)
            {
                int group = (int)(num % 1000);
                if (group > 0)
                {
                    string groupText = ConvertGroupToWords(group, ones);
                    result = groupText + " " + thousands[groupIndex] + " " + result;
                }
                num /= 1000;
                groupIndex++;
            }

            return char.ToUpper(result.Trim()[0]) + result.Trim().Substring(1) + " đồng";
        }

        private string ConvertGroupToWords(int num, string[] ones)
        {
            string result = "";
            int hundreds = num / 100;
            int tens = (num % 100) / 10;
            int units = num % 10;

            if (hundreds > 0)
                result += ones[hundreds] + " trăm ";

            if (tens > 1)
                result += ones[tens] + " mươi ";
            else if (tens == 1)
                result += "mười ";

            if (units > 0)
                result += ones[units];

            return result.Trim();
        }

        // Sự kiện quay lại (nếu có - nếu là UserControl thì ẩn control thay vì Close)
        private void btnReturn_Click(object sender, EventArgs e)
        {
            BackToHistory?.Invoke(this, EventArgs.Empty);
        }
        private class TicketPrintData
        {
            public string MaPhieu { get; set; }
            public string MaDonDatVe { get; set; }
            //public string HoTen { get; set; }
           // public string Email { get; set; }
            public string NgayDatVe { get; set; }
            public string TenPhim { get; set; }
            public string SuatChieu { get; set; }
            public string Ghe { get; set; }
            public string PhongChieu { get; set; }
            public string DichVu { get; set; }
            public decimal TongTien { get; set; }
        }
    }
}