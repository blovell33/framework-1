using System.Linq;
using System.Text.RegularExpressions;

namespace Core {
	public static class UtilityExtensions {
		public static string ToPascalCase(this string s) {
			if (string.IsNullOrWhiteSpace(s)) {
				return null;
			}

			var tokens =
				Regex
					.Split(s, @"\s+")
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Select(x => char.ToUpper(x[0]) + x.Substring(1));

			return string.Join("", tokens);
		}

		public static string ToCamelCase(this string s) {
			if (string.IsNullOrWhiteSpace(s)) {
				return null;
			}

			var tokens =
				Regex
					.Split(s, @"\s+")
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Select((x, i) => i == 0 ? char.ToLower(x[0]) + x.Substring(1) : char.ToUpper(x[0]) + x.Substring(1));

			return string.Join("", tokens);
		}

		public static string ToSentenceCase(this string s) {
			if (string.IsNullOrWhiteSpace(s)) {
				return null;
			}

			s = Regex.Replace(s.Trim(), @"\s+", " ");
			s = Regex.Replace(s.Trim(), "[A-Z]", m => " " + char.ToLower(m.Value[0]));
			s = s.Trim();

			return char.ToUpper(s[0]) + s.Substring(1);
		}
	}
}