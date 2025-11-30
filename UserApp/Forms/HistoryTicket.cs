using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Windows.Forms;
using SharedData;

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

            using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        b.bill_id,
                        m.title AS movie_name,
                        s.show_date AS show_time,
                        b.bill_date,
                        b.quantity_ticket,
                        b.per_seat_ticket_price
                    FROM Bill b
                    JOIN Showtime s ON b.showtime_id = s.showtime_id
                    JOIN Movie m ON s.movie_id = m.movie_id
                    WHERE b.customer_id = @customer_id
                    ORDER BY b.bill_date DESC;
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
                            string showTime = reader["show_time"].ToString();
                            string billDate = Convert.ToDateTime(reader["bill_date"]).ToString("yyyy-MM-dd");

                            int qtyTicket = Convert.ToInt32(reader["quantity_ticket"]);
                            int ticketPrice = Convert.ToInt32(reader["per_seat_ticket_price"]);

                            int totalMoney = qtyTicket * ticketPrice;

                            string ticketCode = GenerateRandomCode();

                            dgvHistoryTicket.Rows.Add(
                                stt++,
                                billId,
                                movieName,
                                showTime,
                                billDate,
                                totalMoney.ToString("N0"),
                                ticketCode,
                                "Xem"
                            );
                        }

                        if (!hasRows)
                        {
                            MessageBox.Show("Khách hàng chưa có lịch sử vé nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private string GenerateRandomCode()
        {
            return "TK" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }

        private void dgvHistoryTicket_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex >= 0) // index cột nút "Xem"
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


