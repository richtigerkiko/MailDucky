using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailDucky.Common.AppOptions;
using MailDucky.GraphConnector.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using MimeKit;

namespace MailDucky.GraphConnector
{
    public class GraphMailingService : GraphConnectorBase
    {
        private static GraphServiceClient graphClient;
        private static User user;

        public GraphMailingService(GraphServiceClient _graphClient, AppSettings _config, User _user) : base (_config)
        {
            graphClient = _graphClient;
            user = _user;
        }

        public async Task SendMail(MimeMessage message)
        {
            var graphMessage = Converts.ConvertMimeToGraphMessage(message);

            await graphClient.Users[user.Id]
                .SendMail(graphMessage, true)
                .Request()
                .PostAsync();
        }

        public async Task MoveMailsToArchiveAsync(List<string> messageIds)
        {
            foreach(var messageId in messageIds)
            {
                try
                {
                    const string ArchiveFolderName = "archive";
                    await graphClient.Users[user.Id]
                            .Messages[messageId]
                            .Move(ArchiveFolderName)
                            .Request()
                            .PostAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }


        // This function returns a Dictionary of messageID, Email Message as MIME Message
        public async Task<Dictionary<string, string>> GetMailsAsync()
        {

            var messages = await graphClient.Users[user.Id]
                                    .MailFolders
                                    .Inbox.Messages
                                    .Request()
                                    .GetAsync();

            var mailList = new Dictionary<string, string>();
            foreach (var message in messages)
            {
                var messageMime = await GetMimeWorkaround(message.Id);
                mailList.Add(message.Id, messageMime);
            }
            return mailList;
        }

        // workaround needed as Graph SDK currently doesnt support MIME Responses
        private async Task<string> GetMimeWorkaround(string messageId)
        {
            var request = graphClient.Users[user.Id].Messages[messageId].Request().GetHttpRequestMessage();
            request.RequestUri = new Uri(request.RequestUri.OriginalString + "/$value");
            var response = await graphClient.HttpProvider.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}