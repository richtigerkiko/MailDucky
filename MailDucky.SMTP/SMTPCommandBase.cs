using MailDucky.SMTP.Enums;
using System;
using System.Threading.Tasks;

namespace MailDucky.SMTP
{
    public abstract class SMTPCommandBase
    {
        public string Command { get; internal set; }
        public string Argument { get; internal set; }
        public SMTPSession Session { get; set; }

        public SMTPCommandBase(string command, string argument)
        {
            Argument = argument;
            Command = command.ToUpper();
        }

        public SMTPCommandType ParseCommandType(string command)
        {
            SMTPCommandType commandType;

            if (Enum.TryParse<SMTPCommandType>(command, out commandType))
            {
                return commandType;
            }
            else
            {
                return SMTPCommandType.INVALID;
            }
        }

        public virtual async Task<string> GetResponseAsync()
        {
            return await Task.Run(() => GetResponse());
        }

        public abstract string GetResponse();
    }
}