namespace ChatClient
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private Button loginButton;

        private Label label1;
        private SplitContainer splitContainer1;
        private TextBox textBox1;
        private Button button1;
        private Button файл;
        private GroupBox messagesGroupBox;
        private FlowLayoutPanel messagesPanel;
        private FlowLayoutPanel usersPanel;


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
            usersPanel = new FlowLayoutPanel();
            messagesGroupBox = new GroupBox();
            messagesPanel = new FlowLayoutPanel();
            textBox1 = new TextBox();
            файл = new Button();
            button1 = new Button();
            loginButton = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            messagesGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // ipTextBox
            // 
            ipTextBox.Location = new Point(110, 13);
            ipTextBox.Name = "ipTextBox";
            ipTextBox.Size = new Size(150, 27);
            ipTextBox.TabIndex = 2;
            ipTextBox.Text = "ip";
            // 
            // portTextBox
            // 
            portTextBox.Location = new Point(275, 13);
            portTextBox.Name = "portTextBox";
            portTextBox.Size = new Size(100, 27);
            portTextBox.TabIndex = 3;
            portTextBox.Text = "port";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(391, 13);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(150, 27);
            usernameTextBox.TabIndex = 4;
            usernameTextBox.Text = "username";
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
            splitContainer1.Panel1.Controls.Add(usersPanel);
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
            // usersPanel
            // 
            usersPanel.AutoScroll = true;
            usersPanel.FlowDirection = FlowDirection.TopDown;
            usersPanel.Location = new Point(0, 3);
            usersPanel.Name = "usersPanel";
            usersPanel.Size = new Size(293, 544);
            usersPanel.TabIndex = 1;
            usersPanel.WrapContents = false;
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
            messagesPanel.FlowDirection = FlowDirection.TopDown;
            messagesPanel.Location = new Point(3, 23);
            messagesPanel.Name = "messagesPanel";
            messagesPanel.Padding = new Padding(5);
            messagesPanel.Size = new Size(554, 434);
            messagesPanel.TabIndex = 0;
            messagesPanel.WrapContents = false;
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
            // файл
            // 
            файл.Location = new Point(8, 488);
            файл.Name = "файл";
            файл.Size = new Size(23, 28);
            файл.TabIndex = 3;
            файл.Text = "+";
            файл.Click += файл_Click;

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
            // loginButton
            // 
            loginButton.Location = new Point(556, 11);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(100, 30);
            loginButton.TabIndex = 0;
            loginButton.Text = "Войти";
            loginButton.Click += LoginButton_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(914, 722);
            Controls.Add(loginButton);
            Controls.Add(splitContainer1);
            Controls.Add(label1);
            Controls.Add(ipTextBox);
            Controls.Add(portTextBox);
            Controls.Add(usernameTextBox);
            Name = "MainForm";
            Text = "Messanger MaxX";
            Load += MainForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            messagesGroupBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
