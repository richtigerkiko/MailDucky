//using MailDucky.POP3;
//using NUnit.Framework;
//using NUnit;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Moq;
//using System.Threading.Tasks;
//using NSubstitute;
//using System.Net.Sockets;
//using MailDucky.Test.MockingData;

//namespace MailDucky.Test.Pop3
//{
//    class Session : MailDuckyBaseTest
//    {
//        private Pop3Session pop3Session;

//        [SetUp]
//        public void Setup()
//        {
//            pop3Session = new Pop3Session(config);
//        }

//        [Test]
//        public async Task StartPop3Session()
//        {
//            //var tcpClient = Substitute.For<TcpClient>();
//            //var tcpClient = new Mock<ITcpClient>();
//            var tcpClient = new TcpClient("localhost", 6666);
//            await pop3Session.BeginSession(tcpClient);
//        }
//    }
//}
