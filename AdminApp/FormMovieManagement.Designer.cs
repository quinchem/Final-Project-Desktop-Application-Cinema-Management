namespace AdminApp
{
    partial class FormMovieManagement
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtTenPhim = new Guna.UI2.WinForms.Guna2TextBox();
            btnTimPhim = new Guna.UI2.WinForms.Guna2Button();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            guna2DataGridView1 = new Guna.UI2.WinForms.Guna2DataGridView();
            TenPhim = new DataGridViewTextBoxColumn();
            NgayChieu = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            ThoiLuong = new DataGridViewTextBoxColumn();
            ChinhSua = new DataGridViewImageColumn();
            Xoa = new DataGridViewImageColumn();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView1).BeginInit();
            SuspendLayout();
            // 
            // txtTenPhim
            // 
            txtTenPhim.BorderColor = Color.Black;
            txtTenPhim.BorderRadius = 10;
            txtTenPhim.CustomizableEdges = customizableEdges1;
            txtTenPhim.DefaultText = "";
            txtTenPhim.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTenPhim.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTenPhim.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTenPhim.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTenPhim.FillColor = Color.LightSteelBlue;
            txtTenPhim.FocusedState.BorderColor = Color.Black;
            txtTenPhim.FocusedState.FillColor = Color.LightSteelBlue;
            txtTenPhim.FocusedState.ForeColor = Color.FromArgb(92, 124, 150);
            txtTenPhim.FocusedState.PlaceholderForeColor = Color.Transparent;
            txtTenPhim.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTenPhim.ForeColor = Color.FromArgb(92, 124, 150);
            txtTenPhim.HoverState.BorderColor = SystemColors.ActiveBorder;
            txtTenPhim.HoverState.ForeColor = Color.FromArgb(92, 124, 150);
            txtTenPhim.HoverState.PlaceholderForeColor = Color.Transparent;
            txtTenPhim.Location = new Point(81, 86);
            txtTenPhim.Margin = new Padding(3, 5, 3, 5);
            txtTenPhim.Name = "txtTenPhim";
            txtTenPhim.PlaceholderForeColor = Color.FromArgb(92, 124, 150);
            txtTenPhim.PlaceholderText = "Nhập tên phim";
            txtTenPhim.SelectedText = "";
            txtTenPhim.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtTenPhim.Size = new Size(497, 45);
            txtTenPhim.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            txtTenPhim.TabIndex = 1;
            // 
            // btnTimPhim
            // 
            btnTimPhim.BorderRadius = 5;
            btnTimPhim.CustomizableEdges = customizableEdges3;
            btnTimPhim.DisabledState.BorderColor = Color.DarkGray;
            btnTimPhim.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTimPhim.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTimPhim.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTimPhim.FillColor = Color.FromArgb(254, 188, 47);
            btnTimPhim.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTimPhim.ForeColor = Color.White;
            btnTimPhim.Image = Properties.Resources.search;
            btnTimPhim.Location = new Point(595, 88);
            btnTimPhim.Name = "btnTimPhim";
            btnTimPhim.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnTimPhim.Size = new Size(75, 37);
            btnTimPhim.TabIndex = 28;
            btnTimPhim.Text = "Tìm";
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 5;
            guna2Button1.CustomizableEdges = customizableEdges5;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(254, 188, 47);
            guna2Button1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Image = Properties.Resources.add;
            guna2Button1.Location = new Point(725, 88);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Button1.Size = new Size(97, 37);
            guna2Button1.TabIndex = 29;
            guna2Button1.Text = "Thêm";
            // 
            // guna2DataGridView1
            // 
            guna2DataGridView1.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            guna2DataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            guna2DataGridView1.BackgroundColor = SystemColors.Window;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            dataGridViewCellStyle2.ForeColor = Color.Gray;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            guna2DataGridView1.ColumnHeadersHeight = 27;
            guna2DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            guna2DataGridView1.Columns.AddRange(new DataGridViewColumn[] { TenPhim, NgayChieu, TrangThai, ThoiLuong, ChinhSua, Xoa });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 192, 128);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            guna2DataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            guna2DataGridView1.GridColor = Color.WhiteSmoke;
            guna2DataGridView1.Location = new Point(76, 164);
            guna2DataGridView1.Name = "guna2DataGridView1";
            guna2DataGridView1.ReadOnly = true;
            guna2DataGridView1.RowHeadersVisible = false;
            guna2DataGridView1.RowHeadersWidth = 51;
            guna2DataGridView1.Size = new Size(1269, 539);
            guna2DataGridView1.TabIndex = 30;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = null;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            guna2DataGridView1.ThemeStyle.BackColor = SystemColors.Window;
            guna2DataGridView1.ThemeStyle.GridColor = Color.WhiteSmoke;
            guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            guna2DataGridView1.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            guna2DataGridView1.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            guna2DataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            guna2DataGridView1.ThemeStyle.HeaderStyle.Height = 27;
            guna2DataGridView1.ThemeStyle.ReadOnly = true;
            guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = Color.White;
            guna2DataGridView1.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            guna2DataGridView1.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            guna2DataGridView1.ThemeStyle.RowsStyle.Height = 29;
            guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // TenPhim
            // 
            TenPhim.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            TenPhim.Frozen = true;
            TenPhim.HeaderText = "Tên Phim";
            TenPhim.MinimumWidth = 6;
            TenPhim.Name = "TenPhim";
            TenPhim.ReadOnly = true;
            TenPhim.Width = 450;
            // 
            // NgayChieu
            // 
            NgayChieu.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            NgayChieu.FillWeight = 65.33226F;
            NgayChieu.Frozen = true;
            NgayChieu.HeaderText = "Ngày Chiếu";
            NgayChieu.MinimumWidth = 6;
            NgayChieu.Name = "NgayChieu";
            NgayChieu.ReadOnly = true;
            NgayChieu.Width = 220;
            // 
            // TrangThai
            // 
            TrangThai.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            TrangThai.FillWeight = 65.33226F;
            TrangThai.Frozen = true;
            TrangThai.HeaderText = "Trạng Thái";
            TrangThai.MinimumWidth = 6;
            TrangThai.Name = "TrangThai";
            TrangThai.ReadOnly = true;
            TrangThai.Width = 220;
            // 
            // ThoiLuong
            // 
            ThoiLuong.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            ThoiLuong.FillWeight = 65.33226F;
            ThoiLuong.Frozen = true;
            ThoiLuong.HeaderText = "Thời Lượng";
            ThoiLuong.MinimumWidth = 6;
            ThoiLuong.Name = "ThoiLuong";
            ThoiLuong.ReadOnly = true;
            ThoiLuong.Width = 220;
            // 
            // ChinhSua
            // 
            ChinhSua.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            ChinhSua.FillWeight = 223.358109F;
            ChinhSua.Frozen = true;
            ChinhSua.HeaderText = "Chỉnh";
            ChinhSua.Image = Properties.Resources.pen;
            ChinhSua.ImageLayout = DataGridViewImageCellLayout.Zoom;
            ChinhSua.MinimumWidth = 6;
            ChinhSua.Name = "ChinhSua";
            ChinhSua.ReadOnly = true;
            ChinhSua.Width = 70;
            // 
            // Xoa
            // 
            Xoa.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Xoa.FillWeight = 80.6451645F;
            Xoa.HeaderText = "Xóa";
            Xoa.Image = Properties.Resources.trash;
            Xoa.ImageLayout = DataGridViewImageCellLayout.Zoom;
            Xoa.MinimumWidth = 6;
            Xoa.Name = "Xoa";
            Xoa.ReadOnly = true;
            Xoa.Width = 70;
            // 
            // guna2Button2
            // 
            guna2Button2.BorderRadius = 5;
            guna2Button2.CustomizableEdges = customizableEdges7;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = Color.FromArgb(254, 188, 47);
            guna2Button2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button2.ForeColor = Color.White;
            guna2Button2.Image = Properties.Resources.pen;
            guna2Button2.Location = new Point(832, 88);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2Button2.Size = new Size(122, 37);
            guna2Button2.TabIndex = 31;
            guna2Button2.Text = "Chỉnh sửa";
            // 
            // guna2Button3
            // 
            guna2Button3.BorderRadius = 5;
            guna2Button3.CustomizableEdges = customizableEdges9;
            guna2Button3.DisabledState.BorderColor = Color.DarkGray;
            guna2Button3.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button3.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button3.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button3.FillColor = Color.Silver;
            guna2Button3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button3.ForeColor = Color.White;
            guna2Button3.Image = Properties.Resources.trash;
            guna2Button3.Location = new Point(967, 88);
            guna2Button3.Name = "guna2Button3";
            guna2Button3.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2Button3.Size = new Size(97, 37);
            guna2Button3.TabIndex = 32;
            guna2Button3.Text = "Xóa";
            // 
            // FormMovieManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(92, 124, 150);
            ClientSize = new Size(1402, 703);
            Controls.Add(guna2Button3);
            Controls.Add(guna2Button2);
            Controls.Add(guna2DataGridView1);
            Controls.Add(guna2Button1);
            Controls.Add(btnTimPhim);
            Controls.Add(txtTenPhim);
            Name = "FormMovieManagement";
            Text = "AdminQLPhim";
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtTenPhim;
        private Guna.UI2.WinForms.Guna2Button btnTimPhim;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2DataGridView guna2DataGridView1;
        private DataGridViewTextBoxColumn TenPhim;
        private DataGridViewTextBoxColumn NgayChieu;
        private DataGridViewTextBoxColumn TrangThai;
        private DataGridViewTextBoxColumn ThoiLuong;
        private DataGridViewImageColumn ChinhSua;
        private DataGridViewImageColumn Xoa;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
    }
}