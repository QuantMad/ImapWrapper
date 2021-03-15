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
            client = new ImapClient("Outlook.Office365.com");
            Console.WriteLine(client.Connect());
            Console.WriteLine($"{client.Host}:{client.Port} Connection {(client.IsConnected ? "success" : "failed")}");
            //Console.WriteLine(client.WriteMessage("NOOP"));
            //Console.WriteLine(client.ReadResponse());
        }

        public void Run()
        {
            Console.Write(">:");
            string input;
            while ((input = Console.ReadLine()) != "qqq")
            {
                if (input == "qqq") return;
                string rs = client.WriteMessage(input);
                Console.WriteLine($"<:" + "A001 " + input);
                Console.WriteLine(">:" + client.ReadResponse());
                Console.Write("<:");
            }
        }
    }
}

