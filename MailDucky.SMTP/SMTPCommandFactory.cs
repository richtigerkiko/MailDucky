using MailDucky.SMTP.Enums;
using MailDucky.SMTP.SMTPCommands;
using MailDucky.SMTP.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP
{
    class SMTPCommandFactory
    {
        public SMTPSession Session { get; set; }

        public SMTPCommandFactory(SMTPSession smtpSession)
        {
            Session = smtpSession;
        }

        public SMTPCommandBase Parse(string commandLine)
        {
            SMTPCommandBase smtpCommand;
            var commandAndArguments = CommandUtilities.SplitCommandLine(commandLine);
            var command = commandAndArguments.Item1;
            var argument = commandAndArguments.Item2;
            var commandType = CommandUtilities.ParseCommandType(command);

            switch (commandType)
            {
                case SMTPCommandType.QUIT:
                    smtpCommand = new SMTPQuitResponse(command, argument);
                    break;
                case SMTPCommandType.EHLO:
                case SMTPCommandType.HELO:
                    smtpCommand = new SMTPEHLOResponse(command, argument);
                    break;
                case SMTPCommandType.AUTH:
                    smtpCommand = new SMTPAUTHResponse(command, argument);
                    break;
                case SMTPCommandType.MAIL:
                    smtpCommand = new SMTPMAILResponse(command, argument);
                    break;
                case SMTPCommandType.RSET:
                    smtpCommand = new SMTPRSETResponse(command, argument);
                    break;
                case SMTPCommandType.NOOP:
                    smtpCommand = new SMTPNOOPResponse(command, argument);
                    break;
                case SMTPCommandType.INVALID:
                default:
                    smtpCommand = new SMTPInvalidResponse(command, argument);
                    break;
            }
            smtpCommand.Session = Session;
            Session.SMTPCommands.Add(smtpCommand);
            return smtpCommand;
        }
    }
}
