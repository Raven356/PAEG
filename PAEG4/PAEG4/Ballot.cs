using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG4
{
    public class Ballot
    {
        //public byte[] Data { get; set; }

        public List<BigInteger> Data { get; set; }

        public string Voter {  get; set; }

        public string Message { get; set; }
    }
}
