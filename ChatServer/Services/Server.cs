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
            try
            {
                _isRunning = false;

                _listener?.Stop();

                foreach (var client in _clients)
                {
                    client.Disconnect();  // пока не работает, нужен метод Disconnect из класса Client
                }
                _clients.Clear();

                Console.WriteLine("Сервер остановлен");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка остановки сервера: {ex.Message}");
            }
        }

        private async Task AcceptClientsAsync()
        {
            while (_isRunning && _listener != null)
            {
                try
                {
                    TcpClient tcpClient = await _listener.AcceptTcpClientAsync();
                    Console.WriteLine("Новое подключение принято!");

                    ClientHandler clientHandler = new ClientHandler(tcpClient, this);
                    _clients.Add(clientHandler);

                    _ = clientHandler.ListenToClientAsync();
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при принятии подключения: {ex.Message}");
                }
            }
        }
    }
}