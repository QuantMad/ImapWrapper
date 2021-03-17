using System;

namespace Imap.Exceptions
{
    public class ImapStateException : Exception
    {
        public ImapStateException() { }
        public ImapStateException(string message) : base(message) { }
    }
}
