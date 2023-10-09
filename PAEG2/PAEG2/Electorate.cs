using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG2
{
    public class Electorate
    {
        public BigInteger Id { get; set; }

        public bool HasVoted { get; set; }

        public Keys RsaKeys { get; set; }

        public List<Bulletin> EncodedBulletins { get; set; } = new List<Bulletin>();

        public BigInteger R { get; set; }
    }
}
