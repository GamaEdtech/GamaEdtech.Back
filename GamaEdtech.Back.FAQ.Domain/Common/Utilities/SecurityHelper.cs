using System.Security.Cryptography;
using System.Text;

namespace GamaEdtech.Back.FAQ.Domain.Common.Utilities
{
    public static class SecurityHelper
    {
        public static string GetSha256Hash(string input)
        {
            //using (var sha256 = new SHA256CryptoServiceProvider())
            using (var sha256 = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = sha256.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
        public static int NewRandomNumber(int First_interval, int Second_interval)
        {
            Random rand = new Random(); int MyCode = rand.Next(First_interval, Second_interval);
            return MyCode;
        }
        public static string GetFileGuid(int countGenerator = 4)
        {
            if (countGenerator > 0)
                return Guid.NewGuid().ToString().Substring(0, countGenerator).Replace("-", "");
            else
                return Guid.NewGuid().ToString().Replace("-", "");
        }
        public static string CreateRandString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (length-- > 0)
                res.Append(valid[rnd.Next(valid.Length)]);
            return res.ToString();
        }
    }
}
