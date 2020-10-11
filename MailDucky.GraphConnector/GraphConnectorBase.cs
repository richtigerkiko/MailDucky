
using MailDucky.Common.AppOptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace MailDucky.GraphConnector
{
    public abstract class GraphConnectorBase
    {
        public GraphConnectorOptions Settings { get; set; }
        protected GraphConnectorBase(AppSettings settings) 
        {
            Settings = settings.GraphConnector;
        }
    }
}