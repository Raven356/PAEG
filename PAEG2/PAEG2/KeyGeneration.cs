using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG2
{
    public static class KeyGeneration
    {
        public static Keys BeginRSA()
        {
            Random random = new Random();
            BigInteger p = FindNextPrime(random.Next(0, 2000));
            BigInteger q = FindNextPrime(random.Next(0, 2000));
            BigInteger n = p * q;
            BigInteger eilerFunction = (p - 1) * (q - 1);
            BigInteger e = FindOddPrime(eilerFunction);
            BigInteger d = FindD(e, eilerFunction);

            return new Keys { D = d, E = e, N = n, P = p, Q = q };
        }



        private static BigInteger ExtendedEuclidean(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            BigInteger x1, y1;
            BigInteger gcd = ExtendedEuclidean(b % a, a, out x1, out y1);

            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }

        private static BigInteger FindD(BigInteger e, BigInteger x)
        {
            BigInteger d, y;
            ExtendedEuclidean(e, x, out d, out y);

            if (d < 0)
            {
                d += x;
            }

            return d;
        }

        private static BigInteger FindOddPrime(BigInteger x)
        {
            for (BigInteger e = 3; e < x; e += 2)
            {
                if (IsCoprime(x, e))
                {
                    return e;
                }
            }
            return -1;
        }

        private static bool IsCoprime(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a == 1;
        }

        private static bool IsPrime(BigInteger num)
        {
            if (num < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt((double)num); i++)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }

        private static BigInteger FindNextPrime(BigInteger start)
        {
            BigInteger num = start;

            while (true)
            {
                num++;

                if (IsPrime(num))
                    return num;
            }
        }
    }
}
