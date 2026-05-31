using System;
using System.Security.Cryptography;

namespace Vendor_Link_Point.Helpers
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16; // 128 bit-es só
        private const int KeySize = 32;  // 256 bit-es hash
        private const int Iterations = 100000; // 100 000 körös fűszerezés (biztonságos iparági standard)
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimiter = ':';

        // Új, sózott és iterált jelszó generálása (Regisztrációkor)
        public static string HashPassword(string password)
        {
            // 1. Véletlenszerű só generálása
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            // 2. A jelszó hashelése a sóval és a megadott iterációszámmal
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            // 3. Visszaadjuk a Só és a Hash összekapcsolt stringjét (Base64 formátumban)
            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        // Jelszó ellenőrzése (Belépéskor)
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Szétválasztjuk az adatbázisból jövő stringet Sóra és Hashre
            var elements = hashedPassword.Split(Delimiter);
            if (elements.Length != 2)
            {
                return false; // Ha nincs benne a ':', akkor ez még egy régi (sózatlan) jelszó, amit elutasítunk
            }

            byte[] salt = Convert.FromBase64String(elements[0]);
            byte[] hash = Convert.FromBase64String(elements[1]);

            // Vesszük a beírt jelszót, és ráküldjük UGYANAZT a sót és iterációt
            byte[] hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            // Összehasonlítjuk, hogy a beírt jelszó hash-e megegyezik-e az adatbázisban lévővel
            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }
    }
}