
namespace CRM.Core
{
   using System;
   using System.Linq;
   using System.Text;
   using System.Text.RegularExpressions;

    public class TextManager
    {
        public static string CreateSEOTitle(string title)
        {
            var match = Regex.Match(title.ToLower(), "[\\w]+");
            var result = new StringBuilder("");
            while (match.Success)
            {
                result.Append(match.Value + "-");
                match = match.NextMatch();
            }

            if (result[result.Length - 1] == '-') result.Remove(result.Length - 1, 1);
            {
                return result.ToString();
            }
        }

        public static string TextEllipsis(string text, int maxLength)
        {
            if( text.Trim().Count() > maxLength)
            {
                return text.Substring(0, maxLength) + "...";
            }

            return text.Trim();
        }
        /// <summary>
        /// Generate random 10 digit text
        /// </summary>
        /// <returns></returns>
        public  static string GetRandomText()
        {
            var builder = new StringBuilder();
            var random = new Random();
            var legalCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char character;

            for (int i = 0; i < 10; i++)
            {
                character = legalCharacters[random.Next(0, legalCharacters.Length)];
                builder.Append(character);
            }

            return builder.ToString();
        }
    }
}
