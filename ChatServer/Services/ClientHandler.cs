using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatCommon.Models;

namespace ChatServer.Services
{
    public class ClientHandler
    {
        private readonly TcpClient _tcpClient;
        private readonly Server _server;
        private NetworkStream? _stream;
        private string _clientName;

        public ClientHandler(TcpClient tcpClient, Server server)
        {
            _tcpClient = tcpClient;
            _server = server;
            _clientName = "Unknown"; // временно
        }

        public string ClientName
        {
            get { return _clientName; }
        }

        public bool IsConnected
        {
            get { return _tcpClient?.Connected ?? false; }
        }

        public async Task ListenToClientAsync()
        {
            try
            {
                _stream = _tcpClient.GetStream();
                StreamReader reader = new StreamReader(_stream, System.Text.Encoding.UTF8);

                string? nameLine = await reader.ReadLineAsync();
                if (!string.IsNullOrEmpty(nameLine))
                {
                    _clientName = nameLine.Trim();
                    Console.WriteLine($"Пользователь представился как: {_clientName}");
                }

                Message connectMessage = new Message
                {
                    Sender = "Система",
                    Text = $"{_clientName} присоединился к чату",
                    Timestamp = DateTime.Now,
                    Receiver = "All"
                };
                _server.BroadcastMessage(connectMessage);

                while (_tcpClient.Connected)
                {
                    string? jsonMessage = await reader.ReadLineAsync();

                    if (string.IsNullOrEmpty(jsonMessage))
                    {
                        break;
                    }

                    Message? message = Message.FromJson(jsonMessage);
                    if (message != null)
                    {
                        Console.WriteLine($"Получено сообщение от {_clientName}: {message.Text}");
                        _server.BroadcastMessage(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с клиентом {ClientName}: {ex.Message}");
            }
            finally
            {
                _server.NotifyClientDisconnected(_clientName);
                Disconnect();
            }
        }

        public void SendMessage(Message msg)
        {
            try
            {
                if (_tcpClient.Connected && _stream != null)
                {
                    string jsonMessage = msg.ToJson();

                    byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonMessage + Environment.NewLine);

                    _stream.Write(data, 0, data.Length);
                    _stream.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки сообщения клиенту {ClientName}: {ex.Message}");
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _tcpClient?.Close();

                Console.WriteLine($"Клиент {ClientName} отключен");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отключении клиента {ClientName}: {ex.Message}");
            }
        }
    }
}