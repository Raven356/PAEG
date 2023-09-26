using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG1.Responses
{
    public class EcpWithPublicKey
    {
        public string EncryptedMessage { get; set; }

        public string GammaKey { get; set; }

        public BigInteger N { get; set; }

        public BigInteger E { get; set; }

        public List<BigInteger> Ecp { get; set; }

        public List<BigInteger> Hash { get; set; }
    }
}
