// See https://aka.ms/new-console-template for more information
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Task_api;


try
{
    Console.WriteLine("Enter the number of users to fetch (1-900):");
    if (!int.TryParse(Console.ReadLine(), out int numberOfUsers) || numberOfUsers < 1 || numberOfUsers > 900)
    {
        Console.WriteLine("Invalid input or number out of range.");
        Console.WriteLine("Please enter a number between 1 and 900.");
        return;
    }

    string apiUrl = $"https://randomuser.me/api/?results={numberOfUsers}";
    var users = await UsersAsync(apiUrl);

    if (users != null && users.Length > 0)
    {
        HashSet<string> processedUserIds = new HashSet<string>();

        foreach (var user in users)
        {
            try
            {
                if (!processedUserIds.Contains(user.Id.ToString()))
                {
                    string vCard = ConvertToVCard(user);
                    string fileName = $"{user.FirstName}_{user.LastName}.vcf";
                    File.WriteAllText(fileName, vCard);
                    Console.WriteLine($"vCard saved as {fileName}");

                    processedUserIds.Add(user.Id.ToString());
                }
                else
                {
                    Console.WriteLine($"User with ID {user.Id} already processed. Skipping...");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing vCard file: {ex.Message}");
            }
        }
    }
    else
    {
        Console.WriteLine("No users were fetched from the API.");
    }
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP request error: {ex.Message}");
}

        

        static async Task<User[]> UsersAsync(string url)
{
    using (HttpClient client = new HttpClient())
    {
        try
        {
            var response = await client.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);

            if (apiResponse != null && apiResponse.Results != null)
            {
                var users = new User[apiResponse.Results.Length];
                for (int i = 0; i < apiResponse.Results.Length; i++)
                {
                    users[i] = new User
                    {
                        Id = Guid.NewGuid(),
                        FirstName = apiResponse.Results[i].Name.First,
                        LastName = apiResponse.Results[i].Name.Last,
                        Email = apiResponse.Results[i].Email,
                        Phone = apiResponse.Results[i].Phone,
                        Country = apiResponse.Results[i].Location.Country,
                        City = apiResponse.Results[i].Location.City
                    };
                }
                return users;
            }
            else
            {
                Console.WriteLine("The API response is invalid.");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP request error: {ex.Message}");
            return null;
        }
        
    }
}

static string ConvertToVCard(User user)
{
    return
        "BEGIN:VCARD\n" +
        "VERSION:3.0\n" +
        $"FN:{user.FirstName} {user.LastName}\n" +
        $"N:{user.LastName};{user.FirstName}\n" +
        $"EMAIL:{user.Email}\n" +
        $"TEL:{user.Phone}\n" +
        $"ADR:;;{user.City};{user.Country}\n" +
        "END:VCARD";
}