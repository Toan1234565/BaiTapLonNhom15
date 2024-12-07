
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using BaiTap.Models;

namespace BaiTap.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        private readonly Model1 db = new Model1();
        private readonly ProductService _productService = new ProductService();
        private static readonly HttpClient client = new HttpClient();
        public ActionResult Error()
        {
            return View();
        }
        // GET: QuanLySanPham/DanhSach
        public async Task<ActionResult> SanPham()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:44383/api/quanlysanpham/sanpham");
            if (response.IsSuccessStatusCode)
            {

                var sanpham = await response.Content.ReadAsAsync<IEnumerable<SanPham>>();
                if (sanpham != null)
                {
                    return View(sanpham);
                }
                ViewBag.Thongbao = "tai danh sach san pham that bai";
                return View("Error");
            }
            return View("Error");
        }


        // GET: QuanLySanPham/ChiTiet/{id}
        public async Task<ActionResult> XemChiTiet(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanlysanpham/xemchitiet/{id}");
            if (response.IsSuccessStatusCode)
            {
                var sanpham = await response.Content.ReadAsAsync<List<ChiTietSanPham>>();
                if (sanpham != null & sanpham.Count > 0)
                {
                    return PartialView("XemChiTiet", sanpham);
                }
                ViewBag.Thongbao = "khong tim thay";
                return View("Error");
            }
            ViewBag.Thongbao = "loi khi gọi API.";
            return View("Error");
        }
       
        // GET: QuanLySanPham/Them
        public ActionResult ThemSanPham()
        {

            var viewModel = new PhieuNhapKhoViewModel
            {
                SanPham = new SanPham(),
                ChiTietSanPham = new ChiTietSanPham()
            };
            return PartialView("_ThemSanPham", viewModel);

        }

        // POST: QuanLySanPham/Them
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThemSanPham(PhieuNhapKhoViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:44383/api/quanlysanpham/them", model);

                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("SanPham");
                }
                else
                {
                    var Thongbao = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Lỗi khi thêm sản phẩm");
                }
            }
            else
            {
                ModelState.AddModelError("", "Dữ liệu nhập vào không hợp lệ.");
            }

            return View(model);
        }

        // GET: QuanLySanPham/Sua/{id}
        // truy cap den San pham khi duoc kich vao 
       
        public async Task<ActionResult> Sua(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/quanlysanpham/sanpham/{id}");
            if (response.IsSuccessStatusCode)
            {
                var sanpham = await response.Content.ReadAsAsync<SanPham>();

                if (sanpham != null)
                {
                    return PartialView("FromSua",sanpham);
                }
                ViewBag.Thongbao = "Không tìm thấy sản phẩm với ID được cung cấp";
                return View("Error");
            }
            ViewBag.Thongbao = "Lỗi khi gọi API";
            return View("Error");
        }

        // POST: QuanLySanPham/Sua
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Sua(SanPham sanpham, TonKho tonkho)
        {
            if (ModelState.IsValid)
            {
                var payload = new { SanPham = sanpham, TonKho = tonkho };
                HttpResponseMessage response = await client.PutAsJsonAsync("https://localhost:44383/api/quanlysanpham/sua", payload);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SanPham");
                }
                ViewBag.Thongbao = "loi";
                return View("Error");
            }
            return View("Error");
        }

        // GET: QuanLySanPham/Xoa/{id}
        public async Task<ActionResult> Xoa(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"https://localhost:44383/api/quanlysanpham/xoa/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SanPham");
            }
            else
            {

                // Lấy thông báo lỗi từ phản hồi API
                var errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Lỗi khi xóa sản phẩm: {errorMessage}";
                return View("Error");
            }
        }



        public ActionResult Index()
        {
            ViewBag.HangList = new SelectList(db.Hang.ToList(), "HangID", "TenHang");
            ViewBag.DanhMucList = new SelectList(db.DanhMuc.ToList(), "DanhMucID", "TenDanhMuc");
            return View();
        }

        public async Task<ActionResult> TimKiem(string name)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/timkiem/timkiemsanpham?name={name}");
            if (response.IsSuccessStatusCode)
            {
                var sanpham = await response.Content.ReadAsAsync<IEnumerable<SanPham>>();

                return View(sanpham);
            }
            ViewBag.Thongbao = "loi khi goi API";
            return View("Error");
        }
        public ActionResult FromLoc()
        {
            return PartialView("FromLoc");
        }
        public async Task<ActionResult> LocSP(string name = null, int? IDHang = null, int? IDDanhMuc = null, double? to = null, double? from = null, string sx = null)
        {
            string url = $"https://localhost:44383/api/timkiem/locsanpham?name={name}&IDHang={IDHang}&IDDanhMuc={IDDanhMuc}&from={from}&to={to}&sx={sx}";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var kq = await response.Content.ReadAsAsync<List<SanPham>>();
                    return View(kq);
                }
                else
                {
                    ViewBag.Thongbao = $"Lỗi khi gọi API: {response.StatusCode} - {response.ReasonPhrase}";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Thongbao = $"Ngoại lệ khi gọi API: {ex.Message}";
                return View("Error");
            }
        }

        // Tạo hàm GET cho các hành động khác tương tự như `TimKiem`
        public async Task<ActionResult> GiaTang()
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/timkiem/giatang");
            if (response.IsSuccessStatusCode)
            {
                var kq = await response.Content.ReadAsAsync<List<SanPham>>();
                return View(kq);
            }
            return View("Error");
        }
        public async Task<ActionResult> GiaGIam()
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/timkiem/giagiam");
            if (response.IsSuccessStatusCode)
            {
                var kq = await response.Content.ReadAsAsync<List<SanPham>>();
                return View(kq);
            }
            return View("Error");
        }
        public ActionResult FromSoSanh()
        {
            return PartialView("FromSoSanh");
        }

        public async Task<ActionResult> Sosanh(int? id1, int? id2)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/timkiem/SOSANH?id1={id1}&id2={id2}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<dynamic>();
                return View(result.comparisonResult);
            }
            return View("Error");
        }












    }
}
