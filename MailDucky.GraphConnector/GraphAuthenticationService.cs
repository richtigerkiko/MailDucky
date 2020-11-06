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
                                                                .WithClientSecret(Settings.AppSecret)
                                                                .Build();

            var authProvider = new ClientCredentialProvider(confidentialClientApplication);
            graphClient = new GraphServiceClient(authProvider);
        }

        public async Task<User> GetUser(string username)
        {
            try
            {
                return await graphClient.Users[username].Request()
                                .GetAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}