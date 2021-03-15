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
            WriteMessage("A002 NOOP");
        }

        public void Capability()
        {
            WriteMessage("A002 CAPABILITY");
        }
    }
}
