using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAEG3
{
    public class RegistrationBureal
    {
        public List<Electorate> electorates = new List<Electorate>();

        public void ReturnRegistrationNumber(Electorate electorate)
        {
            Random random = new Random();
            electorate.RegistrationNumber = random.Next(1000, 100000);
            electorates.Add(electorate);
        }

        public void SendToCVK(ElectorateComission comission)
        {
            foreach (var electorate in electorates)
            {
                comission.RegistrationNumbers.Add(electorate.RegistrationNumber);
            }
        }
    }
}
