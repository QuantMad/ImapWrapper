using System;
using System.Collections.Generic;
using System.Text;

namespace Imap.Exceptions
{
    class ImapConnectionException : Exception
    {
        public ImapConnectionException() : base() { }
        public ImapConnectionException(string message) : base(message) { }
    }
}
