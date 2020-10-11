using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MailDucky.Common;
using MailDucky.Common.AppOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MailDucky.POP3
{
    public class Pop3Server: BackgroundService
    {
        private readonly Pop3Options config = new Pop3Options();
        private AppSettings AppSettings { get; set; }
        public Pop3Server(IConfiguration _config)
        {
            AppSettings = new AppSettings();
            _config.Bind(AppSettings);
            config = AppSettings.Pop3;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var listener = new TcpListener(IPAddress.Loopback, config.Port);
            listener.Start();

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
                var session = new Pop3Session(AppSettings)
                {

                };
                try
                {
                    SessionManager.GetSessionManager.MailDuckySessions.Add(session);
                    await session.BeginSession(client);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    SessionManager.GetSessionManager.MailDuckySessions.Remove(session);
                }
            });
        }
    }
}
