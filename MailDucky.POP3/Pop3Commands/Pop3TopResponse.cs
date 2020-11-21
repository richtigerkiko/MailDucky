using System;
using System.Linq;
using System.Text.RegularExpressions;
using MailDucky.POP3.Enums;
using MailDucky.POP3.Utilities;

namespace MailDucky.POP3.Pop3Commands
{
    public class Pop3TopResponse : Pop3CommandBase
    {
        public Pop3TopResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            int messageId;
            int topcount;
            var args = Argument.Split(' ');
            if(args.Length > 0)
            {
                if (Session.SessionState == SessionState.TRANS)
                {
                    if (int.TryParse(args[0], out messageId))
                    {
                        if (Session.MessageStore.Count >= messageId)
                        {
                            var message = Session.MessageStore.ElementAt(messageId - 1).Value;
                            var splittetMessage = SplitHeaderAndContent(message);
                            if (splittetMessage.Length > 0 && int.TryParse(args[1], out topcount))
                            {
                                var topMessage = ShortenTextByLines(splittetMessage[1], topcount);
                                splittetMessage[1] = topMessage;
                            }
                            string[] returnArray = { splittetMessage[0], splittetMessage[1] };
                            return FormatRetrRespone(string.Join("/r/n", returnArray));
                        }
                        else return String.Format(Pop3Responses.MessageNotFound, messageId);
                    }
                    else return Pop3Responses.InvalidCommand;
                }
                else return Pop3Responses.NotAuthenticated;

            }
            else return Pop3Responses.InvalidCommand;
        }

        private string FormatRetrRespone(string message){
            var returnString = String.Format("+OK {0} octets", StringUtils.GetStringOctedSize(message));
            
            returnString += Session.NEWLINE;
            returnString += message;
            returnString += Session.TERMINATOR;

            return returnString;
        }

        private string[] SplitHeaderAndContent(string message)
        {
            var returnString = Regex.Split(message, "Content-Transfer-Encoding: quoted-printable");
            returnString[0] += "Content-Transfer-Encoding: quoted-printable /r/n";
            return returnString;
        }
        private string ShortenTextByLines(string message, int linecount)
        {
            var messageLines = message.Split('\n');
            var messageLinesToReturn = "";

            for(int i = 0; i < linecount && i < messageLines.Length; i++)
            {
                messageLinesToReturn += messageLines[i];
            }
            return messageLinesToReturn;
        }
    }
}