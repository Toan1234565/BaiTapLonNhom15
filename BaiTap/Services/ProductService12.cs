using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BaiTap.Models;

namespace BaiTap.Services
{
    public class ProductService12
    {
        private readonly HttpClient client = new HttpClient();
        private readonly EmailService emailService = new EmailService();

        public async Task CheckInventoryLevels()
        {
            using (var db = new Model1())
            {
                var lowStockThreshold = 10; // Ngưỡng tồn kho thấp
                var highStockThreshold = 100; // Ngưỡng tồn kho cao
                var comparisonDate = DateTime.Now.AddDays(-30); // Tính toán ngày so sánh trước

                var lowStockProducts = db.TonKho
                    .Where(t => t.SoLuongTon <= lowStockThreshold)
                    .ToList();

                var highStockProducts = db.TonKho
                    .Where(t => t.SoLuongTon >= highStockThreshold && t.NgayCapNhat <= comparisonDate)
                    .ToList();

                if (lowStockProducts.Any())
                {
                    // Gửi cảnh báo tồn kho thấp
                    foreach (var product in lowStockProducts)
                    {
                        string subject = $"Cảnh báo: Sản phẩm {product.SanPham.TenSanPham} tồn kho xuống thấp";
                        string body = $"Sản phẩm {product.SanPham.TenSanPham} tồn kho xuống thấp ({product.SoLuongTon} cái).";
                        emailService.SendEmail("admin@example.com", subject, body);
                    }
                }

                if (highStockProducts.Any())
                {
                    // Gửi cảnh báo tồn kho cao trong một khoảng thời gian
                    foreach (var product in highStockProducts)
                    {
                        string subject = $"Cảnh báo: Sản phẩm {product.SanPham.TenSanPham} tồn kho cao";
                        string body = $"Sản phẩm {product.SanPham.TenSanPham} tồn kho còn nhiều ({product.SoLuongTon} cái) trong hơn 30 ngày.";
                        emailService.SendEmail("admin@example.com", subject, body);
                    }
                }
            }
        }
    }

}
