using Imap.Exceptions;
using Imap.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imap.Connector
{
    public class ImapConnection
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

        int counter = 0;

        private readonly ConcurrentDictionary<string, ImapResponse> MarkedResponses = new ConcurrentDictionary<string, ImapResponse>();
        private readonly List<ImapResponse> UnmarkedResponses = new List<ImapResponse>();

        public ImapConnection(string host = null, int port = IMAP_STANDART_PORT)
        {
            _Host = host;
            _Port = port;
        }

        public bool Connect() => Connect(_Host, _Port);

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
                ImapStreamReader = new StreamReader(ImapStream);
                ServerHello();
                Capability();
                ListenServer();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void ServerHello()
        {
            /// TODO Обработать приветствие сервера до начала прослушивания сервера
        }

        private void Capability()
        {
            /// TODO Обработать возможности сервера до начала прослушивания сервера
        }

        public async Task<ImapResponse> WaitForResponse(string tag)
        {
            return await Task.Run(() =>
            {
                while (!MarkedResponses.ContainsKey(tag))
                {
                    Task.Delay(500);
                }
                ImapResponse response;
                MarkedResponses.TryRemove(tag, out response);
                Console.WriteLine("Ответ на " + response.Tag + " пришёл: " + response.Content);
                return response;
            });
        }

        public ImapResponse Sand(string message)
        {
            if (!IsConnected)
                throw new ImapConnectionException("Соединение разорвано либо не было установлено");

            string tag = $"IMAP{counter++}";
            string command = $"{tag} {message} {CRLF}";

            byte[] bytes = Encoding.ASCII.GetBytes(command);
            ImapStream.Write(bytes, 0, bytes.Length);
            Console.WriteLine("Ждём ответ на " + tag);
            return WaitForResponse(tag).Result;
        }

        public void ListenServer()
        {
            new Thread(() =>
            {
                while (IsConnected)
                {
                    var response = new ImapResponse(ImapStreamReader.ReadLine());

                    if (response != null)
                    {
                        if (!response.IsMarked)
                        {
                            UnmarkedResponses.Add(response);
                        }
                        else
                        {
                            MarkedResponses.TryAdd(response.Tag, response);
                        }
                    }
                    Thread.Sleep(500);
                }
            }).Start();
        }

        public override string ToString()
        {
            return $"Host: {_Host}\nPort: {_Port}\nIsConnected: {IsConnected}";
        }
    }
}
