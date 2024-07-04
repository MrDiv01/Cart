using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VCard.Models;
using VCard.Responses;
namespace RandomUserApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("İstifadəçi sayını daxil edin: ");
            int numberOfUsers = int.Parse(Console.ReadLine());

            using (HttpClient client = new HttpClient())
            {
                string url = $"https://randomuser.me/api/?results={numberOfUsers}";

                HttpResponseMessage response = await client.GetAsync(url);
                try
                {

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        List<VCards> vcards = new List<VCards>();


                        foreach (var result in apiResponse.Results)
                        {
                            VCards vcard = new VCards
                            {
                                Firstname = result.Name.First,
                                Surname = result.Name.Last,
                                Email = result.Email,
                                Phone = result.Phone,
                                Country = result.Location.Country,
                                City = result.Location.City
                            };
                            vcards.Add(vcard);
                        }

                        foreach (var vcard in vcards)
                        {
                            Console.WriteLine("Ad: " + vcard.Firstname);
                            Console.WriteLine("Soyad: " + vcard.Surname);
                            Console.WriteLine("Email: " + vcard.Email);
                            Console.WriteLine("Telefon: " + vcard.Phone);
                            Console.WriteLine("Ölkə: " + vcard.Country);
                            Console.WriteLine("Şəhər: " + vcard.City);
                            Console.WriteLine("---------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine("API-ya müraciət zamanı xəta baş verdi: " + response.StatusCode);
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
