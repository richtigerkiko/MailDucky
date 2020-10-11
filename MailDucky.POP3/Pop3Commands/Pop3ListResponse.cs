using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailDucky.GraphConnector;
using MailDucky.POP3.Enums;
using MailDucky.POP3.Utilities;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3ListResponse : Pop3CommandBase
    {
        public Pop3ListResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            if (Session.SessionState == SessionState.TRANS)
            {
                var getGraphMails = new GraphMailingService(Session.GraphClient, Session.Settings);
                return FormatPop3MessageResponse(Session.MessageStore);
            }
            else return Pop3Responses.NotAuthenticated;
        }

        private string FormatPop3MessageResponse(Dictionary<string, string> messageList)
        {
            var returnString = String.Format("+OK {0} messages:", messageList.Count) + Session.NEWLINE;

            var mailcounter = 0;
            foreach (var message in messageList)
            {
                mailcounter++;
                
                if (Session.MessageIdsMarkedForDeletion.Any(x => x == message.Key))
                {
                    continue;
                }

                returnString += Session.NEWLINE;

                var messageSize = StringUtils.GetStringOctedSize(message.Value);

                returnString += mailcounter + " " + messageSize;
            }

            returnString += Session.TERMINATOR;

            return returnString;
        }
    }
}