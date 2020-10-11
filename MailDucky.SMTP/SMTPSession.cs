using MailDucky.Common.Abstact;
using MailDucky.Common.AppOptions;
using MailDucky.Common.Interfaces;
using MailDucky.SMTP.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailDucky.SMTP
{
    public class SMTPSession: AbstractSession
    {

        public List<SMTPCommandBase> SMTPCommands { get; set; }
        public bool isAuthenticated = false;

        public SMTPData smtpData { get; set; }

        public SMTPSession(AppSettings _config)  :base (_config)
        {
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
            SMTPCommands = new List<SMTPCommandBase>();
            smtpData = new SMTPData();
    }

        public override async Task BeginSession(TcpClient client)
        {
            Logger.Debug("Starting Session");

            InitializeStreams(client);
            Timer = new System.Timers.Timer((double)60000)
            {
                AutoReset = false,
                Enabled = true
            };
            Timer.Elapsed += IdleTimeout;

            try
            {
                Logger.Info("Beginning Session with {0}", client.Client.RemoteEndPoint.ToString());

                var commandFactory = new SMTPCommandFactory(this);

                await SendResponseAsync(SMTPServerResponse.Welcome);

                while (!SMTPCommands.Any(x => x.Command == SMTPCommandType.QUIT.ToString()))
                {
                    Token.ThrowIfCancellationRequested();
                    var request = await ListenRequestAsync();

                    var SMTPCommand = commandFactory.Parse(request);
                    var response = await SMTPCommand.GetResponseAsync();

                    await SendResponseAsync(response);
                }
            }
            catch (OperationCanceledException)
            {
                await SendResponseAsync(SMTPServerResponse.Goodbye);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                await SendResponseAsync(SMTPServerResponse.Goodbye);
            }
            finally
            {
                this.Reader.Dispose();
                this.Writer.Dispose();
                this.NetworkStream.Close();
            }

            throw new System.NotImplementedException();
        }

        private void IdleTimeout(object sender, System.Timers.ElapsedEventArgs e)
        {
            StopSession();
        }
    }
}