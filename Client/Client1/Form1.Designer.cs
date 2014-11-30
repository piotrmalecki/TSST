namespace Client1
{
    partial class Form1
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ChatBox = new System.Windows.Forms.ListBox();
            this.SendBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.InfoBox = new System.Windows.Forms.ListBox();
            this.Disconnect = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Capacity = new System.Windows.Forms.TextBox();
            this.Username = new System.Windows.Forms.ComboBox();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(362, 100);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(83, 24);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Remote Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Capacity";
            // 
            // ChatBox
            // 
            this.ChatBox.FormattingEnabled = true;
            this.ChatBox.Location = new System.Drawing.Point(12, 153);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(433, 121);
            this.ChatBox.TabIndex = 5;
            this.ChatBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // SendBox
            // 
            this.SendBox.Location = new System.Drawing.Point(12, 311);
            this.SendBox.Name = "SendBox";
            this.SendBox.Size = new System.Drawing.Size(361, 20);
            this.SendBox.TabIndex = 6;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(379, 311);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(66, 22);
            this.SendButton.TabIndex = 7;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(12, 280);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(91, 25);
            this.ClearButton.TabIndex = 8;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // InfoBox
            // 
            this.InfoBox.FormattingEnabled = true;
            this.InfoBox.Location = new System.Drawing.Point(15, 83);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(341, 56);
            this.InfoBox.TabIndex = 9;
            this.InfoBox.SelectedIndexChanged += new System.EventHandler(this.InfoBox_SelectedIndexChanged);
            // 
            // Disconnect
            // 
            this.Disconnect.Location = new System.Drawing.Point(362, 280);
            this.Disconnect.Name = "Disconnect";
            this.Disconnect.Size = new System.Drawing.Size(83, 25);
            this.Disconnect.TabIndex = 10;
            this.Disconnect.Text = "Disconnect";
            this.Disconnect.UseVisualStyleBackColor = true;
            this.Disconnect.Click += new System.EventHandler(this.Disconnect_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(413, -4);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(32, 23);
            this.exit.TabIndex = 11;
            this.exit.Text = "exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(9, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(94, 17);
            this.listBox1.TabIndex = 12;
            // 
            // Capacity
            // 
            this.Capacity.Location = new System.Drawing.Point(259, 57);
            this.Capacity.Name = "Capacity";
            this.Capacity.Size = new System.Drawing.Size(186, 20);
            this.Capacity.TabIndex = 4;
            // 
            // Username
            // 
            this.Username.FormattingEnabled = true;
            this.Username.Items.AddRange(new object[] {
            "Franek",
            "Henio",
            "Zenek"});
            this.Username.Location = new System.Drawing.Point(324, 30);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(121, 21);
            this.Username.TabIndex = 13;
  
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Location = new System.Drawing.Point(12, 33);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(124, 17);
            this.checkBox.TabIndex = 14;
            this.checkBox.Text = "via ManagementApp";
            this.checkBox.UseVisualStyleBackColor = true;
            this.checkBox.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 358);
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.Username);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.Disconnect);
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.SendBox);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.Capacity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectButton);
            this.Name = "Form1";
            this.Text = "Klient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox ChatBox;
        private System.Windows.Forms.TextBox SendBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.ListBox InfoBox;
        private System.Windows.Forms.Button Disconnect;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox Capacity;
        private System.Windows.Forms.ComboBox Username;
        private System.Windows.Forms.CheckBox checkBox;
    }
}

