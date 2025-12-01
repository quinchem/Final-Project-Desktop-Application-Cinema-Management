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
            rctChat.Location = new Point(-2, 3);
            rctChat.Name = "rctChat";
            rctChat.Size = new Size(1062, 437);
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
            txtChat.Location = new Point(-2, 461);
            txtChat.Margin = new Padding(4, 5, 4, 5);
            txtChat.Name = "txtChat";
            txtChat.PlaceholderText = "";
            txtChat.SelectedText = "";
            txtChat.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtChat.Size = new Size(970, 40);
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
            btnSend.Location = new Point(975, 461);
            btnSend.Name = "btnSend";
            btnSend.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSend.Size = new Size(74, 40);
            btnSend.TabIndex = 2;
            btnSend.Text = "Send";
            // 
            // FormChatbot
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1061, 515);
            Controls.Add(btnSend);
            Controls.Add(txtChat);
            Controls.Add(rctChat);
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