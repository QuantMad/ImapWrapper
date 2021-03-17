using Imap.Enums;
using Imap.Responses;
using System.Text;

namespace Imap.Connector
{
    public class ImapResponse : ResponseType
    {

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

        private bool _IsMarked;
        public bool IsMarked
        {
            get => _IsMarked;
        }

        private string _Tag;
        public string Tag
        {
            get => _Tag;
        }

        private ResponseStates _State;
        public ResponseStates State
        {
            get => _State;
        }

        public ImapResponse(string content)
        {
            _Content = content.Replace("\r\n", "");
            if (content.StartsWith(Prefix))
            {
                _IsMarked = false;
                _Tag = Prefix;
                _Content = _Content.Remove(0, 2);
            }
            else
            {
                _IsMarked = true;
                _Tag = content.Substring(0, content.IndexOf(' '));
                _Content = _Content.Remove(0, content.IndexOf(' ') + 1);
            }
            if (_Content.StartsWith(Ok))
            {
                _State = ResponseStates.OK;
                _Content = Content.Remove(0, 3);
            }
            else if (_Content.StartsWith(No))
            {
                _State = ResponseStates.NO;
                _Content = Content.Remove(0, 3);
            }
            else if (_Content.StartsWith("BYE"))
            {
                _State = ResponseStates.BYE;
                _Content = Content.Remove(0, 4);
            }
            else
            {
                _State = ResponseStates.BAD;
                _Content = Content.Remove(0, 4);
            }
        }
    }
}
