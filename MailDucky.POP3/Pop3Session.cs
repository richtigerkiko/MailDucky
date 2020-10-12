using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using MailDucky.Common.Abstact;
using MailDucky.Common.AppOptions;
using MailDucky.POP3.Enums;
using MailDucky.POP3.Interfaces;
using Microsoft.Graph;

namespace MailDucky.POP3
{
    public class Pop3Session : AbstractSession
    {

        public const int Timeout = 60000;
        public SessionState SessionState { get; internal set; }
        public Dictionary<string, string> MessageStore { get; set; }
        public List<string> MessageIdsMarkedForDeletion { get; internal set; }

        public List<Pop3CommandBase> Pop3Commands { get; set; }

        public Pop3Session(AppSettings _config) : base(_config)
        {
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
            SessionState = SessionState.AUTH;
            Pop3Commands = new List<Pop3CommandBase>();
            MessageStore = new Dictionary<string, string>();
            MessageIdsMarkedForDeletion = new List<string>();
        }

        public override async Task BeginSession(TcpClient client)
        {
            InitializeStreams(client);
            Timer = new System.Timers.Timer((double)Timeout)
            {
                AutoReset = false,
                Enabled = true
            };
            Timer.Elapsed += IdleTimeout;

            try
            {
                Logger.Info("Beginning Session with {0}", client.Client.RemoteEndPoint.ToString());

                var commandFactory = new Pop3CommandFactory(this);

                await SendResponseAsync(Pop3Responses.Announcement);

                while (!Pop3Commands.Any(x => x.Command == Pop3CommandType.QUIT.ToString()))
                {
                    Token.ThrowIfCancellationRequested();
                    var request = await ListenRequestAsync();

                    Timer.Stop();
                    Timer.Start();

                    var pop3Command = commandFactory.Parse(request);
                    var response = await pop3Command.GetResponseAsync();

                    await SendResponseAsync(response);
                }
            }
            catch (OperationCanceledException)
            {
                await SendResponseAsync("+ERR Connection Closing");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                await SendResponseAsync("+ERR Server Error");
            }
            finally
            {
                this.Reader.Dispose();
                this.Writer.Dispose();
                this.NetworkStream.Close();
            }
        }

        private void IdleTimeout(object sender, System.Timers.ElapsedEventArgs e)
        {
            StopSession();
        }
    }
}
