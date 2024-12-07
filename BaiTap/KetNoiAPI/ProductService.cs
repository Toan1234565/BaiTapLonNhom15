using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BaiTap.Models;
using HtmlAgilityPack;

public class ProductService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> GetProductImageAsync(string productName)
    {
        string apiKey = "AIzaSyDmLv5QpNYJ_Fqqx2qqK9XZtUBzEaObgFg"; // Thay thế bằng API Key của bạn
        string searchEngineId = "54a2eefbf7c49446b"; // Thay thế bằng Search Engine ID của bạn
        string searchUrl = $"https://www.googleapis.com/customsearch/v1?q={productName}&cx={searchEngineId}&searchType=image&key={apiKey}";

        HttpResponseMessage response = await client.GetAsync(searchUrl);
        string responseBody = await response.Content.ReadAsStringAsync();

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

    private string apiKey = "AIzaSyDmLv5QpNYJ_Fqqx2qqK9XZtUBzEaObgFg"; // Thay thế bằng API Key của bạn

   
    private string searchEngineId = "31d0dba6f706a4d7a"; // Thay thế bằng Search Engine ID của bạn

    public async Task<ChiTietSanPham> GetProductDetailsFromWebAsync(string productName)
    {
        // URL của Google Custom Search API để tìm kiếm thông tin sản phẩm
        string searchUrl = $"https://www.googleapis.com/customsearch/v1?q={productName}&cx={searchEngineId}&key={apiKey}";

        HttpResponseMessage response = await client.GetAsync(searchUrl);
        string responseBody = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            JObject searchResults = JObject.Parse(responseBody);
            if (searchResults["items"] != null && searchResults["items"].HasValues)
            {
                // Lấy URL của trang chi tiết sản phẩm
                string productUrl = searchResults["items"][0]["link"].ToString();
                return await ScrapeProductDetailsFromUrl(productUrl);
            }
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode + " - " + response.ReasonPhrase);
        }
        return null;
    }

    private async Task<ChiTietSanPham> ScrapeProductDetailsFromUrl(string url)
    {
        var productDetails = new ChiTietSanPham();

        HttpResponseMessage response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();

        var doc = new HtmlDocument();
        doc.LoadHtml(responseBody);

        // Ví dụ: lấy thông tin màn hình từ trang web
        var manHinhNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Màn hình']");
        if (manHinhNode != null)
        {
            productDetails.ManHinh = manHinhNode.InnerText.Trim();
        }

        // Tương tự, bạn có thể lấy các thông tin khác
        var heDieuHanhNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Hệ điều hành']");
        if (heDieuHanhNode != null)
        {
            productDetails.HeDieuHanh = heDieuHanhNode.InnerText.Trim();
        }

        var cameraTruocNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Camera trước']");
        if (cameraTruocNode != null)
        {
            productDetails.CameraTruoc = cameraTruocNode.InnerText.Trim();
        }

        var cameraSauNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Camera sau']");
        if (cameraSauNode != null)
        {
            productDetails.CameraSau = cameraSauNode.InnerText.Trim();
        }

        var chipNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Chip']");
        if (chipNode != null)
        {
            productDetails.Chip = chipNode.InnerText.Trim();
        }

        var ramNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='RAM']");
        if (ramNode != null)
        {
            productDetails.RAM = ramNode.InnerText.Trim();
        }

        var boNhoTrongNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Bộ nhớ trong']");
        if (boNhoTrongNode != null)
        {
            productDetails.BoNhoTrong = boNhoTrongNode.InnerText.Trim();
        }

        var simNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Sim']");
        if (simNode != null)
        {
            productDetails.Sim = simNode.InnerText.Trim();
        }

        var pinNode = doc.DocumentNode.SelectSingleNode("//div[@class='parameter']//td[@data-title='Pin']");
        if (pinNode != null)
        {
            productDetails.Pin = pinNode.InnerText.Trim();
        }

        return productDetails;
    }
}


