using PAEG5;
using System.Security.Cryptography;
using System.Text;
using PAEG2;
using System.Numerics;

string candidat1Id = Guid.NewGuid().ToString();
string candidat2Id = Guid.NewGuid().ToString();
List<string> candidates = new() { candidat1Id, candidat2Id };

string electorate1Id = Guid.NewGuid().ToString();
string electorate2Id = Guid.NewGuid().ToString();
string electorate3Id = Guid.NewGuid().ToString();
string electorate4Id = Guid.NewGuid().ToString();


var rsa = KeyGeneration.BeginRSA();
Random random = new Random();

string el1Choice = candidates[random.Next(0, 2)];
string el2Choice = candidates[random.Next(0, 2)];
string el3Choice = candidates[random.Next(0, 2)];
string el4Choice = candidates[random.Next(0, 2)];

var split1 = el1Choice
    .Select((c, i) => new { Char = c, Index = i })
    .GroupBy(x => x.Index < el1Choice.Length / 2)
    .Select(g => new string(g.Select(x => x.Char).ToArray()))
    .ToArray();
var split2 = el2Choice
    .Select((c, i) => new { Char = c, Index = i })
    .GroupBy(x => x.Index < el2Choice.Length / 2)
    .Select(g => new string(g.Select(x => x.Char).ToArray()))
    .ToArray();
var split3 = el3Choice
    .Select((c, i) => new { Char = c, Index = i })
    .GroupBy(x => x.Index < el3Choice.Length / 2)
    .Select(g => new string(g.Select(x => x.Char).ToArray()))
    .ToArray();
var split4 = el4Choice
    .Select((c, i) => new { Char = c, Index = i })
    .GroupBy(x => x.Index < el4Choice.Length / 2)
    .Select(g => new string(g.Select(x => x.Char).ToArray()))
    .ToArray();
List<DSA> list = new List<DSA>();
DSA dSA1 = new DSACryptoServiceProvider();
DSA dSA2 = new DSACryptoServiceProvider();
DSA dSA3 = new DSACryptoServiceProvider();
DSA dSA4 = new DSACryptoServiceProvider();
list.Add(dSA1);
list.Add(dSA2);
list.Add(dSA3);
list.Add(dSA4);



List<Bulletein> LeftParts = new()
{
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split1[0]),
        Id = electorate1Id,
        ECP = dSA1.SignData(System.Text.Encoding.UTF8.GetBytes(split1[0]), HashAlgorithmName.SHA1),
        Message = split1[0]
    },
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split2[0]),
        Id = electorate2Id,
        ECP = dSA2.SignData(System.Text.Encoding.UTF8.GetBytes(split2[0]), HashAlgorithmName.SHA1),
        Message = split2[0]
    },
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split3[0]),
        Id = electorate3Id,
        ECP = dSA3.SignData(System.Text.Encoding.UTF8.GetBytes(split3[0]), HashAlgorithmName.SHA1),
        Message = split3[0]
    },
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split4[0]),
        Id = electorate4Id,
        ECP = dSA4.SignData(System.Text.Encoding.UTF8.GetBytes(split4[0]), HashAlgorithmName.SHA1),
        Message = split4[0]
    }
};

List<Bulletein> RightParts = new()
{
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split1[1]),
        Id = electorate1Id,
        ECP = dSA1.SignData(System.Text.Encoding.UTF8.GetBytes(split1[1]), HashAlgorithmName.SHA1),
        Message = split1[1]
    },
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split2[1]),
        Id = electorate2Id,
        ECP = dSA2.SignData(System.Text.Encoding.UTF8.GetBytes(split2[1]), HashAlgorithmName.SHA1),
        Message = split2[1]
    },
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split3[1]),
        Id = electorate3Id,
        ECP = dSA3.SignData(System.Text.Encoding.UTF8.GetBytes(split3[1]), HashAlgorithmName.SHA1),
        Message = split3[1]
    },
    new Bulletein
    {
        Encrypted = KeyGeneration.Encrypt(rsa, split4[1]),
        Id = electorate4Id,
        ECP = dSA4.SignData(System.Text.Encoding.UTF8.GetBytes(split4[1]), HashAlgorithmName.SHA1),
        Message = split4[1]
    }
};

List<string> VK1Id = new List<string>();
List<string> VK2Id = new List<string>();
int i = 0;
foreach (var x in LeftParts)
{
    if (VK1Id.Contains(x.Id))
    {
        Console.WriteLine($"Id {x.Id} already voted ignoring!");
        continue;
    }
    VK1Id.Add(x.Id);
    if (!list[i].VerifyData(System.Text.Encoding.UTF8.GetBytes(x.Message), x.ECP, HashAlgorithmName.SHA1))
    {
        Console.WriteLine("Manipulations!");
        return;
    }
    i++;
}


i = 0;
foreach (var x in RightParts)
{
    if (VK2Id.Contains(x.Id))
    {
        Console.WriteLine($"Id {x.Id} already voted ignoring!");
        continue;
    }
    VK2Id.Add(x.Id);
    if (!list[i].VerifyData(System.Text.Encoding.UTF8.GetBytes(x.Message), x.ECP, HashAlgorithmName.SHA1))
    {
        Console.WriteLine("Manipulations!");
        return;
    }
    i++;
}

for (int x = 0; x < LeftParts.Count; x++)
{
    //var left = Encoding.UTF8.GetString(rsa.Decrypt(LeftParts[x].Encrypted, RSAEncryptionPadding.Pkcs1));
    //var right = Encoding.UTF8.GetString(rsa.Decrypt(RightParts[x].Encrypted, RSAEncryptionPadding.Pkcs1));
    var fullArr = new List<BigInteger>();
    fullArr.AddRange(LeftParts[x].Encrypted);
    fullArr.AddRange(RightParts[x].Encrypted);
    var full = KeyGeneration.Decrypt(rsa, fullArr);
    Console.WriteLine($"{LeftParts[x].Id} voted for {full}");
    //Console.WriteLine($"Control {full}");
}