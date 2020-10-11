using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP
{
    public static class SMTPServerResponse
    {
        public static string HELPReply { get { return "214 There is no help for you"; } }
		public static string Goodbye { get { return "221 Service closing transmission channel"; } }
		public static string OK { get { return "250 OK"; } }
		public static string CantVerify { get { return "252 Cannot VRFY user, but we will try to deliver the message anyway"; } }

		public static string Welcome { get { return "220 Mailducky SMTP Server"; } }
		public static string InvalidCommand { get { return "500 Syntax error, command unrecognized"; } }
		public static string AuthSuccess { get { return "235 Authentication successful"; } }
		public static string AuthFailureCred { get { return "535 Authentication credentials invalid"; } }

        public static string AuthRequired { get { return "530 5.7.0 Authentication required"; } }
        public static string StartData { get { return "354 Start mail input; end with <CRLF>.<CRLF>"; } }
        public static string PlainThreeThreeFour { get { return "334"; } }

        //{ 354, "Start mail input; end with <CRLF>.<CRLF>"},
        //{ 500, "Syntax error, command unrecognized"},
        //{ 501, "Syntax error in parameters or arguments"},
        //{ 502, "Unrecognized command"},
        //{ 503, "Bad sequence of commands"},
        //{ 538, "Encryption required for requested authentication mechanism"},
        //{ 550, "Requested action not taken: mailbox unavailable"}
    }
}
