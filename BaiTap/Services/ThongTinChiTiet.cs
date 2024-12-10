using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BaiTap.Services
{
    public class ThongTinChiTiet
    {
        private readonly HttpClient _httpClient;

        public ThongTinChiTiet(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JObject> LayThongTin(string productName)
        {
            var url = $"https://api.amazon.com/product?name={productName}"; // Thay đổi URL tùy theo cấu trúc API
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }
    }
}
