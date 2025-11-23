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
            dgvHistoryTicket = new DataGridView();
            STT = new DataGridViewTextBoxColumn();
            MaDatVe = new DataGridViewTextBoxColumn();
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
            dgvHistoryTicket.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dgvHistoryTicket.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistoryTicket.BackgroundColor = Color.FromArgb(236, 230, 224);
            dgvHistoryTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistoryTicket.Columns.AddRange(new DataGridViewColumn[] { STT, MaDatVe, TenPhim, SuatChieu, NgayDatVe, TongTien, TicketCode, XemChiTiet });
            dgvHistoryTicket.Location = new Point(0, 0);
            dgvHistoryTicket.Name = "dgvHistoryTicket";
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
            // MaDatVe
            // 
            MaDatVe.HeaderText = "Mã Đặt Vé";
            MaDatVe.MinimumWidth = 8;
            MaDatVe.Name = "MaDatVe";
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
            Controls.Add(dgvHistoryTicket);
            Name = "HistoryTicket";
            Size = new Size(1328, 552);
            ((System.ComponentModel.ISupportInitialize)dgvHistoryTicket).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvHistoryTicket;
        private DataGridViewTextBoxColumn STT;
        private DataGridViewTextBoxColumn MaDatVe;
        private DataGridViewTextBoxColumn TenPhim;
        private DataGridViewTextBoxColumn SuatChieu;
        private DataGridViewTextBoxColumn NgayDatVe;
        private DataGridViewTextBoxColumn TongTien;
        private DataGridViewTextBoxColumn TicketCode;
        private DataGridViewButtonColumn XemChiTiet;
    }
}
