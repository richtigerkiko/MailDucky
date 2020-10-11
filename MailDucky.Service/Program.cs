using MailDucky.POP3;
using MailDucky.SMTP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using System.Threading;
using MailDucky.API;

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
                    // Add Pop3Server to Host Services
                    services.AddHostedService<Pop3Server>();

                    // Add SMTPServer to Host Services
                    services.AddHostedService<SMTPServer>();
                })
                .ConfigureLogging(logging =>
                {
                    // Configure NLOG
                    logging.ClearProviders();
                    logging.AddNLog();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureAppConfiguration(config =>
                {
                    // Configuring Appsettings JSON
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
                    config.Build();
                });
                //.ConfigureWebHostDefaults(webBuilder =>
                //{
                //    // Web api Host to control Services and Configure stuff
                //    // not implemented !
                //    // webBuilder.UseStartup<Startup>();
                //});
    }
}
