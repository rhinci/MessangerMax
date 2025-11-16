namespace ChatClient
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.ListBox chatHistoryListBox;
        private System.Windows.Forms.ListBox usersListBox;





        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 722);
            Name = "MainForm";
            Text = "Messanger MaxX";
            Load += MainForm_Load;
            ResumeLayout(false);


            TextBox ipTextBox = new TextBox();
            ipTextBox.Name = "ipTextBox";
            ipTextBox.Location = new Point(10, 10);
            ipTextBox.Size = new Size(150, 25);
            this.Controls.Add(ipTextBox);


            chatHistoryListBox = new ListBox();
            chatHistoryListBox.Location = new Point(10, 50);
            chatHistoryListBox.Size = new Size(600, 500);
            this.Controls.Add(chatHistoryListBox);

            usersListBox = new ListBox();
            usersListBox.Location = new Point(620, 50);
            usersListBox.Size = new Size(200, 500);
            this.Controls.Add(usersListBox);

        }

        #endregion
    }
}
