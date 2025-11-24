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
            btnInformation.Location = new Point(50, 196);
            btnInformation.Name = "btnInformation";
            btnInformation.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnInformation.Size = new Size(207, 66);
            btnInformation.TabIndex = 1;
            btnInformation.Text = "Thông tin chung";
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
            btnChangPassword.Location = new Point(377, 196);
            btnChangPassword.Name = "btnChangPassword";
            btnChangPassword.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnChangPassword.Size = new Size(207, 66);
            btnChangPassword.TabIndex = 2;
            btnChangPassword.Text = "Đổi mật khẩu";
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
            btnHistory.Location = new Point(702, 196);
            btnHistory.Name = "btnHistory";
            btnHistory.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnHistory.Size = new Size(207, 66);
            btnHistory.TabIndex = 3;
            btnHistory.Text = "Lịch sử đặt vé";
            btnHistory.Click += btnHistory_Click;
            // 
            // panelContent
            // 
            panelContent.CustomizableEdges = customizableEdges7;
            panelContent.Location = new Point(50, 316);
            panelContent.Name = "panelContent";
            panelContent.ShadowDecoration.CustomizableEdges = customizableEdges8;
            panelContent.Size = new Size(1328, 552);
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