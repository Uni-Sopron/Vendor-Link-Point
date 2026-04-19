using System;
using System.Linq;

namespace Vendor_Link_Point.Helpers
{
    public static class StringHelper
    {
        // Generál egy megadott hosszúságú, nagybetűkből és számokból álló kódot
        public static string GenerateRandomId(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            string generated = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"VLP-{generated}"; // Egy kis előtag, pl: VLP-A8F3X9Q1
        }
    }
}
