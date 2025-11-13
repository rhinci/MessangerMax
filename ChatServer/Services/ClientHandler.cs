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
            // слушать сообщения от клиента
        }

        public void SendMessage(Message msg)
        {
            // отправить сообщение клиенту
        }

        public void Disconnect()
        {
            // отключить клиента
        }
    }
}