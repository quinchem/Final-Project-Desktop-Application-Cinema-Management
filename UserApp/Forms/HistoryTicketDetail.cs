using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using SharedData;

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
                using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            b.bill_id,
                            b.bill_date,
                            b.quantity_ticket,
                            b.per_seat_ticket_price,
                            m.title AS movie_title,
                            s.show_date,
                            s.start_time,
                            s.end_time,
                            a.name AS auditorium_name,
                            at.auditorium_type,
                            c.full_name,
                            c.email
                        FROM Bill b
                        INNER JOIN Customer c ON b.customer_id = c.customer_id
                        INNER JOIN Showtime s ON b.showtime_id = s.showtime_id
                        INNER JOIN Movie m ON s.movie_id = m.movie_id
                        INNER JOIN Auditorium a ON s.auditorium_id = a.auditorium_id
                        INNER JOIN Auditorium_type at ON a.auditorium_type_id = at.auditorium_type_id
                        WHERE b.bill_id = @billId";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", _billId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int quantity = Convert.ToInt32(reader["quantity_ticket"]);
                                decimal pricePerSeat = Convert.ToDecimal(reader["per_seat_ticket_price"]);
                                decimal total = quantity * pricePerSeat;

                                _printData = new TicketPrintData
                                {
                                    MaPhieu = "TT" + DateTime.Now.ToString("ddMMyyyy") + "-" + _billId.Substring(Math.Max(0, _billId.Length - 5)),
                                    MaDonDatVe = reader["bill_id"].ToString(),
                                    HoTen = reader["full_name"].ToString(),
                                    Email = reader["email"].ToString(),
                                    NgayDatVe = Convert.ToDateTime(reader["bill_date"]).ToString("dd/MM/yyyy"),
                                    TenPhim = reader["movie_title"].ToString(),
                                    SuatChieu = $"{reader["start_time"]} - {reader["end_time"]}, {reader["show_date"]}",
                                    Ghe = quantity.ToString(),
                                    PhongChieu = $"{reader["auditorium_type"]} - {reader["auditorium_name"]}",
                                    TongTien = total
                                };

                                // Gán dữ liệu lên UI
                                txtMaDatVe.Text = _printData.MaDonDatVe;
                                txtTenPhim.Text = _printData.TenPhim;
                                txtSuatChieu.Text = _printData.SuatChieu;
                                txtGhe.Text = _printData.Ghe;
                                txtTinhTrang.Text = "Thành công";
                                txtPhongChieu.Text = _printData.PhongChieu;
                                txtNgayDatVe.Text = _printData.NgayDatVe;
                                txtTongTien.Text = $"{_printData.TongTien:N0} VND";
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
                MessageBox.Show($"Error loading information: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            BackToHistory?.Invoke(this, EventArgs.Empty);
        }

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

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDoc,
                Width = 800,
                Height = 1000
            };

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

            // 1. Vẽ nền toàn trang màu #ece6e0
            g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#ece6e0")), e.PageBounds);

            // 2. Vẽ logo từ Resource
            try
            {
                //Image logo = Properties.Resources.Logo_trang; // Logo đã add vào Resources
                //int logoWidth = 100;
                //int logoHeight = 100;
                //g.DrawImage(logo, leftMargin, yPos, logoWidth, logoHeight);
            }
            catch
            {
                // Nếu không load được logo, bỏ qua
            }

            // 3. Header - tên công ty
            g.DrawString("CÔNG TY TNHH HAMSTER", headerFont, blackBrush, leftMargin + 120, yPos + 30);
            yPos += 120;

            // 4. Tiêu đề phiếu
            g.DrawString("PHIẾU ĐẶT VÉ", titleFont, blackBrush, e.PageBounds.Width / 2 - 150, yPos);

            // 5. Thời gian in
            string printTime = $"Thời gian in: {DateTime.Now:dd/MM/yyyy}";
            g.DrawString(printTime, new Font("Segoe UI", 9, FontStyle.Italic), grayBrush, e.PageBounds.Width - 250, yPos + 5);
            yPos += 80;

            // 6. Box thông tin
            Rectangle infoBox = new Rectangle(leftMargin, yPos, e.PageBounds.Width - 100, 350);
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), infoBox); // Box màu sáng
            g.DrawRectangle(Pens.Black, infoBox);
            yPos += 30;

            // 7. Thông tin chi tiết
            DrawInfoLine(g, "Mã phiếu:", _printData.MaPhieu, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Mã đơn đặt vé:", _printData.MaDonDatVe, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Họ và tên:", _printData.HoTen, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Email:", _printData.Email, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Ngày đặt vé:", _printData.NgayDatVe, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Tên phim:", _printData.TenPhim, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Suất chiếu:", _printData.SuatChieu, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Số lượng ghế:", _printData.Ghe, leftMargin + 30, yPos, normalFont);
            yPos += 30;
            DrawInfoLine(g, "Phòng chiếu:", _printData.PhongChieu, leftMargin + 30, yPos, normalFont);
            yPos += 50;

            g.DrawLine(Pens.Gray, leftMargin + 30, yPos, e.PageBounds.Width - 70, yPos);
            yPos += 30;

            DrawInfoLine(g, "Tổng tiền:", $"{_printData.TongTien:N0} VND", leftMargin + 30, yPos, totalFont, redBrush);
            yPos += 40;
            DrawInfoLine(g, "Tổng tiền (bằng chữ):", NumberToVietnameseWords(_printData.TongTien), leftMargin + 30, yPos, normalFont);
            yPos += 40;
            DrawInfoLine(g, "Tình trạng thanh toán:", "Thành công", leftMargin + 30, yPos, normalFont, Brushes.Green);
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

        private class TicketPrintData
        {
            public string MaPhieu { get; set; }
            public string MaDonDatVe { get; set; }
            public string HoTen { get; set; }
            public string Email { get; set; }
            public string NgayDatVe { get; set; }
            public string TenPhim { get; set; }
            public string SuatChieu { get; set; }
            public string Ghe { get; set; }
            public string PhongChieu { get; set; }
            public decimal TongTien { get; set; }
        }
    }
}


