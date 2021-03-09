namespace Imap.Connector
{
    public class ImapClient : ImapConnector
    {
        public ImapClient(string hostname = null, int port = STANDART_PORT, bool logEnabled = true) :
            base(hostname, port, logEnabled)
        {

        }

        public void Noop()
        {
            SendMessage("A002 NOOP");
        }

        public void Capability()
        {
            SendMessage("A002 CAPABILITY");
        }
    }
}
