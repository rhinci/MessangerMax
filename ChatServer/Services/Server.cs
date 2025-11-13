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
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);

                _listener.Start();
                _isRunning = true;

                Console.WriteLine($"Сервер запущен на порту {port}");

                _ = AcceptClientsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска сервера: {ex.Message}");
                _isRunning = false;
            }
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