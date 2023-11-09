using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PAEG4
{
    internal class RsaKeys
    {
        public RSAParameters PublicKey { get; set; }

        public RSAParameters PrivateKey { get; set; }
    }
}
