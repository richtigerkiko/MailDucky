using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MailDucky.POP3
{
    public static class Pop3Responses
    {

        public static string OK { get { return "+OK {0}"; } }
        public static string Error { get { return "-ERR {0}"; } }
        public static string InvalidCommand { get { return String.Format(Error, "Invalid Command!"); } }
        public static string Announcement { get { return String.Format(OK, "Hello, Mailducky here!"); } }
        public static string Quitting { get { return String.Format(OK, "Mailducky signing off, bye!"); } }
        public static string NotAuthenticated { get { return String.Format(Error, "Not Authorized!"); } }

        public static string AlreadyAuthenticated
        {
            get { return String.Format(Error, "Already Authenticated!"); }
        }

        public static string UsernameOK { get { return String.Format(OK, ""); } }

        public static string AuthSucceeded { get { return String.Format(OK, ""); } }

        public static string AuthFailed { get { return String.Format(Error, "Authentication Failed!"); } }

        public static string MessageNotFound { get { return "-ERR There is no Message {0}."; } }
        public static string UsernameNotFound { get { return "-ERR sorry, no mailbox for {0} here"; } }
    }
}