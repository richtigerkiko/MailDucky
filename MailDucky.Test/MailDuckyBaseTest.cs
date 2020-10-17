using MailDucky.Common.AppOptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.Test
{
    public class MailDuckyBaseTest
    {
        public AppSettings config;

        public MailDuckyBaseTest ()
        {
            config = new AppSettings();
            var configuratioFile = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            configuratioFile.Bind(config);
        }

    }
}
