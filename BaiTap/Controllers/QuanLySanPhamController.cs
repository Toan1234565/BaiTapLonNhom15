using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // Khai báo DbContext
        private Model1 db = new Model1();

        // GET: QuanLySanPham
        public ActionResult Index()
        {
            return View();
        }

        // Hiển thị danh sách sản phẩm
        public ActionResult SanPham()
        {
            List<SanPham> ds = db.SanPham.ToList();
            return View(ds);
        }

        // Cho phép sửa sản phẩm
        public ActionResult Sua(int id)
        {
            var sp = db.SanPham.Find(id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }

        [HttpPost]
        public ActionResult Sua(SanPham sanpham, TonKho tonkho)
        {
            if (string.IsNullOrEmpty(sanpham.TenSanPham))
            {
                ModelState.AddModelError("", "Tên sản phẩm không được để trống");
                return View(sanpham);
            }

            if (sanpham.Soluong < tonkho.SoLuongTon)
            {
                ModelState.AddModelError("", "Tổng số lượng sản phẩm không thể nhỏ hơn số lượng sản phẩm tồn kho");
                return View(sanpham);
            }

            if (sanpham.Soluong < 0)
            {
                ModelState.AddModelError("", "Số lượng không được nhỏ hơn 0");
                return View(sanpham);
            }

            if (sanpham.Gia < 0)
            {
                ModelState.AddModelError("", "Giá không được nhỏ hơn 0");
                return View(sanpham);
            }

            var update = db.SanPham.Find(sanpham.SanPhamID);
            if (update == null)
            {
                return HttpNotFound();
            }

            update.TenSanPham = sanpham.TenSanPham;
            update.Soluong = sanpham.Soluong;
            update.Gia = sanpham.Gia;
            update.MoTa = sanpham.MoTa;
            update.HangID = sanpham.HangID;
            update.DanhMucID = sanpham.DanhMucID;
            update.HinhAnh = sanpham.HinhAnh;

            var id = db.SaveChanges();
            if (id > 0)
            {
                return RedirectToAction("SanPham");
            }
            else
            {
                ModelState.AddModelError("", "Thay đổi thông tin sản phẩm thất bại");
                return View(sanpham);
            }
        }

        // Xóa sản phẩm và các chi tiết liên quan
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            var sanpham = db.SanPham.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int id)
        {
            try
            {
                var sp = db.SanPham.Find(id);
                if (sp == null)
                {
                    return HttpNotFound();
                }
                var chiTietSanPham = db.ChiTietSanPham.Where(x => x.SanPhamID == id).ToList();
                foreach (var chitiet in chiTietSanPham)
                {
                    db.ChiTietSanPham.Remove(chitiet);
                }
                db.SanPham.Remove(sp);
                db.SaveChanges();

                return RedirectToAction("SanPham");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                ModelState.AddModelError("", "Không thể xóa sản phẩm này. Vui lòng thử lại sau.");
                return View(db.SanPham.Find(id));
            }
        }

        // Hiển thị tất cả chi tiết của các sản phẩm
        public ActionResult DSChiTiet()
        {
            List<ChiTietSanPham> ds = db.ChiTietSanPham.ToList();
            return View(ds);
        }

        // Xem chi tiết của một sản phẩm
        public ActionResult XemChiTiet(int id)
        {
            var chiTietSanPham = db.ChiTietSanPham.Where(c => c.SanPhamID == id).ToList();
            if (chiTietSanPham == null)
            {
                return HttpNotFound();
            }
            return View(chiTietSanPham);
        }

        public ActionResult ThemSanPham()
        {
            var viewmodel = new PhieuNhapKhoViewModel
            {
                SanPham = new SanPham(),
                ChiTietSanPham = new ChiTietSanPham()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSanPham(PhieuNhapKhoViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.SanPham.Add(model.SanPham);
                db.SaveChanges();

                model.ChiTietSanPham.SanPhamID = model.SanPham.SanPhamID;
                db.ChiTietSanPham.Add(model.ChiTietSanPham);

                var id = db.SaveChanges();
                if (id > 0)
                {
                    return RedirectToAction("SanPham");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm thất bại");
                    return View(model);
                }
            }
            return View(model);
        }

        //private static readonly HttpClient client = new HttpClient();

        //    // GET: SanPhamMVC
        //    public ActionResult Index()
        //    {
        //        return View();
        //    }

        //// Hiển thị danh sách sản phẩm
        //public async Task<ActionResult> SanPham()
        //{
        //    HttpResponseMessage response = await client.GetAsync("https://localhost:44383/api/quanlysanpham/sanpham");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var sanPhams = await response.Content.ReadAsAsync<IEnumerable<SanPham>>();
        //        return View(sanPhams);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Không thể kết nối tới API");
        //        return View(new List<SanPham>());
        //    }
        //}


        //// Hiển thị chi tiết sản phẩm
        //public async Task<ActionResult> ChiTiet(int id)
        //    {
        //        HttpResponseMessage response = await client.GetAsync($"https://localhost:44383//api/quanlysanpham/sanpham/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var sanPham = await response.Content.ReadAsAsync<SanPham>();
        //            return View(sanPham);
        //        }
        //        return HttpNotFound();
        //    }

        //    // Tạo mới sản phẩm
        //    [HttpGet]
        //    public ActionResult ThemSanPham()
        //    {
        //        return View();
        //    }

        //    [HttpPost]
        //    public async Task<ActionResult> ThemSanPham(PhieuNhapKhoViewModel model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:44383//api/quanlysanpham/them", model);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("SanPham");
        //            }
        //            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
        //        }
        //        return View(model);
        //    }

        //    // Sửa sản phẩm
        //    [HttpGet]
        //    public async Task<ActionResult> SuaSanPham(int id)
        //    {
        //        HttpResponseMessage response = await client.GetAsync($"https://localhost:44383//api/quanlysanpham/sanpham/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var sanPham = await response.Content.ReadAsAsync<SanPham>();
        //            return View(sanPham);
        //        }
        //        return HttpNotFound();
        //    }

        //    [HttpPost]
        //    public async Task<ActionResult> SuaSanPham(SanPham sanPham)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:44383//api/quanlysanpham/sua", sanPham);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("SanPham");
        //            }
        //            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
        //        }
        //        return View(sanPham);
        //    }

        //    // Xóa sản phẩm
        //    [HttpGet]
        //    public async Task<ActionResult> XoaSanPham(int id)
        //    {
        //        HttpResponseMessage response = await client.GetAsync($"https://localhost:44383//api/quanlysanpham/sanpham/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var sanPham = await response.Content.ReadAsAsync<SanPham>();
        //            return View(sanPham);
        //        }
        //        return HttpNotFound();
        //    }

        //    [HttpPost, ActionName("XoaSanPham")]
        //    public async Task<ActionResult> XacNhanXoa(int id)
        //    {
        //        HttpResponseMessage response = await client.DeleteAsync($"https://localhost:44383//api/quanlysanpham/xoa/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("SanPham");
        //        }
        //        ModelState.AddModelError("", "Xóa sản phẩm thất bại");
        //        return View();
        //    }
        }
    }