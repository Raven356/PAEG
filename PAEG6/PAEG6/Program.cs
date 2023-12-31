﻿using System.Numerics;
using PAEG6;

class Program
{
    static void Main()
    {

        var ids = new List<Guid>();
        int amount = 4;
        for (int i = 0; i < amount; i++)
        {
            ids.Add(Guid.NewGuid());
        }
        List<Keys> keys = new List<Keys>();
        for (int i = 0; i < amount; i++)
        {
            keys.Add(ElGamal.GetKeys());
        }

        BBS bBS = new BBS();
        var bbsKeys = bBS.GenerateKeys();

        List<Token> tokens = new List<Token>();
        for (int i = 0; i < amount; i++)
        {
            tokens.Add(new Token { Id = ids[i], Keys = keys[i], N = bbsKeys.Item1 });
        }

        List<Bulletein> bulleteins = new List<Bulletein>();
        List<string> candidates = new List<string> { "candidate1", "candidate2" };
        var rand = new Random();
        for (int i = 0; i < amount; i++)
        {
            bulleteins.Add(new Bulletein() { Id = tokens[i].Id, Message = candidates[rand.Next(0, 2)] });
        }

        List<byte[]> bytes = new List<byte[]>();

        foreach (var bulletein in bulleteins) 
        {
            ElGamal.Encrypt(bulletein, tokens[bulleteins.IndexOf(bulletein)].Keys);
            bytes.Add(Bulletein.ObjectToByteArray(bulletein));
            bBS.Encrypt(bytes[^1], tokens[bulleteins.IndexOf(bulletein)].N);
        }

        for (int i = 0; i < bytes.Count; i++)
        {
            var decr = bBS.Decrypt(bytes[i], tokens[i].N);
            bulleteins[i] = Bulletein.ByteArrayToObject(decr);
            ElGamal.Decrypt(bulleteins[i], keys[i]);
            if (!ids.Contains(bulleteins[i].Id))
            {
                Console.WriteLine("Id is not in the list skipping!");
                continue;
            }
            Console.WriteLine($"User {bulleteins[i].Id} voted for {bulleteins[i].Message}");
            ids.Remove(bulleteins[i].Id);
        }
    }
    
}
