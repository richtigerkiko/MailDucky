using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailDucky.Common.AppOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using MimeKit;

namespace MailDucky.GraphConnector
{
    public class GraphMailingService : GraphConnectorBase
    {
        private static GraphServiceClient graphClient;

        public GraphMailingService(GraphServiceClient _graphClient, AppSettings _config) : base (_config)
        {
            graphClient = _graphClient;
        }

        public async Task SendMail(MimeMessage message)
        {
            var graphMessage = ConvertMimeToGraphMessage(message);

            await graphClient.Me
                .SendMail(graphMessage, true)
                .Request()
                .PostAsync();
        }

        public Message ConvertMimeToGraphMessage(MimeMessage mimeMessage)
        {
            var message = new Message();

            message.Subject = mimeMessage.Subject;
            

            if(mimeMessage.HtmlBody != null)
            {
                message.Body = new ItemBody() 
                { 
                    ContentType = BodyType.Html,
                    Content = mimeMessage.HtmlBody
                };
            }
            else
            {
                message.Body = new ItemBody()
                {
                    ContentType = BodyType.Text,
                    Content = mimeMessage.TextBody
                };
            }
            var recipientList = new List<Recipient>();
            mimeMessage.To.ToList().ForEach(x =>
            {
                recipientList.Add(new Recipient() 
                {
                    EmailAddress = new EmailAddress()
                    {
                        Address = x.Name
                    }
                });
            });
            message.ToRecipients = recipientList;

            var ccRecipient = new List<Recipient>();
            mimeMessage.Cc.ToList().ForEach(x =>
            {
                ccRecipient.Add(new Recipient()
                {
                    EmailAddress = new EmailAddress()
                    {
                        Address = x.Name
                    }
                });
            });
            message.CcRecipients = ccRecipient;


            return message;
        }

        public async Task RemoveMailsAsync(List<string> mailIdsToDelete, Dictionary<string, string> messageStore)
        {
            var categories = await graphClient.Me.Outlook.MasterCategories
                                        .Request()
                                        .GetAsync();
            if (!categories.Any(x => x.DisplayName == Settings.DeletedMessageCategory))
            {
                var outlookCategory = new OutlookCategory
                {
                    DisplayName = Settings.DeletedMessageCategory,
                    Color = CategoryColor.Preset6
                };
                await graphClient.Me.Outlook.MasterCategories.Request().AddAsync(outlookCategory);
            }

            foreach (var mailId in mailIdsToDelete)
            {
                var message = new Message()
                {
                    Categories = new List<string>(){ Settings.DeletedMessageCategory }
                };
                await graphClient.Me.Messages[mailId].Request().UpdateAsync(message);
            }

        }

        public async Task MoveMailsToArchiveAsync(List<string> messageIds)
        {
            foreach(var messageId in messageIds)
            {
                MoveMailToArchiveAsync(messageId);
            }
        }

        public async Task MoveMailToArchiveAsync(string messageId)
        {
            try
            {
                const string ArchiveFolderName = "archive";
                await graphClient.Me.Messages[messageId].Move(ArchiveFolderName).Request().PostAsync();
            }
            catch (Exception ex)
            {

            }
        }

        // This function returns a Dictionary of messageID, Email Message as MIME Message
        public async Task<Dictionary<string, string>> GetMailsAsync(User user)
        {

            //var messages = await graphClient.Me
            //                                .MailFolders
            //                                .Inbox
            //                                .Messages
            //                                .Request()
            //                                .GetAsync();
            var messages = await graphClient.Users[user.Id].MailFolders.Inbox.Messages.Request().GetAsync();

            var mailList = new Dictionary<string, string>();
            foreach (var message in messages)
            {
                var messageMime = await GetMimeWorkaround(message.Id);

                // In the future it should be possible to do without a workaround
                //var messageMime = await graphClient.Me.Messages[message.Id].Content.Request().GetAsync();

                var tester = await graphClient.Me.Messages[message.Id].Request().GetAsync();
                mailList.Add(message.Id, messageMime);
            }
            return mailList;
        }

        // workaround needed as Graph SDK currently doesnt support MIME Responses
        private async Task<string> GetMimeWorkaround(string messageId)
        {
            var request = graphClient.Me.Messages[messageId].Request().GetHttpRequestMessage();
            request.RequestUri = new Uri(request.RequestUri.OriginalString + "/$value");
            var response = await graphClient.HttpProvider.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}