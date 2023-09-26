using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PAEG1
{
    public class GammaEncryption
    {
        public static string GenerateRandomKey(int length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public static string Encrypt(string message, string key)
        {
            byte[] messageBytes = Encoding.Unicode.GetBytes(message);
            byte[] keyBytes = Convert.FromBase64String(key);

            byte[] cipherBytes = new byte[messageBytes.Length];

            for (int i = 0; i < messageBytes.Length; i++)
            {
                cipherBytes[i] = (byte)(messageBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Convert.ToBase64String(cipherBytes);
        }

        public static string Decrypt(string cipherText, string key)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Convert.FromBase64String(key);

            byte[] decryptedBytes = new byte[cipherBytes.Length];

            for (int i = 0; i < cipherBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(cipherBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Encoding.Unicode.GetString(decryptedBytes);
        }
    }
}
