using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PAEG3
{
    public class Bulletein
    {
        public BigInteger A { get; set; }

        public List<BigInteger> Bs { get; set; }

        public string Encrypted { get; set; }

        public byte[] ECP { get; set; }

        public DSAParameters DSAPublicKey { get; set; }
    }
}
