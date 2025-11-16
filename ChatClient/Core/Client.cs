using ChatCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ChatCommon.Models;
using Newtonsoft.Json;

namespace ChatClient.Core
{
    internal class Client
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private string _serverIp;
        private int _port;
        private string _username;
        private ChatLogger _logger;
        private bool _isConnected;
        private StreamReader _reader;
        private StreamWriter _writer;

        public string ServerIp
        {
            get => _serverIp;
            set => _serverIp = value;
        }

        public int Port
        {
            get => _port;
            set => _port = value;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public bool IsConnected => _isConnected;

        public Client(string serverIp, int port, string username)
        {
            _serverIp = serverIp;
            _port = port;
            _username = username;

            _logger = new ChatLogger("chat_history.json");
        }

        public bool Connect()
        {
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_serverIp, _port);

                _stream = _tcpClient.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                _isConnected = true;
                _writer.WriteLine(_username);

                ListenToServerAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            if (!_isConnected) return;

            _isConnected = false;

            _reader?.Dispose();
            _writer?.Dispose();
            _stream?.Dispose();
            _tcpClient?.Close();
        }


        public void SendMessage(Message msg)
        {
            if (!_isConnected)
                return;

            string json = msg.ToJson();
           
        }

        public void SendFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            byte[] data = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);

            var msg = new Message
            {
                Sender = _username,
                Text = "",
                Timestamp = DateTime.Now,
                Receiver = "", // broadcast
                FileName = fileName,
                FileData = data
            };

            SendMessage(msg);
        }

     
        public List<Message> LoadChatHistory()
        {
            return _logger.LoadHistory();
        }

       //ассихроно
        public async Task ListenToServerAsync()
        {
            while (_isConnected)
            {
                try
                {
                    string line = await _reader.ReadLineAsync();

                    if (line == null)
                    {
                        Disconnect();
                        break;
                    }

              

                    _logger.LogMessage(msg);
                }
                
            }
        }

        public event Action<Message> MessageReceived;

        protected virtual void OnMessageReceived(Message msg)
        {
            MessageReceived?.Invoke(msg);
        }
    }
}
