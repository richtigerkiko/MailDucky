using Microsoft.Graph;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailDucky.GraphConnector.Utils
{
    public static class Converts
    {
        public static Message ConvertMimeToGraphMessage(MimeMessage mimeMessage)
        {
            var message = new Message();

            message.Subject = mimeMessage.Subject;


            if (mimeMessage.HtmlBody != null)
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
    }
}
