namespace AdminApp
{
    partial class FormCustomerManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCustomerManagement));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            btnTim = new Guna.UI2.WinForms.Guna2Button();
            btnXuatFile = new Guna.UI2.WinForms.Guna2Button();
            btnChinhSua = new Guna.UI2.WinForms.Guna2Button();
            btnXoa = new Guna.UI2.WinForms.Guna2Button();
            DataGridViewCustomerManagement = new Guna.UI2.WinForms.Guna2DataGridView();
            HoTen = new DataGridViewTextBoxColumn();
            GioiTinh = new DataGridViewTextBoxColumn();
            NgaySinh = new DataGridViewTextBoxColumn();
            SĐT = new DataGridViewTextBoxColumn();
            Email = new DataGridViewTextBoxColumn();
            DiaChi = new DataGridViewTextBoxColumn();
            ThoiGianTaoTK = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)DataGridViewCustomerManagement).BeginInit();
            SuspendLayout();
            // 
            // txtTimKiem
            // 
            txtTimKiem.BorderColor = Color.Transparent;
            txtTimKiem.BorderThickness = 0;
            txtTimKiem.CustomizableEdges = null;
            txtTimKiem.DefaultText = "";
            txtTimKiem.FillColor = Color.FromArgb(235, 235, 235);
            txtTimKiem.FocusedState.BorderColor = Color.FromArgb(0, 120, 215);
            txtTimKiem.Font = new Font("Segoe UI", 10.5F);
            txtTimKiem.ForeColor = Color.Black;
            txtTimKiem.HoverState.BorderColor = Color.FromArgb(0, 120, 215);
            txtTimKiem.Location = new Point(68, 54);
            txtTimKiem.Margin = new Padding(3, 4, 3, 4);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderForeColor = Color.Gray;
            txtTimKiem.PlaceholderText = "Hãy nhập từ khóa";
            txtTimKiem.SelectedText = "";
            txtTimKiem.ShadowDecoration.CustomizableEdges = customizableEdges1;
            txtTimKiem.Size = new Size(270, 35);
            txtTimKiem.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            txtTimKiem.TabIndex = 0;
            // 
            // btnTim
            // 
            btnTim.BorderRadius = 8;
            btnTim.CustomizableEdges = customizableEdges2;
            btnTim.DisabledState.BorderColor = Color.DarkGray;
            btnTim.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTim.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTim.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTim.FillColor = Color.FromArgb(254, 188, 47);
            btnTim.FocusedColor = Color.White;
            btnTim.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTim.ForeColor = Color.Black;
            btnTim.Image = Properties.Resources.search;
            btnTim.Location = new Point(372, 54);
            btnTim.Name = "btnTim";
            btnTim.ShadowDecoration.CustomizableEdges = customizableEdges3;
            btnTim.Size = new Size(75, 37);
            btnTim.TabIndex = 29;
            btnTim.Text = "Tìm";
            btnTim.Click += btnTim_Click;
            // 
            // btnXuatFile
            // 
            btnXuatFile.BorderRadius = 8;
            btnXuatFile.CustomizableEdges = customizableEdges4;
            btnXuatFile.DisabledState.BorderColor = Color.DarkGray;
            btnXuatFile.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXuatFile.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXuatFile.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXuatFile.FillColor = Color.FromArgb(254, 188, 47);
            btnXuatFile.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXuatFile.ForeColor = Color.Black;
            btnXuatFile.Image = (Image)resources.GetObject("btnXuatFile.Image");
            btnXuatFile.Location = new Point(897, 54);
            btnXuatFile.Name = "btnXuatFile";
            btnXuatFile.ShadowDecoration.CustomizableEdges = customizableEdges5;
            btnXuatFile.Size = new Size(189, 37);
            btnXuatFile.TabIndex = 30;
            btnXuatFile.Text = "Xuất file excel";
            btnXuatFile.Click += btnXuatFile_Click;
            // 
            // btnChinhSua
            // 
            btnChinhSua.BorderRadius = 8;
            btnChinhSua.CustomizableEdges = customizableEdges6;
            btnChinhSua.DisabledState.BorderColor = Color.DarkGray;
            btnChinhSua.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChinhSua.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChinhSua.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChinhSua.FillColor = Color.FromArgb(254, 188, 47);
            btnChinhSua.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChinhSua.ForeColor = Color.Black;
            btnChinhSua.Image = Properties.Resources.pen;
            btnChinhSua.Location = new Point(1104, 54);
            btnChinhSua.Name = "btnChinhSua";
            btnChinhSua.ShadowDecoration.CustomizableEdges = customizableEdges7;
            btnChinhSua.Size = new Size(122, 37);
            btnChinhSua.TabIndex = 32;
            btnChinhSua.Text = "Chỉnh sửa";
            btnChinhSua.Click += btnChinhSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.BorderRadius = 8;
            btnXoa.CustomizableEdges = customizableEdges8;
            btnXoa.DisabledState.BorderColor = Color.DarkGray;
            btnXoa.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXoa.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXoa.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXoa.FillColor = Color.Silver;
            btnXoa.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXoa.ForeColor = Color.Black;
            btnXoa.Image = Properties.Resources.trash;
            btnXoa.Location = new Point(1251, 54);
            btnXoa.Name = "btnXoa";
            btnXoa.ShadowDecoration.CustomizableEdges = customizableEdges9;
            btnXoa.Size = new Size(97, 37);
            btnXoa.TabIndex = 33;
            btnXoa.Text = "Xóa";
            // 
            // DataGridViewCustomerManagement
            // 
            DataGridViewCustomerManagement.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            DataGridViewCustomerManagement.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            DataGridViewCustomerManagement.BackgroundColor = SystemColors.Window;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            dataGridViewCellStyle2.ForeColor = Color.Gray;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            DataGridViewCustomerManagement.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            DataGridViewCustomerManagement.ColumnHeadersHeight = 27;
            DataGridViewCustomerManagement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            DataGridViewCustomerManagement.Columns.AddRange(new DataGridViewColumn[] { HoTen, GioiTinh, NgaySinh, SĐT, Email, DiaChi, ThoiGianTaoTK });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 192, 128);
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            DataGridViewCustomerManagement.DefaultCellStyle = dataGridViewCellStyle3;
            DataGridViewCustomerManagement.GridColor = Color.WhiteSmoke;
            DataGridViewCustomerManagement.Location = new Point(12, 157);
            DataGridViewCustomerManagement.Name = "DataGridViewCustomerManagement";
            DataGridViewCustomerManagement.ReadOnly = true;
            DataGridViewCustomerManagement.RowHeadersVisible = false;
            DataGridViewCustomerManagement.RowHeadersWidth = 51;
            DataGridViewCustomerManagement.Size = new Size(1396, 539);
            DataGridViewCustomerManagement.TabIndex = 34;
            DataGridViewCustomerManagement.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            DataGridViewCustomerManagement.ThemeStyle.AlternatingRowsStyle.Font = null;
            DataGridViewCustomerManagement.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            DataGridViewCustomerManagement.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            DataGridViewCustomerManagement.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            DataGridViewCustomerManagement.ThemeStyle.BackColor = SystemColors.Window;
            DataGridViewCustomerManagement.ThemeStyle.GridColor = Color.WhiteSmoke;
            DataGridViewCustomerManagement.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            DataGridViewCustomerManagement.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            DataGridViewCustomerManagement.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            DataGridViewCustomerManagement.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            DataGridViewCustomerManagement.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            DataGridViewCustomerManagement.ThemeStyle.HeaderStyle.Height = 27;
            DataGridViewCustomerManagement.ThemeStyle.ReadOnly = true;
            DataGridViewCustomerManagement.ThemeStyle.RowsStyle.BackColor = Color.White;
            DataGridViewCustomerManagement.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            DataGridViewCustomerManagement.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            DataGridViewCustomerManagement.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            DataGridViewCustomerManagement.ThemeStyle.RowsStyle.Height = 29;
            DataGridViewCustomerManagement.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            DataGridViewCustomerManagement.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            DataGridViewCustomerManagement.CellEndEdit += DataGridViewCustomerManagement_CellEndEdit;
            // 
            // HoTen
            // 
            HoTen.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            HoTen.DataPropertyName = "full_name";
            HoTen.Frozen = true;
            HoTen.HeaderText = "Họ và tên";
            HoTen.MinimumWidth = 6;
            HoTen.Name = "full_name";
            HoTen.ReadOnly = true;
            HoTen.Width = 220;
            // 
            // GioiTinh
            // 
            GioiTinh.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            GioiTinh.DataPropertyName = "gender";
            GioiTinh.FillWeight = 65.33226F;
            GioiTinh.Frozen = true;
            GioiTinh.HeaderText = "Giới tính";
            GioiTinh.MinimumWidth = 6;
            GioiTinh.Name = "gender";
            GioiTinh.ReadOnly = true;
            GioiTinh.Width = 175;
            // 
            // NgaySinh
            // 
            NgaySinh.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            NgaySinh.DataPropertyName = "date_of_birth";
            NgaySinh.FillWeight = 65.33226F;
            NgaySinh.Frozen = true;
            NgaySinh.HeaderText = "Ngày sinh";
            NgaySinh.MinimumWidth = 6;
            NgaySinh.Name = "date_of_birth";
            NgaySinh.ReadOnly = true;
            NgaySinh.Width = 175;
            // 
            // SĐT
            // 
            SĐT.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            SĐT.DataPropertyName = "phone_number";
            SĐT.FillWeight = 65.33226F;
            SĐT.Frozen = true;
            SĐT.HeaderText = "Số điện thoại";
            SĐT.MinimumWidth = 6;
            SĐT.Name = "phone_number";
            SĐT.ReadOnly = true;
            SĐT.Width = 175;
            // 
            // Email
            // 
            Email.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Email.DataPropertyName = "email";
            Email.FillWeight = 65.33226F;
            Email.Frozen = true;
            Email.HeaderText = "Email";
            Email.MinimumWidth = 6;
            Email.Name = "email";
            Email.ReadOnly = true;
            Email.Width = 200;
            // 
            // DiaChi
            // 
            DiaChi.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DiaChi.DataPropertyName = "address";
            DiaChi.FillWeight = 65.33226F;
            DiaChi.HeaderText = "Địa chỉ";
            DiaChi.MinimumWidth = 6;
            DiaChi.Name = "address";
            DiaChi.ReadOnly = true;
            DiaChi.Width = 200;
            // 
            // ThoiGianTaoTK
            // 
            ThoiGianTaoTK.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            ThoiGianTaoTK.DataPropertyName = "create_date";
            ThoiGianTaoTK.FillWeight = 65.33226F;
            ThoiGianTaoTK.HeaderText = "Thời gian tạo tài khoản";
            ThoiGianTaoTK.MinimumWidth = 6;
            ThoiGianTaoTK.Name = "create_date";
            ThoiGianTaoTK.ReadOnly = true;
            ThoiGianTaoTK.Width = 250;
            // 
            // FormCustomerManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(92, 124, 150);
            ClientSize = new Size(1420, 750);
            Controls.Add(DataGridViewCustomerManagement);
            Controls.Add(btnXoa);
            Controls.Add(btnChinhSua);
            Controls.Add(btnXuatFile);
            Controls.Add(btnTim);
            Controls.Add(txtTimKiem);
            Name = "FormCustomerManagement";
            Text = "Quản lý khách hàng";
            ((System.ComponentModel.ISupportInitialize)DataGridViewCustomerManagement).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button btnTim;
        private Guna.UI2.WinForms.Guna2Button btnXuatFile;
        private Guna.UI2.WinForms.Guna2Button btnChinhSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Guna.UI2.WinForms.Guna2DataGridView DataGridViewCustomerManagement;
        private DataGridViewTextBoxColumn HoTen;
        private DataGridViewTextBoxColumn GioiTinh;
        private DataGridViewTextBoxColumn NgaySinh;
        private DataGridViewTextBoxColumn SĐT;
        private DataGridViewTextBoxColumn Email;
        private DataGridViewTextBoxColumn DiaChi;
        private DataGridViewTextBoxColumn ThoiGianTaoTK;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
    }
}
