using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class TimKiemController : Controller
    {
        private Model1 db = new Model1();
        // GET: TimKiem
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TimKiem(string name)
        {
            var kq = db.SanPham.Where(sp=> sp.TenSanPham.Contains(name) || sp.MoTa.Contains(name)).ToList();
            return View(kq);
        }

        public ActionResult LocSP(string name, int? IDHang, int? IDDanhMuc, double? to = null, double? from = null, string sx = null)
        {
            IQueryable<SanPham> kq = db.SanPham;

            // Lọc theo danh mục
            if (IDDanhMuc.HasValue && IDDanhMuc.Value != 0)
            {
                kq = kq.Where(sp => sp.DanhMucID == IDDanhMuc.Value);
            }

            // Lọc theo hãng
            if (IDHang.HasValue && IDHang.Value != 0)
            {
                kq = kq.Where(sp => sp.HangID == IDHang.Value);
            }

            // Lọc theo khoảng giá
            if (from.HasValue && to.HasValue && from.Value > 0 && to.Value > 0)
            {
                kq = kq.Where(sp => sp.Gia >= from.Value && sp.Gia <= to.Value);
            }

            // Lọc theo tên sản phẩm
            if (!string.IsNullOrEmpty(name))
            {
                kq = kq.Where(sp => sp.TenSanPham.Contains(name));
            }

            // Sắp xếp
            switch (sx)
            {
                case "Giatang":
                    kq = kq.OrderBy(sp => sp.Gia);
                    break;
                case "Giagiam":
                    kq = kq.OrderByDescending(sp => sp.Gia);
                    break;
                default:
                    kq = kq.OrderBy(sp => sp.Gia);
                    break;
            }

            return View(kq.ToList());
        }
        public ActionResult GiaTang()
        {
            var kq = db.SanPham.OrderBy(x => x.Gia).ToList();
            return View(kq);
        }
        public ActionResult GiaGiam()
        {
            var kq = db.SanPham.OrderByDescending(x => x.Gia).ToList();
            return View(kq);
        }
        public ActionResult Banchay1()
        {
           var sanpham = db.SanPham.ToList();
            var kq = sanpham.Select(x => new
            {
                SanPham = x,
                SoLuongDaban = x.Soluong.GetValueOrDefault() - x.TonKho.Sum(tk => tk.SoLuongTon)
            }). OrderByDescending(x => x.SoLuongDaban).Take(10).Select(x => x.SanPham).ToList();
            return View(kq);    
        }
       public ActionResult Banchay2()
        {
            var kq = db.SanPham.OrderBy(x=>x.SoLuongDaBan).Take(5).ToList ();
            return View(kq);
        }
    }
 }
