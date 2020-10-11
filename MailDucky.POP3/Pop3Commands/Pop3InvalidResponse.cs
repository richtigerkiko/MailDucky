using System;
using System.Threading.Tasks;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3InvalidResponse : Pop3CommandBase
    {
        public Pop3InvalidResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            return String.Format(Pop3Responses.Error, "Invalid Command");
        }
    }
}