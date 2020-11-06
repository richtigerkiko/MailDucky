using MailDucky.GraphConnector;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailDucky.SMTP.SMTPCommands
{
    class SMTPMAILResponse : SMTPCommandBase
    {

        private string MailFrom { get; set; }
        private List<string> Recipients { get; set; }
        private string RawData = "";

        public SMTPMAILResponse(string command, string argument) : base(command, argument)
        {
            MailFrom = ExtractSenderFromArguments(Argument);
            Recipients = new List<string>();
        }

        public override async Task<string> GetResponseAsync()
        {
            string response = string.Empty;

            return await StartMailTransaction();
        }

        private async Task<string> StartMailTransaction()
        {
            // Check if Authenticated
            if(!Session.isAuthenticated || Session.User.Mail != MailFrom)
            {
                return SMTPServerResponse.AuthRequired;
            }
            else
            {
                Session.SendResponse("250 OK");
                await ProcessRecipients();
                await ProcessData();
                var mimeMessage = ConvertMime();

                var grapService = new GraphMailingService(Session.GraphClient, Session.Settings, Session.User);
                await grapService.SendMail(mimeMessage);
                return "250 OK";
            }
        }

        private async Task ProcessRecipients()
        {
            var request = await Session.ListenRequestAsync();
            while (request.Split(' ')[0] == "RCPT")
            {
                Recipients.Add(ExtractSenderFromArguments(request));
                Session.SendResponse("250 OK");
                request = await Session.ListenRequestAsync();
            }
        }

        private async Task ProcessData()
        {

            await Session.SendResponseAsync(SMTPServerResponse.StartData);

            var sending = true;

            while (sending)
            {
                var request = await Session.ListenRequestAsync();
                RawData += request + Session.NEWLINE;
                if (RawData.Contains(Session.TERMINATOR))
                {
                    sending = false;
                }
            }
            
        }

        private MimeMessage ConvertMime()
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(RawData);
            MemoryStream stream = new MemoryStream(byteArray);
            var message = MimeMessage.Load(stream);
            stream.Dispose();
            return message;
        }

        public override string GetResponse()
        {
            return SMTPServerResponse.InvalidCommand;
        }

        public string ExtractSenderFromArguments(string args)
        {
            var pattern = "(?<=<)(.*)(?=>)";
            var emailMatch = Regex.Match(args, pattern);
            if (emailMatch.Success)
            {
                return emailMatch.Value;
            }
            else
            {
                return "";
            }
        }
    }
}
