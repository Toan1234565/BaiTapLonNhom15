using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using BaiTap.Models;
using static Google.Rpc.Context.AttributeContext.Types;

namespace BaiTap.Controllers
{
    public class QuanLyTonKhoController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiUrl = "https://localhost:44383/api/quanlytonkho";

        // GET: QuanLyTonKho
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> SanPhamTonKho()
        {
            try
            {
                HttpResponseMessage kq = await client.GetAsync($"{apiUrl}/sanphamtonkho");
                if (kq.IsSuccessStatusCode)
                {
                    var sanpham = await kq.Content.ReadAsAsync<IEnumerable<TonKho>>();
                    if (sanpham != null)
                    {
                        return View(sanpham);
                    }
                    ViewBag.Thongbao = "tai danh sach san pham that bai";
                    return View("Error");
                }
                ViewBag.Thongbao = "Có lỗi xảy ra";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Thongbao = $"Có lỗi khi lấy API: {ex.Message}";
                return View("Error");
            }

        }
        public ActionResult nhap()
        {
            return PartialView("nhap",new PhieuNhapKhoViewModel());
        }
        [HttpPost]
        public async Task<ActionResult> Nhap(PhieuNhapKhoViewModel model)
        {
            try
            {
                HttpResponseMessage kq = await client.PostAsJsonAsync($"{apiUrl}/nhap", model);
                if (kq.IsSuccessStatusCode)
                {
                    return RedirectToAction("SanPhamTonKho");
                }
                ViewBag.Thongbao = "Có lỗi xảy ra";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Thongbao = $"Có lỗi khi lấy API: {ex.Message}";
                return View("Error");
            }
        }
        public async Task<ActionResult> SuaTonKho(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{apiUrl}/suatonkho/{id}");
            if (response.IsSuccessStatusCode)
            {
                var tonKho = await response.Content.ReadAsAsync<TonKho>();
                if (tonKho != null)
                {
                    return PartialView("Sua", tonKho);
                }
                ViewBag.ErrorMessage = "that bai.";
                return View("Error");
            }
            ViewBag.ErrorMessage = "ko ket noi dc voi API.";
            return View("Error");
        }

        [HttpPost]
        public async Task<ActionResult> SuaTonKho(TonKho model)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{apiUrl}/suatonkho/{model.SanPhamID}", model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "Unable to update data.";
            return View("Error");
        }

        public async Task<ActionResult> XemThongTin(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{apiUrl}/thongtinsp/{id}");
            if (response.IsSuccessStatusCode)
            {
                var sanPham = await response.Content.ReadAsAsync<SanPham>(); // Adjust according to the data structure
                if (sanPham != null)
                {
                    return PartialView("_XemThongTin", sanPham);
                }
                ViewBag.ErrorMessage = "that bai.";
                return View("Error");
            }
            ViewBag.ErrorMessage = "ko ket noi dc API.";
            return View("Error");
        }
        public async Task<ActionResult> SapxepTang()
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanly");
            if (response.IsSuccessStatusCode)
            {
                var kq = await response.Content.ReadAsAsync<List<TonKho>>();
                return View(kq);
            }
            return View("Error");
        }
        public async Task<ActionResult> SapxepGiam()
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanlytonkho/sapxepgiam");
            if (response.IsSuccessStatusCode)
            {
                var kq = await response.Content.ReadAsAsync<List<TonKho>>();
                return View(kq);
            }
            return View("Error");
        }


    }

}

