using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG3
{
    public class Electorate
    {
        public string Name { get; set; }

        public BigInteger RegistrationNumber { get; set; }

        public BigInteger Id { get; set; }

        public Keys Keys { get; set; }

        public Bulletein Bulletein { get; set; }
    }
}
