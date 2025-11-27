using Microsoft.Data.Sqlite;
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
    public partial class HistoryTicketDetail : UserControl
    {
        private string _billId;
        public HistoryTicketDetail(string billId)
        {
            InitializeComponent();
            _billId = billId;
            LoadDetail();
        }

        private void LoadDetail()
        {
            // Kết nối DB và load thông tin bill chi tiết
            using (var conn = new SqliteConnection(DatabaseHelper2.GetConnectionString()))
            {
                conn.Open();
                string query = "SELECT * FROM Bill_detail WHERE bill_id = @bill_id";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@bill_id", _billId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Hiển thị dữ liệu lên DataGridView hoặc Label trong HistoryDetail
                    }
                }
            }
        }
    }
}
