using ChatClient.Core;
using ChatCommon.Models;
using ChatCommon.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ChatMessage = ChatCommon.Models.Message;
using System.Drawing;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        private Client _networkClient;
        private int _messageYOffset = 10;   // Отступ сверху для сообщений
        private int _userYOffset = 10;      // Отступ для списка пользователей
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private async void OnSendButton_Click(object sender, EventArgs e)
        {
            if (_networkClient == null)
            {
                MessageBox.Show("Сначала подключитесь к серверу!");
                return;
            }
            string text = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            var msg = new ChatMessage
            {
                Sender = _networkClient.Username,
                Text = text,
                Timestamp = DateTime.Now
            };

            await _networkClient.SendMessageAsync(msg);
            AddMessage(msg, isOwnMessage: true);
            textBox1.Clear();
        }
        private async void attachFileButton_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await _networkClient.SendFileAsync(dialog.FileName);
            }
        }
        private async void connectButton_Click(object sender, EventArgs e)
        {
            string ip = ipTextBox.Text.Trim();
            int port = int.Parse(portTextBox.Text.Trim());
            string username = usernameTextBox.Text.Trim();

            _networkClient = new Client(ip, port, username);
            _networkClient.MessageReceived += OnMessageReceived;
            _networkClient.Connected += OnConnected;
            _networkClient.Disconnected += OnDisconnected;

            if (await _networkClient.ConnectAsync())
            {
                DisplaySystemMessage("Connected to server.");
            }
            else
            {
                DisplaySystemMessage("Failed to connect.");
            }
        }
        private void OnConnected(object? sender, EventArgs e)
        {
            DisplaySystemMessage("Connected.");
        }

        private void OnDisconnected(object? sender, EventArgs e)
        {
            DisplaySystemMessage("Disconnected from server.");
        }

        private void OnMessageReceived(ChatMessage msg)
        {
            AddMessage(msg, false);
        }


        public void UpdateUserList(List<string> users)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUserList(users)));
                return;
            }

            usersListBox.Items.Clear();
            foreach (var user in users)
                usersListBox.Items.Add(user);
        }

        private void ipTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private async void файл_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await _networkClient.SendFileAsync(dialog.FileName);
            }
        }

        // Вставь это внутри класса MainForm, рядом с другими методами
        private Image GetAvatarImage(string? username = null)
        {
            // Создаём простой placeholder (серый квадрат с инициалами)
            int size = 40;
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);

                // Нарисуем инициалы (первые буквы username) если есть
                if (!string.IsNullOrEmpty(username))
                {
                    string initials = GetInitials(username);
                    using (Brush brush = new SolidBrush(Color.White))
                    using (Font f = new Font("Segoe UI", 12, FontStyle.Bold, GraphicsUnit.Pixel))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString(initials, f, brush, new RectangleF(0, 0, size, size), sf);
                    }
                }
            }

            return bmp;
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var parts = name.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[1].Substring(0, 1)).ToUpperInvariant();
        }


        public void AddMessage(ChatMessage msg, bool isOwnMessage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddMessage(msg, isOwnMessage)));
                return;
            }

            Panel block = new Panel();
            block.Width = messagesPanel.Width - 20;
            block.Height = 80;
            block.BackColor = isOwnMessage ? Color.LightBlue : Color.WhiteSmoke;
            block.Left = 10;
            block.Top = _messageYOffset;

            // Аватар
            PictureBox avatar = new PictureBox();
            avatar.Image = GetAvatarImage(msg.Sender); // Используй свою картинку
            avatar.SizeMode = PictureBoxSizeMode.Zoom;
            avatar.Width = 40;
            avatar.Height = 40;
            avatar.Left = 10;
            avatar.Top = 10;

            // Имя
            Label nameLabel = new Label();
            nameLabel.Text = msg.Sender;
            nameLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            nameLabel.Left = 60;
            nameLabel.Top = 10;
            nameLabel.AutoSize = true;

            // Сообщение
            Label textLabel = new Label();
            textLabel.Text = msg.Text;
            textLabel.Left = 60;
            textLabel.Top = 35;
            textLabel.Width = block.Width - 70;
            textLabel.AutoSize = false;
            textLabel.Height = 40;

            block.Controls.Add(avatar);
            block.Controls.Add(nameLabel);
            block.Controls.Add(textLabel);

            messagesPanel.Controls.Add(block);

            // Обновляем Y-отступ для следующего сообщения
            _messageYOffset += block.Height + 10;

            messagesPanel.AutoScroll = true;
        }


        public void AddUser(string username)
        {
            Panel userBlock = new Panel();
            userBlock.Width = splitContainer1.Panel1.Width - 20;
            userBlock.Height = 60;
            userBlock.Left = 10;
            userBlock.Top = _userYOffset;
            userBlock.BackColor = Color.WhiteSmoke;

            PictureBox avatar = new PictureBox();
            avatar.Image = GetAvatarImage(username);
            avatar.SizeMode = PictureBoxSizeMode.Zoom;
            avatar.Width = 40;
            avatar.Height = 40;
            avatar.Left = 10;
            avatar.Top = 10;

            Label nameLabel = new Label();
            nameLabel.Text = username;
            nameLabel.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            nameLabel.Left = 60;
            nameLabel.Top = 20;
            nameLabel.AutoSize = true;

            userBlock.Controls.Add(avatar);
            userBlock.Controls.Add(nameLabel);

            splitContainer1.Panel1.Controls.Add(userBlock);

            _userYOffset += 70;
        }


        // ================================
        //  СИСТЕМНЫЕ СООБЩЕНИЯ (простой текст)
        // ================================
        public void DisplaySystemMessage(string msg)
        {
            AddMessage(new ChatMessage
            {
                Sender = "System",
                Text = msg,
                Timestamp = DateTime.Now
            }, false);
        }
    }
}
