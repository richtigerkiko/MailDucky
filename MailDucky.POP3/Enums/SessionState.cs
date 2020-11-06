using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace MailDucky.POP3.Enums
{
    public enum SessionState
    {
        AUTH,
        TRANS,
        WAITINGPASSWORD
    }
}
