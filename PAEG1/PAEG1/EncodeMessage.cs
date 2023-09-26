using PAEG1.Models;
using PAEG1.Responses;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Keys = PAEG1.Responses.Keys;

namespace PAEG1
{
    public class EncodeMessage
    {
        private string _message;

        public EncodeMessage(string message) { 
            _message = message;
        }

        private string EncryptMessage(string message, string key)
        {
            //шифруємо гамма шифруванням
            return GammaEncryption.Encrypt(message, key);
        }

        public EcpWithPublicKey GenerateECP()
        {
            //отримуємо відкритий і закритий рса ключі
            var key = BeginRSA();

            //хешуємо повідомлення
            var hash = Hash.HashFunction(_message, key);
            List<BigInteger> ecp = new List<BigInteger>();

            for (int i = 0; i < hash.Count; i++)
            {
                //використовуємо закритий ключ і хеш повідомлення для формування ЕЦП
                BigInteger ecpPart = BigInteger.ModPow(hash[i], key.D, key.N);
                ecp.Add(ecpPart);
            }

            //генеруємо ключ для гамма шифрування
            var gammaKey = GammaEncryption.GenerateRandomKey(_message.Length);
            //відсилаємо інформація
            return new EcpWithPublicKey { N = key.N, E = key.E, Ecp = ecp, Hash = hash, EncryptedMessage = EncryptMessage(_message, gammaKey), GammaKey = gammaKey };
        }

        private Keys BeginRSA()
        {
            Random random = new Random();
            BigInteger p = FindNextPrime(random.Next(0, 2000));
            BigInteger q = FindNextPrime(random.Next(0, 2000));
            BigInteger n = p * q;
            BigInteger eilerFunction = (p - 1) * (q - 1);
            BigInteger e = FindOddPrime(eilerFunction);
            BigInteger d = FindD(e, eilerFunction);

            return new Keys { D = d, E = e, N = n };
        }



        private BigInteger ExtendedEuclidean(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
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

        private BigInteger FindD(BigInteger e, BigInteger x)
        {
            BigInteger d, y;
            ExtendedEuclidean(e, x, out d, out y);

            if (d < 0)
            {
                d += x;
            }

            return d;
        }

        private BigInteger FindOddPrime(BigInteger x)
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

        private bool IsPrime(BigInteger num)
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

        private BigInteger FindNextPrime(BigInteger start)
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
