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

        // GET: QuanLyKhachHang/ChiTietKH/{id}
        public async Task<ActionResult> ChiTietKH(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanlykhachhang/chitiet/{id}");
            if (response.IsSuccessStatusCode)
            {
                var khachhang = await response.Content.ReadAsAsync<List<ChiTietKhachHang>>();
                if (khachhang != null && khachhang.Count > 0)
                {
                    return PartialView("ChiTietKH", khachhang);
                }
                ViewBag.Thongbao = "Không tìm thấy chi tiết khách hàng.";
                return View("Error");
            }
            ViewBag.Thongbao = "Lỗi khi gọi API.";
            return View("Error");
        }
    }
}
