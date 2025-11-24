using Guna.UI2.WinForms.Suite;

namespace UserApp
{
    partial class FormForgetPassword
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
            CustomizableEdges customizableEdges1 = new CustomizableEdges();
            CustomizableEdges customizableEdges2 = new CustomizableEdges();
            CustomizableEdges customizableEdges9 = new CustomizableEdges();
            CustomizableEdges customizableEdges10 = new CustomizableEdges();
            CustomizableEdges customizableEdges3 = new CustomizableEdges();
            CustomizableEdges customizableEdges4 = new CustomizableEdges();
            CustomizableEdges customizableEdges5 = new CustomizableEdges();
            CustomizableEdges customizableEdges6 = new CustomizableEdges();
            CustomizableEdges customizableEdges7 = new CustomizableEdges();
            CustomizableEdges customizableEdges8 = new CustomizableEdges();
            panelQuenMK = new Panel();
            btnDangNhap = new Guna.UI2.WinForms.Guna2Button();
            panelQMK = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            btnQuayLai = new Guna.UI2.WinForms.Guna2Button();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnGui = new Guna.UI2.WinForms.Guna2Button();
            lblyc = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblEmail = new Guna.UI2.WinForms.Guna2HtmlLabel();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            panelQuenMK.SuspendLayout();
            panelQMK.SuspendLayout();
            SuspendLayout();
            // 
            // panelQuenMK
            // 
            panelQuenMK.BackColor = Color.FromArgb(92, 124, 150);
            panelQuenMK.Controls.Add(btnDangNhap);
            panelQuenMK.Controls.Add(panelQMK);
            panelQuenMK.Dock = DockStyle.Fill;
            panelQuenMK.Location = new Point(0, 0);
            panelQuenMK.Name = "panelQuenMK";
            panelQuenMK.Size = new Size(1200, 750);
            panelQuenMK.TabIndex = 0;
            // 
            // btnDangNhap
            // 
            btnDangNhap.BorderRadius = 10;
            btnDangNhap.CustomizableEdges = customizableEdges1;
            btnDangNhap.DisabledState.BorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.FillColor = Color.FromArgb(45, 76, 101);
            btnDangNhap.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDangNhap.FillColor = Color.FromArgb(44, 84, 115);
            btnDangNhap.FocusedColor = Color.FromArgb(245, 131, 35);
            btnDangNhap.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangNhap.ForeColor = SystemColors.Window;
            btnDangNhap.Image = Properties.Resources.QuenMK;
            btnDangNhap.Location = new Point(290, 35);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnDangNhap.Size = new Size(197, 46);
            btnDangNhap.TabIndex = 8;
            btnDangNhap.Text = "QUÊN MẬT KHẨU";
            // 
            // panelQMK
            // 
            panelQMK.BackColor = Color.Transparent;
            panelQMK.BorderColor = Color.FromArgb(236, 230, 224);
            panelQMK.BorderRadius = 15;
            panelQMK.Controls.Add(btnQuayLai);
            panelQMK.Controls.Add(guna2HtmlLabel1);
            panelQMK.Controls.Add(btnGui);
            panelQMK.Controls.Add(lblyc);
            panelQMK.Controls.Add(lblEmail);
            panelQMK.Controls.Add(txtEmail);
            panelQMK.CustomizableEdges = customizableEdges9;
            panelQMK.FillColor = Color.FromArgb(236, 230, 224);
            panelQMK.FillColor2 = Color.FromArgb(236, 230, 224);
            panelQMK.FillColor3 = Color.FromArgb(236, 230, 224);
            panelQMK.FillColor4 = Color.FromArgb(236, 230, 224);
            panelQMK.Location = new Point(271, 102);
            panelQMK.Name = "panelQMK";
            panelQMK.ShadowDecoration.Color = Color.FromArgb(64, 64, 64);
            panelQMK.ShadowDecoration.CustomizableEdges = customizableEdges10;
            panelQMK.ShadowDecoration.Depth = 20;
            panelQMK.ShadowDecoration.Enabled = true;
            panelQMK.ShadowDecoration.Shadow = new Padding(1, 1, 5, 5);
            panelQMK.Size = new Size(713, 332);
            panelQMK.TabIndex = 7;
            // 
            // btnQuayLai
            // 
            btnQuayLai.BorderRadius = 8;
            btnQuayLai.CustomizableEdges = customizableEdges3;
            btnQuayLai.DisabledState.BorderColor = Color.DarkGray;
            btnQuayLai.DisabledState.CustomBorderColor = Color.DarkGray;
            btnQuayLai.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnQuayLai.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnQuayLai.FillColor = Color.FromArgb(236, 230, 224);
            btnQuayLai.FocusedColor = Color.FromArgb(245, 131, 35);
            btnQuayLai.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuayLai.ForeColor = Color.FromArgb(245, 131, 35);
            btnQuayLai.Location = new Point(46, 261);
            btnQuayLai.Name = "btnQuayLai";
            btnQuayLai.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnQuayLai.Size = new Size(270, 35);
            btnQuayLai.TabIndex = 7;
            btnQuayLai.Text = "QUAY VỀ TRANG ĐĂNG NHẬP";
            btnQuayLai.TextAlign = HorizontalAlignment.Left;
            btnQuayLai.Click += btnQuayLai_Click;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.Location = new Point(58, 261);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(3, 2);
            guna2HtmlLabel1.TabIndex = 6;
            guna2HtmlLabel1.Text = null;
            // 
            // btnGui
            // 
            btnGui.BorderRadius = 8;
            btnGui.CustomizableEdges = customizableEdges5;
            btnGui.DisabledState.BorderColor = Color.DarkGray;
            btnGui.DisabledState.CustomBorderColor = Color.DarkGray;
            btnGui.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnGui.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnGui.FillColor = Color.FromArgb(44, 84, 115);
            btnGui.FocusedColor = Color.FromArgb(245, 131, 35);
            btnGui.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGui.ForeColor = Color.White;
            btnGui.Location = new Point(558, 261);
            btnGui.Name = "btnGui";
            btnGui.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnGui.Size = new Size(91, 35);
            btnGui.TabIndex = 5;
            btnGui.Text = "GỬI";
            btnGui.Click += btnGui_Click;
            // 
            // lblyc
            // 
            lblyc.AutoSize = false;
            lblyc.BackColor = Color.Transparent;
            lblyc.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblyc.Location = new Point(58, 41);
            lblyc.Name = "lblyc";
            lblyc.Size = new Size(652, 59);
            lblyc.TabIndex = 0;
            lblyc.Text = "Vui lòng nhập địa chỉ email của bạn vào ô bên dưới.<br>Bạn sẽ nhận được một liên kết để thiết lập lại mật khẩu.";
            // 
            // lblEmail
            // 
            lblEmail.BackColor = Color.Transparent;
            lblEmail.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(58, 131);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(60, 30);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.BorderRadius = 8;
            txtEmail.CustomizableEdges = customizableEdges7;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.ForeColor = Color.FromArgb(64, 64, 64);
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(58, 169);
            txtEmail.Margin = new Padding(3, 5, 3, 5);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtEmail.Size = new Size(591, 45);
            txtEmail.TabIndex = 1;
            // 
            // FormForgetPassword
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 750);
            Controls.Add(panelQuenMK);
            Name = "FormForgetPassword";
            Text = "Quên mật khẩu";
            panelQuenMK.ResumeLayout(false);
            panelQMK.ResumeLayout(false);
            panelQMK.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelQuenMK;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelQMK;
        private Guna.UI2.WinForms.Guna2Button btnGui;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblyc;
        private Guna.UI2.WinForms.Guna2Button btnDangNhap;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Button btnQuayLai;
    }
}