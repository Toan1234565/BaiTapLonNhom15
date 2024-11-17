using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class QuanLyTonKhoController : Controller
    {
        private Model1 db = new Model1();

        // GET: QuanLyTonKho
        public ActionResult Index()
        {
            return View();
        }

        // Hiển thị danh sách sản phẩm tồn kho
        public ActionResult SanPhamTonKho()
        {
            List<TonKho> ds = db.TonKho.ToList();
            return View(ds);
        }

        // Nhập sản phẩm mới
        public ActionResult Nhap()
        {
            return View(new PhieuNhapKhoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nhap(PhieuNhapKhoViewModel model)
        {
            if (model.SanPhamID.HasValue)
            {
                var sanpham = db.SanPham.Find(model.SanPhamID.Value);
                if (sanpham != null)
                {
                    return RedirectToAction("NhapSanPhamCoSan", new { id = model.SanPhamID.Value });
                }
            }
            return RedirectToAction("NhapKho");
        }

        // Nhập sản phẩm có sẵn
        public ActionResult NhapSanPhamCoSan(int id)
        {
            var sanPham = db.SanPham.Find(id);
            if (sanPham != null)
            {
                var viewModel = new PhieuNhapKhoViewModel
                {
                    SanPhamID = sanPham.SanPhamID,
                    soluong = 0,
                    Gia = sanPham.Gia ?? 0
                };
                return View(viewModel);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NhapSanPhamCoSan(PhieuNhapKhoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sp = db.SanPham.Find(model.SanPhamID);
                if (sp != null)
                {
                    var tonKho = db.TonKho.FirstOrDefault(p => p.SanPhamID == model.SanPhamID);
                    if (tonKho == null)
                    {
                        tonKho = new TonKho
                        {
                            SanPhamID = model.SanPhamID.Value,
                            SoLuongTon = 0,
                            NgayCapNhat = DateTime.Now
                        };
                        db.TonKho.Add(tonKho);
                    }

                    sp.Soluong += model.soluong;

                    sp.Gia = model.Gia;
                    tonKho.SoLuongTon += model.soluong;
                    tonKho.NgayCapNhat = DateTime.Now;
                    //sp.SoLuongDaBan = sp.Soluong - tonKho.SoLuongTon;
                    //sp.TongDoanhThu = sp.SoLuongDaBan * sp.Gia;

                    db.SaveChanges();
                    return RedirectToAction("SanPhamTonKho");
                }
            }
            return View(model);
        }

        // Nhập sản phẩm mới vào kho
        public ActionResult NhapKho()
        {
            var viewmodel = new PhieuNhapKhoViewModel
            {
                SanPham = new SanPham(),
                ChiTietSanPham = new ChiTietSanPham(),
                TonKho = new TonKho()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NhapKho(PhieuNhapKhoViewModel model, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {

                string path = "";
                if (HinhAnh != null)
                {
                    string filename = Path.GetFileName(HinhAnh.FileName);
                    path = Path.Combine(Server.MapPath("~/img"), filename);
                    HinhAnh.SaveAs(path);
                    model.SanPham.HinhAnh = "~/img/" + filename;
                }

                var sanpham = model.SanPham;
                var chiTiet = model.ChiTietSanPham;
                var tonKho = model.TonKho;

                // Cập nhật số lượng đã bán và tổng doanh thu
                sanpham.Soluong = tonKho.SoLuongTon;
                //sanpham.SoLuongDaBan = sanpham.Soluong - tonKho.SoLuongTon;
                //sanpham.TongDoanhThu = sanpham.Gia * sanpham.SoLuongDaBan;
                // Lưu thông tin sản phẩm
                db.SanPham.Add(sanpham);
                db.SaveChanges();

                // Lưu thông tin chi tiết sản phẩm
                chiTiet.SanPhamID = sanpham.SanPhamID;
                db.ChiTietSanPham.Add(chiTiet);
                db.SaveChanges();

                // Lưu thông tin tồn kho
                tonKho.SanPhamID = sanpham.SanPhamID;
                tonKho.NgayCapNhat = DateTime.Now;
                db.TonKho.Add(tonKho);
                db.SaveChanges();
                if (sanpham.SanPhamID > 0)
                {

                    return RedirectToAction("SanPhamTonKho");
                }
                else
                {
                    ModelState.AddModelError("", "Them that bai");
                    return View(model);
                }
            }

            return View(model);
        }

        public ActionResult ThongTinSP(int id)
        {
            var Sanpham = db.SanPham.Where(c => c.SanPhamID == id).ToList();
            if (Sanpham == null)
            {
                return HttpNotFound();
            }

            return View();
        }
        // Xuất kho (Chức năng có thể được mở rộng)
        public ActionResult XuatKho()
        {
            return View();
        }

        // Sắp xếp sản phẩm tồn kho
        public ActionResult SapxepTang()
        {
            var kq = db.TonKho.OrderBy(x => x.SoLuongTon).ToList();
            return View(kq);
        }
        public ActionResult SapxepGiam()
        {
            var kq = db.TonKho.OrderByDescending(x => x.SoLuongTon).ToList();
            return View(kq);
        }
        public ActionResult LocNgay(DateTime? Time, DateTime? enddate)
        {
            var tonkho = db.TonKho.AsQueryable();
            if (Time.HasValue)
            {
                tonkho = tonkho.Where(x => x.NgayCapNhat >= Time.Value);
            }
            if (enddate.HasValue)
            {
                tonkho = tonkho.Where(x => x.NgayCapNhat <= enddate.Value);
            }
            return View(tonkho.ToList());
        }
        public ActionResult SuaTonKho(int id)
        {
            var tonkho = db.TonKho.Find(id);
            if(tonkho == null)
            {
                return HttpNotFound();
            }
            return View(tonkho);
        }
        [HttpPost]
        public ActionResult SuaTonKho(int id, TonKho ton)
        {
            var updata = db.TonKho.Find(id);
            if(updata == null)
            {
                return HttpNotFound();  
            }
            updata.SoLuongTon = ton.SoLuongTon;
            var s = db.SaveChanges();
            if(s > 0)
            {
                return View("SanPhamTonKho");
            }
            else
            {
                ModelState.AddModelError("", "Thay đổi thông tin thất bại!");
                return View(ton);
            }
        }


        private static readonly HttpClient client = new HttpClient();
        public async Task<ActionResult> CheckKho()
        {
            var s = await client.GetAsync("");
            if (s.IsSuccessStatusCode)
            {
                var tonkho = await s.Content.ReadAsAsync<IEnumerable<TonKho>>();
                return View(tonkho); // hien thij san pham ton kho thap
            }
            return View("Error");// hien thi trang loi neu co van de API
        }
        public async Task<ActionResult> TonKho()
        {
            // Replace with your actual API key and company ID
            string apiKey = "your_api_key";
            string companyId = "your_company_id";

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://cloudapi.inflowinventory.com");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", apiKey);

            var response = await client.GetAsync($"/api/warehouses/{companyId}/stocklevels");
            if (response.IsSuccessStatusCode)
            {
                var tonKhoList = await response.Content.ReadAsAsync<List<TonKho>>();
                return View(tonKhoList);
            }
            return View(new List<TonKho>());
        }


    }
}








