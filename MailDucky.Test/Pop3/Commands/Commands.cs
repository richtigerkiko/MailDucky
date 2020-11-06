using MailDucky.POP3;
using MailDucky.POP3.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailDucky.Test.Pop3.Commands
{
    class Commands: MailDuckyBaseTest
    {

        private Pop3CommandFactory commandFactory;

        private Pop3Session pop3Session;

        [SetUp]
        public void Setup()
        {
            pop3Session = new Pop3Session(config);
            commandFactory = new Pop3CommandFactory(pop3Session);
        }

        [Test]
        public async Task Noop()
        {
            var noopCommand = commandFactory.Parse("NOOP");
            var response = await noopCommand.GetResponseAsync();

            Assert.AreEqual(Pop3Responses.OK, response);
        }

        [Test]
        public async Task Capa()
        {
            var userInput = commandFactory.Parse("CAPA");
            var response = await userInput.GetResponseAsync();

            var itShouldBe = String.Format(Pop3Responses.OK, "Capability list follows") + Environment.NewLine;

            // iterate through Pop3Enum because this is what the server supports
            Enum.GetNames(typeof(Pop3CommandType))
            .ToList()
            .ForEach(x => {

                itShouldBe += x + Environment.NewLine;
            });

            // Add Terminator
            itShouldBe += "\r\n.\r\n";

            Assert.AreEqual(itShouldBe, response);
        }

        [Test]
        public async Task InvalidCommand()
        {
            var userInput = commandFactory.Parse(Path.GetRandomFileName());
            var response = await userInput.GetResponseAsync();

            var itShouldBe = String.Format(Pop3Responses.Error, "Invalid Command");

            Assert.AreEqual(itShouldBe, response);
        }

        //[Test]
        //public async Task User()
        //{
        //    var userInput = commandFactory.Parse("USER n.Koch@devbrett.onmicrosoft.com");
        //    pop3Session.NetworkStream = new System.Net.Sockets.NetworkStream();
        //    var response = await userInput.GetResponseAsync();

        //    var itShouldBe = String.Format(Pop3Responses.Error, "Invalid Command");

        //    Assert.AreEqual(itShouldBe, response);
        //}
    }
}
