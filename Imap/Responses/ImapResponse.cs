using Imap.Responses;

namespace Imap.Connector
{
    public class ImapResponse
    {
        private ResponseType _Type;
        public ResponseType Type
        {
            get => _Type;
        }

        private bool _IsSuccess = false;
        public bool IsSuccess
        {
            get => _IsSuccess;
            private set => _IsSuccess = value;
        }

        private string _Content;
        public string Content
        {
            get => _Content;
            private set
            {
                _Content = value.Remove(value.Length - 2);
                IsSuccess = _Content.Contains(" OK ");
            }
        }

        public ImapResponse(string content) => Content = content;
    }
}
