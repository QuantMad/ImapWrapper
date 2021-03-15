using Imap.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Imap.Connector
{
    public class ImapConnection : IDisposable
    {
        private const string CRLF = "\r\n";

        public const int IMAP_STANDART_PORT = 143;
        public const int IMAP_STANDART_SSL_PORT = 993;

        protected string _Host;
        public string Host
        {
            get => _Host;
            set
            {
                if (IsConnected)
                    throw new ImapConnectionException("Попутка изменить целевой хост уже установленного соединения");
                _Host = value;
            }
        }

        protected int _Port;
        public int Port
        {
            get => _Port;
            set
            {
                if (IsConnected)
                    throw new ImapConnectionException("Попутка изменить целевой порт уже установленного соединения");
                _Port = value;
            }
        }

        public bool IsConnected
        {
            get => Connection != null && Connection.Connected;
        }

        private TcpClient Connection;
        private Stream ImapStream;
        private StreamReader ImapStreamReader;

        public ImapConnection(string host = null, int port = IMAP_STANDART_PORT)
        {
            _Host = host;
            _Port = port;
        }

        public bool Connect()
        {
            return Connect(_Host, _Port);
        }

        public bool Connect(string host, int port = IMAP_STANDART_PORT)
        {
            if (IsConnected) 
                throw new ImapConnectionException("Попытка подключения с уже установленным соединением");
                
            _Host = host;
            _Port = port;

            if (_Host == null)
                throw new ImapConnectionException("Ошибка подключения: Не указан целевой хост");

            Connection = new TcpClient();

            try
            {
                Connection.Connect(_Host, _Port);
                if (!IsConnected)
                    return false;

                ImapStream = Connection.GetStream();
                //(ImapStream as SslStream).AuthenticateAsClient(_Host);
                ImapStreamReader = new StreamReader(ImapStream);
                string result = ImapStreamReader.ReadLine(); /// TODO: обработать приветствие сервера
                Capability();
                Console.WriteLine(result);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        protected string Capability()
        {
            /// TODO структура отвечающая за возможности IMAP сервера
            return "";
        }

        public string ReadResponse()
        {
            return ImapStreamReader.ReadLine();
        }

        public string WriteMessage(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("A001 " + message + CRLF);
            ImapStream.Write(bytes, 0, bytes.Length);

            return "";
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
