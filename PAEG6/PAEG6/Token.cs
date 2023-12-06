using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG6
{
    internal class Token
    {
        public Guid Id { get; set; }

        public Keys Keys { get; set; }

        public BigInteger N { get; set; }
    }
}
