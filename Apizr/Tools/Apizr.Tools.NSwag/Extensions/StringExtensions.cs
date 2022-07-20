using System.Text;
using System.Text.RegularExpressions;

namespace Apizr.Tools.NSwag.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string s)
        {
            string withoutSymbols = Regex.Replace(s, "[^a-zA-Z0-9]", " ");

            string[] words = withoutSymbols.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder(words.Sum(x => x.Length));

            foreach (string word in words)
            {
                sb.Append(word[0].ToString().ToUpper() + word.Substring(1));
            }

            return sb.ToString();
        }
    }
}
