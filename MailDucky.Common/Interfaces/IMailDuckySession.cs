using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MailDucky.Common.Interfaces
{
    public interface IMailDuckySession
    {
        void StopSession();
        Task BeginSession(TcpClient client);
    }
}
