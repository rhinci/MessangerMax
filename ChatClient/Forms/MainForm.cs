using ChatClient.Core;
using ChatClient.ModelsClient;
using ChatCommon.Models;
using ChatCommon.Services;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChatMessage = ChatCommon.Models.Message;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        private Client _networkClient;
        private int _messageYOffset = 10;   // Отступ сверху для сообщений
        private int _userYOffset = 10;      // Отступ для списка пользователей
        private List<ChatUser> _users = new List<ChatUser>();


        private Dictionary<string, List<ChatMessage>> _chatHistory = new Dictionary<string, List<ChatMessage>>();

        private string _currentChatUser = null; // Кто сейчас открыт


        private void SetupPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Tag = placeholder;
            textBox.ForeColor = Color.Gray;
            textBox.Text = placeholder;

            textBox.Enter += RemovePlaceholder;
            textBox.Leave += SetPlaceholder;
        }

        private void RemovePlaceholder(object? sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender!;
            string placeholder = tb.Tag!.ToString()!;

            if (tb.Text == placeholder)
            {
                tb.Text = "";
                tb.ForeColor = Color.Black;
            }
        }

        private void SetPlaceholder(object? sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender!;
            string placeholder = tb.Tag!.ToString()!;

            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = placeholder;
                tb.ForeColor = Color.Gray;
            }
        }


        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupPlaceholder(ipTextBox, "ip");
            SetupPlaceholder(portTextBox, "port");
            SetupPlaceholder(usernameTextBox, "username");
            var user1 = new ChatUser
            {
                Name = "Alex",
                LastOnline = "Онлайн",
                Avatar = GetAvatarImage("Alex")

            };

            var user2 = new ChatUser
            {
                Name = "Masha",
                LastOnline = "Был(а) 5 минут назад",
                Avatar = GetAvatarImage("Masha")
            };

            AddUserBlock(user1);
            AddUserBlock(user2);

            messagesPanel.Visible = false;
            DisplaySystemMessage("Выберите чат слева.");
        }




        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string ip = ipTextBox.Text.Trim();
            string portStr = portTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portStr) || string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Заполните все поля: IP, порт и имя пользователя!");
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("Порт должен быть числом!");
                return;
            }

            _networkClient = new Client(ip, port, username);
            _networkClient.MessageReceived += OnMessageReceived;
            _networkClient.Connected += OnConnected;
            _networkClient.Disconnected += OnDisconnected;

            bool connected = await _networkClient.ConnectAsync();
            if (connected)
            {
                messagesPanel.Visible = true;    
                messagesPanel.Controls.Clear();
                DisplaySystemMessage($"Пользователь '{username}' подключён к серверу!");
                DisplaySystemMessage($"Добро пожаловать, {username}!");
            }
            else
            {
                DisplaySystemMessage("Не удалось подключиться к серверу.");
            }
        }


        private async void OnSendButton_Click(object sender, EventArgs e)
        {
            if (_networkClient == null)
            {
                MessageBox.Show("Сначала подключитесь к серверу!");
                return;
            }
            if (_currentChatUser == null)
            {
                MessageBox.Show("Сначала выберите пользователя слева!");
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

        private void SaveToHistory(string user, ChatMessage msg)
        {
            if (msg.Sender == "System")
                return;

            if (!_chatHistory.ContainsKey(user))
                _chatHistory[user] = new List<ChatMessage>();

            _chatHistory[user].Add(msg);

            // Сохраняем файл JSON
            string dir = "ChatHistory";
            Directory.CreateDirectory(dir);

            string file = Path.Combine(dir, $"{usernameTextBox.Text}_{user}.json");
            File.WriteAllText(file, System.Text.Json.JsonSerializer.Serialize(_chatHistory[user]));
        }

        // Внутри класса MainForm
        private void AddSystemMessageToPanel(ChatMessage msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddSystemMessageToPanel(msg)));
                return;
            }

            if (!messagesPanel.Visible)
            {
                messagesPanel.Visible = true;
                messagesPanel.Controls.Clear();
            }

            Panel sysblock = new Panel();
            sysblock.Width = messagesPanel.ClientSize.Width - 25;
            sysblock.BackColor = Color.LightGray;
            sysblock.Padding = new Padding(5);
            sysblock.Margin = new Padding(5);
            sysblock.AutoSize = true;
            sysblock.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Label systextLabel = new Label();
            systextLabel.Text = msg.Text;
            systextLabel.Left = 5;
            systextLabel.Top = 5;
            systextLabel.AutoSize = true;
            systextLabel.MaximumSize = new Size(sysblock.Width - 10, 0);

            sysblock.Controls.Add(systextLabel);
            messagesPanel.Controls.Add(sysblock);
            messagesPanel.ScrollControlIntoView(sysblock);
        }

        private void AddMessageToPanel(ChatMessage msg, bool isOwnMessage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddMessageToPanel(msg, isOwnMessage)));
                return;
            }

            Panel block = new Panel();
            block.Width = messagesPanel.ClientSize.Width - 25;
            block.BackColor = msg.Sender == "System" ? Color.LightGray : (isOwnMessage ? Color.LightBlue : Color.WhiteSmoke);
            block.Padding = new Padding(5);
            block.Margin = new Padding(5);
            block.AutoSize = true;
            block.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            if (msg.Sender != "System")
            {
                PictureBox avatar = new PictureBox
                {
                    Image = GetAvatarImage(msg.Sender),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 40,
                    Height = 40,
                    Left = 5,
                    Top = 5
                };

                Label nameLabel = new Label
                {
                    Text = msg.Sender,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Left = 50,
                    Top = 5,
                    AutoSize = true
                };

                Label textLabel = new Label
                {
                    Text = msg.Text,
                    Left = 50,
                    Top = 25,
                    Width = block.Width - 60,
                    AutoSize = true,
                    MaximumSize = new Size(block.Width - 60, 0)
                };

                block.Controls.Add(avatar);
                block.Controls.Add(nameLabel);
                block.Controls.Add(textLabel);
            }
            else
            {
                Label systextLabel = new Label
                {
                    Text = msg.Text,
                    Left = 5,
                    Top = 5,
                    AutoSize = true,
                    MaximumSize = new Size(block.Width - 10, 0)
                };
                block.Controls.Add(systextLabel);
            }

            messagesPanel.Controls.Add(block);
            messagesPanel.ScrollControlIntoView(block);
        }


        private void LoadChat(string user)
        {
            string dir = "ChatHistory";
            string file = Path.Combine(dir, $"{usernameTextBox.Text}_{user}.json");
            messagesPanel.Controls.Clear();
            messagesPanel.Visible = true;

            if (File.Exists(file))
            {
                var messages = System.Text.Json.JsonSerializer.Deserialize<List<ChatMessage>>(File.ReadAllText(file));

                if (messages != null)
                {
                    foreach (var m in messages)
                    {
                        // При загрузке из файла isOwnMessage вычисляем по имени отправителя
                        bool isOwn = m.Sender == usernameTextBox.Text;
                        AddMessageToPanel(m, isOwn);
                    }
                }
            }
            else
            {
                DisplaySystemMessage($"Пустой чат с {user}");
            }
        }



        public void AddMessage(ChatMessage msg, bool isOwnMessage)
        {
            if (msg.Sender == "System")
            {
                // просто показываем системные сообщения, но не сохраняем
                if (!messagesPanel.Visible)
                {
                    messagesPanel.Visible = true;
                    messagesPanel.Controls.Clear();
                }

                if (InvokeRequired)
                {
                    Invoke(new Action(() => AddMessage(msg, false)));
                    return;
                }

                Panel sysblock = new Panel();
                sysblock.Width = messagesPanel.ClientSize.Width - 25;
                sysblock.BackColor = Color.LightGray;  // серый для системного
                sysblock.Padding = new Padding(5);
                sysblock.Margin = new Padding(5);
                sysblock.AutoSize = true;
                sysblock.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                Label systextLabel = new Label();
                systextLabel.Text = msg.Text;
                systextLabel.Left = 5;
                systextLabel.Top = 5;
                systextLabel.AutoSize = true;
                systextLabel.MaximumSize = new Size(sysblock.Width - 10, 0);

                sysblock.Controls.Add(systextLabel);
                messagesPanel.Controls.Add(sysblock);
                messagesPanel.ScrollControlIntoView(sysblock);
                return;
            }
            if (_currentChatUser == null)
            {
                // Это НЕ ошибка программы — это ошибка пользователя
                MessageBox.Show("Выберите чат слева перед отправкой сообщения.");
                return;
            }
            SaveToHistory(_currentChatUser, msg);

            if (_currentChatUser != msg.Sender && !isOwnMessage)
            {

                return;
            }

            // Если чат скрыт — показать
            if (!messagesPanel.Visible)
            {
                messagesPanel.Visible = true;
                messagesPanel.Controls.Clear();
            } 

            if (InvokeRequired)
            {
                Invoke(new Action(() => AddMessage(msg, isOwnMessage)));
                return;
            }

            // Панель для одного сообщения
            Panel block = new Panel();
            block.Width = messagesPanel.ClientSize.Width - 25; // немного отступа справа
            block.BackColor = isOwnMessage ? Color.LightBlue : Color.WhiteSmoke;
            block.Padding = new Padding(5);
            block.Margin = new Padding(5); // расстояние между блоками
            block.AutoSize = true;           // авто-подгон по содержимому
            block.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Аватар
            PictureBox avatar = new PictureBox();
            avatar.Image = GetAvatarImage(msg.Sender);
            avatar.SizeMode = PictureBoxSizeMode.Zoom;
            avatar.Width = 40;
            avatar.Height = 40;
            avatar.Left = 5;
            avatar.Top = 5;

            // Имя
            Label nameLabel = new Label();
            nameLabel.Text = msg.Sender;
            nameLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            nameLabel.Left = 50;
            nameLabel.Top = 5;
            nameLabel.AutoSize = true;

            // Сообщение
            Label textLabel = new Label();
            textLabel.Text = msg.Text;
            textLabel.Left = 50;
            textLabel.Top = 25;
            textLabel.Width = block.Width - 60;
            textLabel.AutoSize = true;
            textLabel.MaximumSize = new Size(block.Width - 60, 0); // перенос строк

            block.Controls.Add(avatar);
            block.Controls.Add(nameLabel);
            block.Controls.Add(textLabel);

            messagesPanel.Controls.Add(block);

            // Прокрутка к последнему сообщению
            messagesPanel.ScrollControlIntoView(block);
        }



        private void AddUserBlock(ChatUser user)
        {
            Panel block = new Panel();
            block.Size = new Size(260, 60);
            block.BackColor = Color.White;
            block.Padding = new Padding(5);
            block.Margin = new Padding(5);
            block.BorderStyle = BorderStyle.FixedSingle;
            block.Cursor = Cursors.Hand;

            // аватар
            PictureBox avatar = new PictureBox();
            avatar.Size = new Size(40, 40);
            avatar.Location = new Point(5, 10);
            avatar.SizeMode = PictureBoxSizeMode.Zoom;
            avatar.Image = user.Avatar;

            // имя
            Label nameLabel = new Label();
            nameLabel.Text = user.Name;
            nameLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            nameLabel.Location = new Point(55, 8);
            nameLabel.AutoSize = true;

            // время
            Label timeLabel = new Label();
            timeLabel.Text = user.LastOnline;
            timeLabel.Font = new Font("Segoe UI", 8, FontStyle.Italic);
            timeLabel.Location = new Point(55, 30);
            timeLabel.AutoSize = true;

            // кликабельность
            block.Click += (s, e) => OnUserBlockClicked(user);
            nameLabel.Click += (s, e) => OnUserBlockClicked(user);
            avatar.Click += (s, e) => OnUserBlockClicked(user);
            timeLabel.Click += (s, e) => OnUserBlockClicked(user);

            block.Controls.Add(avatar);
            block.Controls.Add(nameLabel);
            block.Controls.Add(timeLabel);

            usersPanel.Controls.Add(block);
        }
        private void OnUserBlockClicked(ChatUser user)
        {
            _currentChatUser = user.Name;
            messagesGroupBox.Text = $"Чат с {user.Name}";
            messagesPanel.Controls.Clear();

            DisplaySystemMessage($"Открыт чат с {user.Name}");
            LoadChat(user.Name);
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
