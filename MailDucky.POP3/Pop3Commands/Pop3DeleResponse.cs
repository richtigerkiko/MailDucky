using System;
using System.Linq;
using System.Threading.Tasks;
using MailDucky.GraphConnector;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3DeleResponse : Pop3CommandBase
    {
        public Pop3DeleResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            throw new NotImplementedException();
        }

        public override async Task<string> GetResponseAsync()
        {
            var mailID = 0;
            if (int.TryParse(Argument, out mailID))
            {
                if (Session.MessageStore.Count >= mailID)
                {
                    Session.MessageIdsMarkedForDeletion.Add(Session.MessageStore.ElementAt(mailID -1 ).Key);
                    return string.Format(Pop3Responses.OK, "");
                }
                else return Pop3Responses.MessageNotFound;
            }
            else
            {
                return Pop3Responses.InvalidCommand;
            }

        }
    }
}