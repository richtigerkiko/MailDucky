using MailDucky.Common.AppOptions;
using MailDucky.Common;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace MailDucky.SMTP
{
    public class SMTPServer : BackgroundService
    {
        private readonly SMTPOptions config = new SMTPOptions();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private AppSettings AppSettings { get; set; }

        public SMTPServer(IConfiguration _config)
        {
            AppSettings = new AppSettings();
            _config.Bind(AppSettings);
            config = AppSettings.SMTP;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            var listener = new TcpListener(IPAddress.Loopback, config.Port);
            listener.Start();
            Logger.Debug("Starting SMTP Server");
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync();
                CreateSession(client);
            }
        }

        private void CreateSession(TcpClient client)
        {
            Task.Run(async () =>
            {
                var session = new SMTPSession(AppSettings)
                {

                };
                try
                {
                    SessionManager.GetSessionManager.MailDuckySessions.Add(session);
                    await session.BeginSession(client);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                finally
                {
                    SessionManager.GetSessionManager.MailDuckySessions.Remove(session);
                }
            });
        }
    }
}
