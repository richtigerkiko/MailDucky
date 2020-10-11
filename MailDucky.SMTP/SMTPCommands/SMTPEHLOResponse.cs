using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP.SMTPCommands
{
    class SMTPEHLOResponse : SMTPCommandBase
    {
        public SMTPEHLOResponse(string command, string argument) : base(command, argument)
        {
            Command = command;
            Argument = argument;
        }

        public override string GetResponse()
        {
            var returnString = string.Empty;

            returnString += "250-localhost Hello" + Session.NEWLINE;
            returnString += "250-8BITMIME" + Session.NEWLINE;
            returnString += "250 AUTH LOGIN PLAIN" + Session.NEWLINE;

            return returnString;
        }
    }
}
