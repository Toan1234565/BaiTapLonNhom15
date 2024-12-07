using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaiTap.Models;

public class InventoryService
{
    private readonly Model1 db = new Model1();

    public async Task<List<TonKho>> CheckInventoryLevels(int lowStockThreshold, int highStockThreshold)
    {
        var lowStockProducts = db.TonKho
            .Where(t => t.SoLuongTon <= lowStockThreshold)
            .ToList();

        var highStockProducts = db.TonKho
            .Where(t => t.SoLuongTon >= highStockThreshold)
            .ToList();

        var alertProducts = lowStockProducts.Concat(highStockProducts).ToList();

        // Gửi cảnh báo cho người quản lý nếu có sản phẩm đạt mức cảnh báo
        if (alertProducts.Any())
        {
            // Gửi email hoặc SMS cảnh báo (có thể thêm phương thức gửi cảnh báo ở đây)
        }

        return alertProducts;
    }
}
