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
            IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder
                                                                .Create(Settings.ClientID)
                                                                .WithTenantId(Settings.TenantId)
                                                                .Build();

            var authProvider = new UsernamePasswordProvider(publicClientApplication, null);
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
    }
}