using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using VCard.Models;
using System.Threading.Tasks;
using System.IO;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Olusturmak istediginiz vCard adet sayısını giriniz: ");
        try
        {
            int count = Convert.ToInt32(Console.ReadLine());
            using HttpClient client = new HttpClient();
            try
            {
                if (count > 2000 || count <= 0)
                {
                    Console.WriteLine("Girdiğiniz sayı 0-dan büyük olmak zorunda ve maksimum 2000 adet kişi alabilirsiniz!!!");
                }
                else
                {
                    HttpResponseMessage response = await client.GetAsync($"https://randomuser.me/api/?nat=us,fr,tr&results={count}");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var resultData = JsonSerializer.Deserialize<Result>(responseBody, options);

                    // Define the Uploads folder path
                    string uploadsFolderPath = Path.Combine("C:\\Users\\nurla\\Desktop\\vCard\\VCard\\VCard\\Uploads");

                    // Ensure the Uploads folder exists
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    foreach (var user in resultData.Results)
                    {
                        CreateCard(user, uploadsFolderPath);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Hata mesaji: {e.Message}");
            }
        }
        catch (Exception message)
        {
            Console.WriteLine("Sayıyı doğru formatta giriniz. Hata mesajı:" + message);
        }
    }

    public static void CreateCard(vCard user, string uploadsFolderPath)
    {
        StringBuilder card = new StringBuilder();
        card.Append($@"BEGIN:VCARD
                    VERSION:4.0
                    N:{user.Name.First};{user.Name.Last};;;
                    FN:{user.Name.First} {user.Name.Last}
                    TEL;TYPE=work,voice;VALUE=uri:tel:{user.Phone}
                    TEL;TYPE=home,voice;VALUE=uri:tel:{user.Phone}
                    ADR;TYPE=WORK:;;{user.Location.Country};;{user.Location.City}
                    EMAIL:{user.Email}
                    REV:20080424T195243Z
                    END:VCARD");

        string fileName = $"{user.Name.First}-{user.Name.Last}.{user.Id.Value}";

        // Save the vCard as .vcf
        string filePathVcf = Path.Combine(uploadsFolderPath, $"{fileName}.vcf");
        File.WriteAllTextAsync(filePathVcf, card.ToString());

        // Save the vCard as .txt
        string filePathTxt = Path.Combine(uploadsFolderPath, $"{fileName}.txt");
        File.WriteAllTextAsync(filePathTxt, card.ToString());

        Console.WriteLine($"{user.Name.First} {user.Name.Last} isimli userin vCard dosyasının yolu: {filePathVcf}");
    }
}
