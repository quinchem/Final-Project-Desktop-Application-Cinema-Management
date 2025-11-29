namespace UserApp
{
    partial class ChatbotDemo
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
            rctChat = new RichTextBox();
            txtChat = new Guna.UI2.WinForms.Guna2TextBox();
            btnSend = new Guna.UI2.WinForms.Guna2Button();
            btnChat = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // rctChat
            // 
            rctChat.Location = new Point(12, 33);
            rctChat.Name = "rctChat";
            rctChat.Size = new Size(1224, 372);
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
            txtChat.Font = new Font("Segoe UI", 9F);
            txtChat.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtChat.Location = new Point(13, 413);
            txtChat.Margin = new Padding(4, 5, 4, 5);
            txtChat.Name = "txtChat";
            txtChat.PlaceholderText = "";
            txtChat.SelectedText = "";
            txtChat.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtChat.Size = new Size(1125, 40);
            txtChat.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.CustomizableEdges = customizableEdges3;
            btnSend.DisabledState.BorderColor = Color.DarkGray;
            btnSend.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSend.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSend.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSend.Font = new Font("Segoe UI", 9F);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(1152, 413);
            btnSend.Name = "btnSend";
            btnSend.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSend.Size = new Size(84, 40);
            btnSend.TabIndex = 2;
            btnSend.Text = "Send";
            // 
            // btnChat
            // 
            btnChat.CustomizableEdges = customizableEdges5;
            btnChat.DisabledState.BorderColor = Color.DarkGray;
            btnChat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChat.Font = new Font("Segoe UI", 9F);
            btnChat.ForeColor = Color.White;
            btnChat.Location = new Point(589, 461);
            btnChat.Name = "btnChat";
            btnChat.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnChat.Size = new Size(84, 40);
            btnChat.TabIndex = 3;
            btnChat.Text = "Chat";
            // 
            // ChatbotDemo
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1261, 512);
            Controls.Add(btnChat);
            Controls.Add(btnSend);
            Controls.Add(txtChat);
            Controls.Add(rctChat);
            Name = "ChatbotDemo";
            Text = "ChatbotDemo";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox rctChat;
        private Guna.UI2.WinForms.Guna2TextBox txtChat;
        private Guna.UI2.WinForms.Guna2Button btnSend;
        private Guna.UI2.WinForms.Guna2Button btnChat;
    }
}