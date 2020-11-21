using MailDucky.GraphConnector;
using MailDucky.POP3.Enums;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailDucky.POP3.Pop3Commands
{
    class Pop3PassResponse : Pop3CommandBase
    {
        public Pop3PassResponse(string command, string argument) : base(command, argument)
        {
            Command = command;
            Argument = argument;
        }

        public override string GetResponse()
        {
            throw new NotImplementedException();
        }

        public override async Task<string> GetResponseAsync()
        {
            var password = Argument;
            if (Session.SessionState == SessionState.WAITINGPASSWORD)
            {
                if (password == Session.Settings.Pop3.AllUserPW)
                {
                    var getGraphMails = new GraphMailingService(Session.GraphClient, Session.Settings, Session.User);
                    Session.MessageStore = await getGraphMails.GetMailsAsync();
                    Session.SessionState = SessionState.TRANS;
                    return Pop3Responses.AuthSucceeded;
                }
                else
                {
                    return Pop3Responses.AuthFailed;
                }
            }
            else
            {
                Session.SessionState = SessionState.AUTH;
                return Pop3Responses.InvalidCommand;
            }
        }
    }
}
