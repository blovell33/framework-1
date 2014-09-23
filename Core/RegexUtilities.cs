using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Core {
	public static class RegexUtilities {
		public static bool ValidateEmail(string email) {
			if (String.IsNullOrWhiteSpace(email)) {
				return false;
			}

			try {
				email = Regex.Replace(email, @"(@)(.+)$", EvaluateEmailMatch, RegexOptions.None, TimeSpan.FromMilliseconds(200));
			}
			catch (RegexMatchTimeoutException) {
				return false;
			}
			catch (ArgumentException) {
				return false;
			}

			try {
				return Regex.IsMatch(email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch (RegexMatchTimeoutException) {
				return false;
			}
		}

		/// <summary>
		/// Converts Unicode domain names to Ascii.
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		private static string EvaluateEmailMatch(Match match) {
			var mapping = new IdnMapping();
			var domainName = match.Groups[2].Value;

			domainName = mapping.GetAscii(domainName);

			return match.Groups[1].Value + domainName;
		}

		public static bool ValidateUrlPart(string part) {
			try {
				return Regex.IsMatch(part, @"^[a-z0-9_\.~]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));
			}
			catch (RegexMatchTimeoutException) {
				return false;
			}
		}
	}
}