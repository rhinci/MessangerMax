namespace ChatClient
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.ListBox usersListBox;

        private Label label1;
        private SplitContainer splitContainer1;
        private PictureBox pictureBox1;
        private GroupBox groupBox1;
        private Label label2;
        private TextBox textBox1;
        private Button button1;
        private Button файл;
        private GroupBox messagesGroupBox;
        private Panel messagesPanel;

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
            ipTextBox = new TextBox();
            portTextBox = new TextBox();
            usernameTextBox = new TextBox();
            label1 = new Label();
            splitContainer1 = new SplitContainer();
            groupBox1 = new GroupBox();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            messagesGroupBox = new GroupBox();
            messagesPanel = new Panel();
            textBox1 = new TextBox();
            button1 = new Button();
            файл = new Button();
            usersListBox = new ListBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            messagesGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // ipTextBox
            // 
            ipTextBox.Location = new Point(110, 13);
            ipTextBox.Name = "ipTextBox";
            ipTextBox.Size = new Size(150, 27);
            ipTextBox.TabIndex = 2;
            // 
            // portTextBox
            // 
            portTextBox.Location = new Point(275, 13);
            portTextBox.Name = "portTextBox";
            portTextBox.Size = new Size(100, 27);
            portTextBox.TabIndex = 3;
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(391, 13);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(150, 27);
            usernameTextBox.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 12F);
            label1.Location = new Point(32, 12);
            label1.Name = "label1";
            label1.Size = new Size(63, 25);
            label1.TabIndex = 1;
            label1.Text = "Вход:";
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(12, 46);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox1);
            splitContainer1.Panel1.Controls.Add(usersListBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(messagesGroupBox);
            splitContainer1.Panel2.Controls.Add(textBox1);
            splitContainer1.Panel2.Controls.Add(файл);
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Size = new Size(890, 552);
            splitContainer1.SplitterDistance = 296;
            splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Location = new Point(14, 15);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(260, 61);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(68, 23);
            label2.Name = "label2";
            label2.Size = new Size(111, 20);
            label2.TabIndex = 0;
            label2.Text = "Name Surname";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(6, 16);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 39);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // messagesGroupBox
            // 
            messagesGroupBox.Controls.Add(messagesPanel);
            messagesGroupBox.Location = new Point(10, 10);
            messagesGroupBox.Name = "messagesGroupBox";
            messagesGroupBox.Size = new Size(560, 460);
            messagesGroupBox.TabIndex = 0;
            messagesGroupBox.TabStop = false;
            // 
            // messagesPanel
            // 
            messagesPanel.AutoScroll = true;
            messagesPanel.Dock = DockStyle.Fill;
            messagesPanel.Location = new Point(3, 23);
            messagesPanel.Name = "messagesPanel";
            messagesPanel.Size = new Size(554, 434);
            messagesPanel.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.ForeColor = SystemColors.InactiveCaption;
            textBox1.Location = new Point(37, 486);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(473, 27);
            textBox1.TabIndex = 1;
            textBox1.Text = "Сообщение";
            // 
            // button1
            // 
            button1.Location = new Point(516, 486);
            button1.Name = "button1";
            button1.Size = new Size(59, 30);
            button1.TabIndex = 4;
            button1.Text = "отправить";
            button1.Click += OnSendButton_Click;
            // 
            // файл
            // 
            файл.Location = new Point(8, 488);
            файл.Name = "файл";
            файл.Size = new Size(23, 28);
            файл.TabIndex = 3;
            файл.Text = "+";
            // 
            // usersListBox
            // 
            usersListBox.FormattingEnabled = true;
            usersListBox.Location = new Point(0, 3);
            usersListBox.Name = "usersListBox";
            usersListBox.Size = new Size(293, 544);
            usersListBox.TabIndex = 5;
            // 
            // MainForm
            // 
            ClientSize = new Size(914, 722);
            Controls.Add(splitContainer1);
            Controls.Add(label1);
            Controls.Add(ipTextBox);
            Controls.Add(portTextBox);
            Controls.Add(usernameTextBox);
            Name = "MainForm";
            Text = "Messanger MaxX";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            messagesGroupBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
