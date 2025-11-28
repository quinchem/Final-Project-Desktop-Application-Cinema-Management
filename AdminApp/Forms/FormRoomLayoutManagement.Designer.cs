namespace AdminApp
{
    partial class FormRoomLayoutManagement
    {
            private System.ComponentModel.IContainer components = null;
            private System.Windows.Forms.Label lblTitle;
            private System.Windows.Forms.Label lblLegendNormal;
            private System.Windows.Forms.Label lblLegendVIP;
            private System.Windows.Forms.Label lblLegendMaintenance;
            private System.Windows.Forms.RadioButton rdoThuong;
            private System.Windows.Forms.RadioButton rdoVip;
            private System.Windows.Forms.RadioButton rdoBinhThuong;
            private System.Windows.Forms.RadioButton rdoBaoTri;

            // Dictionary để lưu các button ghế
            private System.Collections.Generic.Dictionary<string, System.Windows.Forms.Button> seatButtons =
                new System.Collections.Generic.Dictionary<string, System.Windows.Forms.Button>();

            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRoomLayoutManagement));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges33 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges34 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges35 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges36 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges37 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges38 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges39 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges40 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges41 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges42 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges43 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges44 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges45 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges46 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblTitle = new Label();
            guna2Button94 = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            guna2Button93 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button92 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button91 = new Guna.UI2.WinForms.Guna2Button();
            lblLegendNormal = new Label();
            lblLegendVIP = new Label();
            lblLegendMaintenance = new Label();
            rdoThuong = new RadioButton();
            rdoVip = new RadioButton();
            rdoBinhThuong = new RadioButton();
            rdoBaoTri = new RadioButton();
            guna2CustomGradientPanel3 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            btnPhong5 = new Guna.UI2.WinForms.Guna2Button();
            btnPhong4 = new Guna.UI2.WinForms.Guna2Button();
            btnPhong3 = new Guna.UI2.WinForms.Guna2Button();
            btnPhong2 = new Guna.UI2.WinForms.Guna2Button();
            btnPhong1 = new Guna.UI2.WinForms.Guna2Button();
            btnLuu = new Guna.UI2.WinForms.Guna2Button();
            panelRoomLayout = new Guna.UI2.WinForms.Guna2Panel();
            cmsSeat = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            cmsEditSeat = new ToolStripMenuItem();
            cmsChangeType = new ToolStripMenuItem();
            cmsDeleteSeat = new ToolStripMenuItem();
            cmsDisableSeat = new ToolStripMenuItem();
            btnFormat = new Guna.UI2.WinForms.Guna2Button();
            btnXoa = new Guna.UI2.WinForms.Guna2Button();
            btnChinhSua = new Guna.UI2.WinForms.Guna2Button();
            lblMaGhe = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnThemHang = new Guna.UI2.WinForms.Guna2Button();
            btnThemGhe = new Guna.UI2.WinForms.Guna2Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            btnQuayLai = new Guna.UI2.WinForms.Guna2Button();
            txtMaGhe = new Guna.UI2.WinForms.Guna2TextBox();
            btnXoaHang = new Guna.UI2.WinForms.Guna2Button();
            panelRoomTabs = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            lblPhong = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtDinhDang = new Guna.UI2.WinForms.Guna2TextBox();
            lblSoGhe = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtSoGhe = new Guna.UI2.WinForms.Guna2TextBox();
            guna2CustomGradientPanel3.SuspendLayout();
            cmsSeat.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            panelRoomTabs.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(255, 128, 0);
            lblTitle.Location = new Point(118, 108);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(214, 38);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "SƠ ĐỒ GHẾ";
            // 
            // guna2Button94
            // 
            guna2Button94.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button94.CustomizableEdges = customizableEdges1;
            guna2Button94.DisabledState.BorderColor = Color.DarkGray;
            guna2Button94.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button94.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button94.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button94.FillColor = Color.FromArgb(35, 150, 62);
            guna2Button94.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button94.ForeColor = Color.Gray;
            guna2Button94.Location = new Point(583, 17);
            guna2Button94.Name = "guna2Button94";
            guna2Button94.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button94.Size = new Size(50, 50);
            guna2Button94.TabIndex = 148;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 10F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(638, 28);
            label1.Name = "label1";
            label1.Size = new Size(137, 31);
            label1.TabIndex = 149;
            label1.Text = "Ghế đang chọn";
            // 
            // guna2Button93
            // 
            guna2Button93.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button93.CustomizableEdges = customizableEdges3;
            guna2Button93.DisabledState.BorderColor = Color.DarkGray;
            guna2Button93.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button93.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button93.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button93.FillColor = Color.DimGray;
            guna2Button93.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button93.ForeColor = Color.Gray;
            guna2Button93.Location = new Point(387, 18);
            guna2Button93.Name = "guna2Button93";
            guna2Button93.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button93.Size = new Size(50, 50);
            guna2Button93.TabIndex = 148;
            // 
            // guna2Button92
            // 
            guna2Button92.BorderColor = Color.FromArgb(255, 193, 7);
            guna2Button92.BorderThickness = 5;
            guna2Button92.CustomizableEdges = customizableEdges5;
            guna2Button92.DisabledState.BorderColor = Color.DarkGray;
            guna2Button92.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button92.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button92.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button92.FillColor = Color.WhiteSmoke;
            guna2Button92.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button92.ForeColor = Color.Gray;
            guna2Button92.Location = new Point(219, 18);
            guna2Button92.Name = "guna2Button92";
            guna2Button92.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Button92.Size = new Size(50, 50);
            guna2Button92.TabIndex = 148;
            // 
            // guna2Button91
            // 
            guna2Button91.BorderColor = Color.DimGray;
            guna2Button91.BorderThickness = 5;
            guna2Button91.CustomizableEdges = customizableEdges7;
            guna2Button91.DisabledState.BorderColor = Color.DarkGray;
            guna2Button91.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button91.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button91.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button91.FillColor = Color.WhiteSmoke;
            guna2Button91.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button91.ForeColor = Color.Gray;
            guna2Button91.Location = new Point(28, 17);
            guna2Button91.Name = "guna2Button91";
            guna2Button91.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2Button91.Size = new Size(50, 50);
            guna2Button91.TabIndex = 148;
            // 
            // lblLegendNormal
            // 
            lblLegendNormal.Font = new Font("Segoe UI", 10F);
            lblLegendNormal.ForeColor = Color.White;
            lblLegendNormal.Location = new Point(82, 28);
            lblLegendNormal.Name = "lblLegendNormal";
            lblLegendNormal.Size = new Size(114, 31);
            lblLegendNormal.TabIndex = 1;
            lblLegendNormal.Text = "Ghế thường";
            // 
            // lblLegendVIP
            // 
            lblLegendVIP.Font = new Font("Segoe UI", 10F);
            lblLegendVIP.ForeColor = Color.White;
            lblLegendVIP.Location = new Point(276, 28);
            lblLegendVIP.Name = "lblLegendVIP";
            lblLegendVIP.Size = new Size(80, 31);
            lblLegendVIP.TabIndex = 3;
            lblLegendVIP.Text = "Ghế VIP";
            // 
            // lblLegendMaintenance
            // 
            lblLegendMaintenance.Font = new Font("Segoe UI", 10F);
            lblLegendMaintenance.ForeColor = Color.White;
            lblLegendMaintenance.Location = new Point(443, 28);
            lblLegendMaintenance.Name = "lblLegendMaintenance";
            lblLegendMaintenance.Size = new Size(100, 31);
            lblLegendMaintenance.TabIndex = 5;
            lblLegendMaintenance.Text = "Ghế bảo trì";
            // 
            // rdoThuong
            // 
            rdoThuong.Checked = true;
            rdoThuong.Font = new Font("Segoe UI", 10F);
            rdoThuong.ForeColor = Color.White;
            rdoThuong.Location = new Point(18, 31);
            rdoThuong.Margin = new Padding(3, 4, 3, 4);
            rdoThuong.Name = "rdoThuong";
            rdoThuong.Size = new Size(148, 38);
            rdoThuong.TabIndex = 1;
            rdoThuong.TabStop = true;
            rdoThuong.Text = "   Ghế thường";
            rdoThuong.CheckedChanged += rdoThuong_CheckedChanged;
            // 
            // rdoVip
            // 
            rdoVip.Font = new Font("Segoe UI", 10F);
            rdoVip.ForeColor = Color.White;
            rdoVip.Location = new Point(18, 74);
            rdoVip.Margin = new Padding(3, 4, 3, 4);
            rdoVip.Name = "rdoVip";
            rdoVip.Size = new Size(162, 38);
            rdoVip.TabIndex = 2;
            rdoVip.Text = "   Ghế VIP";
            rdoVip.CheckedChanged += rdoVip_CheckedChanged;
            // 
            // rdoBinhThuong
            // 
            rdoBinhThuong.Checked = true;
            rdoBinhThuong.Font = new Font("Segoe UI", 10F);
            rdoBinhThuong.ForeColor = Color.White;
            rdoBinhThuong.Location = new Point(18, 34);
            rdoBinhThuong.Margin = new Padding(3, 4, 3, 4);
            rdoBinhThuong.Name = "rdoBinhThuong";
            rdoBinhThuong.Size = new Size(162, 38);
            rdoBinhThuong.TabIndex = 1;
            rdoBinhThuong.TabStop = true;
            rdoBinhThuong.Text = "   Bình thường";
            rdoBinhThuong.CheckedChanged += rdoBinhThuong_CheckedChanged;
            // 
            // rdoBaoTri
            // 
            rdoBaoTri.Font = new Font("Segoe UI", 10F);
            rdoBaoTri.ForeColor = Color.White;
            rdoBaoTri.Location = new Point(19, 73);
            rdoBaoTri.Margin = new Padding(3, 4, 3, 4);
            rdoBaoTri.Name = "rdoBaoTri";
            rdoBaoTri.Size = new Size(148, 38);
            rdoBaoTri.TabIndex = 2;
            rdoBaoTri.Text = "   Bảo trì";
            rdoBaoTri.CheckedChanged += rdoBaoTri_CheckedChanged;
            // 
            // guna2CustomGradientPanel3
            // 
            guna2CustomGradientPanel3.BorderColor = Color.Gainsboro;
            guna2CustomGradientPanel3.BorderRadius = 2;
            guna2CustomGradientPanel3.BorderThickness = 4;
            guna2CustomGradientPanel3.Controls.Add(lblLegendNormal);
            guna2CustomGradientPanel3.Controls.Add(guna2Button94);
            guna2CustomGradientPanel3.Controls.Add(guna2Button91);
            guna2CustomGradientPanel3.Controls.Add(lblLegendVIP);
            guna2CustomGradientPanel3.Controls.Add(label1);
            guna2CustomGradientPanel3.Controls.Add(guna2Button92);
            guna2CustomGradientPanel3.Controls.Add(lblLegendMaintenance);
            guna2CustomGradientPanel3.Controls.Add(guna2Button93);
            guna2CustomGradientPanel3.CustomizableEdges = customizableEdges9;
            guna2CustomGradientPanel3.FillColor = Color.Transparent;
            guna2CustomGradientPanel3.FillColor2 = Color.Transparent;
            guna2CustomGradientPanel3.FillColor3 = Color.Transparent;
            guna2CustomGradientPanel3.FillColor4 = Color.Transparent;
            guna2CustomGradientPanel3.Location = new Point(157, 764);
            guna2CustomGradientPanel3.Name = "guna2CustomGradientPanel3";
            guna2CustomGradientPanel3.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2CustomGradientPanel3.Size = new Size(795, 84);
            guna2CustomGradientPanel3.TabIndex = 149;
            // 
            // btnPhong5
            // 
            btnPhong5.BorderRadius = 5;
            btnPhong5.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btnPhong5.CheckedState.BorderColor = Color.Transparent;
            btnPhong5.CheckedState.FillColor = Color.FromArgb(48, 76, 105);
            btnPhong5.CheckedState.ForeColor = Color.White;
            btnPhong5.CustomizableEdges = customizableEdges11;
            btnPhong5.DisabledState.BorderColor = Color.DarkGray;
            btnPhong5.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPhong5.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPhong5.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPhong5.FillColor = Color.DarkGray;
            btnPhong5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPhong5.ForeColor = Color.White;
            btnPhong5.HoverState.FillColor = Color.FromArgb(181, 207, 235);
            btnPhong5.Location = new Point(638, 38);
            btnPhong5.Name = "btnPhong5";
            btnPhong5.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnPhong5.Size = new Size(104, 37);
            btnPhong5.TabIndex = 155;
            btnPhong5.Text = "Phòng 5";
            btnPhong5.CheckedChanged += btnPhong5_CheckedChanged;
            // 
            // btnPhong4
            // 
            btnPhong4.BorderRadius = 5;
            btnPhong4.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btnPhong4.CheckedState.BorderColor = Color.Transparent;
            btnPhong4.CheckedState.FillColor = Color.FromArgb(48, 76, 105);
            btnPhong4.CheckedState.ForeColor = Color.White;
            btnPhong4.CustomizableEdges = customizableEdges13;
            btnPhong4.DisabledState.BorderColor = Color.DarkGray;
            btnPhong4.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPhong4.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPhong4.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPhong4.FillColor = Color.DarkGray;
            btnPhong4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPhong4.ForeColor = Color.White;
            btnPhong4.HoverState.FillColor = Color.FromArgb(181, 207, 235);
            btnPhong4.Location = new Point(501, 38);
            btnPhong4.Name = "btnPhong4";
            btnPhong4.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnPhong4.Size = new Size(108, 37);
            btnPhong4.TabIndex = 154;
            btnPhong4.Text = "Phòng 4";
            btnPhong4.CheckedChanged += btnPhong4_CheckedChanged;
            // 
            // btnPhong3
            // 
            btnPhong3.BorderRadius = 5;
            btnPhong3.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btnPhong3.CheckedState.BorderColor = Color.Transparent;
            btnPhong3.CheckedState.FillColor = Color.FromArgb(48, 76, 105);
            btnPhong3.CheckedState.ForeColor = Color.White;
            btnPhong3.CustomizableEdges = customizableEdges15;
            btnPhong3.DisabledState.BorderColor = Color.DarkGray;
            btnPhong3.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPhong3.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPhong3.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPhong3.FillColor = Color.DarkGray;
            btnPhong3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPhong3.ForeColor = Color.White;
            btnPhong3.HoverState.FillColor = Color.FromArgb(181, 207, 235);
            btnPhong3.Location = new Point(365, 38);
            btnPhong3.Name = "btnPhong3";
            btnPhong3.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnPhong3.Size = new Size(108, 37);
            btnPhong3.TabIndex = 153;
            btnPhong3.Text = "Phòng 3";
            btnPhong3.CheckedChanged += btnPhong3_CheckedChanged;
            // 
            // btnPhong2
            // 
            btnPhong2.BorderRadius = 5;
            btnPhong2.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btnPhong2.CheckedState.BorderColor = Color.Transparent;
            btnPhong2.CheckedState.FillColor = Color.FromArgb(48, 76, 105);
            btnPhong2.CheckedState.ForeColor = Color.White;
            btnPhong2.CustomizableEdges = customizableEdges17;
            btnPhong2.DisabledState.BorderColor = Color.DarkGray;
            btnPhong2.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPhong2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPhong2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPhong2.FillColor = Color.DarkGray;
            btnPhong2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPhong2.ForeColor = Color.White;
            btnPhong2.HoverState.FillColor = Color.FromArgb(181, 207, 235);
            btnPhong2.Location = new Point(231, 38);
            btnPhong2.Name = "btnPhong2";
            btnPhong2.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnPhong2.Size = new Size(109, 37);
            btnPhong2.TabIndex = 152;
            btnPhong2.Text = "Phòng 2";
            btnPhong2.CheckedChanged += btnPhong2_CheckedChanged;
            // 
            // btnPhong1
            // 
            btnPhong1.BackColor = Color.Transparent;
            btnPhong1.BorderRadius = 5;
            btnPhong1.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btnPhong1.Checked = true;
            btnPhong1.CheckedState.BorderColor = Color.Transparent;
            btnPhong1.CheckedState.FillColor = Color.FromArgb(48, 76, 105);
            btnPhong1.CheckedState.ForeColor = Color.White;
            btnPhong1.CustomizableEdges = customizableEdges19;
            btnPhong1.DisabledState.BorderColor = Color.DarkGray;
            btnPhong1.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPhong1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPhong1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPhong1.FillColor = Color.DarkGray;
            btnPhong1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPhong1.ForeColor = Color.White;
            btnPhong1.HoverState.FillColor = Color.FromArgb(181, 207, 235);
            btnPhong1.Location = new Point(98, 38);
            btnPhong1.Name = "btnPhong1";
            btnPhong1.ShadowDecoration.CustomizableEdges = customizableEdges20;
            btnPhong1.Size = new Size(107, 37);
            btnPhong1.TabIndex = 151;
            btnPhong1.Text = "Phòng 1";
            btnPhong1.CheckedChanged += btnPhong1_CheckedChanged;
            // 
            // btnLuu
            // 
            btnLuu.BorderRadius = 5;
            btnLuu.CustomizableEdges = customizableEdges21;
            btnLuu.DisabledState.BorderColor = Color.DarkGray;
            btnLuu.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLuu.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLuu.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLuu.FillColor = Color.FromArgb(255, 128, 0);
            btnLuu.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLuu.ForeColor = Color.White;
            btnLuu.Image = (Image)resources.GetObject("btnLuu.Image");
            btnLuu.Location = new Point(1097, 759);
            btnLuu.Name = "btnLuu";
            btnLuu.ShadowDecoration.CustomizableEdges = customizableEdges22;
            btnLuu.Size = new Size(119, 47);
            btnLuu.TabIndex = 157;
            btnLuu.Text = "Lưu";
            btnLuu.Click += btnLuu_Click;
            // 
            // panelRoomLayout
            // 
            panelRoomLayout.CustomizableEdges = customizableEdges23;
            panelRoomLayout.Location = new Point(38, 157);
            panelRoomLayout.Name = "panelRoomLayout";
            panelRoomLayout.ShadowDecoration.CustomizableEdges = customizableEdges24;
            panelRoomLayout.Size = new Size(1012, 585);
            panelRoomLayout.TabIndex = 159;
            // 
            // cmsSeat
            // 
            cmsSeat.BackColor = Color.NavajoWhite;
            cmsSeat.ImageScalingSize = new Size(20, 20);
            cmsSeat.Items.AddRange(new ToolStripItem[] { cmsEditSeat, cmsChangeType, cmsDeleteSeat, cmsDisableSeat });
            cmsSeat.Name = "guna2ContextMenuStrip1";
            cmsSeat.RenderStyle.ArrowColor = Color.FromArgb(151, 143, 255);
            cmsSeat.RenderStyle.BorderColor = Color.Gainsboro;
            cmsSeat.RenderStyle.ColorTable = null;
            cmsSeat.RenderStyle.RoundedEdges = true;
            cmsSeat.RenderStyle.SelectionArrowColor = Color.White;
            cmsSeat.RenderStyle.SelectionBackColor = Color.FromArgb(100, 88, 255);
            cmsSeat.RenderStyle.SelectionForeColor = Color.White;
            cmsSeat.RenderStyle.SeparatorColor = Color.Gainsboro;
            cmsSeat.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            cmsSeat.Size = new Size(172, 100);
            // 
            // cmsEditSeat
            // 
            cmsEditSeat.Name = "cmsEditSeat";
            cmsEditSeat.Size = new Size(171, 24);
            cmsEditSeat.Text = "Chỉnh sửa ghế";
            // 
            // cmsChangeType
            // 
            cmsChangeType.BackColor = Color.NavajoWhite;
            cmsChangeType.Name = "cmsChangeType";
            cmsChangeType.Size = new Size(171, 24);
            cmsChangeType.Text = "Đổi loại ghế";
            // 
            // cmsDeleteSeat
            // 
            cmsDeleteSeat.Name = "cmsDeleteSeat";
            cmsDeleteSeat.Size = new Size(171, 24);
            cmsDeleteSeat.Text = "Xoá ghế";
            // 
            // cmsDisableSeat
            // 
            cmsDisableSeat.Name = "cmsDisableSeat";
            cmsDisableSeat.Size = new Size(171, 24);
            cmsDisableSeat.Text = "Khoá ghế";
            // 
            // btnFormat
            // 
            btnFormat.BorderRadius = 5;
            btnFormat.CustomizableEdges = customizableEdges25;
            btnFormat.DisabledState.BorderColor = Color.DarkGray;
            btnFormat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnFormat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnFormat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnFormat.FillColor = Color.FromArgb(242, 182, 41);
            btnFormat.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFormat.ForeColor = Color.White;
            btnFormat.Image = Properties.Resources.simple_data_format;
            btnFormat.ImageSize = new Size(25, 25);
            btnFormat.Location = new Point(1241, 759);
            btnFormat.Name = "btnFormat";
            btnFormat.ShadowDecoration.CustomizableEdges = customizableEdges26;
            btnFormat.Size = new Size(119, 45);
            btnFormat.TabIndex = 160;
            btnFormat.Text = "Format";
            btnFormat.Click += btnRefresh_Click;
            // 
            // btnXoa
            // 
            btnXoa.BorderRadius = 5;
            btnXoa.CustomizableEdges = customizableEdges27;
            btnXoa.DisabledState.BorderColor = Color.DarkGray;
            btnXoa.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXoa.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXoa.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXoa.FillColor = Color.FromArgb(227, 115, 98);
            btnXoa.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.Image = Properties.Resources.trash;
            btnXoa.Location = new Point(1097, 686);
            btnXoa.Name = "btnXoa";
            btnXoa.ShadowDecoration.CustomizableEdges = customizableEdges28;
            btnXoa.Size = new Size(119, 46);
            btnXoa.TabIndex = 161;
            btnXoa.Text = "Xóa ghế";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnChinhSua
            // 
            btnChinhSua.BackColor = Color.Transparent;
            btnChinhSua.BorderRadius = 5;
            btnChinhSua.CustomizableEdges = customizableEdges29;
            btnChinhSua.DisabledState.BorderColor = Color.DarkGray;
            btnChinhSua.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChinhSua.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChinhSua.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChinhSua.FillColor = Color.FromArgb(34, 55, 110);
            btnChinhSua.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChinhSua.ForeColor = Color.White;
            btnChinhSua.Image = (Image)resources.GetObject("btnChinhSua.Image");
            btnChinhSua.Location = new Point(1097, 543);
            btnChinhSua.Name = "btnChinhSua";
            btnChinhSua.ShadowDecoration.CustomizableEdges = customizableEdges30;
            btnChinhSua.Size = new Size(119, 46);
            btnChinhSua.TabIndex = 162;
            btnChinhSua.Text = "Chỉnh sửa";
            btnChinhSua.Click += guna2Button2_Click;
            // 
            // lblMaGhe
            // 
            lblMaGhe.BackColor = Color.Transparent;
            lblMaGhe.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMaGhe.ForeColor = Color.FromArgb(12, 41, 64);
            lblMaGhe.Location = new Point(1145, 163);
            lblMaGhe.Name = "lblMaGhe";
            lblMaGhe.Size = new Size(65, 27);
            lblMaGhe.TabIndex = 164;
            lblMaGhe.Text = "Mã ghế";
            // 
            // btnThemHang
            // 
            btnThemHang.BorderRadius = 5;
            btnThemHang.CustomizableEdges = customizableEdges31;
            btnThemHang.DisabledState.BorderColor = Color.DarkGray;
            btnThemHang.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThemHang.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThemHang.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThemHang.FillColor = Color.FromArgb(80, 179, 61);
            btnThemHang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThemHang.ForeColor = Color.White;
            btnThemHang.Image = Properties.Resources.add;
            btnThemHang.Location = new Point(1241, 613);
            btnThemHang.Name = "btnThemHang";
            btnThemHang.ShadowDecoration.CustomizableEdges = customizableEdges32;
            btnThemHang.Size = new Size(119, 46);
            btnThemHang.TabIndex = 165;
            btnThemHang.Text = "Thêm hàng";
            btnThemHang.Click += btnThemHang_Click;
            // 
            // btnThemGhe
            // 
            btnThemGhe.BorderRadius = 5;
            btnThemGhe.CustomizableEdges = customizableEdges33;
            btnThemGhe.DisabledState.BorderColor = Color.DarkGray;
            btnThemGhe.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThemGhe.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThemGhe.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThemGhe.FillColor = Color.FromArgb(125, 186, 114);
            btnThemGhe.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThemGhe.ForeColor = Color.White;
            btnThemGhe.Image = Properties.Resources.add;
            btnThemGhe.Location = new Point(1097, 613);
            btnThemGhe.Name = "btnThemGhe";
            btnThemGhe.ShadowDecoration.CustomizableEdges = customizableEdges34;
            btnThemGhe.Size = new Size(119, 46);
            btnThemGhe.TabIndex = 166;
            btnThemGhe.Text = "Thêm ghế";
            btnThemGhe.Click += btnThemGhe_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rdoVip);
            groupBox1.Controls.Add(rdoThuong);
            groupBox1.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.FromArgb(12, 41, 64);
            groupBox1.Location = new Point(1135, 226);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(187, 125);
            groupBox1.TabIndex = 167;
            groupBox1.TabStop = false;
            groupBox1.Text = "Loại ghế";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rdoBaoTri);
            groupBox2.Controls.Add(rdoBinhThuong);
            groupBox2.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = Color.FromArgb(12, 41, 64);
            groupBox2.Location = new Point(1135, 382);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(187, 125);
            groupBox2.TabIndex = 168;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tình trạng";
            // 
            // btnQuayLai
            // 
            btnQuayLai.BorderRadius = 5;
            btnQuayLai.CustomizableEdges = customizableEdges35;
            btnQuayLai.DisabledState.BorderColor = Color.DarkGray;
            btnQuayLai.DisabledState.CustomBorderColor = Color.DarkGray;
            btnQuayLai.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnQuayLai.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnQuayLai.FillColor = Color.FromArgb(79, 194, 158);
            btnQuayLai.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnQuayLai.ForeColor = Color.White;
            btnQuayLai.Image = Properties.Resources.refresh;
            btnQuayLai.ImageSize = new Size(30, 30);
            btnQuayLai.Location = new Point(1241, 544);
            btnQuayLai.Name = "btnQuayLai";
            btnQuayLai.ShadowDecoration.CustomizableEdges = customizableEdges36;
            btnQuayLai.Size = new Size(119, 45);
            btnQuayLai.TabIndex = 169;
            btnQuayLai.Text = "Quay lại";
            btnQuayLai.Click += btnQuayLai_Click;
            // 
            // txtMaGhe
            // 
            txtMaGhe.BorderRadius = 4;
            txtMaGhe.CustomizableEdges = customizableEdges37;
            txtMaGhe.DefaultText = "";
            txtMaGhe.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtMaGhe.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtMaGhe.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtMaGhe.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtMaGhe.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtMaGhe.Font = new Font("Segoe UI", 11F);
            txtMaGhe.ForeColor = Color.Black;
            txtMaGhe.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtMaGhe.Location = new Point(1225, 154);
            txtMaGhe.Margin = new Padding(3, 4, 3, 4);
            txtMaGhe.Name = "txtMaGhe";
            txtMaGhe.PlaceholderText = "";
            txtMaGhe.SelectedText = "";
            txtMaGhe.ShadowDecoration.CustomizableEdges = customizableEdges38;
            txtMaGhe.Size = new Size(83, 40);
            txtMaGhe.TabIndex = 163;
            txtMaGhe.TextChanged += txtMaGhe_TextChanged;
            // 
            // btnXoaHang
            // 
            btnXoaHang.BorderRadius = 5;
            btnXoaHang.CustomizableEdges = customizableEdges39;
            btnXoaHang.DisabledState.BorderColor = Color.DarkGray;
            btnXoaHang.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXoaHang.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXoaHang.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXoaHang.FillColor = Color.FromArgb(217, 89, 69);
            btnXoaHang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnXoaHang.ForeColor = Color.White;
            btnXoaHang.Image = Properties.Resources.trash;
            btnXoaHang.Location = new Point(1241, 686);
            btnXoaHang.Name = "btnXoaHang";
            btnXoaHang.ShadowDecoration.CustomizableEdges = customizableEdges40;
            btnXoaHang.Size = new Size(119, 46);
            btnXoaHang.TabIndex = 170;
            btnXoaHang.Text = "Xóa hàng";
            btnXoaHang.Click += btnXoaHang_Click;
            // 
            // panelRoomTabs
            // 
            panelRoomTabs.BackColor = Color.Transparent;
            panelRoomTabs.Controls.Add(btnPhong1);
            panelRoomTabs.Controls.Add(btnPhong2);
            panelRoomTabs.Controls.Add(btnPhong3);
            panelRoomTabs.Controls.Add(btnPhong4);
            panelRoomTabs.Controls.Add(btnPhong5);
            panelRoomTabs.CustomizableEdges = customizableEdges41;
            panelRoomTabs.Dock = DockStyle.Top;
            panelRoomTabs.FillColor = Color.Transparent;
            panelRoomTabs.FillColor2 = Color.Transparent;
            panelRoomTabs.FillColor3 = Color.Transparent;
            panelRoomTabs.FillColor4 = Color.Transparent;
            panelRoomTabs.Location = new Point(0, 0);
            panelRoomTabs.Name = "panelRoomTabs";
            panelRoomTabs.ShadowDecoration.CustomizableEdges = customizableEdges42;
            panelRoomTabs.Size = new Size(1420, 98);
            panelRoomTabs.TabIndex = 171;
            // 
            // lblPhong
            // 
            lblPhong.BackColor = Color.Transparent;
            lblPhong.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPhong.ForeColor = Color.FromArgb(12, 41, 64);
            lblPhong.Location = new Point(740, 119);
            lblPhong.Name = "lblPhong";
            lblPhong.Size = new Size(148, 27);
            lblPhong.TabIndex = 172;
            lblPhong.Text = "Định dạng phòng\r\n";
            // 
            // txtDinhDang
            // 
            txtDinhDang.BorderRadius = 4;
            txtDinhDang.CustomizableEdges = customizableEdges43;
            txtDinhDang.DefaultText = "";
            txtDinhDang.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtDinhDang.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtDinhDang.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtDinhDang.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtDinhDang.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtDinhDang.Font = new Font("Segoe UI", 11F);
            txtDinhDang.ForeColor = Color.Black;
            txtDinhDang.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtDinhDang.Location = new Point(894, 108);
            txtDinhDang.Margin = new Padding(3, 4, 3, 4);
            txtDinhDang.Name = "txtDinhDang";
            txtDinhDang.PlaceholderText = "";
            txtDinhDang.RightToLeft = RightToLeft.No;
            txtDinhDang.SelectedText = "";
            txtDinhDang.ShadowDecoration.CustomizableEdges = customizableEdges44;
            txtDinhDang.Size = new Size(66, 40);
            txtDinhDang.TabIndex = 173;
            // 
            // lblSoGhe
            // 
            lblSoGhe.BackColor = Color.Transparent;
            lblSoGhe.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSoGhe.ForeColor = Color.FromArgb(12, 41, 64);
            lblSoGhe.Location = new Point(464, 119);
            lblSoGhe.Name = "lblSoGhe";
            lblSoGhe.Size = new Size(148, 27);
            lblSoGhe.TabIndex = 174;
            lblSoGhe.Text = "Định dạng phòng\r\n";
            // 
            // txtSoGhe
            // 
            txtSoGhe.BorderRadius = 4;
            txtSoGhe.CustomizableEdges = customizableEdges45;
            txtSoGhe.DefaultText = "";
            txtSoGhe.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSoGhe.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSoGhe.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSoGhe.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSoGhe.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoGhe.Font = new Font("Segoe UI", 11F);
            txtSoGhe.ForeColor = Color.Black;
            txtSoGhe.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoGhe.Location = new Point(618, 108);
            txtSoGhe.Margin = new Padding(3, 4, 3, 4);
            txtSoGhe.Name = "txtSoGhe";
            txtSoGhe.PlaceholderText = "";
            txtSoGhe.ReadOnly = true;
            txtSoGhe.RightToLeft = RightToLeft.No;
            txtSoGhe.SelectedText = "";
            txtSoGhe.ShadowDecoration.CustomizableEdges = customizableEdges46;
            txtSoGhe.Size = new Size(66, 40);
            txtSoGhe.TabIndex = 175;
            // 
            // FormRoomLayoutManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(800, 800);
            BackColor = Color.FromArgb(88, 115, 140);
            ClientSize = new Size(1420, 938);
            Controls.Add(txtSoGhe);
            Controls.Add(lblSoGhe);
            Controls.Add(txtDinhDang);
            Controls.Add(lblPhong);
            Controls.Add(btnXoaHang);
            Controls.Add(btnQuayLai);
            Controls.Add(groupBox2);
            Controls.Add(btnThemGhe);
            Controls.Add(btnThemHang);
            Controls.Add(lblMaGhe);
            Controls.Add(txtMaGhe);
            Controls.Add(btnChinhSua);
            Controls.Add(btnXoa);
            Controls.Add(btnFormat);
            Controls.Add(btnLuu);
            Controls.Add(guna2CustomGradientPanel3);
            Controls.Add(lblTitle);
            Controls.Add(panelRoomLayout);
            Controls.Add(groupBox1);
            Controls.Add(panelRoomTabs);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormRoomLayoutManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản Lý Ghế Rạp Phim";
            Load += FormRoomLayoutManagement_Load;
            guna2CustomGradientPanel3.ResumeLayout(false);
            cmsSeat.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            panelRoomTabs.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }


        private Button CreateSingleSeat(string seatId, int x, int y, int size, System.Drawing.Color backColor, System.Drawing.Color foreColor)
            {
                Button btnSeat = new Button();
                btnSeat.Name = "btn" + seatId;
                btnSeat.Location = new System.Drawing.Point(x, y);
                btnSeat.Size = new System.Drawing.Size(size, size);
                btnSeat.Text = seatId;
                btnSeat.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                btnSeat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnSeat.Cursor = System.Windows.Forms.Cursors.Hand;
                btnSeat.Tag = seatId;
                btnSeat.BackColor = backColor;
                btnSeat.ForeColor = foreColor;
                btnSeat.FlatAppearance.BorderSize = 1;
                btnSeat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(100, 100, 100);
                btnSeat.Click += new System.EventHandler(this.SeatButton_Click);
                return btnSeat;
            }

            private void SetMaintenanceSeats(string row, int fromCol, int toCol)
            {
                Button btn10 = seatButtons[row + "10"];
                btn10.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn10.ForeColor = System.Drawing.Color.White;

                Button btn11 = seatButtons[row + "11"];
                btn11.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn11.ForeColor = System.Drawing.Color.White;

                Button btn12 = seatButtons[row + "12"];
                btn12.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn12.ForeColor = System.Drawing.Color.White;

                Button btn13 = seatButtons[row + "13"];
                btn13.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn13.ForeColor = System.Drawing.Color.White;

                Button btn14 = seatButtons[row + "14"];
                btn14.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn14.ForeColor = System.Drawing.Color.White;

                Button btn15 = seatButtons[row + "15"];
                btn15.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
                btn15.ForeColor = System.Drawing.Color.White;
            }

            private void SetSelectedSeat(string seatId)
            {
                Button btn = seatButtons[seatId];
                btn.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            }

            // Event handlers - Bạn cần implement trong file .cs chính
            private void SeatButton_Click(object sender, System.EventArgs e)
            {
                Button clickedSeat = sender as Button;
                string seatId = clickedSeat.Tag.ToString();
                System.Windows.Forms.MessageBox.Show("Đã click vào ghế: " + seatId);
            }


        private Guna.UI2.WinForms.Guna2Button guna2Button92;
        private Guna.UI2.WinForms.Guna2Button guna2Button91;
        private Guna.UI2.WinForms.Guna2Button guna2Button93;
        private Guna.UI2.WinForms.Guna2Button guna2Button94;
        private Label label1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel3;
        private Guna.UI2.WinForms.Guna2Button btnPhong5;
        private Guna.UI2.WinForms.Guna2Button btnPhong4;
        private Guna.UI2.WinForms.Guna2Button btnPhong3;
        private Guna.UI2.WinForms.Guna2Button btnPhong2;
        private Guna.UI2.WinForms.Guna2Button btnPhong1;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Panel panelRoomLayout;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip cmsSeat;
        private ToolStripMenuItem cmsEditSeat;
        private ToolStripMenuItem cmsChangeType;
        private ToolStripMenuItem cmsDeleteSeat;
        private ToolStripMenuItem cmsDisableSeat;
        private Guna.UI2.WinForms.Guna2Button btnFormat;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Guna.UI2.WinForms.Guna2Button btnChinhSua;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblMaGhe;
        private Guna.UI2.WinForms.Guna2Button btnThemHang;
        private Guna.UI2.WinForms.Guna2Button btnThemGhe;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Guna.UI2.WinForms.Guna2Button btnQuayLai;
        private Guna.UI2.WinForms.Guna2TextBox txtMaGhe;
        private Guna.UI2.WinForms.Guna2Button btnXoaHang;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelRoomTabs;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPhong;
        private Guna.UI2.WinForms.Guna2TextBox txtDinhDang;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSoGhe;
        private Guna.UI2.WinForms.Guna2TextBox txtSoGhe;
    }
    }




