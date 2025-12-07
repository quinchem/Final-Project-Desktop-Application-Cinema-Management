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
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.HotTrack;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvHistoryTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvHistoryTicket.ColumnHeadersHeight = 34;
            dgvHistoryTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHistoryTicket.Columns.AddRange(new DataGridViewColumn[] { STT, MaDatVe, TenPhim, SuatChieu, NgayDatVe, TongTien, XemChiTiet });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvHistoryTicket.DefaultCellStyle = dataGridViewCellStyle2;
            dgvHistoryTicket.Location = new Point(0, 0);
            dgvHistoryTicket.Name = "dgvHistoryTicket";
            dgvHistoryTicket.RowHeadersWidth = 62;
            dgvHistoryTicket.Size = new Size(1328, 552);
            dgvHistoryTicket.TabIndex = 0;
            dgvHistoryTicket.CellContentClick += dgvHistoryTicket_CellContentClick;
            dgvHistoryTicket.CellPainting += dgvHistoryTicket_CellPainting;
            // 
            // STT
            // 
            STT.FillWeight = 58.88074F;
            STT.HeaderText = "STT";
            STT.MinimumWidth = 8;
            STT.Name = "STT";
            // 
            // MaDatVe
            // 
            MaDatVe.DataPropertyName = "MaDatVe";
            MaDatVe.FillWeight = 114.957649F;
            MaDatVe.HeaderText = "Mã đơn đặt vé";
            MaDatVe.MinimumWidth = 8;
            MaDatVe.Name = "MaDatVe";
            // 
            // TenPhim
            // 
            TenPhim.FillWeight = 139.6315F;
            TenPhim.HeaderText = "Tên phim";
            TenPhim.MinimumWidth = 8;
            TenPhim.Name = "TenPhim";
            // 
            // SuatChieu
            // 
            SuatChieu.FillWeight = 111.794319F;
            SuatChieu.HeaderText = "Suất chiếu";
            SuatChieu.MinimumWidth = 8;
            SuatChieu.Name = "SuatChieu";
            // 
            // NgayDatVe
            // 
            NgayDatVe.FillWeight = 108.225479F;
            NgayDatVe.HeaderText = "Ngày đặt vé";
            NgayDatVe.MinimumWidth = 8;
            NgayDatVe.Name = "NgayDatVe";
            // 
            // TongTien
            // 
            TongTien.FillWeight = 104.199074F;
            TongTien.HeaderText = "Tổng Tiền";
            TongTien.MinimumWidth = 8;
            TongTien.Name = "TongTien";
            // 
            // TicketCode
            // 
            TicketCode.FillWeight = 90.9091F;
            TicketCode.HeaderText = "Ticket Code";
            TicketCode.MinimumWidth = 8;
            TicketCode.Name = "TicketCode";
            // 
            // XemChiTiet
            // 
            XemChiTiet.FillWeight = 71.4022446F;
            XemChiTiet.HeaderText = "Xem chi tiết";
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
