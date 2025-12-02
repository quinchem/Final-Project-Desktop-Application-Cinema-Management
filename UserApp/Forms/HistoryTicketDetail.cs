using Microsoft.Data.Sqlite;
using SharedData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using SharedData.Models;

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

        private string GenerateTicketCode(string billId)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(billId));
                string hash = BitConverter.ToString(bytes).Replace("-", "");
                return $"TK-{billId}-{hash.Substring(0, 4)}";
            }
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
                            m.title AS movie_title,
                            s.show_date,
                            s.start_time,
                            s.end_time,
                            a.name AS auditorium_name,
                            c.full_name,
                            c.email
                        FROM bill b
                        INNER JOIN customer c ON b.customer_id = c.customer_id
                        INNER JOIN showtime s ON b.showtime_id = s.showtime_id
                        INNER JOIN movie m ON s.movie_id = m.movie_id
                        INNER JOIN auditorium a ON s.auditorium_id = a.auditorium_id
                        WHERE b.bill_id = @billId";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", _billId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = reader["full_name"].ToString();
                                string email = reader["email"].ToString();
                                string movieTitle = reader["movie_title"].ToString();
                                string showDate = reader["show_date"].ToString();
                                string startTime = reader["start_time"].ToString();
                                string endTime = reader["end_time"].ToString();
                                string auditoriumName = reader["auditorium_name"].ToString();

                                string seatQuery = @"
                                    SELECT 
                                        se.seat_id,
                                        se.location,
                                        se.per_seat_ticket_price
                                    FROM bill_seat bs
                                    INNER JOIN seat se ON bs.seat_id = se.seat_id
                                    WHERE bs.bill_id = @billId
                                    ORDER BY se.seat_id";

                                List<string> seatLocations = new List<string>();
                                decimal totalPrice = 0;

                                using (var seatCmd = new SqliteCommand(seatQuery, conn))
                                {
                                    seatCmd.Parameters.AddWithValue("@billId", _billId);

                                    using (var seatReader = seatCmd.ExecuteReader())
                                    {
                                        while (seatReader.Read())
                                        {
                                            string location = seatReader["location"].ToString();
                                            decimal price = Convert.ToDecimal(seatReader["per_seat_ticket_price"]);
                                            seatLocations.Add(location);
                                            totalPrice += price;
                                        }
                                    }
                                }

                                int seatCount = seatLocations.Count;
                                string seatList = string.Join(", ", seatLocations);

                                _printData = new TicketPrintData
                                {
                                    MaPhieu = "TT" + DateTime.Now.ToString("ddMMyyyy") + "-" + _billId.Substring(Math.Max(0, _billId.Length - 4)),
                                    MaDonDatVe = _billId,
                                    TicketCode = GenerateTicketCode(_billId),
                                    HoTen = fullName,
                                    Email = email,
                                    NgayDatVe = DateTime.Now.ToString("dd/MM/yyyy"),
                                    TenPhim = movieTitle,
                                    SuatChieu = $"{startTime} - {endTime}, {showDate}",
                                    Ghe = seatList,
                                    SoLuongGhe = seatCount,
                                    PhongChieu = auditoriumName,
                                    TongTien = totalPrice
                                };

                                // Gán các thông tin vào các hộp textbox
                                txtMaDatVe.Text = _printData.MaDonDatVe;
                                txtTenPhim.Text = _printData.TenPhim;
                                txtSuatChieu.Text = _printData.SuatChieu;
                                txtGhe.Text = $"{_printData.SoLuongGhe} ghế ({_printData.Ghe})";
                                txtTinhTrang.Text = "Thành công";
                                txtPhongChieu.Text = _printData.PhongChieu;
                                txtNgayDatVe.Text = _printData.NgayDatVe;
                                txtTongTien.Text = $"{_printData.TongTien:N0} VND";
                                txtTicketCode.Text = _printData.TicketCode;

                            }
                            else
                            {
                                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                                player.Play();
                                MessageBox.Show("Không tìm thấy thông tin đặt vé!", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show($"Lỗi khi load thông tin:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Lỗi",
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
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Định dạng font chữ cho phiếu in
            Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font normalFont = new Font("Segoe UI", 10);
            Font smallFont = new Font("Segoe UI", 9, FontStyle.Italic);
            Font totalFont = new Font("Segoe UI", 12, FontStyle.Bold);


            Brush blackBrush = Brushes.Black;
            Brush grayBrush = Brushes.Gray;
            Brush redBrush = Brushes.Red;
            Brush greenBrush = Brushes.Green;

            int leftMargin = 50;
            int topMargin = 50;
            int yPos = topMargin;

            // Định dạng nền trang
            g.FillRectangle(Brushes.White, e.PageBounds);

            // Định dạng cho logo ở góc trái
            int logoWidth = 80;
            int logoHeight = 80;
            try
            {
                byte[] logoBytes = Properties.Resources.Logo_trang; 
                using (var ms = new System.IO.MemoryStream(logoBytes))
                {
                    Image logo = Image.FromStream(ms);
                    g.DrawImage(logo, leftMargin, yPos, logoWidth, logoHeight);
                }
            }
            catch { }

            // Định dạng tên công ty căn giữa theo chiều cao logo
            SizeF companySize = g.MeasureString("CÔNG TY TNHH HAMSTER", headerFont);
            float companyY = yPos + (logoHeight - companySize.Height) / 2;
            g.DrawString("CÔNG TY TNHH HAMSTER", headerFont, blackBrush, leftMargin + logoWidth + 20, companyY);

            // Định dạng vị trí của dòng thời gian in phiếu
            string printTime = $"Thời gian in: {DateTime.Now:dd/MM/yyyy HH:mm}";
            g.DrawString(printTime, smallFont, grayBrush, e.PageBounds.Width - 250, yPos + 30);
            yPos += logoHeight + 20;

            // Định dạng tiêu đề phiếu
            g.DrawString("PHIẾU ĐẶT VÉ", titleFont, blackBrush, e.PageBounds.Width / 2 - 100, yPos);
            yPos += 40;

            // Định dạng tạo khung chữ nhật bao quát toàn bộ thông tin
            int boxHeight = 500; // chiều cao có thể điều chỉnh tùy số lượng thông tin
            Rectangle infoBox = new Rectangle(leftMargin, yPos, e.PageBounds.Width - 2 * leftMargin, boxHeight);
            g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#ECE6E0")), infoBox);
            g.DrawRectangle(Pens.Gray, infoBox);

            int labelX = leftMargin + 20;
            int valueX = labelX + 200;
            int lineHeight = 30;
            int infoY = yPos + 20;

            // Các thông tin chi tiết
            DrawInfoLine(g, "Mã phiếu:", _printData.MaPhieu, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Mã vé (Ticket Code):", _printData.TicketCode, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Mã đơn đặt vé:", _printData.MaDonDatVe, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Họ và tên:", _printData.HoTen, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Email:", _printData.Email, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Ngày đặt vé:", _printData.NgayDatVe, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Tên phim:", _printData.TenPhim, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Suất chiếu:", _printData.SuatChieu, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Số lượng ghế:", _printData.SoLuongGhe.ToString(), labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Danh sách ghế:", _printData.Ghe, labelX, infoY, normalFont); infoY += lineHeight;
            DrawInfoLine(g, "Phòng chiếu:", _printData.PhongChieu, labelX, infoY, normalFont); infoY += lineHeight + 10;

            g.DrawLine(Pens.Gray, labelX, infoY, e.PageBounds.Width - leftMargin - 20, infoY);
            infoY += 10;

            // Tính tổng tiền bằng số và chữ, tạo phần tình trạng
            DrawInfoLine(g, "Tổng tiền:", $"{_printData.TongTien:N0} VND", labelX, infoY, totalFont, redBrush); infoY += 35;
            DrawInfoLine(g, "Tổng tiền (bằng chữ):", NumberToVietnameseWords(_printData.TongTien), labelX, infoY, normalFont); infoY += 35;
            DrawInfoLine(g, "Tình trạng:", "Thành công", labelX, infoY, normalFont, greenBrush);
        }

        
        private void DrawInfoLine(Graphics g, string label, string value, int x, int y, Font font, Brush valueBrush = null)
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

        
    }
}
