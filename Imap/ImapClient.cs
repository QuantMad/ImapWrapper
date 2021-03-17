using System;

namespace Imap.Connector
{
    public class ImapClient : ImapConnection
    {
        public ImapClient(string hostname = null, int port = IMAP_STANDART_PORT) :
            base(hostname, port)
        {

        }

        public void Noop()
        {
            Sand("NOOP");
        }

        public ImapResponse Login(string name, string password)
        {
            var result = Sand($"LOGIN {name} {password}");
            if (result.Content.Contains("OK"))
                return result;

            return null;
        }
    }
}
