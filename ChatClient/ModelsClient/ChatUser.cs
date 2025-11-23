using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.ModelsClient
{
    internal class ChatUser
    {
        public string Name { get; set; }
        public Image Avatar { get; set; }
        public string LastOnline { get; set; } // null = онлайн
    }
}
