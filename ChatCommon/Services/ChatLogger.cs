using ChatCommon.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChatCommon.Services
{
    public class ChatLogger
    {
        private readonly string _logFilePath;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


        //проверка, чтобы не был пустой путь
        public ChatLogger(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
                throw new ArgumentException("logFilePath cannot be null or empty.", nameof(logFilePath));

            _logFilePath=logFilePath;

            var dir = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        //ассинхроная версия
        public async Task LogMessage(Message msg)
        {
            if (msg==null) throw new ArgumentNullException(nameof(msg));
            string line = msg.ToJson();

            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                await File.AppendAllTextAsync(_logFilePath, line + Environment.NewLine).ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        
        public async Task<List<Message>> LoadHistory()
        {
            var result=new List<Message>();

            if (!File.Exists(_logFilePath))
                return result;

            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                using var sr=new StreamReader(_logFilePath);
                string line;
                while ((line=await sr.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    try
                    {
                        var msg=Message.FromJson(line);
                        if (msg!=null) result.Add(msg);
                    }
                    catch
                    {
                        //игнорируем плохие строки
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return result;
        }
    }
}
