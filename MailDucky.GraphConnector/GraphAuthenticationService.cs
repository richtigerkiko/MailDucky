using MailDucky.Common.AppOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MailDucky.GraphConnector
{
    public class GraphAuthenticationService: GraphConnectorBase
    {
        public GraphServiceClient graphClient;

        public GraphAuthenticationService(AppSettings _config) : base (_config)
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                                                                .Create(Settings.ClientID)
                                                                .WithTenantId(Settings.TenantId)
                                                                .WithClientSecret("u_BEJa7W2~DA4E3u~-3O_tI1N.ck9nKY9n")
                                                                .Build();

            var authProvider = new ClientCredentialProvider(confidentialClientApplication);
            graphClient = new GraphServiceClient(authProvider);
        }

        public async Task<User> GetUser(string username, string password)
        {
            try
            {
                var secureString = new System.Security.SecureString();
                foreach (var character in password)
                {
                    secureString.AppendChar(character);
                }
                return await graphClient.Me.Request()
                                .WithUsernamePassword(username, secureString)
                                .GetAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> GetUser(string username)
        {
            try
            {
                return await graphClient.Users[username].Request()
                                .GetAsync();
                //return await graphClient.Users[username].Request()
                //                .GetAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}