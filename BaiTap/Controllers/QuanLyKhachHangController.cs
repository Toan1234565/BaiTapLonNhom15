using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class QuanLyKhachHangController : Controller
    {
        private Model1 db = new Model1();
        // GET: QuanLyKhachHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DSKhachHang()
        {
            List<KhachHang> ds = db.KhachHang.ToList();
            return View(ds);
        }
        public ActionResult ChiTietKH(int id)
        {
            var khachhang = db.KhachHang.Include("DonHang").Include("TaiKhoanKH").FirstOrDefault(kh => kh.KhachHangID == id);
            if (khachhang == null)
            {
                return HttpNotFound();
            }
            return View(khachhang);
        }
    }
}