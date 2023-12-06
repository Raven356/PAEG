using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PAEG6
{
    [DataContract]
    internal class Bulletein
    {
        [DataMember]
        public Guid Id {  get; set; }

        [DataMember]
        public BigInteger A { get; set; }

        [DataMember]
        public List<BigInteger> Bs { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public BigInteger N { get; set; }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            DataContractSerializer bf = new DataContractSerializer(typeof(Bulletein));
            using (MemoryStream ms = new MemoryStream())
            {
                bf.WriteObject(ms, obj);
                return ms.ToArray();
            }
        }

        public static Bulletein ByteArrayToObject(byte[] byteArray)
        {
            if (byteArray == null)
                return null;

            DataContractSerializer serializer = new DataContractSerializer(typeof(Bulletein));

            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return (Bulletein)serializer.ReadObject(ms);
            }
        }
    }
}
