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
using ChatMessage = ChatCommon.Models.Message;


namespace ChatClient.Core
{
    internal class Client
    {
        private TcpClient? _tcpClient;
        private NetworkStream? _stream;
        private string _serverIp;
        private int _port;
        private string _username;
        private ChatLogger _logger;
        private bool _isConnected;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public event EventHandler? Connected;
        public event EventHandler? Disconnected;


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
            get=> _username;
            set => _username = value;
        }

        public bool IsConnected => _isConnected;

        //конструктор
        public Client(string serverIp, int port, string username)
        {
            _serverIp=serverIp;
            _port=port;
            _username=username;
            _logger=new ChatLogger("chat_history.json");
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _tcpClient =new TcpClient();
                await _tcpClient.ConnectAsync(_serverIp, _port);
                _stream = _tcpClient.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
                _isConnected=true;
                await _writer.WriteLineAsync(_username);
                Connected?.Invoke(this, EventArgs.Empty); //для MainForm
                _ =ListenToServerAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка подключения: " + ex.Message);
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

            Disconnected?.Invoke(this, EventArgs.Empty); //для MainForm
        }


        public async Task SendMessageAsync(ChatMessage msg)
        {
            if (!_isConnected) return;

            string json = msg.ToJson();
            await _writer.WriteLineAsync(_username);
            await _writer.FlushAsync();
            await _logger.LogMessage(msg);
        }

        public async Task SendFileAsync(string filePath)
        {
            if (!File.Exists(filePath))return;
            byte[] data=File.ReadAllBytes(filePath);
            string fileName= Path.GetFileName(filePath);

            var msg=new ChatMessage
            {
                Sender= _username,
                Text="",
                Timestamp=DateTime.Now,
                Receiver="", // broadcast
                FileName=fileName,
                FileData=data
            };

            await SendMessageAsync(msg);
        }

     
        public async Task<List<ChatMessage>> LoadChatHistory()
        {
            return await _logger.LoadHistory();
        }

       //асинхронно
        public async Task ListenToServerAsync()
        {
            while (_isConnected)
            {
                try
                {
                    string line= await _reader.ReadLineAsync();

                    if (line==null)
                    {
                        Disconnect();
                        break;
                    }
                    var msg= ChatMessage.FromJson(line);
                    if (msg!=null)
                    {
                        await _logger.LogMessage(msg);
                        MessageReceived?.Invoke(msg);
                    }
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        public event Action<ChatMessage> MessageReceived;

        protected virtual void OnMessageReceived(ChatMessage msg)
        {
            MessageReceived?.Invoke(msg);
        }
    }
}
