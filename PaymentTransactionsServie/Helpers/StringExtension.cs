using System.Text.RegularExpressions;

namespace PaymentTransactionsServie.Helpers
{
	internal static class StringExtension
	{
		public static string[] CuttAddressAndFormat(this string str, out string address)
		{
			var pattern = @"“(.*?)”,";
			var match = Regex.Match(str, pattern);
			address = match.Groups[1].Value;
			str = str.Replace(match.Groups[0].Value, "");

			return Regex.Replace(str, @"[\s“”]+", "").Split(',');
			
		}
	}
}
