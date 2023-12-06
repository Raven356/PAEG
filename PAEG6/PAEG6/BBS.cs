using System.Numerics;
using System.Text;

namespace PAEG6
{
    internal class BBS
    {
        private static Random random = new Random();
        private BigInteger? x = null;

        public BBS(){ }

        public BBS(BigInteger x)
        {
            this.x = x;
        }

        private BigInteger Gcd(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private bool IsPrime(BigInteger n)
        {
            if (n <= 1) return false;
            if (n <= 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;

            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            }

            return true;
        }

        private  Tuple<BigInteger, BigInteger> GeneratePQ()
        {
            List<int> primes = Enumerable.Range(100, 900).Where(i => IsPrime(i) && i % 4 == 3).ToList();
            BigInteger p = primes[random.Next(primes.Count)];
            BigInteger q;
            do
            {
                q = primes[random.Next(primes.Count)];
            } while (p == q);
            return Tuple.Create(p, q);
        }

        public Tuple<BigInteger, BigInteger, BigInteger> GenerateKeys()
        {
            var pq = GeneratePQ();
            BigInteger n = pq.Item1 * pq.Item2;
            return Tuple.Create(n, pq.Item1, pq.Item2);
        }

        public BigInteger GenerateInitialState(BigInteger n)
        {
            if (x == null)
            {
                x = random.Next(2, (int)n - 1);
                while (Gcd((BigInteger)x, n) != 1)
                {
                    x = random.Next(2, (int)n - 1);
                }
            }
            return (BigInteger)x;
        }

        private string BBSGenerator(BigInteger n, BigInteger x, BigInteger length)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                x = BigInteger.ModPow(x, 2, n);
                BigInteger bit = x & 1;
                output.Append(bit);
            }
            return output.ToString();
        }

        public byte[] Encrypt(byte[] input, BigInteger n)
        {
            int length = input.Length;
            BigInteger x0 = GenerateInitialState(n);
            string bbsOutput = BBSGenerator(n, x0, length);

            for (int i = 0; i < length; i++)
            {
                input[i] = ((byte)(input[i] ^ bbsOutput[i]));
            }
            return input;
        }

        public byte[] Decrypt(byte[] input, BigInteger n)
        {
            int length = input.Length;
            BigInteger x0 = GenerateInitialState(n);
            string bbsOutput = BBSGenerator(n, x0, length);

            for (int i = 0; i < length; i++)
            {
                input[i] = (byte)(input[i] ^ bbsOutput[i]);
            }

            return input;
        }

        public string Encrypt(string message, BigInteger n)
        {
            int length = message.Length;
            BigInteger x0 = GenerateInitialState(n);
            string bbsOutput = BBSGenerator(n, x0, length);

            StringBuilder encrypted = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                encrypted.Append((char)(message[i] ^ bbsOutput[i]));
            }

            return encrypted.ToString();
        }

        public string Decrypt(string encrypted, BigInteger n)
        {
            int length = encrypted.Length;
            BigInteger x0 = GenerateInitialState(n);
            string bbsOutput = BBSGenerator(n, x0, length);

            StringBuilder decrypted = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                decrypted.Append((char)(encrypted[i] ^ bbsOutput[i]));
            }

            return decrypted.ToString();
        }
    }
}
