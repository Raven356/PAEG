using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG4
{
    public class EncryptedECP
    {
        public List<BigInteger> S {  get; set; }

        public BigInteger R { get; set; }

        public ECPKeys ECPKeys { get; set; }
    }
}
