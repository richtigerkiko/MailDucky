using System;
using System.Linq;
using MailDucky.POP3.Enums;
using MailDucky.POP3.Utilities;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3RetrResponse : Pop3CommandBase
    {
        public Pop3RetrResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            int messageId;
            if(Session.SessionState == SessionState.TRANS){
                if(int.TryParse(Argument, out messageId)){
                    if(Session.MessageStore.Count >= messageId){
                        return FormatRetrRespone(Session.MessageStore.ElementAt(messageId - 1).Value);
                    }
                    else return String.Format(Pop3Responses.MessageNotFound, messageId);
                }
                else return Pop3Responses.InvalidCommand;
            }
            else return Pop3Responses.NotAuthenticated;
        }

        private string FormatRetrRespone(string message){
            var returnString = String.Format("+OK {0} octets", StringUtils.GetStringOctedSize(message));
            
            returnString += Session.NEWLINE;
            returnString += message;
            returnString += Session.TERMINATOR;

            return returnString;
        }
    }
}