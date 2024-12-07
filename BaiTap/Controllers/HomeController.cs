using BaiTap.Models;
using Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            var sanpham = db.SanPham.ToList();
            var kq = sanpham.Select(x => new
            {
                SanPham = x,
                SoLuongDaban = x.Soluong.GetValueOrDefault() - x.TonKho.Sum(tk => tk.SoLuongTon)
            }).OrderByDescending(x => x.SoLuongDaban).Take(10).Select(x => x.SanPham).ToList();

            var Danhthu = sanpham.Select(p => new
            {
                SanPham = p,
                Tong = (p.Soluong.GetValueOrDefault() - p.TonKho.Sum(tk => tk.SoLuongTon) * p.Gia.GetValueOrDefault())
            }).OrderByDescending(p => p.Tong).Take(10).Select(p => p.SanPham).ToList();
            var viewmodel = new Dashboard
            {
                kq = kq,
                Doanhthu = Danhthu
            };

            return View(viewmodel);
        }
        public class Dashboard
        {
            public List<SanPham> kq { get; set; }
            public List<SanPham> Doanhthu { get; set; }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Banchay1()
        {
            var sanpham = db.SanPham.ToList();
            var kq = sanpham.Select(x => new
            {
                SanPham = x,
                SoLuongDaban = x.Soluong.GetValueOrDefault() - x.TonKho.Sum(tk => tk.SoLuongTon)
            }).OrderByDescending(x => x.SoLuongDaban).Take(10).Select(x => x.SanPham).ToList();
            return View(kq);
        }

        public ActionResult Index1()
        {
            var productBrands = db.SanPham
                .GroupBy(p => p.Hang.TenHang)
                .Select(g => new { Brand = g.Key, Quantity = g.Sum(p => p.Soluong) ?? 0
                })
                .ToList();
            ViewBag.TotalProducts = db.SanPham.Sum(p => p.Soluong);
            ViewBag.TotalOrders = db.DonHang.Sum(o => o.TongTien);
            ViewBag.ProductBrands = productBrands;

            return View();
        }
    }
}






