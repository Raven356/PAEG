using System.Numerics;

namespace PAEG6
{
    internal class RandomPrimeGenerator
    {
        private static Random random = new Random();

        public static BigInteger GenerateRandomPrime(int minValue, int maxValue)
        {
            BigInteger randomNum = 0;
            bool isPrime = false;

            while (!isPrime)
            {
                randomNum = random.Next(minValue, maxValue);
                isPrime = IsPrime(randomNum);
            }

            return randomNum;
        }

        private static bool IsPrime(BigInteger num)
        {
            if (num < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt((int)num); i++)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }
    }
}
