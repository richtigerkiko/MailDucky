using System.Threading.Tasks;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3RsetResponse : Pop3CommandBase
    {
        public Pop3RsetResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            Session.MessageIdsMarkedForDeletion.Clear();
            return string.Format(Pop3Responses.OK, "");
        }

    }
}