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
            var graphAuthService = new GraphAuthenticationService(Session.Settings);
            Session.GraphClient = graphAuthService.graphClient;
            var user = await graphAuthService.GetUser(Username);
            if (user != null)
            {
                Session.SessionState = SessionState.WAITINGPASSWORD;
                Session.User = user;
                return Pop3Responses.UsernameOK;
            }
            else
            {
                Session.SessionState = SessionState.AUTH;
                return string.Format(Pop3Responses.UsernameNotFound, Username);
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