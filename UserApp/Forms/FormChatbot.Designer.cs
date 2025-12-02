namespace UserApp.Forms
{
    partial class FormChatbot
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
            rctChat = new RichTextBox();
            txtChat = new Guna.UI2.WinForms.Guna2TextBox();
            btnSend = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // rctChat
            // 
            rctChat.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rctChat.Location = new Point(11, 11);
            rctChat.Margin = new Padding(2);
            rctChat.Name = "rctChat";
            rctChat.ReadOnly = true;
            rctChat.Size = new Size(763, 424);
            rctChat.TabIndex = 0;
            rctChat.Text = "";
            // 
            // txtChat
            // 
            txtChat.CustomizableEdges = customizableEdges1;
            txtChat.DefaultText = "";
            txtChat.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtChat.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtChat.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtChat.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtChat.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtChat.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtChat.ForeColor = Color.FromArgb(64, 64, 64);
            txtChat.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtChat.Location = new Point(13, 442);
            txtChat.Margin = new Padding(4, 5, 4, 5);
            txtChat.Name = "txtChat";
            txtChat.PlaceholderText = "";
            txtChat.SelectedText = "";
            txtChat.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtChat.Size = new Size(659, 43);
            txtChat.TabIndex = 1;
            txtChat.KeyDown += txtChat_KeyDown;
            // 
            // btnSend
            // 
            btnSend.BorderRadius = 3;
            btnSend.CustomizableEdges = customizableEdges3;
            btnSend.DisabledState.BorderColor = Color.DarkGray;
            btnSend.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSend.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSend.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSend.FillColor = Color.FromArgb(255, 128, 0);
            btnSend.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(678, 442);
            btnSend.Margin = new Padding(2);
            btnSend.Name = "btnSend";
            btnSend.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSend.Size = new Size(96, 43);
            btnSend.TabIndex = 2;
            btnSend.Text = "Gửi";
            // 
            // FormChatbot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(787, 499);
            Controls.Add(btnSend);
            Controls.Add(txtChat);
            Controls.Add(rctChat);
            Margin = new Padding(2);
            Name = "FormChatbot";
            Text = "FormChatbot";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox rctChat;
        private Guna.UI2.WinForms.Guna2TextBox txtChat;
        private Guna.UI2.WinForms.Guna2Button btnSend;
    }
}