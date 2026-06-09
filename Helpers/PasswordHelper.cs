using System.Security.Cryptography;
using System.Text;

namespace SefIzFioke.Helpers
{
    /// <summary>
    /// Hashovanje lozinke (SHA256).
    /// NAPOMENA: Za produkciju koristiti BCrypt ili Argon2.
    /// </summary>
    public static class PasswordHelper
    {
        public static string Hash(string lozinka)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(lozinka));
            var sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static bool Verify(string lozinka, string hash)
        {
            return Hash(lozinka) == hash;
        }
    }
}
