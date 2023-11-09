using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG4
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

        public static Ballot PrepareBallot(string voter, MyUserRSAKeys userRsaKeys)
        {
            Random random = new Random();
            var choice = random.Next(1, 3);
            string message = $"{voter}: choosed {choice}";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            List<BigInteger> encryptedMessage = Encrypt(userRsaKeys.UserDKeys, FromByteToBig(messageBytes));
            encryptedMessage = Encrypt(userRsaKeys.UserCKeys, encryptedMessage);
            encryptedMessage = Encrypt(userRsaKeys.UserBKeys, encryptedMessage);
            encryptedMessage = Encrypt(userRsaKeys.UserAKeys, encryptedMessage);

            encryptedMessage.AddRange(FromByteToBig(Encoding.UTF8.GetBytes("Text")));
            encryptedMessage = Encrypt(userRsaKeys.UserDKeys, encryptedMessage);
            encryptedMessage.AddRange(FromByteToBig(Encoding.UTF8.GetBytes("Text")));

            encryptedMessage = Encrypt(userRsaKeys.UserCKeys, encryptedMessage);
            encryptedMessage.AddRange(FromByteToBig(Encoding.UTF8.GetBytes("Text")));

            encryptedMessage = Encrypt(userRsaKeys.UserBKeys, encryptedMessage);

            encryptedMessage.AddRange(FromByteToBig(Encoding.UTF8.GetBytes("Text")));

            encryptedMessage = Encrypt(userRsaKeys.UserAKeys, encryptedMessage);

            return new Ballot { Data = encryptedMessage, Voter = voter, Message = message };
        }
        //розшифровка бюлетеней
        public static List<string> DecryptBallots(List<Ballot> ballot, MyUserRSAKeys userRsaKeys)
        {
            for (int i = 0; i < ballot.Count; i++)
            {
                ballot[i] = Decrypt(ballot[i], userRsaKeys.UserAKeys);
            }
            for (int i = 0; i < ballot.Count; i++)
            {
                ballot[i] = Decrypt(ballot[i], userRsaKeys.UserBKeys);
            }
            for (int i = 0; i < ballot.Count; i++)
            {
                ballot[i] = Decrypt(ballot[i], userRsaKeys.UserCKeys);
            }
            for (int i = 0; i < ballot.Count; i++)
            {
                ballot[i] = Decrypt(ballot[i], userRsaKeys.UserDKeys);
            }
            for (int i = 0; i < ballot.Count; i++)
            {
                ballot[i] = Decrypt(ballot[i], userRsaKeys.UserAKeys);
            }
            List<BallotWithECP> ballotWithECPs = new List<BallotWithECP>();
            //підпис бюлетеней, перемішення, відправка, підпис, перемішення, відправка...
            ElGamalECP elGamalECP = new ElGamalECP();
            foreach (var b in ballot)
            {
                ballotWithECPs.Add(new BallotWithECP { Ballot = b, EncryptedECP = elGamalECP.BeginECP(b.Message) });
            }
            ballotWithECPs = Reshufle(ballotWithECPs);
            for (int i = 0; i < ballotWithECPs.Count; i++)
            {
                if (!elGamalECP.VerifyECP(ballotWithECPs[i].EncryptedECP, ballotWithECPs[i].Ballot.Message))
                {
                    throw new Exception("Error!");
                }
                ballotWithECPs[i].Ballot = Decrypt(ballotWithECPs[i].Ballot, userRsaKeys.UserBKeys);
                ballotWithECPs[i].EncryptedECP = elGamalECP.BeginECP(ballotWithECPs[i].Ballot.Message);
            }
            ballotWithECPs = Reshufle(ballotWithECPs);
            for (int i = 0; i < ballotWithECPs.Count; i++)
            {
                if (!elGamalECP.VerifyECP(ballotWithECPs[i].EncryptedECP, ballotWithECPs[i].Ballot.Message))
                {
                    throw new Exception("Error!");
                }
                ballotWithECPs[i].Ballot = Decrypt(ballotWithECPs[i].Ballot, userRsaKeys.UserCKeys);
                ballotWithECPs[i].EncryptedECP = elGamalECP.BeginECP(ballotWithECPs[i].Ballot.Message);
            }
            ballotWithECPs = Reshufle(ballotWithECPs);
            for (int i = 0; i < ballotWithECPs.Count; i++)
            {
                if (!elGamalECP.VerifyECP(ballotWithECPs[i].EncryptedECP, ballotWithECPs[i].Ballot.Message))
                {
                    throw new Exception("Error!");
                }
                ballotWithECPs[i].Ballot = Decrypt(ballotWithECPs[i].Ballot, userRsaKeys.UserDKeys);
                ballotWithECPs[i].EncryptedECP = elGamalECP.BeginECP(ballotWithECPs[i].Ballot.Message);
            }
            List<string> result = new List<string>();
            ballotWithECPs = Reshufle(ballotWithECPs);
            for (int i = 0; i < ballotWithECPs.Count; i++)
            {
                if (!elGamalECP.VerifyECP(ballotWithECPs[i].EncryptedECP, ballotWithECPs[i].Ballot.Message))
                {
                    throw new Exception("Error!");
                }
                List<byte> bytes = new List<byte>();
                foreach (var x in ballotWithECPs[i].Ballot.Data)
                {
                    bytes.Add((byte)x);
                }
                result.Add(Encoding.UTF8.GetString(bytes.ToArray()));
            }
            return result;
        }
        //розшифрування
        public static Ballot Decrypt(Ballot ballot, Keys keys)
        {
            for (int i = 0; i < ballot.Data.Count; i++)
            {
                ballot.Data[i] = BigInteger.ModPow(ballot.Data[i], keys.D, keys.N);
            }
            for (int i = 1;  i < ballot.Data.Count; ++i)
            {
                try
                {
                    //давайте поможемо Даші-мандрівниці найти + (розшифрування частинами)
                    var symbol = (char)ballot.Data[i];
                    if (symbol == '+' && i > 0 && i < ballot.Data.Count - 1)
                    {
                        var leftPart = ballot.Data[i - 1].ToString();
                        var rightPart = ballot.Data[i + 1].ToString();
                        var full = leftPart + rightPart;
                        ballot.Data[i - 1] = BigInteger.Parse(full);
                        var plus = ballot.Data[i];
                        var right = ballot.Data[i + 1];
                        ballot.Data.Remove(plus);
                        ballot.Data.Remove(right);
                        i --;
                    }
                }
                catch (Exception)
                {

                }
            }
            try
            {
                byte[] foreighnBytes = new List<byte> { (byte)ballot.Data[ballot.Data.Count - 4]
                , (byte)ballot.Data[ballot.Data.Count - 3]
                , (byte)ballot.Data[ballot.Data.Count - 2]
                , (byte)ballot.Data[ballot.Data.Count - 1] }.ToArray();
                if (Encoding.UTF8.GetString(foreighnBytes).Equals("Text"))
                {
                    ballot.Data.Remove(ballot.Data[ballot.Data.Count - 1]);
                    ballot.Data.Remove(ballot.Data[ballot.Data.Count - 1]);
                    ballot.Data.Remove(ballot.Data[ballot.Data.Count - 1]);
                    ballot.Data.Remove(ballot.Data[ballot.Data.Count - 1]);
                }
            }
            catch (Exception e)
            {

            }

            return ballot;
        }

        private static List<BigInteger> Encrypt(Keys keys, List<BigInteger> message)
        {
            for (int i = 0; i < message.Count; i++)
            {
                bool funTime = false;
                //розділення на частини якщо число більше N
                if (message[i] > keys.N)
                {
                    string full = message[i].ToString();
                    string N = keys.N.ToString();
                    StringBuilder sb = new StringBuilder();
                    int middle = N.Length / 2;
                    //карусель карусель весела карусель...
                    while (full[middle] == '0' || middle < 1 || middle > full.Length - 1)
                    {
                        //100000-погані числа не працюють в алгоритмі
                        if (full.Count(s => !s.Equals('0')) < 2)
                        {
                            throw new Exception("I don't know try again");
                        }
                        Random random = new Random();
                        int r = random.Next(1, 3);
                        middle += r == 1 ? 1 : -1;

                        if (middle < 1)
                        {
                            middle++;
                        }
                        if(middle > full.Length - 1)
                        {
                            middle--;
                        }

                    }
                    for (int j = 0; j < middle; j++)
                    {
                        sb.Append(full[j]);
                    }
                    //має працювати лише раз піймав
                    if (BigInteger.Parse(sb.ToString()) > keys.N){
                        Console.WriteLine("Alarm!!!!");
                        funTime = true;
                        i--;
                    }
                    //щоб не було зайвих плюсів
                    if (BigInteger.Parse(sb.ToString()) == 43)
                    {
                        middle--;
                        sb.Replace("3", "");
                    }

                    message[i] = BigInteger.Parse(sb.ToString());

                    message.Insert(i + 1, '+');
                    sb = new StringBuilder();
                    for (int j = middle; j < full.Length; j++)
                    {
                        sb.Append(full[j]);
                    }
                    message.Insert(i + 2, BigInteger.Parse(sb.ToString()));
                }
                if (!funTime)
                    message[i] = BigInteger.ModPow(message[i], keys.E, keys.N);
            }
            return message;
        }

        private static List<BigInteger> FromByteToBig(byte[] arr)
        {
            var result = new List<BigInteger>();
            foreach (var x in arr)
            {
                result.Add(x);
            }
            return result;
        }
        //перемішення бюлетенів
        private static List<BallotWithECP> Reshufle(List<BallotWithECP> ballotWithECPs)
        {
            Random random = new Random();
            int amount = random.Next(0, 11);
            for (int i = 0; i < amount; i++)
            {
                int left = random.Next(0, ballotWithECPs.Count);
                int right = random.Next(0, ballotWithECPs.Count);
                (ballotWithECPs[left], ballotWithECPs[right]) = (ballotWithECPs[right], ballotWithECPs[left]);
            }
            return ballotWithECPs;
        }
    }
}
