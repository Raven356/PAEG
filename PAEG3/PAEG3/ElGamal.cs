using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG3
{
    public class ElGamal
    {
        public static Keys GetKeys()
        {
            BigInteger p = RandomPrimeGenerator.GenerateRandomPrime(600, 1000);
            BigInteger g = RandomPrimeGenerator.GenerateRandomPrime(1, (int)p);
            BigInteger x = RandomPrimeGenerator.GenerateRandomPrime(2, (int)(p - 2));
            BigInteger y = BigInteger.ModPow(g, x, p);

            return new Keys() { G = g, X = x, Y = y, P = p};
        }

        public static Bulletein Encrypt(string message, Keys keys)
        {
            Random random = new Random();

            BigInteger k = random.Next(2, (int)keys.P - 1);

            List<string> parts = new List<string>();
            
            if (message.Length > keys.P)
            {
                int j = 1;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < message.Length; i++)
                {
                    if (i < j * keys.P)
                    {
                        stringBuilder.Append(message[i]);
                    }
                    else
                    {
                        parts.Add(stringBuilder.ToString());
                        j++;
                        stringBuilder = new StringBuilder();
                    }
                }
            }
            else
            {
                parts.Add(message);
            }
            BigInteger a = BigInteger.ModPow(keys.G, k, keys.P);
            List<BigInteger> bs = new List<BigInteger>();
            StringBuilder sb = new StringBuilder();
            foreach (string part in parts)
            {
                foreach (var p in part)
                {
                    BigInteger b = BigInteger.ModPow((BigInteger.Pow(keys.Y, (int)k) * p), 1, keys.P);
                    bs.Add(b);
                    sb.Append((char)b);
                }
            }
            return new Bulletein { A = a, Bs = bs, Encrypted = sb.ToString()};
        }

        public static string Decrypt(Bulletein bulletein, Keys keys)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(var b in bulletein.Bs)
            {
                stringBuilder.Append((char)(BigInteger.ModPow(b * BigInteger.Pow(bulletein.A, (int)(keys.P - 1 - keys.X)), 1, keys.P)));
            }

            return stringBuilder.ToString();
        }
    }
}
