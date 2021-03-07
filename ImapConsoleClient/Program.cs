using System;
using Imap;

namespace ImapConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        private ImapClient client;

        public Program()
        {
            client = new ImapClient("Outlook.Office365.com", 993, false);
            client.LogEnabled = false;
            client.Connect();
            Console.WriteLine($"{client.hostname}:{client.port} Connection {(client.Connected() ? "success" : "failed")}");
            client.ResponseRecieved += (response) =>
            {
                Console.WriteLine($"<:{response}");
            };
        }

        public void Run()
        {
            Console.Write(">:");
            string input;
            while ((input = Console.ReadLine()) != "qqq")
            {
                if (input == "qqq") return;
                client.SendMessage("A002 " + input);
                Console.Write(">:");
            }
        }
    }
}

