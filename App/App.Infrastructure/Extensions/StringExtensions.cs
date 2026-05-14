using System.Text.RegularExpressions;

namespace App.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToReadable(this string input)
        {
            return Regex.Replace(input, "(\\B[A-Z])", " $1");
        }
        public static bool Compare(this string source, string value)
        {
            return $"{source}".ToLower() == $"{value}".ToLower();
        }
    }
}
