using System;
using System.Collections.Generic;
using System.Text;

namespace MailDucky.SMTP.Enums
{
    public enum SMTPCommandType
    {
        // Minimal Implementation
        EHLO,
        HELO,
        MAIL,
        RCPT,
        DATA,
        RSET,
        NOOP,
        QUIT,
        VRFY,
        // Extensions
        AUTH,
        // Internal Usage
        INVALID
    }
}
