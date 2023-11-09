// See https://aka.ms/new-console-template for more information
using PAEG4;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

var a = KeyGeneration.BeginRSA();
var b = KeyGeneration.BeginRSA();
var c = KeyGeneration.BeginRSA();
var d = KeyGeneration.BeginRSA();


MyUserRSAKeys myUserRSAKeys = new MyUserRSAKeys { UserAKeys = a, UserBKeys = b, UserCKeys = c, UserDKeys = d };
List<Ballot> ballots = new List<Ballot>
{
    KeyGeneration.PrepareBallot("VoterA", myUserRSAKeys),
    KeyGeneration.PrepareBallot("VoterB", myUserRSAKeys),
    KeyGeneration.PrepareBallot("VoterC", myUserRSAKeys),
    KeyGeneration.PrepareBallot("VoterD", myUserRSAKeys)
};

var result = KeyGeneration.DecryptBallots(ballots, myUserRSAKeys);
foreach (var s in result)
{
    Console.WriteLine(s);
}
