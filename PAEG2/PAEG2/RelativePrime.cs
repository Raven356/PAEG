using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG2
{
    public class RelativePrime
    {
        private static bool IsPrime(BigInteger num)
        {
            if (num < 2) return false;
            for (int i = 2; i <= Math.Sqrt((double)num); i++)
            {
                if (num % i == 0)
                    return false;
            }
            return true;
        }

        public static BigInteger FindRelativePrime(BigInteger n, BigInteger p, BigInteger q)
        {
            Random rand = new Random();
            BigInteger relativePrime = 0;

            while (true)
            {
                relativePrime = rand.Next(2, (int)n); // Generate a random number within the range of 2 to n-1

                if (IsPrime(relativePrime) && relativePrime != p && relativePrime != q && GetGCD(n, relativePrime) == 1)
                {
                    break;
                }
            }

            return relativePrime;
        }

        private static BigInteger GetGCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }
}
