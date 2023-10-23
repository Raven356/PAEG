using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PAEG3
{
    public class ElectorateComission
    {
        public List<BigInteger> RegistrationNumbers {get; set; } = new List<BigInteger>();

        BigInteger voteForCand1;

        BigInteger voteForCand2;

        public CompletedBulletein Vote(BigInteger id, BigInteger registrationNumber, Bulletein bulletein, Keys keys)
        {
            if(RegistrationNumbers.Contains(registrationNumber))
            {
                RegistrationNumbers.Remove(registrationNumber);
            }
            else
            {
                throw new Exception("Already voted or not allowed!");
            }
            using DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            dsa.ImportParameters(bulletein.DSAPublicKey);

            byte[] messageBytes = Encoding.UTF8.GetBytes(bulletein.Encrypted);

            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(messageBytes);
            }

            if (!dsa.VerifyData(hash, bulletein.ECP))
            {
                throw new Exception("Wrong ECP!");
            }

            var mess = ElGamal.Decrypt(bulletein, keys);

            if (mess.Equals("candidate1"))
            {
                voteForCand1++;
            }
            else if (mess.Equals("candidate2"))
            {
                voteForCand2++;
            }
            else
            {
                throw new Exception("Something went wrong!!");
            }
            return new CompletedBulletein { Message = mess, UserId = id };
        }

        public void Count()
        {
            Console.WriteLine($"For candidate1: {voteForCand1}, candidate2: {voteForCand2}");
        }
    }
}
