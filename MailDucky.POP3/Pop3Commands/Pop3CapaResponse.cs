using MailDucky.POP3.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MailDucky.POP3.Pop3Commands
{
    class Pop3CapaResponse : Pop3CommandBase
    {
        public Pop3CapaResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            var returnString = String.Format(Pop3Responses.OK, "Capability list follows") + Session.NEWLINE;
            Enum.GetNames(typeof(Pop3CommandType))
                .ToList()
                .ForEach(x => {

                    returnString += x + Session.NEWLINE;
                });
            returnString += Session.TERMINATOR;

            return returnString;

        }
    }
}
