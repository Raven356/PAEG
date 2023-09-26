using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PAEG1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = PAEG1.Responses.Keys;

namespace PAEG1
{
    public class Hash
    {
        private static List<string> _ukrainianAlphabet = new List<string>()
        {
            "А", "Б", "В", "Г", "Ґ", "Д", "Е", "Є", "Ж", "З", "И", "І", "Ї", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ь", "Ю", "Я", " ", "-", "\""
        };

        public static List<BigInteger> HashFunction(string message, Keys key)
        {
            BigInteger hash = 0;
            BigInteger h = 0;
            List<BigInteger> hashed = new List<BigInteger>();

            foreach (var letter in message)
            {
                int index = _ukrainianAlphabet.IndexOf(char.ToUpper(letter).ToString());
                BigInteger hi = BigInteger.ModPow(h + index, 2, key.N);
                h = hi;
                if (hash + hi > key.N)
                {
                    hashed.Add(hash);
                    hash = hi;
                }
                else
                {
                    hash += hi;
                }
            }
            return hashed;
        }
    }
}
