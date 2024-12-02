using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class ProductService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> GetProductImageAsync(string productName)
    {
        string apiKey = "AIzaSyChjfhK8a-wHmUpqHJCaPjiBngzl0-XVt4"; // Thay thế bằng API Key của bạn
        string searchEngineId = "65d901ff3b2d243c8"; // Thay thế bằng Search Engine ID của bạn
        string searchUrl = $"https://www.googleapis.com/customsearch/v1?q={productName}&cx={searchEngineId}&searchType=image&key={apiKey}";

        Console.WriteLine(searchUrl); // In URL ra để kiểm tra

        HttpResponseMessage response = await client.GetAsync(searchUrl);
        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseBody); // Kiểm tra phản hồi
        if (response.IsSuccessStatusCode)
        {
            JObject searchResults = JObject.Parse(responseBody);
            if (searchResults["items"] != null && searchResults["items"].HasValues)
            {
                string imageUrl = searchResults["items"][0]["link"].ToString();
                return imageUrl;
            }
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);
        }
        return null;
    }
}
