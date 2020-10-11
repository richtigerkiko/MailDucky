using System;
using System.Collections.Generic;
using System.Linq;
using MailDucky.POP3.Enums;

namespace MailDucky.POP3.Utilities
{
    public static class CommandUtilities
    {
        public static Pop3CommandType ParseCommandType(string command)
        {
            Pop3CommandType commandType;

            if (Enum.TryParse<Pop3CommandType>(command, out commandType))
            {
                return commandType;
            }
            else
            {
                return Pop3CommandType.INVALID;
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