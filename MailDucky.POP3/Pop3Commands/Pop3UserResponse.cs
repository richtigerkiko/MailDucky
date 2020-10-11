using System;
using System.Threading.Tasks;
using MailDucky.GraphConnector;
using MailDucky.POP3.Enums;
using MailDucky.POP3.Utilities;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3UserResponse : Pop3CommandBase
    {
        private string Username { get; set; }
        private string Password { get; set; }

        public Pop3UserResponse(string command, string argument) : base(command, argument)
        {
            Command = command;
            Argument = argument;
        }

        public override async Task<string> GetResponseAsync()
        {
            string response = string.Empty;

            if (Session.SessionState == SessionState.TRANS)
            {
                return Pop3Responses.AlreadyAuthenticated;
            }
            else
            {
                return await GetAuthResponse();
            }
        }

        private async Task<string> GetAuthResponse()
        {
            Username = Argument;
            await Session.SendResponseAsync(Pop3Responses.UsernameOK);

            var passCommandLine = await Session.ListenRequestAsync();

            var commandAndArgument = CommandUtilities.SplitCommandLine(passCommandLine);
            var passCommand = commandAndArgument.Item1;
            Password = commandAndArgument.Item2;

            var passCommandType = CommandUtilities.ParseCommandType(passCommand);

            if (passCommandType == Pop3CommandType.PASS)
            {
                var graphAuthService = new GraphAuthenticationService(Session.Settings);
                Session.GraphClient = graphAuthService.graphClient;
                var user = await graphAuthService.GetUser(Username, Password);
                if (user != null)
                {
                    Session.SessionState = SessionState.TRANS;
                    Session.User = user;
                    var getGraphMails = new GraphMailingService(Session.GraphClient, Session.Settings);
                    var mailList = await getGraphMails.GetMailsAsync(user);
                    Session.MessageStore = mailList;
                    return Pop3Responses.AuthSucceeded;
                }
                else
                {
                    return Pop3Responses.AuthFailed;
                }
            }
            else
            {
                return Pop3Responses.InvalidCommand;
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            if(username == "alex" && password == "1234") return true;
            else return false;
        }

        public override string GetResponse()
        {
            throw new System.NotImplementedException();
        }
    }
}