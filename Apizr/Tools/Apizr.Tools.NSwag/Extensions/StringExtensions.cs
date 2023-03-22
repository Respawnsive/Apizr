using System.Text;
using System.Text.RegularExpressions;

namespace Apizr.Tools.NSwag.Extensions
{
    internal static class StringExtensions
    {
        public static string ToFormattedCase(this string s)
        {
            var withoutSymbols = Regex.Replace(s, "[^a-zA-Z]", " ");

            var words = withoutSymbols.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder(words.Sum(x => x.Length));

            foreach (var word in words)
            {
                sb.Append(word[0].ToString().ToUpper() + word[1..].ToLower());
            }

            return sb.ToString();
        }

        public static string ToApiName(this string s)
        {
            var name = !string.IsNullOrWhiteSpace(s)
                    ? s
                    : "Rest";

            var className = name
                .Replace("swagger", "", StringComparison.InvariantCultureIgnoreCase)
                .Replace("api", "", StringComparison.InvariantCultureIgnoreCase)
                .ToFormattedCase();

            var apiName = $"I{className}Api";

            return apiName;
        }
    }
}
