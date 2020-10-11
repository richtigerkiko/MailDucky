using System;
using MailDucky.POP3.Enums;
using MailDucky.POP3.Pop3Commands;
using MailDucky.POP3.Utilities;

namespace MailDucky.POP3
{
    public class Pop3CommandFactory
    {
        public Pop3Session Session { get; set; }

        public Pop3CommandFactory(Pop3Session pop3Session)
        {
            Session = pop3Session;
        }

        public Pop3CommandBase Parse(string commandLine)
        {
            Pop3CommandBase pop3Command;
            var commandAndArguments = CommandUtilities.SplitCommandLine(commandLine);
            var command = commandAndArguments.Item1;
            var argument = commandAndArguments.Item2;
            var commandType = CommandUtilities.ParseCommandType(command);

            switch (commandType)
            {
                case Pop3CommandType.QUIT:
                    pop3Command = new Pop3QuitResponse(command, argument);
                    break;
                case Pop3CommandType.CAPA:
                    pop3Command = new Pop3CapaResponse(command, argument);
                    break;
                case Pop3CommandType.USER:
                    pop3Command = new Pop3UserResponse(command, argument);
                    break;
                case Pop3CommandType.STAT:
                    pop3Command = new Pop3StatResponse(command, argument);
                    break;
                case Pop3CommandType.LIST:
                    pop3Command = new Pop3ListResponse(command, argument);
                    break;
                case Pop3CommandType.RETR:
                    pop3Command = new Pop3RetrResponse(command, argument);
                    break;
                case Pop3CommandType.DELE:
                    pop3Command = new Pop3DeleResponse(command, argument);
                    break;
                case Pop3CommandType.RSET:
                    pop3Command = new Pop3RsetResponse(command, argument);
                    break;                       
                case Pop3CommandType.NOOP:
                    pop3Command = new Pop3NoopResponse(command, argument);
                    break;
                case Pop3CommandType.INVALID:
                default:
                    pop3Command = new Pop3InvalidResponse(command, argument);
                    break;
            }
            pop3Command.Session = Session;
            Session.Pop3Commands.Add(pop3Command);
            return pop3Command;
        }
    }
}