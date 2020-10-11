using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP
{
    public class SMTPData
    {
        public string Sender { get; set; }
        public List<string> Recipients { get; set; }
        public string RawMessage { get; set; }

        public SMTPData()
        {
            Recipients = new List<string>();
        }
    }
}
