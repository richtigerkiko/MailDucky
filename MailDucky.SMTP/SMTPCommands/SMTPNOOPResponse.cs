﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP.SMTPCommands
{
    class SMTPNOOPResponse : SMTPCommandBase
    {
        public SMTPNOOPResponse(string command, string argument) : base(command, argument)
        {
            Command = command;
            Argument = argument;
        }

        public override string GetResponse()
        {
            return "250 OK";
        }
    }
}