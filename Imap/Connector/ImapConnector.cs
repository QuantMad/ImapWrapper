using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;

namespace Imap.Connector
{
    /// TODO: Состояние подключения

    /// <summary>
    /// Базовый класс. Реализует логику взаимодействия с IMAP сервером на уровне потоков данных
    /// </summary>
    public class ImapConnector
    {
        // Стандартный IMAP порт
        public const int STANDART_PORT = 993;

        public enum ImapState
        {
            connected,
            disconnected,
            unauthenticated,
            authenticated
        }

        //public delegate void RequestHandler(string command);
        public delegate void ResponseHandler(ImapResponse command);
        // Событие приёма ответа сервера
        //public event RequestHandler RequestTransmitted;
        // Событие отправки запроса серверу
        public event ResponseHandler ResponseRecieved;

        // Флаг, определяющий будет ли производиться ведение лога запросов/ответов
        public bool LogEnabled = true;

        // Хранит последний ответ сервера
        protected ImapResponse _Response;
        public ImapResponse Response
        {
            get
            {
                var response = _Response;
                _Response = null;
                return response;
            }
            private set
            {
                _Response = value;
                ResponseRecieved?.Invoke(_Response);
            }
        }

        public string hostname;
        public int port;

        private TcpClient ImapConnection;
        private SslStream ImapStream;

        public ImapConnector(string hostname = null, int port = STANDART_PORT, bool LogEnabled = true)
        {
            this.hostname = hostname;
            this.port = port;
            this.LogEnabled = LogEnabled;
        }

        /// <summary>
        /// Выполняет подключение к удалённму IPAM серверу
        /// </summary>
        public void Connect()
        {
            Log("************* " + DateTime.Now.ToString() + " *************");
            ImapConnection = new TcpClient();
            ImapConnection.Connect(this.hostname, this.port);
            ImapStream = new SslStream(ImapConnection.GetStream(), true);
            ImapStream.AuthenticateAsClient(this.hostname);
            Response = new ImapResponse(ImapStream.ReadAll());
        }

        public void SendMessage(string msg)
        {
            ImapStream.WriteString(msg);
            //RequestTransmitted?.Invoke(msg);
            Log("C: " + msg);
            Response = new ImapResponse(ImapStream.ReadAll());
        }

        public void Disconnect()
        {
            ImapConnection.Client.Close();
            ImapConnection.Close();
        }

        public bool Connected()
        {
            return ImapConnection != null && ImapConnection.Connected;
        }

        public ImapResponse GetResponse()
        {
            return _Response;
        }

        private void Log(string val)
        {
            if (LogEnabled)
            {
                using (var LogWriter = new StreamWriter(@"D:\log.txt", true))
                {
                    LogWriter.WriteLine(val);
                    LogWriter.Close();
                }
            }
        }
    }
}
