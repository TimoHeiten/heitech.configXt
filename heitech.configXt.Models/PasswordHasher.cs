using System.Security.Cryptography;
using System.Text;

namespace heitech.configXt.Models
{
    public static class PasswordHasher
    {
        public static string GenerateHash(string password)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(password);
            var sha1data = sha1.ComputeHash(bytes);

            sha1.Clear();
            return password;
        }
    }
}