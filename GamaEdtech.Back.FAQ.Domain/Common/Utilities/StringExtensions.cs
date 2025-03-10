using System.Text.RegularExpressions;
namespace GamaEdtech.Back.FAQ.Domain.Common.Utilities
{
    public static class StringExtensions
    {
        public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }

        public static bool HasValues(bool ignoreWhiteSpace, params string[] values)
        {
            return ignoreWhiteSpace ? values.All(a => !string.IsNullOrWhiteSpace(a))
                : values.All(a => !string.IsNullOrEmpty(a));
        }

        public static string TrimEnd(this string source, string value)
        {
            while (source.EndsWith(value, StringComparison.OrdinalIgnoreCase))
                source = source.Substring(0, source.Length - value.Length);
            return source;
        }

        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }

        public static decimal ToDecimal(this string value)
        {
            return Convert.ToDecimal(value);
        }

        public static string ToNumeric(this int value)
        {
            return value.ToString("N0"); //"123,456"
        }

        public static string ToNumeric(this long value)
        {
            return value.ToString("N0"); //"123,456"
        }

        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            char[] array = s.ToCharArray();
            for (int i = 0; i < array.Length && (i != 1 || char.IsUpper(array[i])); i++)
            {
                bool flag = i + 1 < array.Length;
                if (i > 0 && flag && !char.IsUpper(array[i + 1]))
                {
                    break;
                }

                array[i] = char.ToLowerInvariant(array[i]);
            }

            return new string(array);
        }

        public static string ToNumeric(this decimal value)
        {
            return value.ToString("N0"); //"123,456"
        }

        public static string ToCurrency(this int value)
        {
            //fa-IR => current culture currency symbol => ریال
            //123456 => "123,123ریال"
            return value.ToString("C0");
        }

        public static string ToCurrency(this decimal value)
        {
            return value.ToString("C0");
        }

        public static string En2Fa(this string str)
        {
            return str.Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string Fa2En(this string str)
        {
            return str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                //iphone numeric
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }

        public static string FixPersianChars(this string str)
        {
            return str.Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace(" ", " ")
                .Replace("‌", " ")
                .Replace("ھ", "ه");//.Replace("ئ", "ی");
        }

        public static Guid ToGuid(this string str)
        {
            try
            {
                return Guid.Parse(str);
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public static string CleanString(this string str)
        {
            return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
        }

        public static string NullIfEmpty(this string str)
        {
            return str?.Length == 0 ? null : str;
        }

        public static string Fix(string text)
        {
            if (text is null)
            {
                return null;
            }

            text =
                text.Trim();

            if (text == string.Empty)
            {
                return null;
            }

            while (text.Contains("  "))
            {
                text =
                    text.Replace("  ", " ");
            }

            return text;
        }

        public static string ConvertToPersian(this string searchTerm)
        {

            var newString = searchTerm
                .ToCharArray()
                .Aggregate("", (current, charItem) => current + charItem switch
                {
                    'Q' => 'ض',
                    'q' => 'ض',
                    'W' => 'ص',
                    'w' => 'ص',
                    'E' => 'ث',
                    'e' => 'ث',
                    'R' => 'ق',
                    'r' => 'ق',
                    'T' => 'ف',
                    't' => 'ف',
                    'Y' => 'غ',
                    'y' => 'غ',
                    'U' => 'ع',
                    'u' => 'ع',
                    'I' => 'ه',
                    'i' => 'ه',
                    'O' => 'خ',
                    'o' => 'خ',
                    'P' => 'ح',
                    'p' => 'ح',
                    '{' => 'ج',
                    '[' => 'ج',
                    '}' => 'چ',
                    ']' => 'چ',
                    'A' => 'ش',
                    'a' => 'ش',
                    'S' => 'س',
                    's' => 'س',
                    'D' => 'ی',
                    'd' => 'ی',
                    'F' => 'ب',
                    'f' => 'ب',
                    'G' => 'ل',
                    'g' => 'ل',
                    'H' => 'ا',
                    'h' => 'ا',
                    'J' => 'ت',
                    'j' => 'ت',
                    'K' => 'ن',
                    'k' => 'ن',
                    'L' => 'م',
                    'l' => 'م',
                    ';' => 'ک',
                    ':' => 'ک',
                    '\'' => 'گ',
                    '\"' => 'گ',
                    'Z' => 'ظ',
                    'z' => 'ظ',
                    'X' => 'ط',
                    'x' => 'ط',
                    'C' => 'ز',
                    'c' => 'ز',
                    'V' => 'ر',
                    'v' => 'ر',
                    'B' => 'ذ',
                    'b' => 'ذ',
                    'N' => 'د',
                    'n' => 'د',
                    'M' => 'پ',
                    'm' => 'پ',
                    '<' => 'و',
                    '\\' => 'پ',
                    'ي' => 'ی',
                    'ك' => 'ک',
                    _ => charItem
                });
            return newString;
        }
        public static string SqlReplacement(this string term)
        {
            return term != null ? $"%{term.Replace("ا", "[ا|آ]")}%" : "%%";
        }

        public static string DecodeAmiFormatPhoneNumber(this string callerIdNum)
        {
            var tenLastChars = callerIdNum?.ToArray().TakeLast(10).ToArray();
            if (tenLastChars?[0] == '9' && tenLastChars.Length == 10)
            {
                return $"0{new string(tenLastChars)}";
            }
            else if (tenLastChars?[0] != '9' && tenLastChars.Length < 4)
            {
                return new string(tenLastChars);
            }
            else
            {
                return $"0{new string(tenLastChars)}";
            }
        }

        public static bool IsNumber(this string stringValue)
        {
            var pattern = @"^-?[0-9]+(?:\.[0-9]+)?$";
            var regex = new Regex(pattern);
            return regex.IsMatch(stringValue);
        }

        public static DateTime ToDateTime(this string value)
        {
            return DateTime.Parse(value);
        }
    }
}
