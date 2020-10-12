using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailDucky.GraphConnector;
using MailDucky.POP3.Enums;
using MailDucky.POP3.Utilities;

namespace MailDucky.POP3.Pop3Commands
{
    /// <summary>
    /// RFC: https://tools.ietf.org/html/rfc1939#page-12
    /// </summary>
    public class Pop3UIDLResponse : Pop3CommandBase
    {
        public Pop3UIDLResponse(string command, string argument) : base(command, argument)
        {
        }

        public override string GetResponse()
        {
            if (Session.SessionState == SessionState.TRANS)
            {
                return FormatPop3MessageResponse(Session.MessageStore);
            }
            else return Pop3Responses.NotAuthenticated;
        }

        private string FormatPop3MessageResponse(Dictionary<string, string> messageList)
        {
            // If no argument is given return list of UIDS
            if(Argument == string.Empty)
            {
                // return one OK first
                Session.SendResponse(Pop3Responses.OK);

                // then send a multiline answer ended by the Terminator
                var mailcounter = 0;
                var returnString = string.Empty;
                foreach (var message in messageList)
                {
                    mailcounter++;
                    
                    // if message is marked for deletion, skip
                    if (Session.MessageIdsMarkedForDeletion.Any(x => x == message.Key))
                    {
                        continue;
                    }

                    returnString += string.Format("{0} {1}", mailcounter, ShortenMicrosoftGraphId(message.Key));
                }

                // add Terminator
                returnString += Session.TERMINATOR;
                return returnString;
            }
            else
            {
                var messageListNumber = 0;
                if(int.TryParse(Argument, out messageListNumber))
                {
                    if(messageListNumber <= messageList.Count)
                    {
                        return string.Format(Pop3Responses.OK + " {1}", messageListNumber.ToString(), ShortenMicrosoftGraphId(messageList.ElementAt(messageListNumber - 1).Key));
                    }
                    else
                    {
                        return Pop3Responses.MessageNotFound;
                    }
                }
                else
                {
                    return Pop3Responses.InvalidCommand;
                }
            }
        }

        /// <summary>
        /// Microsoft.Graph Message IDs are 152 Characters Long but the RFID only allows for 70 Characters, so we have to truncate.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string ShortenMicrosoftGraphId(string id)
        {
            return id.Substring(0, 70);
        }
    }
}