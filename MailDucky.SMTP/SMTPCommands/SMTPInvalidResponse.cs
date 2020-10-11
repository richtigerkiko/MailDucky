using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP.SMTPCommands
{
    class SMTPInvalidResponse: SMTPCommandBase
    {
        public SMTPInvalidResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            return SMTPServerResponse.InvalidCommand;
        }
    }
}
