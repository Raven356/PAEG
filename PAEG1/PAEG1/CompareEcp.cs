using PAEG1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG1
{
    public class CompareEcp
    {
        //перевіряємо підпис
        public static bool Compare(EcpWithPublicKey encoded)
        {
            List<BigInteger> result = new List<BigInteger>();
            foreach(var ecpPart in encoded.Ecp)
            {
                BigInteger hashDecoded = BigInteger.ModPow(ecpPart, encoded.E, encoded.N);
                result.Add(hashDecoded);
            }

            BigInteger sumHash = 0;
            BigInteger sumEcp = 0;
            foreach( var hash in result )
            {
                sumHash += hash;
            }

            foreach( var ecpPart in encoded.Hash)
            {
                sumEcp += ecpPart;
            }
            return sumEcp == sumHash;
        }

        //розшифровуємо повідомлення
        public static string DecryptMessage(EcpWithPublicKey encoded)
        {
            return GammaEncryption.Decrypt(encoded.EncryptedMessage, encoded.GammaKey);
        }
    }
}
