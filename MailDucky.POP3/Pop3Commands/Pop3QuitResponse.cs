using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailDucky.GraphConnector;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3QuitResponse : Pop3CommandBase
    {
        public Pop3QuitResponse(string command, string argument) : base(command, argument)
        {
            Command = command;
            Argument = argument;
        }
        public override async Task<string> GetResponseAsync(){
            if(Session.SessionState == Enums.SessionState.AUTH)
            {
                return Pop3Responses.Quitting;
            }
            var graphMailingService = new GraphMailingService(Session.GraphClient, Session.Settings, Session.User);
            await graphMailingService.MoveMailsToArchiveAsync(Session.MessageIdsMarkedForDeletion);
            return Pop3Responses.Quitting;
        }
        public override string GetResponse()
        {
            throw new NotImplementedException();
        }
    }
}