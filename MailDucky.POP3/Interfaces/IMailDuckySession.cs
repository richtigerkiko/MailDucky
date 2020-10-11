using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MailDucky.POP3.Interfaces
{
    public interface IMailDuckySession
    {
        void StopSession();
        Task BeginSession(TcpClient client);
    }
}
