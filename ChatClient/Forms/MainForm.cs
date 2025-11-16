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

        private async void OnSendButton_Click(object sender, EventArgs e)
        {
            string text = messageTextBox.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            var msg = new ChatMessage
            {
                Sender = _networkClient.Username,
                Text = text,
                Timestamp = DateTime.Now
            };

            await _networkClient.SendMessageAsync(msg);
            messageTextBox.Clear();
        }
        

    }
}
