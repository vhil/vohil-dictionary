namespace Pintle.Dictionary.Extensions
{
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;

	internal static class StringExtensions
	{
		public static string ToPascalCase(this string s)
		{
			var result = new StringBuilder();
			var nonWordChars = new Regex(@"[^a-zA-Z0-9]+");
			var tokens = nonWordChars.Split(s);
			foreach (var token in tokens)
			{
				result.Append(token.PascalCaseSingleWord());
			}

			return result.ToString();
		}

		public static string PascalCaseSingleWord(this string s)
		{
			var match = Regex.Match(s, @"^(?<word>\d+|^[a-z]+|[A-Z]+|[A-Z][a-z]+|\d[a-z]+)+$");
			var groups = match.Groups["word"];

			var textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
			var result = new StringBuilder();
			foreach (var capture in groups.Captures.Cast<Capture>())
			{
				result.Append(textInfo.ToTitleCase(capture.Value.ToLower()));
			}
			return result.ToString();
		}
	}
}
