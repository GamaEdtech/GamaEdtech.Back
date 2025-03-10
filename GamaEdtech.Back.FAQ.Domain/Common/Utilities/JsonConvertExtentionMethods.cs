using System.Globalization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Web;

namespace GamaEdtech.Back.FAQ.Domain.Common.Utilities
{
    public static class ConvertHelpers
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            return JsonSerializer.Deserialize<T>(await httpContent.ReadAsStringAsync());
        }

        public static string WriteObject<T>(T anObject)
        {
            var js = new DataContractJsonSerializer(typeof(T));
            using var ms = new MemoryStream();
            js.WriteObject(ms, anObject);
            return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
        }

        public static T ReadObject<T>(string jsonData)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonData);
            writer.Flush();
            stream.Position = 0;
            return (T)js.ReadObject(stream);
        }
        public static T DeserializeJson<T>(this string str, bool isReturnDefaultIfNull = true) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    if (isReturnDefaultIfNull)
                        return default;
                    else
                        return null;
                }

                T bsObj2 = null;

                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(str)))
                {
                    // Deserialization from JSON
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
                    bsObj2 = (T)deserializer.ReadObject(ms);
                }
                return bsObj2;
            }
            catch (Exception ex)
            {
                if (isReturnDefaultIfNull)
                    return default;
                else
                    return null;
            }

        }
        public static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
            }

            return result;
        }

        public static string BytesToHexString(byte[] input)
        {
            var hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(string.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }

        public static string CleanResultString(this string str)
        {
            try
            {
                var myWriter = new StringWriter();
                HttpUtility.HtmlDecode(str, myWriter);
                string myDecodedString = myWriter.ToString();

                str = System.Net.WebUtility.HtmlDecode(myDecodedString);

                str = str.Replace("\"{\\\"", "{\"");
                str = str.Replace("\\\"", "\"");
                str = str.Replace("}\"", "}");
                return str;
            }
            catch (Exception ex)
            {

                return str;
            }
        }
    }
}
