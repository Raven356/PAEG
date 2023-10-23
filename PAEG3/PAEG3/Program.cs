using PAEG3;
using System.Security.Cryptography;
using System.Text;


try
{
    //створюємо виборців, виборче бюро присвоює регестраційний номер
    List<Electorate> electorates = new List<Electorate>();
    RegistrationBureal registrationBureal = new RegistrationBureal();

    for (int i = 0; i < 4; i++)
    {
        //виборець обирає собі айді
        electorates.Add(new Electorate() { Name = $"Name {i + 1}", Id = new Random().Next() });
    }

    foreach (var electorate in electorates)
    {// бюро видає номера
        registrationBureal.ReturnRegistrationNumber(electorate);
    }

    ElectorateComission comission = new ElectorateComission();
    //бюро відсилає номери до цвк
    registrationBureal.SendToCVK(comission);

    List<string> candidates = new List<string> { "candidate1", "candidate2" };
    List<CompletedBulletein> completedBulleteins = new List<CompletedBulletein>();
    foreach (var electorate in electorates)
    {
        // виборець шифрує повідомлення за допомогою ель гамаля
        Random random = new Random();
        electorate.Keys = ElGamal.GetKeys();
        electorate.Bulletein = ElGamal.Encrypt(candidates[random.Next(0, 2)], electorate.Keys);

        //виборець підписує повідомлення використовуючи dsa
        using DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
        electorate.Bulletein.DSAPublicKey = dsa.ExportParameters(false);

        byte[] messageBytes = Encoding.UTF8.GetBytes(electorate.Bulletein.Encrypted);
        //хешуємо повідомлення
        byte[] hash;
        using (SHA256 sha256 = SHA256.Create())
        {
            hash = sha256.ComputeHash(messageBytes);
        }

        byte[] signature = dsa.SignData(hash);
        electorate.Bulletein.ECP = signature;
        //виборець голосує
        completedBulleteins.Add(comission.Vote(electorate.Id, electorate.RegistrationNumber, electorate.Bulletein, electorate.Keys));
    }

    foreach(var compl in completedBulleteins)
    {
        //вивід результатів
        Console.WriteLine(compl);
    }
    comission.Count();
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}