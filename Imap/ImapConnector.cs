using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;

namespace Imap
{
    /// <summary>
    /// Базовый класс. Реализует логику взаимодействия с IMAP сервером на уровне потоков данных
    /// </summary>
    public class ImapConnector
    {
        // Стандартный IMAP порт
        public const int STANDART_PORT = 993;

        public delegate void CommandHandler(string command);
        // Событие приёма ответа сервера
        public event CommandHandler RequestTransmitted;
        // Событие отправки запроса серверу
        public event CommandHandler ResponseRecieved;

        // Флаг, определяющий будет ли производиться ведение лога запросов/ответов
        public bool LogEnabled = true;

        // Хранит последний ответ сервера
        protected string _Response;
        public bool RequestSuccess = false;

        // Свойство реализует логику одноразового чтения и многоразовой записи.
        public string Response
        {
            get
            {
                string resp = _Response;
                _Response = "";

                return resp;
            }

            private set
            {
                _Response = value;
                Log("S: " + _Response);
                RequestSuccess = _Response.Contains(" OK ");
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
            Response = ImapStream.ReadAll();
        }

        public void SendMessage(string msg)
        {
            ImapStream.WriteString(msg);
            RequestTransmitted?.Invoke(msg);
            Log("C: " + msg);
            Response = ImapStream.ReadAll();
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

        public string GetResponse()
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
