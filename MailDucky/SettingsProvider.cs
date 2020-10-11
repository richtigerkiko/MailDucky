using System;
using System.IO;
using MailDucky.Common.AppOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailDucky
{
    public class SettingsProvider
    {
        private static readonly Lazy<SettingsProvider> _instance = new Lazy<SettingsProvider>(() => new SettingsProvider());
        public static SettingsProvider GetSettingsProvider { get { return _instance.Value; } }
        public AppSettings Settings { get; set; }

        private SettingsProvider()
        {
            Settings = new AppSettings();

            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            config.Bind(Settings);
        }
    }
}