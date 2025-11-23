using System;
using ChatServer.Services;

namespace ChatServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();

            server.Start(8080);
            Console.ReadLine();

            Console.WriteLine("Сервер запущен. Нажмите Enter для остановки...");
            Console.ReadLine();

            server.Stop();

            Console.WriteLine("Сервер остановлен. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}