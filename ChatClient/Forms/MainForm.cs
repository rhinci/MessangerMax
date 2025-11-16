using ChatClient.Core;
using ChatCommon.Models;
using ChatCommon.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ChatMessage = ChatCommon.Models.Message;

namespace ChatClient
{
    public partial class MainForm : Form
    {
        private Client _networkClient;

        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private async void OnSendButton_Click(object sender, EventArgs e)
        {
            string text=messageTextBox.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            var msg = new ChatMessage
            {
                Sender= _networkClient.Username,
                Text=text,
                Timestamp=DateTime.Now
            };

            await _networkClient.SendMessageAsync(msg);
            messageTextBox.Clear();
        }
        private async void attachFileButton_Click(object sender, EventArgs e)
        {
            using var dialog=new OpenFileDialog();
            if (dialog.ShowDialog()==DialogResult.OK)
            {
                await _networkClient.SendFileAsync(dialog.FileName);
            }
        }
        private async void connectButton_Click(object sender, EventArgs e)
        {
            string ip=ipTextBox.Text.Trim();
            int port=int.Parse(portTextBox.Text.Trim());
            string username=usernameTextBox.Text.Trim();

            _networkClient=new Client(ip, port, username);
            _networkClient.MessageReceived += OnMessageReceived;
            _networkClient.Connected += OnConnected;
            _networkClient.Disconnected +=OnDisconnected;

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
            UpdateChat(msg);
        }

        public void UpdateChat(ChatMessage msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>UpdateChat(msg)));
                return;
            }

            string line = $"[{msg.Timestamp:HH:mm}] {msg.Sender}: {msg.Text}";
            chatHistoryListBox.Items.Add(line);

            chatHistoryListBox.TopIndex = chatHistoryListBox.Items.Count-1;
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

        public void DisplaySystemMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => DisplaySystemMessage(message)));
                return;
            }

            chatHistoryListBox.Items.Add($"[SYSTEM] {message}");
        }

    }
}
