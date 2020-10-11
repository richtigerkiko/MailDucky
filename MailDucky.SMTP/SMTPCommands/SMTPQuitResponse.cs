using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailDucky.SMTP.SMTPCommands
{
    class SMTPQuitResponse : SMTPCommandBase
    {
        public SMTPQuitResponse(string command, string argument) : base(command, argument)
        {
            Command = command;
            Argument = argument;
        }

        public override string GetResponse()
        {
            return SMTPServerResponse.Goodbye;
        }
    }
}
