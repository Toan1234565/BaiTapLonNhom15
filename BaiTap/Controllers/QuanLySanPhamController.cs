
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
                return View(sanpham);
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
                if(sanpham != null & sanpham.Count > 0)
                {
                    return View(sanpham);
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
            
            return View();
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
            ViewBag.DanhMucs = new SelectList(db.DanhMuc.ToList(), "DanhMucID", "TenDanhMuc"); 
            ViewBag.Hangs = new SelectList(db.Hang.ToList(), "HangID", "TenHang");
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
                
                if(sanpham != null)
                {
                    return View(sanpham);
                }
                ViewBag.Thongbao = "Không tìm thấy sản phẩm với ID đuọc cung câp";
                return View("Eror");
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
            }
            return View(sanpham);
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
        public async Task<ActionResult> ANH()
        {
            var sp = db.SanPham.ToList();
            foreach (var sanpham in sp)
            {
                string url = await _productService.GetProductImageAsync(sanpham.TenSanPham);
                if (!string.IsNullOrEmpty(url)) // Cập nhật khi URL không rỗng
                {
                    sanpham.HinhAnh = url;
                    db.Entry(sanpham).State = EntityState.Modified;
                }
            }
            await db.SaveChangesAsync();
            return RedirectToAction("SanPham");
        }

    }
}
