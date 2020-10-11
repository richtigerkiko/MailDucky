using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP.SMTPCommands
{
    class SMTPRSETResponse : SMTPCommandBase
    {
        public SMTPRSETResponse(string command, string argument) : base(command, argument)
        {
            Command = command;
            Argument = argument;
        }

        public override string GetResponse()
        {
            Session.SMTPCommands.Clear();
            Session.smtpData = new SMTPData();

            return "250 OK";
        }
    }
}
