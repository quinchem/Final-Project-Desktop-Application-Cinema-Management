namespace UserApp
{
    partial class FormProfile
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            btnInformation = new Guna.UI2.WinForms.Guna2Button();
            btnChangPassword = new Guna.UI2.WinForms.Guna2Button();
            btnHistory = new Guna.UI2.WinForms.Guna2Button();
            panelContent = new Guna.UI2.WinForms.Guna2Panel();
            SuspendLayout();
            // 
            // btnInformation
            // 
            btnInformation.BorderRadius = 10;
            btnInformation.CustomizableEdges = customizableEdges1;
            btnInformation.DisabledState.BorderColor = Color.DarkGray;
            btnInformation.DisabledState.CustomBorderColor = Color.DarkGray;
            btnInformation.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnInformation.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnInformation.FillColor = Color.FromArgb(44, 84, 115);
            btnInformation.FocusedColor = Color.FromArgb(245, 131, 35);
            btnInformation.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnInformation.ForeColor = Color.White;
            btnInformation.Image = Properties.Resources.ThongtinChung;
            btnInformation.Location = new Point(105, 75);
            btnInformation.Name = "btnInformation";
            btnInformation.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnInformation.Size = new Size(254, 58);
            btnInformation.TabIndex = 1;
            btnInformation.Text = "THÔNG TIN CHUNG";
            btnInformation.Click += btnInformation_Click;
            // 
            // btnChangPassword
            // 
            btnChangPassword.BorderRadius = 10;
            btnChangPassword.CustomizableEdges = customizableEdges3;
            btnChangPassword.DisabledState.BorderColor = Color.DarkGray;
            btnChangPassword.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChangPassword.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChangPassword.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChangPassword.FillColor = Color.FromArgb(44, 84, 115);
            btnChangPassword.FocusedColor = Color.FromArgb(245, 131, 35);
            btnChangPassword.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnChangPassword.ForeColor = Color.White;
            btnChangPassword.Image = Properties.Resources.DatlaiMK;
            btnChangPassword.Location = new Point(446, 75);
            btnChangPassword.Name = "btnChangPassword";
            btnChangPassword.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnChangPassword.Size = new Size(207, 58);
            btnChangPassword.TabIndex = 2;
            btnChangPassword.Text = "ĐỔI MẬT KHẨU";
            btnChangPassword.Click += btnChangePassword_Click;
            // 
            // btnHistory
            // 
            btnHistory.BorderRadius = 10;
            btnHistory.CustomizableEdges = customizableEdges5;
            btnHistory.DisabledState.BorderColor = Color.DarkGray;
            btnHistory.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHistory.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHistory.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHistory.FillColor = Color.FromArgb(44, 84, 115);
            btnHistory.FocusedColor = Color.FromArgb(245, 131, 35);
            btnHistory.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnHistory.ForeColor = Color.White;
            btnHistory.Image = Properties.Resources.BookingHistory;
            btnHistory.Location = new Point(731, 75);
            btnHistory.Name = "btnHistory";
            btnHistory.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnHistory.Size = new Size(207, 58);
            btnHistory.TabIndex = 3;
            btnHistory.Text = "LỊCH SỬ ĐẶT VÉ";
            btnHistory.Click += btnHistory_Click;
            // 
            // panelContent
            // 
            panelContent.BorderRadius = 20;
            panelContent.CustomizableEdges = customizableEdges7;
            panelContent.Location = new Point(34, 190);
            panelContent.Name = "panelContent";
            panelContent.ShadowDecoration.CustomizableEdges = customizableEdges8;
            panelContent.Size = new Size(1417, 620);
            panelContent.TabIndex = 4;
            // 
            // FormProfile
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(92, 124, 150);
            ClientSize = new Size(1500, 938);
            Controls.Add(panelContent);
            Controls.Add(btnHistory);
            Controls.Add(btnChangPassword);
            Controls.Add(btnInformation);
            ForeColor = SystemColors.ActiveCaptionText;
            Name = "FormProfile";
            Text = "FormProfile";
            Load += FormProfile_Load;
            ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnInformation;
        private Guna.UI2.WinForms.Guna2Button btnChangPassword;
        private Guna.UI2.WinForms.Guna2Button btnHistory;
        private Guna.UI2.WinForms.Guna2Panel panelContent;
    }
}