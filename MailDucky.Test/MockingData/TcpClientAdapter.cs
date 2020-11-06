using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MailDucky.Test.MockingData
{
    public class TcpClientAdapter : ITcpClient
    {
        public TcpClient wrappedClient { get; set; }

        public TcpClientAdapter(TcpClient client)
        {
            wrappedClient = client;
        }

        public Stream GetStream()
        {
            return wrappedClient.GetStream();
        }
    }
}