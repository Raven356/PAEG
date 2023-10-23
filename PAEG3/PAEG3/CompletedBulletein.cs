using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PAEG3
{
    public class CompletedBulletein
    {
        public BigInteger UserId { get; set; }

        public string Message { get; set; }

        public override string? ToString()
        {
            return $"User id = {UserId} Voted for: {Message}";
        }
    }
}
