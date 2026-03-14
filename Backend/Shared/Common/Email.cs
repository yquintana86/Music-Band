using System.Net.Mail;

namespace Shared.Common;

public sealed class Email
{
    public static bool IsValid(string emailAddress) 
    {
		try
		{
			var mail = new MailAddress(emailAddress);
			return mail.Address == emailAddress.Replace(" ","");
		}
		catch { return false; }
    }
}
