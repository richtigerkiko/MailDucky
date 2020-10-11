using MailDucky.Common.AppOptions;
using MailDucky.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailDucky.Common.Abstact
{
    public abstract class AbstractSession : IMailDuckySession
    {
        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // Setting Newline and Terminator for Pop3 and SMTP
        public string NEWLINE = "\r\n";
        public string TERMINATOR = "\r\n.\r\n";

        public StreamReader Reader { get; internal set; }
        public StreamWriter Writer { get; internal set; }
        public System.Timers.Timer Timer { get; set; }
        public NetworkStream NetworkStream { get; set; }
        public CancellationTokenSource TokenSource { get; set; }
        public CancellationToken Token { get; set; }
        public GraphServiceClient GraphClient { get; set; }
        public User User { get; set; }
        public AppSettings Settings {get; set;}

        public abstract Task BeginSession(TcpClient client);

        protected AbstractSession(AppSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// This will stop Sessions
        /// </summary>
        public void StopSession()
        {
            Logger.Debug("Stopping Session");
            TokenSource.Cancel();
        }

        protected void InitializeStreams(TcpClient client)
        {
            Logger.Info("Starting Network Stream with {0}", client.Client.RemoteEndPoint.ToString());
            try
            {
                NetworkStream = client.GetStream();
                Writer = new StreamWriter(NetworkStream, new ASCIIEncoding())
                {
                    AutoFlush = true,
                    NewLine = NEWLINE
                };

                Reader = new StreamReader(NetworkStream, Encoding.ASCII);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        public async Task SendResponseAsync(string response)
        {
            Logger.Debug("Sending Response: {0}", response);
            await Writer.WriteLineAsync(response);
        }

        public void SendResponse(string response)
        {
            Logger.Debug("Sending Response: {0}", response);
            Writer.WriteLine(response);
        }

        public async Task<string> ListenRequestAsync()
        {


            Timer.Start();

            var response = string.Empty;

            response = await Reader.ReadLineAsync();

            Timer.Stop();

            Console.WriteLine("Client: {0}", response);

            return response;
        }
    }
}
