using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaiTap.Models;
using BaiTap.Services;
using static Google.Rpc.Context.AttributeContext.Types;

namespace BaiTap.Controllers
{
    public class QuanLyTonKhoController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ExportService ex = new ExportService();
        private static readonly string apiUrl = "https://localhost:44383/api/quanlytonkho";
        private static Model1 db = new Model1();

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
        public async Task<ActionResult> PhieuNhapKho()
        {
            try
            {
                HttpResponseMessage kq = await client.GetAsync($"{apiUrl}/phieunhapkho");
                if (kq.IsSuccessStatusCode)
                {
                    var phieu = await kq.Content.ReadAsAsync<IEnumerable<ChiTietPhieuNhap>>();
                    if (phieu != null)
                    {
                        return View(phieu);
                    }
                    else
                    {
                        ViewBag.Thongbao = "co loi trong qua trinh lay danh sach";
                        return View("Error");
                    }

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
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file) {
            if( file == null || file.ContentLength <= 0)
            {
                ViewBag.Thongbao = "tep ko hop le";
                return RedirectToAction("PhieuNhapKho");
            }
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    var filecontennt = new MultipartFormDataContent();
                    filecontennt.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("from-data")
                    {
                        Name = "file",
                        FileName = file.FileName
                    };
                    filecontennt.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                   

                    content.Add(filecontennt);
                    HttpResponseMessage kq = await client.GetAsync($"{apiUrl}/nhapfile");

                    if (kq.IsSuccessStatusCode)
                    {
                        return RedirectToAction("PhieuNhapKho");
                    }
                    ViewBag.Thongbao = "Có lỗi xảy ra";
                    return View("Error");
                }
                
            }
            catch(Exception ex)
            {
                ViewBag.Thongbao = $"Có lỗi khi lấy API: {ex.Message}";
                return View("Error");
            }
             
        }

        public async Task<ActionResult> PhieuXuatKho()
        {
            try
            {
                HttpResponseMessage kq = await client.GetAsync($"{apiUrl}/phieuxuatkho");
                if (kq.IsSuccessStatusCode)
                {
                    var phieu = await kq.Content.ReadAsAsync<IEnumerable<ChiTietPhieuXuat>>();
                    if (phieu != null)
                    {
                        return View(phieu);
                    }
                    else
                    {
                        ViewBag.Thongbao = "co loi trong qua trinh lay danh sach";
                        return View("Error");
                    }
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
        private readonly ExportService _exportService = new ExportService();

        [HttpGet]
        public ActionResult CreateExport()
        {
            var sanPhamList = db.SanPham.ToList();
            ViewBag.SanPhamList = new SelectList(sanPhamList, "SanPhamID", "TenSanPham");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateExport(FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                var SanPhamList = db.SanPham.ToList();
                ViewBag.SanPhamList = new SelectList(SanPhamList, "SanPhamID", "TenSanPham");
                return View();
            }

            // Kiểm tra các giá trị đầu vào từ form
            var ngayXuatStr = form["NgayXuat"];
            var soDienThoai = form["KhachHang.SoDienThoai"];
            var hoTen = form["KhachHang.HoTen"];
            var productIDs = form.GetValues("SanPhamID");
            var quantities = form.GetValues("SoLuong");
            var prices = form.GetValues("Gia");

            // Xử lý ngoại lệ khi giá trị null
            if (string.IsNullOrEmpty(ngayXuatStr) || string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(hoTen) || productIDs == null || quantities == null || prices == null)
            {
                ModelState.AddModelError("", "Một hoặc nhiều giá trị nhập vào bị thiếu hoặc không hợp lệ.");
                return View();
            }

            // Chuyển đổi và xử lý giá trị chuỗi
            var phieuXuat = new PhieuXuat
            {
                NgayXuat = DateTime.Parse(ngayXuatStr),
                KhachHang = new KhachHang { SoDienThoai = soDienThoai, HoTen = hoTen },
                ChiTietPhieuXuat = new List<ChiTietPhieuXuat>()
            };

            for (int i = 0; i < productIDs.Length; i++)
            {
                phieuXuat.ChiTietPhieuXuat.Add(new ChiTietPhieuXuat
                {
                    SanPhamID = int.Parse(productIDs[i]),
                    SoLuong = int.Parse(quantities[i]),
                    DonGia = double.Parse(prices[i])
                });
            }

            var createdExport = await _exportService.CreateExportAsync(phieuXuat);
            return RedirectToAction("ExportDetails", new { id = createdExport.PhieuXuatID });
        }



        [HttpGet]
        public async Task<ActionResult> ExportDetails(int id)
        {
            var phieuXuat = await _exportService.GetExportDetailsAsync(id);
            return View(phieuXuat);
        }

        [HttpGet]
        public JsonResult GetGiaSanPham(int id)
        {
            var sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return Json(new { error = "Sản phẩm không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Gia = sanPham.Gia }, JsonRequestBehavior.AllowGet);
        }






        public ActionResult nhap()
        {
            return PartialView("nhap", new PhieuNhapKhoViewModel());
        }
        [HttpPost]
        public async Task<ActionResult> Nhap(PhieuNhapKhoViewModel model)
        {
            try
            {
                HttpResponseMessage kq = await client.PostAsJsonAsync($"https://localhost:44383/api/quanlytonkho/nhap", model);
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
        public async Task<ActionResult> SuaTonKho(TonKho ton, int id)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{apiUrl}/suatonkho/{id}",ton);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SanPhamTonKho");
            }
            ViewBag.ErrorMessage = "cap nhat that bai.";
            return View("Error");
        }

        public async Task<ActionResult> XemThongTin(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{apiUrl}/thongtinsp/{id}");
            if (response.IsSuccessStatusCode)
            {
                var sanPham = await response.Content.ReadAsAsync<List<SanPham>>(); // Adjust according to the data structure
                if (sanPham != null && sanPham.Count > 0)
                {
                    return PartialView("XemThongTin", sanPham);
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

        private readonly ChartService BD = new ChartService();
        public ActionResult BD1()
        {
            var data = BD.GetPieChartData();
            return View(data);
        }
        public ActionResult BD2()
        {
            var data = BD.GetInventoryHistory();
            return View(data);
        }
       
        public async Task<ActionResult> Check()
        {
            HttpResponseMessage ton = await client.GetAsync("https://localhost:44383/api/quanlytonkho/check");
            if (ton.IsSuccessStatusCode)
            {
                var tonkho = await ton.Content.ReadAsAsync<IEnumerable<TonKho>>();
               
                if (tonkho != null && tonkho.Any() )
                {
                   
                    return View(tonkho);
                }
                ViewBag.Thongbao = "tai danh sach san pham that bai";
                return View("Error");
            }
            ViewBag.ErrorMessage = "ko ket noi dc API.";
            return View("Error");
        }
    }

}

