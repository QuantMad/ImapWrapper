using System;
using Imap;
using Imap.Connector;

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
            client.ResponseRecieved += (response) =>
            {
                Console.WriteLine("<:" + response.Content);
            };
            client.Connect();
            Console.WriteLine($"{client.hostname}:{client.port} Connection {(client.Connected() ? "success" : "failed")}");
        }

        public void Run()
        {
            Console.Write(">:");
            string input;
            while ((input = Console.ReadLine()) != "qqq")
            {
                if (input == "qqq") return;
                client.SendMessage("A002 " + input);
                //Console.WriteLine($"<:{client.Response.Content}");
                Console.Write(">:");
            }
        }
    }
}

