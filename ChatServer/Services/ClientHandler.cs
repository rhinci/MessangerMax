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
                // ВРЕМЕННАЯ ЗАГЛУШКА
                await Task.Delay(100);
                Console.WriteLine($"Клиент {ClientName} подключен и слушает...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в ListenToClientAsync: {ex.Message}");
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
            // отключить клиента
        }
    }
}