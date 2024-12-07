using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTap.Services
{
    public class ChartService
    {
        private readonly Model1 db = new Model1();
        public List<PieChartData> GetPieChartData()
        {
            using (var db = new Model1())
            {
                var query = db.TonKho
                    .GroupBy(t => t.SanPham.TenSanPham)
                    .Select(g => new PieChartData
                    {
                        Label = g.Key,
                        Value = g.Sum(t => t.SoLuongTon) ?? 0
                    })
                    .ToList();

                return query;
            }
        }
        public List<InventoryHistory> GetInventoryHistory()
        {
            var query = db.TonKho
                .Where(t => t.NgayCapNhat.HasValue)
                .GroupBy(t => t.NgayCapNhat.Value)
                .Select(g => new InventoryHistory
                {
                    Date = g.Key,
                    TotalStock = g.Sum(t => t.SoLuongTon) ?? 0
                })
                .OrderBy(h => h.Date)
                .ToList();

            return query;
        }
        
    }
    public class LineChartData
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
    }
    public class InventoryHistory
    {
        public DateTime Date { get; set; }
        public int TotalStock { get; set; }
    }
}

public class PieChartData
{
    public string Label { get; set; }
    public int Value { get; set; }
}
public class ProductBrandViewModel
{
    public string Brand { get; set; }
    public int Quantity { get; set; }
}





