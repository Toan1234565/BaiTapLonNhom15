using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using BaiTap.Models;
using Newtonsoft.Json;

public class ImportService
{
    private readonly HttpClient _client = new HttpClient();

    public async Task<List<ChiTietPhieuXuat>> UploadFileAsync(HttpContent content)
    {
        var response = await _client.PostAsync("https://localhost:44383/api/import/upload", content);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<dynamic>(result);
        return JsonConvert.DeserializeObject<List<ChiTietPhieuXuat>>(responseData.data.ToString());
    }
}
