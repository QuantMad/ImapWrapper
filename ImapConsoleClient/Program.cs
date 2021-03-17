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
            client.Connect();
        }

        public void Run()
        {
            Console.WriteLine(client.ToString() + "\n");
            Console.Write(">:");
            string input;
            while ((input = Console.ReadLine()) != "qqq")
            {
                if (input == "qqq") return;
                var rs = client.Sand(input);
                Console.WriteLine($"<:" + input);
                Console.WriteLine(">:" + rs.Content);
                Console.Write("<:");
            }
        }
    }
}

