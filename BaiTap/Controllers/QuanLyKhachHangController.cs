using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class QuanLyKhachHangController : Controller
    {
        private Model1 db = new Model1();
        private static readonly HttpClient client = new HttpClient();
        // GET: QuanLyKhachHang/DSKhachHang
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> DSKhachHang()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:44383/api/quanlykhachhang/khachhang");
            if (response.IsSuccessStatusCode)
            {

                var khachhang = await response.Content.ReadAsAsync<IEnumerable<KhachHang>>();
                return View(khachhang);
            }
            return View("Error");
        }

        public async Task<ActionResult> ChiTietKH(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanlykhachhang/chitiet/{id}");
            if (response.IsSuccessStatusCode)
            {
                var khachhang = await response.Content.ReadAsAsync<List<ChiTietKhachHang>>();
                if (khachhang != null & khachhang.Count > 0)
                {
                    return View(khachhang);
                }
                ViewBag.Thongbao = "khong tim thay";
                return View("Error");
            }
            ViewBag.Thongbao = "loi khi gọi API.";
            return View("Error");
        }
    }

}