using System.Net.Security;
using System.Text;

namespace Imap
{
    static class SslStreamExt
    {
        private const int BUFFER_SIZE = 2048;
        private const string CRLF = "\r\n";

        public static string ReadAll(this SslStream stream)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            int len = stream.Read(buffer, 0, buffer.Length);

            return Encoding.ASCII.GetString(buffer, 0, len);
        }

        public static string WriteString(this SslStream stream, object obj)
        {
            string val = obj.ToString();
            stream.Write(Encoding.ASCII.GetBytes(val + CRLF));

            return val;
        }
    }
}
