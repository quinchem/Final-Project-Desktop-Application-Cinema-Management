namespace AdminApp
{
    partial class FormProduct
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dgvProduct = new Guna.UI2.WinForms.Guna2DataGridView();
            STT = new DataGridViewTextBoxColumn();
            LoaiSanPham = new DataGridViewTextBoxColumn();
            TenSanPham = new DataGridViewTextBoxColumn();
            SoLuongTonKho = new DataGridViewTextBoxColumn();
            GiaNhap = new DataGridViewTextBoxColumn();
            GiaBan = new DataGridViewTextBoxColumn();
            panelAddProduct = new Panel();
            pctProduct = new PictureBox();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            guna2TextBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            guna2TextBox3 = new Guna.UI2.WinForms.Guna2TextBox();
            guna2TextBox4 = new Guna.UI2.WinForms.Guna2TextBox();
            guna2TextBox5 = new Guna.UI2.WinForms.Guna2TextBox();
            btnSaveProduct = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)dgvProduct).BeginInit();
            panelAddProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pctProduct).BeginInit();
            SuspendLayout();
            // 
            // dgvProduct
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvProduct.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvProduct.BackgroundColor = Color.FromArgb(217, 217, 217);
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvProduct.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvProduct.ColumnHeadersHeight = 27;
            dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvProduct.Columns.AddRange(new DataGridViewColumn[] { STT, LoaiSanPham, TenSanPham, SoLuongTonKho, GiaNhap, GiaBan });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvProduct.DefaultCellStyle = dataGridViewCellStyle3;
            dgvProduct.GridColor = Color.FromArgb(231, 229, 255);
            dgvProduct.Location = new Point(-1, 73);
            dgvProduct.Name = "dgvProduct";
            dgvProduct.RowHeadersVisible = false;
            dgvProduct.RowHeadersWidth = 62;
            dgvProduct.Size = new Size(1284, 649);
            dgvProduct.TabIndex = 0;
            dgvProduct.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvProduct.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvProduct.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvProduct.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvProduct.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvProduct.ThemeStyle.BackColor = Color.FromArgb(217, 217, 217);
            dgvProduct.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvProduct.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvProduct.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProduct.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvProduct.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvProduct.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvProduct.ThemeStyle.HeaderStyle.Height = 27;
            dgvProduct.ThemeStyle.ReadOnly = false;
            dgvProduct.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvProduct.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProduct.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvProduct.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvProduct.ThemeStyle.RowsStyle.Height = 33;
            dgvProduct.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvProduct.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // STT
            // 
            STT.HeaderText = "STT";
            STT.MinimumWidth = 8;
            STT.Name = "STT";
            // 
            // LoaiSanPham
            // 
            LoaiSanPham.HeaderText = "LoaiSanPham";
            LoaiSanPham.MinimumWidth = 8;
            LoaiSanPham.Name = "LoaiSanPham";
            // 
            // TenSanPham
            // 
            TenSanPham.HeaderText = "Tên Sản Phẩm";
            TenSanPham.MinimumWidth = 8;
            TenSanPham.Name = "TenSanPham";
            // 
            // SoLuongTonKho
            // 
            SoLuongTonKho.HeaderText = "Số lượng tồn kho";
            SoLuongTonKho.MinimumWidth = 8;
            SoLuongTonKho.Name = "SoLuongTonKho";
            // 
            // GiaNhap
            // 
            GiaNhap.HeaderText = "Giá Nhập";
            GiaNhap.MinimumWidth = 8;
            GiaNhap.Name = "GiaNhap";
            // 
            // GiaBan
            // 
            GiaBan.HeaderText = "Giá Bán";
            GiaBan.MinimumWidth = 8;
            GiaBan.Name = "GiaBan";
            // 
            // panelAddProduct
            // 
            panelAddProduct.BackColor = Color.FromArgb(247, 244, 241);
            panelAddProduct.Controls.Add(btnSaveProduct);
            panelAddProduct.Controls.Add(guna2TextBox5);
            panelAddProduct.Controls.Add(guna2TextBox4);
            panelAddProduct.Controls.Add(guna2TextBox3);
            panelAddProduct.Controls.Add(guna2TextBox2);
            panelAddProduct.Controls.Add(guna2TextBox1);
            panelAddProduct.Controls.Add(guna2HtmlLabel5);
            panelAddProduct.Controls.Add(guna2HtmlLabel4);
            panelAddProduct.Controls.Add(guna2HtmlLabel3);
            panelAddProduct.Controls.Add(guna2HtmlLabel2);
            panelAddProduct.Controls.Add(guna2HtmlLabel1);
            panelAddProduct.Controls.Add(pctProduct);
            panelAddProduct.Location = new Point(104, 164);
            panelAddProduct.Name = "panelAddProduct";
            panelAddProduct.Size = new Size(1078, 460);
            panelAddProduct.TabIndex = 1;
            // 
            // pctProduct
            // 
            pctProduct.Location = new Point(42, 43);
            pctProduct.Name = "pctProduct";
            pctProduct.Size = new Size(172, 175);
            pctProduct.TabIndex = 0;
            pctProduct.TabStop = false;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            guna2HtmlLabel1.ForeColor = Color.Coral;
            guna2HtmlLabel1.Location = new Point(306, 43);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(151, 32);
            guna2HtmlLabel1.TabIndex = 1;
            guna2HtmlLabel1.Text = "Loại sản phẩm";
            // 
            // guna2HtmlLabel2
            // 
            guna2HtmlLabel2.BackColor = Color.Transparent;
            guna2HtmlLabel2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            guna2HtmlLabel2.ForeColor = Color.Coral;
            guna2HtmlLabel2.Location = new Point(306, 148);
            guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            guna2HtmlLabel2.Size = new Size(95, 32);
            guna2HtmlLabel2.TabIndex = 2;
            guna2HtmlLabel2.Text = "Giá nhập";
            // 
            // guna2HtmlLabel3
            // 
            guna2HtmlLabel3.BackColor = Color.Transparent;
            guna2HtmlLabel3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            guna2HtmlLabel3.ForeColor = Color.Coral;
            guna2HtmlLabel3.Location = new Point(306, 253);
            guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            guna2HtmlLabel3.Size = new Size(181, 32);
            guna2HtmlLabel3.TabIndex = 3;
            guna2HtmlLabel3.Text = "Số lượng tồn kho";
            // 
            // guna2HtmlLabel4
            // 
            guna2HtmlLabel4.BackColor = Color.Transparent;
            guna2HtmlLabel4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            guna2HtmlLabel4.ForeColor = Color.Coral;
            guna2HtmlLabel4.Location = new Point(686, 43);
            guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            guna2HtmlLabel4.Size = new Size(147, 32);
            guna2HtmlLabel4.TabIndex = 4;
            guna2HtmlLabel4.Text = "Tên sản phẩm";
            // 
            // guna2HtmlLabel5
            // 
            guna2HtmlLabel5.BackColor = Color.Transparent;
            guna2HtmlLabel5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            guna2HtmlLabel5.ForeColor = Color.Coral;
            guna2HtmlLabel5.Location = new Point(686, 148);
            guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            guna2HtmlLabel5.Size = new Size(82, 32);
            guna2HtmlLabel5.TabIndex = 5;
            guna2HtmlLabel5.Text = "Giá bán";
            // 
            // guna2TextBox1
            // 
            guna2TextBox1.BorderRadius = 10;
            guna2TextBox1.CustomizableEdges = customizableEdges11;
            guna2TextBox1.DefaultText = "";
            guna2TextBox1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.Font = new Font("Segoe UI", 9F);
            guna2TextBox1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.Location = new Point(306, 83);
            guna2TextBox1.Margin = new Padding(4, 5, 4, 5);
            guna2TextBox1.Name = "guna2TextBox1";
            guna2TextBox1.PlaceholderText = "";
            guna2TextBox1.SelectedText = "";
            guna2TextBox1.ShadowDecoration.CustomizableEdges = customizableEdges12;
            guna2TextBox1.Size = new Size(262, 37);
            guna2TextBox1.TabIndex = 6;
            // 
            // guna2TextBox2
            // 
            guna2TextBox2.BorderRadius = 10;
            guna2TextBox2.CustomizableEdges = customizableEdges9;
            guna2TextBox2.DefaultText = "";
            guna2TextBox2.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox2.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox2.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox2.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox2.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox2.Font = new Font("Segoe UI", 9F);
            guna2TextBox2.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox2.Location = new Point(306, 188);
            guna2TextBox2.Margin = new Padding(4, 5, 4, 5);
            guna2TextBox2.Name = "guna2TextBox2";
            guna2TextBox2.PlaceholderText = "";
            guna2TextBox2.SelectedText = "";
            guna2TextBox2.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2TextBox2.Size = new Size(262, 37);
            guna2TextBox2.TabIndex = 7;
            // 
            // guna2TextBox3
            // 
            guna2TextBox3.BorderRadius = 10;
            guna2TextBox3.CustomizableEdges = customizableEdges7;
            guna2TextBox3.DefaultText = "";
            guna2TextBox3.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox3.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox3.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox3.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox3.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox3.Font = new Font("Segoe UI", 9F);
            guna2TextBox3.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox3.Location = new Point(306, 293);
            guna2TextBox3.Margin = new Padding(4, 5, 4, 5);
            guna2TextBox3.Name = "guna2TextBox3";
            guna2TextBox3.PlaceholderText = "";
            guna2TextBox3.SelectedText = "";
            guna2TextBox3.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2TextBox3.Size = new Size(262, 37);
            guna2TextBox3.TabIndex = 8;
            // 
            // guna2TextBox4
            // 
            guna2TextBox4.BorderRadius = 10;
            guna2TextBox4.CustomizableEdges = customizableEdges5;
            guna2TextBox4.DefaultText = "";
            guna2TextBox4.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox4.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox4.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox4.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox4.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox4.Font = new Font("Segoe UI", 9F);
            guna2TextBox4.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox4.Location = new Point(686, 83);
            guna2TextBox4.Margin = new Padding(4, 5, 4, 5);
            guna2TextBox4.Name = "guna2TextBox4";
            guna2TextBox4.PlaceholderText = "";
            guna2TextBox4.SelectedText = "";
            guna2TextBox4.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2TextBox4.Size = new Size(262, 37);
            guna2TextBox4.TabIndex = 9;
            // 
            // guna2TextBox5
            // 
            guna2TextBox5.BorderRadius = 10;
            guna2TextBox5.CustomizableEdges = customizableEdges3;
            guna2TextBox5.DefaultText = "";
            guna2TextBox5.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox5.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox5.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox5.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox5.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox5.Font = new Font("Segoe UI", 9F);
            guna2TextBox5.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox5.Location = new Point(686, 188);
            guna2TextBox5.Margin = new Padding(4, 5, 4, 5);
            guna2TextBox5.Name = "guna2TextBox5";
            guna2TextBox5.PlaceholderText = "";
            guna2TextBox5.SelectedText = "";
            guna2TextBox5.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2TextBox5.Size = new Size(262, 37);
            guna2TextBox5.TabIndex = 10;
            // 
            // btnSaveProduct
            // 
            btnSaveProduct.BorderRadius = 10;
            btnSaveProduct.CustomizableEdges = customizableEdges1;
            btnSaveProduct.DisabledState.BorderColor = Color.DarkGray;
            btnSaveProduct.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSaveProduct.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSaveProduct.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSaveProduct.FillColor = Color.FromArgb(44, 84, 115);
            btnSaveProduct.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            btnSaveProduct.ForeColor = Color.White;
            btnSaveProduct.Location = new Point(762, 367);
            btnSaveProduct.Name = "btnSaveProduct";
            btnSaveProduct.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnSaveProduct.Size = new Size(186, 55);
            btnSaveProduct.TabIndex = 11;
            btnSaveProduct.Text = "Lưu";
            // 
            // FormProduct
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(92, 124, 150);
            ClientSize = new Size(1285, 751);
            Controls.Add(panelAddProduct);
            Controls.Add(dgvProduct);
            Name = "FormProduct";
            Text = "FormProduct";
            ((System.ComponentModel.ISupportInitialize)dgvProduct).EndInit();
            panelAddProduct.ResumeLayout(false);
            panelAddProduct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pctProduct).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvProduct;
        private DataGridViewTextBoxColumn STT;
        private DataGridViewTextBoxColumn LoaiSanPham;
        private DataGridViewTextBoxColumn TenSanPham;
        private DataGridViewTextBoxColumn SoLuongTonKho;
        private DataGridViewTextBoxColumn GiaNhap;
        private DataGridViewTextBoxColumn GiaBan;
        private Panel panelAddProduct;
        private PictureBox pctProduct;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel4;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2Button btnSaveProduct;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox5;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox4;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox3;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox2;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
    }
}