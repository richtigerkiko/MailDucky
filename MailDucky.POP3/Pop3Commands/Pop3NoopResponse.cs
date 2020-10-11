using System.Threading.Tasks;
using MailDucky.POP3.Enums;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3NoopResponse : Pop3CommandBase
    {
        public Pop3NoopResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            if(Session.SessionState == SessionState.TRANS) return Pop3Responses.OK;
            else return Pop3Responses.NotAuthenticated;
        }
    }
}