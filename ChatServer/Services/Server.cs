using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatCommon.Models;

namespace ChatServer.Services
{
    public class Server
    {
        private TcpListener? _listener;
        private List<ClientHandler> _clients;
        private bool _isRunning;


        public Server()
        {
            _clients = new List<ClientHandler>();
            _isRunning = false;
        }


        public bool IsRunning
        {
            get { return _isRunning; }
        }



        public void Start(int port)
        {
            // запуск сервера
        }

        public void Stop()
        {
            // остановка сервера
        }

        private async Task AcceptClientsAsync()
        {
            // принятие подключений
        }
    }
}