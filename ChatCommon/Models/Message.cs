using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatCommon.Models
{
    public class Message
    {
        private string? _sender;
        private string? _text;
        private DateTime _timestamp;
        private string? _receiver;
        private byte[]? _fileData;
        private string? _fileName;
        public string? FileType { get; set; }

        public string? Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public string? Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        public string? Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        public byte[]? FileData
        {
            get { return _fileData; }
            set { _fileData = value; }
        }

        public string? FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }


        // упаковка письма в JSON
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        // распаковка JSON в письмо
        public static Message? FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Message>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}

