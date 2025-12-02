
using Microsoft.Data.Sqlite;
using SharedData;
using System;
using System.Drawing;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace UserApp
{
    public partial class HistoryTicket : UserControl
    {
        private string _customerId;

        public HistoryTicket(string customerId)
        {
            InitializeComponent();
            _customerId = customerId;

            dgvHistoryTicket.AutoGenerateColumns = false;
            LoadHistoryData();
        }

        private void LoadHistoryData()
        {
            dgvHistoryTicket.Rows.Clear();

            try
            {
                using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();

                    // Debug: Kiểm tra xem có bill nào của customer không
                    /*string checkQuery = "SELECT COUNT(*) FROM bill WHERE customer_id = @customer_id";
                    using (var checkCmd = new SqliteCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@customer_id", _customerId);
                        long count = (long)checkCmd.ExecuteScalar();
                        MessageBox.Show($"Tìm thấy {count} bill của khách hàng {_customerId}", "Debug Info");
                    }*/

                    string query = @"
                        SELECT 
                            b.bill_id,
                            m.title AS movie_name,
                            s.show_date,
                            s.start_time,
                            SUM(se.per_seat_ticket_price) as total
                        FROM bill b
                        JOIN showtime s ON b.showtime_id = s.showtime_id
                        JOIN movie m ON s.movie_id = m.movie_id
                        JOIN bill_seat bs ON b.bill_id = bs.bill_id
                        JOIN seat se ON bs.seat_id = se.seat_id
                        WHERE b.customer_id = @customer_id
                        GROUP BY b.bill_id, m.title, s.show_date, s.start_time
                        ORDER BY s.show_date DESC, s.start_time DESC;
                    ";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customer_id", _customerId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            int stt = 1;
                            bool hasRows = false;

                            while (reader.Read())
                            {
                                hasRows = true;
                                string billId = reader["bill_id"].ToString();
                                string movieName = reader["movie_name"].ToString();
                                string showDate = reader["show_date"].ToString();
                                string startTime = reader["start_time"].ToString();

                                // Kết hợp ngày và giờ để hiển thị suất chiếu
                                string showDateTime = $"{startTime}"; // Chỉ giờ chiếu

                                // Ngày đặt vé (dùng show_date vì database không có bill_date)
                                string bookingDate = showDate;

                                // Format tiền tệ VND
                                decimal total = Convert.ToDecimal(reader["total"]);
                                string formattedTotal = total.ToString("N0") + " VNĐ";

                                // Tự sinh Ticket Code
                                string ticketCode = GenerateTicketCode(billId);

                                dgvHistoryTicket.Rows.Add(
                                    stt++,              // Cột 0: STT
                                    billId,             // Cột 1: Mã đặt vé
                                    movieName,          // Cột 2: Tên phim
                                    showDateTime,       // Cột 3: Suất chiếu (giờ)
                                    bookingDate,        // Cột 4: Ngày đặt vé
                                    formattedTotal,     // Cột 5: Tổng tiền
                                    ticketCode,         // Cột 6: Ticket Code
                                    "Xem"               // Cột 7: Xem chi tiết
                                );
                            }

                            if (!hasRows)
                            {
                                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                                player.Play();
                                MessageBox.Show($"Khách hàng {_customerId} chưa có lịch sử vé nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            /*else
                            {
                                MessageBox.Show($"Đã load {stt - 1} vé thành công!", "Thành công");
                            }*/
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm tự sinh Ticket Code
        private string GenerateTicketCode(string billId)
        {
            // Tạo SHA256 hash cố định từ billId
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(billId));

                // Lấy 4 ký tự đầu tiên của hash dưới dạng HEX
                string hashPart = BitConverter.ToString(bytes).Replace("-", "").Substring(0, 4);

                return $"TK-{billId}-{hashPart}";
            }
        }

        private void dgvHistoryTicket_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Kiểm tra cột "Xem" - cột cuối cùng
            if (e.ColumnIndex == dgvHistoryTicket.Columns["XemChiTiet"]?.Index && e.RowIndex >= 0)
            {
                e.Handled = true;

                Color btnColor = ColorTranslator.FromHtml("#2C5473");
                e.Graphics.FillRectangle(new SolidBrush(btnColor), e.CellBounds);

                TextRenderer.DrawText(
                    e.Graphics,
                    "Xem",
                    e.CellStyle.Font,
                    e.CellBounds,
                    Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Graphics.DrawRectangle(Pens.Black, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
            }
        }

        public event Action<string> OnViewBillDetail; // string là bill_id

        private void dgvHistoryTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvHistoryTicket.Columns[e.ColumnIndex].Name == "XemChiTiet") // tên cột nút "Xem"
            {
                string billId = dgvHistoryTicket.Rows[e.RowIndex].Cells["MaDatVe"].Value.ToString();
                OnViewBillDetail?.Invoke(billId);
            }
        }
    }
}
