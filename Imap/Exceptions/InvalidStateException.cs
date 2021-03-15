using System;

namespace Imap.Exceptions
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException() { }
        public InvalidStateException(string message) : base(message) { }
    }
}
