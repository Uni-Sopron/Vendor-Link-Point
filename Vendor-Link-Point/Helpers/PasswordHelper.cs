using System.Security.Cryptography;
using System.Text;

namespace Vendor_Link_Point.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPasswordSHA256(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // A jelszó byte-tömbbé alakítása
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // A byte-tömb visszaalakítása olvasható hexadecimális stringgé
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
