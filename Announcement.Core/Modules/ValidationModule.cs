using System;
using System.Text.RegularExpressions;

namespace Announcement.Core
{
	public class ValidationModule
	{
		public ValidationModule ()
		{
		}

		public static bool ValidateUserName(string userName)
		{
			const string regexPattern = @"^[a-zA-Z0-9]+$";

			var match = Regex.Match (userName, regexPattern);

			return match.Success;
		}

		public static bool ValidatePassword(string password)
		{
			const string regexPattern = @"^[\x00-\x7F]+$";

			var match = Regex.Match (password, regexPattern);

			return match.Success;
		}
	}
}

