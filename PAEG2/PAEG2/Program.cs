using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace PAEG2
{
    internal class Program
    {
        static int first = 0;
        static int second = 0;

        static void Main(string[] args)
        {
            List<Electorate> electorates = new List<Electorate>();
            //генеруємо електорат
            GenerateElectorate(electorates, 4);
            //генеруємо ключі
            foreach (var electorate in electorates)
            {
                Random random = new Random();
                electorate.RsaKeys = KeyGeneration.BeginRSA();
                electorate.R = RelativePrime.FindRelativePrime(electorate.RsaKeys.N, electorate.RsaKeys.P, electorate.RsaKeys.Q);
            }
            //генеруємо ключі виборчої комісії
            ElectiveCommite commite = new ElectiveCommite {RsaKeys = KeyGeneration.BeginRSA() };
            //шифруємо бюлетені
            EncodeBulletins(electorates, commite.RsaKeys.E, commite.RsaKeys.N);

            int i = 0;
            foreach (var electorate in electorates)
            {
                //комісія обирає один з 10 бюлетенів
                Bulletin choosed = ChooseBulletin(electorate.EncodedBulletins, electorate.R, commite);
                //шифрує його
                BigInteger rOb = ExtendedEuclidian.FindMultiplicativeInverse(electorate.R, commite.RsaKeys.N);
                choosed.M1 = choosed.M1 * rOb;
                choosed.M2 = choosed.M2 * rOb;
                //виборець перевіряє отриманий бюлетень з надісланим
                Bulletin check = FormateBulletine(electorate.Id);
                Bulletin gotten = new Bulletin();
                gotten.M1 = BigInteger.ModPow(choosed.M1, commite.RsaKeys.E, commite.RsaKeys.N);
                gotten.M2 = BigInteger.ModPow(choosed.M2, commite.RsaKeys.E, commite.RsaKeys.N);
                if (!check.Equals(gotten))
                {
                    Console.WriteLine("Detected manipulations");
                    return;
                }
                //виборець голосує
                BigInteger c = Elect(gotten, commite.RsaKeys.E, commite.RsaKeys.N);
                Vote(c, commite.RsaKeys.D, commite.RsaKeys.N, electorate, i++);
            }
            Console.WriteLine("For first: " + first);
            Console.WriteLine("For second: " + second);

        }

        public static void Vote(BigInteger c, BigInteger dVk, BigInteger nVk, Electorate electorate, int voteNumber)
        {
            BigInteger answer = BigInteger.ModPow(c, dVk, nVk);
            electorate.HasVoted = true;
            string voteStatus = electorate.HasVoted ? "Так" : "Ні";

            // Виведення шапки таблиці
            if (voteNumber == 0)
            {
                Console.WriteLine("┌─────┬───────────┬───────────────┐");
                Console.WriteLine("|  №  |ID виборця |Враховано голос|");
                Console.WriteLine("├─────┼───────────┼───────────────┤");
            }

            // Виведення даних у вигляді таблиці
            Console.WriteLine($"| {voteNumber + 1,-4}| {electorate.Id,-9} | {voteStatus,-13} |");

            // Виведення ліній, що розділяють колонки
            Console.WriteLine("├─────┼───────────┼───────────────┤");

        }

        public static BigInteger Elect(Bulletin bulletin, BigInteger eVk, BigInteger nVk)
        {
            Random random = new Random();
            int choosed = random.Next(0, 2);
            BigInteger answer;
            if(choosed == 0)
            {
                answer = BigInteger.ModPow(bulletin.M1, eVk, nVk);
                first++;
            }
            else
            {
                answer = BigInteger.ModPow(bulletin.M2, eVk, nVk);
                second++;
            }
            return answer;
        }

        public static Bulletin ChooseBulletin(List<Bulletin> encoded, BigInteger r, ElectiveCommite electiveCommite)
        {
            Random random = new Random();
            Bulletin bulletin = encoded[random.Next(encoded.Count)];
            bulletin.M1 = BigInteger.ModPow(bulletin.M1, electiveCommite.RsaKeys.D, electiveCommite.RsaKeys.N);
            bulletin.M2 = BigInteger.ModPow(bulletin.M2, electiveCommite.RsaKeys.D, electiveCommite.RsaKeys.N);
            return bulletin;
        }

        public static void EncodeBulletins(List<Electorate> electorates, BigInteger eVk, BigInteger nVk)
        {
            foreach (var electorate in electorates)
            {
                Bulletin encodeBulletin = FormateBulletine(electorate.Id);
                encodeBulletin.M1 = (encodeBulletin.M1 * BigInteger.Pow(electorate.R, (int)eVk)) % nVk;
                encodeBulletin.M2 = (encodeBulletin.M2 * BigInteger.Pow(electorate.R, (int)eVk)) % nVk;
                for (int i = 0; i < 10; i++)
                {
                    electorate.EncodedBulletins.Add(encodeBulletin);
                }
            }
        }

        public static void GenerateElectorate(List<Electorate> electorates, int amount)
        {
            Random rnd = new Random();
            for (int i = 0; i < amount; i++)
            {
                Electorate electorate = new Electorate {HasVoted = false, Id = rnd.Next(amount * 40)};
                electorates.Add(electorate);
            }
        }

        public static Bulletin FormateBulletine(BigInteger id)
        {
            Bulletin bulletin = new Bulletin();
            bulletin.M1 = 10 * id + 1;
            bulletin.M2 = 10 * id + 2;
            return bulletin;
        }
    }
}