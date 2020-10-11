using MailDucky.POP3;
using MailDucky.SMTP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Globalization;
using System.IO;
using System.Threading;

namespace MailDucky.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Pop3Server>();
                    services.AddHostedService<SMTPServer>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddNLog();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
                    config.Build();
                });
    }
}
