using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using BaiTap.Models;
namespace BaiTap.Controllers
{
    public class QuanLyKhachHangController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: KhachHang
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:44383/api/quanlykhachhang/khachhang");
            if (response.IsSuccessStatusCode)
            {
                var khachhang = await response.Content.ReadAsAsync<IEnumerable<KhachHang>>();
                return View(khachhang);
            }
            return View("Error");
        }

        // GET: KhachHang/ChiTiet/{id}
        public async Task<ActionResult> ChiTiet(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanlykhachhang/khachhang/{id}");
            if (response.IsSuccessStatusCode)
            {
                var khachhang = await response.Content.ReadAsAsync<KhachHang>();
                return View(khachhang);
            }
            return View("Error");
        }

        // GET: KhachHang/DSChiTiet/{id}
        public async Task<ActionResult> DSChiTiet(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanlykhachhang/chitiet/{id}");
            if (response.IsSuccessStatusCode)
            {
                var chiTietKhachHang = await response.Content.ReadAsAsync<IEnumerable<ChiTietKhachHang>>();
                return View(chiTietKhachHang);
            }
            return View("Error");
        }
    }
}
