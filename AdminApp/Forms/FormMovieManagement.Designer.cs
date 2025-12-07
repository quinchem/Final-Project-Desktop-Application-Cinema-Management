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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMovieManagement));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            btnTimPhim = new Guna.UI2.WinForms.Guna2Button();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            dgvMovies = new Guna.UI2.WinForms.Guna2DataGridView();
            btnXuatFile = new Guna.UI2.WinForms.Guna2Button();
            cboStatusFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            title = new DataGridViewTextBoxColumn();
            release_date = new DataGridViewTextBoxColumn();
            status = new DataGridViewTextBoxColumn();
            duration = new DataGridViewTextBoxColumn();
            colEdit = new DataGridViewImageColumn();
            colDelete = new DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)dgvMovies).BeginInit();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.BorderColor = Color.Black;
            txtSearch.BorderRadius = 10;
            txtSearch.CustomizableEdges = customizableEdges1;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FillColor = Color.LightSteelBlue;
            txtSearch.FocusedState.BorderColor = Color.Black;
            txtSearch.FocusedState.FillColor = Color.LightSteelBlue;
            txtSearch.FocusedState.ForeColor = Color.Black;
            txtSearch.FocusedState.PlaceholderForeColor = Color.Transparent;
            txtSearch.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.ForeColor = Color.FromArgb(92, 124, 150);
            txtSearch.HoverState.BorderColor = SystemColors.ActiveBorder;
            txtSearch.HoverState.ForeColor = Color.FromArgb(92, 124, 150);
            txtSearch.HoverState.PlaceholderForeColor = Color.Transparent;
            txtSearch.Location = new Point(116, 60);
            txtSearch.Margin = new Padding(3, 5, 3, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderForeColor = Color.FromArgb(92, 124, 150);
            txtSearch.PlaceholderText = "Nhập tên phim";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSearch.Size = new Size(549, 54);
            txtSearch.TabIndex = 1;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // btnTimPhim
            // 
            btnTimPhim.BorderRadius = 10;
            btnTimPhim.CustomizableEdges = customizableEdges3;
            btnTimPhim.DisabledState.BorderColor = Color.DarkGray;
            btnTimPhim.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTimPhim.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTimPhim.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTimPhim.FillColor = Color.FromArgb(254, 188, 47);
            btnTimPhim.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTimPhim.ForeColor = Color.White;
            btnTimPhim.Image = Properties.Resources.search;
            btnTimPhim.Location = new Point(671, 70);
            btnTimPhim.Name = "btnTimPhim";
            btnTimPhim.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnTimPhim.Size = new Size(98, 36);
            btnTimPhim.TabIndex = 28;
            btnTimPhim.Text = "TÌM";
            btnTimPhim.Click += BtnSearch_Click;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 10;
            guna2Button1.CustomizableEdges = customizableEdges5;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(254, 188, 47);
            guna2Button1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Image = Properties.Resources.add;
            guna2Button1.Location = new Point(1284, 61);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Button1.Size = new Size(115, 45);
            guna2Button1.TabIndex = 29;
            guna2Button1.Text = "THÊM";
            guna2Button1.Click += btnThem_Click;
            // 
            // dgvMovies
            // 
            dgvMovies.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvMovies.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvMovies.BackgroundColor = SystemColors.Window;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            dataGridViewCellStyle2.ForeColor = Color.Gray;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvMovies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvMovies.ColumnHeadersHeight = 27;
            dgvMovies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvMovies.Columns.AddRange(new DataGridViewColumn[] { title, release_date, status, duration, colEdit, colDelete });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(255, 192, 128);
            dataGridViewCellStyle6.SelectionForeColor = Color.Black;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvMovies.DefaultCellStyle = dataGridViewCellStyle6;
            dgvMovies.GridColor = Color.WhiteSmoke;
            dgvMovies.Location = new Point(116, 180);
            dgvMovies.Name = "dgvMovies";
            dgvMovies.ReadOnly = true;
            dgvMovies.RowHeadersVisible = false;
            dgvMovies.RowHeadersWidth = 51;
            dgvMovies.Size = new Size(1510, 577);
            dgvMovies.TabIndex = 30;
            dgvMovies.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvMovies.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvMovies.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvMovies.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvMovies.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvMovies.ThemeStyle.BackColor = SystemColors.Window;
            dgvMovies.ThemeStyle.GridColor = Color.WhiteSmoke;
            dgvMovies.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvMovies.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvMovies.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvMovies.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvMovies.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvMovies.ThemeStyle.HeaderStyle.Height = 27;
            dgvMovies.ThemeStyle.ReadOnly = true;
            dgvMovies.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvMovies.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMovies.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvMovies.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvMovies.ThemeStyle.RowsStyle.Height = 29;
            dgvMovies.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvMovies.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dgvMovies.CellClick += dgvMovies_CellClick;
            dgvMovies.CellDoubleClick += dgvMovie_CellDoubleClick;
            // 
            // btnXuatFile
            // 
            btnXuatFile.BorderRadius = 10;
            btnXuatFile.CustomizableEdges = customizableEdges7;
            btnXuatFile.DisabledState.BorderColor = Color.DarkGray;
            btnXuatFile.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXuatFile.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXuatFile.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXuatFile.FillColor = Color.FromArgb(254, 188, 47);
            btnXuatFile.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXuatFile.ForeColor = Color.White;
            btnXuatFile.Image = (Image)resources.GetObject("btnXuatFile.Image");
            btnXuatFile.Location = new Point(1419, 60);
            btnXuatFile.Name = "btnXuatFile";
            btnXuatFile.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnXuatFile.Size = new Size(207, 46);
            btnXuatFile.TabIndex = 32;
            btnXuatFile.Text = "XUẤT FILE EXCEL";
            btnXuatFile.Click += btnExportExcel_Click;
            // 
            // cboStatusFilter
            // 
            cboStatusFilter.BackColor = Color.Transparent;
            cboStatusFilter.BorderColor = Color.FromArgb(64, 64, 64);
            cboStatusFilter.BorderRadius = 10;
            cboStatusFilter.CustomizableEdges = customizableEdges9;
            cboStatusFilter.DrawMode = DrawMode.OwnerDrawFixed;
            cboStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatusFilter.FillColor = Color.LightSteelBlue;
            cboStatusFilter.FocusedColor = Color.FromArgb(94, 148, 255);
            cboStatusFilter.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cboStatusFilter.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboStatusFilter.ForeColor = Color.FromArgb(68, 88, 112);
            cboStatusFilter.ItemHeight = 40;
            cboStatusFilter.Location = new Point(1088, 60);
            cboStatusFilter.Name = "cboStatusFilter";
            cboStatusFilter.ShadowDecoration.CustomizableEdges = customizableEdges10;
            cboStatusFilter.Size = new Size(178, 46);
            cboStatusFilter.TabIndex = 33;
            cboStatusFilter.SelectedIndexChanged += cboStatusFilter_SelectedIndexChanged;
            // 
            // title
            // 
            title.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            title.DataPropertyName = "title";
            title.HeaderText = "Tên Phim";
            title.MinimumWidth = 6;
            title.Name = "title";
            title.ReadOnly = true;
            title.Width = 500;
            // 
            // release_date
            // 
            release_date.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            release_date.DataPropertyName = "release_date";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            release_date.DefaultCellStyle = dataGridViewCellStyle3;
            release_date.FillWeight = 65.33226F;
            release_date.HeaderText = "Ngày Chiếu";
            release_date.MinimumWidth = 6;
            release_date.Name = "release_date";
            release_date.ReadOnly = true;
            release_date.Width = 200;
            // 
            // status
            // 
            status.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            status.DataPropertyName = "status";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            status.DefaultCellStyle = dataGridViewCellStyle4;
            status.FillWeight = 65.33226F;
            status.HeaderText = "Trạng Thái";
            status.MinimumWidth = 6;
            status.Name = "status";
            status.ReadOnly = true;
            status.Width = 280;
            // 
            // duration
            // 
            duration.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            duration.DataPropertyName = "duration";
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            duration.DefaultCellStyle = dataGridViewCellStyle5;
            duration.FillWeight = 65.33226F;
            duration.HeaderText = "Thời Lượng";
            duration.MinimumWidth = 6;
            duration.Name = "duration";
            duration.ReadOnly = true;
            duration.Width = 280;
            // 
            // colEdit
            // 
            colEdit.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colEdit.FillWeight = 223.358109F;
            colEdit.HeaderText = "Chỉnh";
            colEdit.Image = Properties.Resources.pen;
            colEdit.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colEdit.MinimumWidth = 6;
            colEdit.Name = "colEdit";
            colEdit.ReadOnly = true;
            colEdit.Width = 125;
            // 
            // colDelete
            // 
            colDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDelete.FillWeight = 80.6451645F;
            colDelete.HeaderText = "Xóa";
            colDelete.Image = Properties.Resources.trash;
            colDelete.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colDelete.MinimumWidth = 6;
            colDelete.Name = "colDelete";
            colDelete.ReadOnly = true;
            colDelete.Width = 125;
            // 
            // FormMovieManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(800, 800);
            BackColor = Color.FromArgb(92, 124, 150);
            ClientSize = new Size(1902, 815);
            Controls.Add(cboStatusFilter);
            Controls.Add(btnXuatFile);
            Controls.Add(dgvMovies);
            Controls.Add(guna2Button1);
            Controls.Add(btnTimPhim);
            Controls.Add(txtSearch);
            Name = "FormMovieManagement";
            Text = "AdminQLPhim";
            Load += FormMovieManagement_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMovies).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnTimPhim;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvMovies;
        private Guna.UI2.WinForms.Guna2Button btnXuatFile;
        private Guna.UI2.WinForms.Guna2ComboBox cboStatusFilter;
        private DataGridViewTextBoxColumn title;
        private DataGridViewTextBoxColumn release_date;
        private DataGridViewTextBoxColumn status;
        private DataGridViewTextBoxColumn duration;
        private DataGridViewImageColumn colEdit;
        private DataGridViewImageColumn colDelete;
    }
}