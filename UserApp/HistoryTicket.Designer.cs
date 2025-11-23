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
            dgvHistoryTicket = new Guna.UI2.WinForms.Guna2DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvHistoryTicket).BeginInit();
            SuspendLayout();
            // 
            // dgvHistoryTicket
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvHistoryTicket.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvHistoryTicket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvHistoryTicket.ColumnHeadersHeight = 4;
            dgvHistoryTicket.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvHistoryTicket.DefaultCellStyle = dataGridViewCellStyle3;
            dgvHistoryTicket.GridColor = Color.FromArgb(231, 229, 255);
            dgvHistoryTicket.Location = new Point(0, 0);
            dgvHistoryTicket.Name = "dgvHistoryTicket";
            dgvHistoryTicket.RowHeadersVisible = false;
            dgvHistoryTicket.RowHeadersWidth = 62;
            dgvHistoryTicket.Size = new Size(1328, 552);
            dgvHistoryTicket.TabIndex = 0;
            dgvHistoryTicket.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvHistoryTicket.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvHistoryTicket.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvHistoryTicket.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvHistoryTicket.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvHistoryTicket.ThemeStyle.BackColor = Color.White;
            dgvHistoryTicket.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvHistoryTicket.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvHistoryTicket.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistoryTicket.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvHistoryTicket.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvHistoryTicket.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvHistoryTicket.ThemeStyle.HeaderStyle.Height = 4;
            dgvHistoryTicket.ThemeStyle.ReadOnly = false;
            dgvHistoryTicket.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvHistoryTicket.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistoryTicket.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvHistoryTicket.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvHistoryTicket.ThemeStyle.RowsStyle.Height = 33;
            dgvHistoryTicket.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvHistoryTicket.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // HistoryTicket
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 230, 224);
            Controls.Add(dgvHistoryTicket);
            Name = "HistoryTicket";
            Size = new Size(1328, 552);
            ((System.ComponentModel.ISupportInitialize)dgvHistoryTicket).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvHistoryTicket;
    }
}
