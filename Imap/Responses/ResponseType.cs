namespace Imap.Responses
{
    public class ResponseType
    {
        public const string Ok = "OK";
        public const string No = "NO";
        public const string Bad = "BAD";
        public const string PreAuth = "PREAUTH";

        public const string Prefix = "* ";
        public const string ServerOk = Prefix + Ok;
        public const string ServerBad = Prefix + Bad;
        public const string ServerBye = Prefix + "BYE";
        public const string ServerNo = Prefix + No;
        public const string ServerPreAuth = Prefix + PreAuth;
    }
}
