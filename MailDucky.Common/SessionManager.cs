using System;
using System.Collections.Generic;
using MailDucky.Common.Interfaces;

namespace MailDucky.Common
{
    public class SessionManager
    {
        private static readonly Lazy<SessionManager> _instance = new Lazy<SessionManager>(() => new SessionManager());
        public static SessionManager GetSessionManager { get { return _instance.Value; } }
        public List<IMailDuckySession> MailDuckySessions { get; set; }

        private SessionManager()
        {
            MailDuckySessions = new List<IMailDuckySession>();
        }

        public void StopSessions()
        {
            foreach (var session in MailDuckySessions)
            {
                session.StopSession();
            }
        }
    }
}
