using System;
using System.Threading.Tasks;
using MailDucky.POP3.Enums;

namespace MailDucky.POP3
{
    public abstract class Pop3CommandBase
    {
        public string Command { get; internal set; }
        public string Argument { get; internal set; }
        public Pop3Session Session { get; set; }

        public Pop3CommandBase(string command, string argument)
        {
            Argument = argument;
            Command = command.ToUpper();
        }

        public Pop3CommandType ParseCommandType(string command)
        {
            Pop3CommandType commandType;

            if(Enum.TryParse<Pop3CommandType>(command, out commandType)){
                return commandType;
            }
            else {
                return Pop3CommandType.INVALID;
            }
        }

        public virtual async Task<string> GetResponseAsync(){
            return await Task.Run(() => GetResponse());
        }
        
        public abstract string GetResponse();
    }
}