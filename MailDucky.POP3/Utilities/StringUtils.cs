using System.Text;

namespace MailDucky.POP3.Utilities
{
    public static class StringUtils
    {
        public static int GetStringOctedSize(string stringToCount)
        {
            return Encoding.UTF8.GetByteCount(stringToCount);
        }
    }
}