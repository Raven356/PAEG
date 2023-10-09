using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG2
{
    public class Bulletin : IEquatable<Bulletin>
    {
        public BigInteger M1 {  get; set; }

        public BigInteger M2 { get; set; }

        public bool Equals(Bulletin? other)
        {
            return this.M1 == other.M1 && this.M2 == other.M2;
        }
    }
}
