namespace UserApp
{
    partial class HistoryTicket
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dgvHistoryTicket = new DataGridView();
            STT = new DataGridViewTextBoxColumn();
            TenPhim = new DataGridViewTextBoxColumn();
            SuatChieu = new DataGridViewTextBoxColumn();
            NgayDatVe = new DataGridViewTextBoxColumn();
            TongTien = new DataGridViewTextBoxColumn();
            TicketCode = new DataGridViewTextBoxColumn();
            XemChiTiet = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dgvHistoryTicket).BeginInit();
            SuspendLayout();
            // 
            // dgvHistoryTicket
            // 
            dgvHistoryTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistoryTicket.BackgroundColor = Color.FromArgb(236, 230, 224);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(198, 198, 200);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvHistoryTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvHistoryTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistoryTicket.Columns.AddRange(new DataGridViewColumn[] { STT, TenPhim, SuatChieu, NgayDatVe, TongTien, TicketCode, XemChiTiet });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(236, 230, 224);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvHistoryTicket.DefaultCellStyle = dataGridViewCellStyle2;
            dgvHistoryTicket.Dock = DockStyle.Fill;
            dgvHistoryTicket.GridColor = Color.Gray;
            dgvHistoryTicket.Location = new Point(0, 0);
            dgvHistoryTicket.Name = "dgvHistoryTicket";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(198, 198, 200);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvHistoryTicket.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvHistoryTicket.RowHeadersWidth = 62;
            dgvHistoryTicket.Size = new Size(1328, 552);
            dgvHistoryTicket.TabIndex = 0;
            // 
            // STT
            // 
            STT.HeaderText = "STT";
            STT.MinimumWidth = 8;
            STT.Name = "STT";
            // 
            // TenPhim
            // 
            TenPhim.HeaderText = "Tên Phim";
            TenPhim.MinimumWidth = 8;
            TenPhim.Name = "TenPhim";
            // 
            // SuatChieu
            // 
            SuatChieu.HeaderText = "Suất Chiếu";
            SuatChieu.MinimumWidth = 8;
            SuatChieu.Name = "SuatChieu";
            // 
            // NgayDatVe
            // 
            NgayDatVe.HeaderText = "Ngày Đặt Vé";
            NgayDatVe.MinimumWidth = 8;
            NgayDatVe.Name = "NgayDatVe";
            // 
            // TongTien
            // 
            TongTien.HeaderText = "Tổng Tiền";
            TongTien.MinimumWidth = 8;
            TongTien.Name = "TongTien";
            // 
            // TicketCode
            // 
            TicketCode.HeaderText = "Ticket Code";
            TicketCode.MinimumWidth = 8;
            TicketCode.Name = "TicketCode";
            // 
            // XemChiTiet
            // 
            XemChiTiet.HeaderText = "Xem Chi Tiết";
            XemChiTiet.MinimumWidth = 8;
            XemChiTiet.Name = "XemChiTiet";
            XemChiTiet.Text = "Xem";
            XemChiTiet.ToolTipText = "Xem";
            XemChiTiet.UseColumnTextForButtonValue = true;
            // 
            // HistoryTicket
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(dgvHistoryTicket);
            Name = "HistoryTicket";
            Size = new Size(1328, 552);
            ((System.ComponentModel.ISupportInitialize)dgvHistoryTicket).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvHistoryTicket;
        private DataGridViewTextBoxColumn STT;
        private DataGridViewTextBoxColumn TenPhim;
        private DataGridViewTextBoxColumn SuatChieu;
        private DataGridViewTextBoxColumn NgayDatVe;
        private DataGridViewTextBoxColumn TongTien;
        private DataGridViewTextBoxColumn TicketCode;
        private DataGridViewButtonColumn XemChiTiet;
    }
}
