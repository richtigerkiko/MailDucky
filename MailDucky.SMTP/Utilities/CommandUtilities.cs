using MailDucky.SMTP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailDucky.SMTP.Utilities
{
    public static class CommandUtilities
    {
        public static SMTPCommandType ParseCommandType(string command)
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

        public static Tuple<string, string> SplitCommandLine(string commandLine)
        {
            var commandList = commandLine.Split(' ').ToList();
            var command = commandList.FirstOrDefault();
            var argument = String.Join(" ", commandList.Skip(1));
            return new Tuple<string, string>(commandList[0], argument);
        }
    }
}
