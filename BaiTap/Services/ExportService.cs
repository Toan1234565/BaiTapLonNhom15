using System;
using System.Net.Http;
using System.Threading.Tasks;
using BaiTap.Models;
using Newtonsoft.Json;

namespace BaiTap.Services
{
    

    public class ExportService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<PhieuXuat> CreateExportAsync(PhieuXuat phieuXuat)
        {
            var response = await _client.PostAsJsonAsync("https://localhost:44383/api/quanlytonkho/xuat", phieuXuat);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PhieuXuat>(result);
        }

        public async Task<PhieuXuat> GetExportDetailsAsync(int id)
        {
            var response = await _client.GetAsync($"https://localhost:44383/api/quanlytonkho/sanphamxuatkho/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PhieuXuat>(result);
        }
    }

}