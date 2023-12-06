using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG5
{
    public class Bulletein
    {
        public List<BigInteger> Encrypted { get; set; }

        public string Id { get; set; }

        public byte[] ECP { get; set; }

        public string Message { get; set; }
    }
}
