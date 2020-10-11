using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailDucky.GraphConnector;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3StatResponse : Pop3CommandBase
    {
        public Pop3StatResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            return FormatPop3StatResponse(Session.MessageStore);
        }

        public string FormatPop3StatResponse(Dictionary<string, string> mailList)
        {

            var octedSize = 0;
            if (mailList.Count > 0)
            {
                for (int i = 0; i < mailList.Count; i++)
                {
                    var mail = mailList.ElementAt(i);
                    // skip if marked for deletion
                    if (Session.MessageIdsMarkedForDeletion.Any(x => x == mail.Key))
                    {
                        continue;
                    }

                    octedSize = octedSize + Encoding.UTF8.GetByteCount(mail.Value);
                }
            }
            return string.Format("+OK {0} {1}", mailList.Count, octedSize);
        }
    }
}