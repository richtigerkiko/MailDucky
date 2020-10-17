using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MailDucky.Test.MockingData
{
    public interface ITcpClient
    {
        Stream GetStream();
    }
}
