namespace MailDucky.Common.AppOptions
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public GraphConnectorOptions GraphConnector { get; set; }
        public Pop3Options Pop3 { get; set; }
        public SMTPOptions SMTP { get; set; }
    }
}