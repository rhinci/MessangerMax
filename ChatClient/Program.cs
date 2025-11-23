using ChatCommon.Models;
using System;
using System.Windows.Forms;
using MyMessage = ChatCommon.Models.Message;

namespace ChatClient
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());

        }
    }
}