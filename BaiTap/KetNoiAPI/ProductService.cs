using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BaiTap.Models;

public class ProductService
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string name = "";

    public async Task<string> GetProductImageAsync(string productName)
    {
        string apiKey = "AIzaSyCYRZEQ1wohBsBPSpiwWuP8EOlSOA82XoE"; // Thay thế bằng API Key của bạn
        string searchEngineId = "54a2eefbf7c49446b"; // Thay thế bằng Search Engine ID của bạn54a2eefbf7c49446b
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
                string sanpham = await GetProductImageAsync(productName);

                return imageUrl;
                
            }
           
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);
        }
        return null;

    }
    private async Task<string> GetProductDescriptionAsync(string TenSanPham)
    {
        return await Task.FromResult($"Mo ta cua san pham ");
    }
}
