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
                    client.Disconnect();
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

        public void BroadcastMessage(Message msg)
        {
            List<ClientHandler> clientsToRemove = new List<ClientHandler>();

            if (msg.Sender == "Система")
            {
                Console.WriteLine($"Системное сообщение: {msg.Text}");
            }

            foreach (var client in _clients)
            {
                if (client.IsConnected)
                {
                    try
                    {
                        client.SendMessage(msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка отправки сообщения клиенту {client.ClientName}: {ex.Message}");
                        clientsToRemove.Add(client);
                    }
                }
                else
                {
                    clientsToRemove.Add(client);
                }
            }

            foreach (var client in clientsToRemove)
            {
                _clients.Remove(client);
                Console.WriteLine($"Клиент {client.ClientName} удалён из списка");
            }
        }

        public void NotifyClientDisconnected(string clientName)
        {
            Message disconnectMessage = new Message
            {
                Sender = "Система",
                Text = $"{clientName} покинул чат",
                Timestamp = DateTime.Now,
                Receiver = "All"
            };

            BroadcastMessage(disconnectMessage);
        }

        public void SendPrivateMessage(Message msg)
        {
            if (string.IsNullOrEmpty(msg.Receiver) || msg.Receiver == "All")
            {
                BroadcastMessage(msg);
                return;
            }

            ClientHandler? targetClient = _clients.Find(client =>
                client.ClientName.Equals(msg.Receiver, StringComparison.OrdinalIgnoreCase));

            if (targetClient != null && targetClient.IsConnected)
            {
                try
                {
                    targetClient.SendMessage(msg);
                    Console.WriteLine($"Личное сообщение от {msg.Sender} для {msg.Receiver}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка отправки личного сообщения: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Получатель {msg.Receiver} не найден или отключен");

                Message errorMsg = new Message
                {
                    Sender = "Система",
                    Text = $"Пользователь {msg.Receiver} не найден",
                    Timestamp = DateTime.Now,
                    Receiver = msg.Sender
                };

                ClientHandler? senderClient = _clients.Find(client =>
                    client.ClientName.Equals(msg.Sender, StringComparison.OrdinalIgnoreCase));
                senderClient?.SendMessage(errorMsg);
            }
        }
    }
}