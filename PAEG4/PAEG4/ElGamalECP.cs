using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PAEG4
{
    internal class ElGamalECP
    {
        public EncryptedECP BeginECP(string message)
        {
            var keys = GetECPKeys();
            var messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hash;

            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(messageBytes);
            }

            BigInteger k = GetRandomCoprime(keys.P);
            BigInteger r = BigInteger.ModPow(keys.G, k, keys.P);
            BigInteger kOb = ExtendedEucludian.FindMultiplicativeInverse(k, keys.P - 1);
            List<BigInteger> s = new List<BigInteger>();
            BigInteger test = 0;
            foreach (var h in hash)
            {
                test += h;
                
            }
            var part = (test - keys.X * r) * kOb;
            var x = keys.P - 1;
            var division = part % (x);
            var res = division < 0 ? x + division : division;
            s.Add(res );


            return new EncryptedECP { R = r, S = s, ECPKeys = keys };
        }

        public bool VerifyECP(EncryptedECP e, string message)
        {
            if(e.R > e.ECPKeys.P || e.R < 0)
            {
                return false;
            }

            foreach (var h in e.S)
            {
                if(h > e.ECPKeys.P - 1 || h < 0)
                {
                    return false;
                }
            }

            var messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hash;

            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(messageBytes);
            }

            BigInteger test = 0;

            foreach(var h in hash)
            {
                test += h;

            }
            var x = (int)((BigInteger.Pow(e.ECPKeys.Y, (int)e.R) * BigInteger.Pow(e.R, (int)e.S[0])) % e.ECPKeys.P);
            var y = (int)BigInteger.ModPow(e.ECPKeys.G, test, e.ECPKeys.P);
            if (x != y)
                return false;
            return true;
        }

        private ECPKeys GetECPKeys()
        {
            BigInteger p = RandomPrimeGenerator.GenerateRandomPrime(500, 1000);
            BigInteger g = RandomPrimeGenerator.GenerateRandomPrime(1, (int)p);
            BigInteger x = RandomPrimeGenerator.GenerateRandomPrime(1, (int)p - 2);
            BigInteger y = BigInteger.ModPow(g, x, p);
            return new ECPKeys { G = g, X = x, P = p, Y = y };
        }

        private BigInteger GetRandomCoprime(BigInteger p)
        {
            BigInteger k;
            do
            {
                k = RandomPrimeGenerator.GenerateRandomPrime(1, (int) p-1);
            } while (IsCoprime(k, p - 1) == false);

            return k;
        }

        private bool IsCoprime(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a == 1;
        }
    }
}
