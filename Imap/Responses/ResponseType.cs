namespace Imap.Responses
{
    public class ResponseType
    {
        public const string Ok = "OK";
        public const string No = "NO";
        public const string Bad = "BAD";
        public const string PreAuth = "PREAUTH";

        public const string Prefix = "*";
        public const string ServerOk = Prefix + " OK";
        public const string ServerBad = Prefix + " BAD";
        public const string ServerNo = Prefix + " NO";
        public const string ServerPreAuth = Prefix + " PREAUTH";
    }
}
