using MailDucky.GraphConnector;
using MailDucky.SMTP.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailDucky.SMTP.SMTPCommands
{
    class SMTPAUTHResponse : SMTPCommandBase
    {
        private string Username { get; set; }
        private string Password { get; set; }

        public SMTPAUTHResponse(string command, string argument) : base(command, argument)
        {
        }

        public override async Task<string> GetResponseAsync()
        {
            string response = string.Empty;

            return await GetAuthResponse();
        }

        private async Task<string> GetAuthResponse()
        {
            var authMethod = Argument.Split(' ')[0];
            if(authMethod == "PLAIN")
            {
                if (Argument.Split(' ').Length > 1)
                {
                    var usernameAndPassword = DecodeBase64(Argument.Split(' ')[1]);
                    usernameAndPassword = usernameAndPassword.Replace('\"', ' ').Trim();
                    Username = usernameAndPassword.Split(' ')[0];
                    Password = usernameAndPassword.Split(' ')[1];
                }
                else
                {
                    await Session.SendResponseAsync("334");
                    var usernameAndPassword = DecodeBase64(Argument.Split(' ')[1]);
                    usernameAndPassword = usernameAndPassword.Replace('\"', ' ').Trim();
                    Username = usernameAndPassword.Split(' ')[0];
                    Password = usernameAndPassword.Split(' ')[1];
                }
            }
            if(authMethod == "LOGIN")
            {
                Session.SendResponse("334 VXNlcm5hbWU6");

                Username = DecodeBase64(await Session.ListenRequestAsync());

                await Session.SendResponseAsync("334 UGFzc3dvcmQ6");

                Password = DecodeBase64(await Session.ListenRequestAsync());
            }


            var graphAuthService = new GraphAuthenticationService(Session.Settings);
            Session.GraphClient = graphAuthService.graphClient;
            var user = await graphAuthService.GetUser(Username, Password);
            if (user != null)
            {
                Session.isAuthenticated = true;
                Session.User = user;
                return SMTPServerResponse.AuthSuccess;
            }
            else
            {
                return SMTPServerResponse.AuthFailureCred;
            }
        }

        private string DecodeBase64 (string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            data = ReplaceZeroBytesWithSpace(data);
            return Encoding.UTF8.GetString(data);
        }

        private byte[] ReplaceZeroBytesWithSpace(byte[] source)
        {

            for(int i= 0; i< source.Length; i++)
            {
                if(source[i] == 0)
                {
                    source[i] = Convert.ToByte(34);
                }
            }

            return source;
        }

        public override string GetResponse()
        {
            throw new System.NotImplementedException();
        }
    }
}
