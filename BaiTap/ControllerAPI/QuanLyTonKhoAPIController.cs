using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using System.Data.Entity;
using BaiTap.Services;
using System.Net.Mail;
using OfficeOpenXml;
using System.Web;
using System.Data.SqlTypes;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
namespace BaiTap.Controllers
{
    [RoutePrefix("api/quanlytonkho")]
    public class QuanLyTonKhoAPIController : ApiController
    {
        private readonly ProductService12 check = new ProductService12();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Model1 db = new Model1();

        // GET: api/quanlytonkho
        [HttpGet]
        [Route("")]
        public IHttpActionResult Index()
        {
            return Ok("Quan Ly Ton Kho API");
        }

        // GET: api/quanlytonkho/sanphamtonkho
        [HttpGet]
        [Route("sanphamtonkho")]
        public IHttpActionResult SanPhamTonKho()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ds = db.TonKho.ToList();
                return Ok(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách sản phẩm tồn kho.");
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("phieunhapkho")]
        public IHttpActionResult PhieuNhapKho()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ds = db.ChiTietPhieuNhap.ToList();
                return Ok(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "loi khi lay danh sach");
                return InternalServerError(ex);
            }
        }

        // POST: api/quanlytonkho/nhapfile
        [HttpPost]
        [Route("nhapfile")]
        public async Task<IHttpActionResult> UploadFile()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var http = HttpContext.Current.Request;
            if (http.Files.Count == 0)
            {
                return BadRequest("Không có file để upload");
            }

            var sanphamfile = http.Files[0];
            if (sanphamfile == null || sanphamfile.ContentLength <= 0)
            {
                return BadRequest("Tệp không hợp lệ");
            }

            // Thiết lập LicenseContext cho EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                var phieuNhap = new PhieuNhap
                {
                    NgayNhap = DateTime.Now,
                    NhaCungCap = "nha cung cap a",
                    Kho = "a"
                };
                using (var package = new ExcelPackage(sanphamfile.InputStream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        return BadRequest("Không tìm thấy trang tính nào trong tệp Excel");
                    }

                    var rowCount = worksheet.Dimension.Rows;
                    for (var row = 2; row <= rowCount; row++)
                    {
                        var masp = worksheet.Cells[row, 1].Value?.ToString();
                        if (string.IsNullOrEmpty(masp))
                        {
                            return BadRequest($"Hàng {row}: Tên sản phẩm không được bỏ trống");
                        }

                        var danhmucId = int.TryParse(worksheet.Cells[row, 5].Value?.ToString(), out int danhmucid) ? danhmucid : (int?)null;
                        try
                        {


                            if (danhmucId != null && !await db.DanhMuc.AnyAsync(dm => dm.DanhMucID == danhmucid))
                            {
                                var DM = await db.DanhMuc.FirstOrDefaultAsync(s => s.DanhMucID == danhmucid);
                                if (DM == null)
                                {
                                    DM = new DanhMuc
                                    {
                                        DanhMucID = danhmucid,
                                        TenDanhMuc = "Khach"
                                    };
                                    db.DanhMuc.Add(DM);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }catch(Exception ex)
                        {
                            return BadRequest($"Loi khi them danh muc: {ex.Message}");
                        }

                        var IDhang = int.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out int Hang) ? Hang : (int?)null;
                        try
                        {


                            if (IDhang != null && !await db.Hang.AnyAsync(h => h.HangID == IDhang))
                            {
                                var hang = await db.Hang.FirstOrDefaultAsync(h => h.HangID == Hang);
                                if (hang == null)
                                {
                                    hang = new Hang
                                    {
                                        HangID = Hang,
                                        TenHang = "Khac"
                                    };
                                    db.Hang.Add(hang);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return BadRequest($" loi trong qua trinh them hang{ex.Message}");
                        }
                        var sanpham = await db.SanPham.FirstOrDefaultAsync(sp => sp.TenSanPham == masp);
                        if (sanpham != null)
                        {
                            var addfile = int.TryParse(worksheet.Cells[row, 2].Value?.ToString(), out int soluong) ? soluong : 0;
                            sanpham.Soluong += addfile;

                            var tonkho = await db.TonKho.FirstOrDefaultAsync(tk => tk.SanPhamID == sanpham.SanPhamID);
                            if (tonkho != null)
                            {
                                tonkho.SoLuongTon += addfile;
                            }
                            else
                            {
                                db.TonKho.Add(new TonKho
                                {
                                    SanPhamID = sanpham.SanPhamID,
                                    SoLuongTon = addfile
                                });
                            }
                            // thực hiện them chi tiết phiếu nhâp 
                            var chitiet = new ChiTietPhieuNhap
                            {
                                PhieuNhapID = phieuNhap.PhieuNhapID,
                                SanPhamID = sanpham.SanPhamID,
                                SoLuong = sanpham.Soluong,
                                DonGia = sanpham.Soluong
                            };

                        }
                        else
                        {
                            var newsanpham = new SanPham
                            {
                                TenSanPham = masp,
                                Soluong = int.TryParse(worksheet.Cells[row, 2].Value?.ToString(), out int soluong) ? soluong : (int?)null,
                                MoTa = worksheet.Cells[row, 3].Value?.ToString(),
                                Gia = double.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out double gia) ? gia : (double?)null,
                                DanhMucID = danhmucid,
                                HangID = IDhang,
                                HinhAnh = worksheet.Cells[row, 7].Value?.ToString()
                            };
                            db.SanPham.Add(newsanpham);
                            await db.SaveChangesAsync();
                            db.TonKho.Add(new TonKho
                            {
                                SanPhamID = newsanpham.SanPhamID,
                                SoLuongTon = newsanpham.Soluong ?? 0
                            });
                            var chitiet = new ChiTietPhieuNhap
                            {
                                PhieuNhapID = phieuNhap.PhieuNhapID,
                                SanPhamID = sanpham.SanPhamID,
                                SoLuong = sanpham.Soluong,
                                DonGia = sanpham.Soluong
                            };
                        }
                    }
                }

                await db.SaveChangesAsync();
                return Ok("Thành công");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Định dạng tệp không hợp lệ: {ex.Message}");
            }
            catch (DbEntityValidationException ex)
            {
                var errors = new List<string>();
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errors.Add($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }
                var errorMessage = string.Join("; ", errors);
                return BadRequest($"Validation failed for one or more entities. Details: {errorMessage}");
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"An error occurred while updating the entries: {errorMessage}");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }






        [HttpGet]
        [Route("phieuxuatkho")]
        public IHttpActionResult PhieuXuatKho()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ds = db.ChiTietPhieuXuat.ToList();
                return Ok(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Loi khi lay danh sach");
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("sanphamxuatkho/{id}")]
        public async Task<IHttpActionResult> SanPhamXuatKho(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var phieuxuat = await db.PhieuXuat.Include(px => px.ChiTietPhieuXuat).FirstOrDefaultAsync(px => px.PhieuXuatID == id);
            if (phieuxuat == null)
            {
                return NotFound();
            }
            return Ok(phieuxuat);
        }


        public async Task<PhieuXuat> CreateExportAsync(PhieuXuat phieuxuat)
        {
            // Tìm kiếm khách hàng dựa trên số điện thoại
            var khachHang = await db.KhachHang.FirstOrDefaultAsync(kh => kh.SoDienThoai == phieuxuat.KhachHang.SoDienThoai);
            if (khachHang == null)
            {
                // Tạo mới khách hàng nếu chưa tồn tại
                khachHang = new KhachHang
                {
                    SoDienThoai = phieuxuat.KhachHang.SoDienThoai,
                    HoTen = phieuxuat.KhachHang.HoTen
                };
                db.KhachHang.Add(khachHang);
                await db.SaveChangesAsync();
            }

            // Thiết lập thông tin khách hàng cho phiếu xuất
            phieuxuat.KhachHangID = khachHang.KhachHangID;
            phieuxuat.KhachHang = khachHang;

            // Kiểm tra và cập nhật số lượng tồn kho
            foreach (var chiTiet in phieuxuat.ChiTietPhieuXuat)
            {
                var tonkho = await db.TonKho.FirstOrDefaultAsync(tk => tk.SanPhamID == chiTiet.SanPhamID);
                if (tonkho == null || tonkho.SoLuongTon < chiTiet.SoLuong)
                {
                    throw new InvalidOperationException("Số lượng không đủ để xuất kho.");
                }
                tonkho.SoLuongTon -= chiTiet.SoLuong;
            }

            // Cập nhật điểm tích lũy cho khách hàng
            khachHang.DiemTichLuy += (int)(phieuxuat.ChiTietPhieuXuat.Sum(ct => ct.ThanhTien / 1000));

            // Thêm phiếu xuất kho vào cơ sở dữ liệu
            db.PhieuXuat.Add(phieuxuat);
            await db.SaveChangesAsync();
            return phieuxuat;
        }


        // POST: api/quanlytonkho/nhap
        [HttpPost]
        [Route("nhap")]
        public IHttpActionResult Nhap(PhieuNhapKhoViewModel model)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                if (model.SanPhamID.HasValue)
                {
                    var sanpham = db.SanPham.Find(model.SanPhamID.Value);
                    if (sanpham != null)
                    {
                        return RedirectToRoute("NhapSanPhamCoSan", new { id = model.SanPhamID.Value });
                    }
                }
                return RedirectToRoute("DefaultApi", new { controler = "QuanLyTonKho", action = "NhapKho" });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi nhập sản phẩm.");
                return InternalServerError(ex);
            }
        }

        // POST: api/quanlytonkho/nhapsanphamcosan
        [HttpPost]
        [Route("nhapsanphamcosan")]
        public IHttpActionResult NhapSanPhamCoSan(int id, PhieuNhapKhoViewModel model)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
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

                        db.SaveChanges();
                        return Ok("Nhập sản phẩm thành công.");
                    }
                }
                return BadRequest("ModelState không hợp lệ.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi nhập sản phẩm có sẵn.");
                return InternalServerError(ex);
            }
        }

        // POST: api/quanlytonkho/nhapkho
        [HttpPost]
        [Route("nhapkho")]
        public IHttpActionResult NhapKho(PhieuNhapKhoViewModel model)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var sanpham = model.SanPham;
                    var chiTiet = model.ChiTietSanPham;
                    var tonKho = model.TonKho;

                    sanpham.Soluong = tonKho.SoLuongTon;
                    db.SanPham.Add(sanpham);
                    db.SaveChanges();

                    chiTiet.SanPhamID = sanpham.SanPhamID;
                    db.ChiTietSanPham.Add(chiTiet);
                    db.SaveChanges();

                    tonKho.SanPhamID = sanpham.SanPhamID;
                    tonKho.NgayCapNhat = DateTime.Now;
                    db.TonKho.Add(tonKho);
                    db.SaveChanges();

                    return Ok("Nhập kho thành công.");
                }
                return BadRequest("ModelState không hợp lệ.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi nhập kho.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlytonkho/thongtinsp/{id}
        [HttpGet]
        [Route("thongtinsp/{id}")]
        public IHttpActionResult ThongTinSP(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var sanpham = db.SanPham.Where(c => c.SanPhamID == id).ToList();
                if (sanpham == null)
                {
                    return NotFound();
                }
                return Ok(sanpham);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy thông tin sản phẩm.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlytonkho/xuatkho
        [HttpGet]
        [Route("xuatkho")]
        public IHttpActionResult XuatKho()
        {

            return Ok("Chức năng xuất kho.");
        }

        // GET: api/quanlytonkho/sapxeptang
        [HttpGet]
        [Route("sapxeptang")]
        public IHttpActionResult SapxepTang()
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var kq = db.TonKho.OrderBy(x => x.SoLuongTon).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi sắp xếp sản phẩm tồn kho tăng dần.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlytonkho/sapxepgiam
        [HttpGet]
        [Route("sapxepgiam")]
        public IHttpActionResult SapxepGiam()
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var kq = db.TonKho.OrderByDescending(x => x.SoLuongTon).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi sắp xếp sản phẩm tồn kho giảm dần.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlytonkho/locngay
        [HttpGet]
        [Route("locngay")]
        public IHttpActionResult LocNgay(DateTime? Time, DateTime? enddate)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
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
                return Ok(tonkho.ToList());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lọc theo ngày.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlytonkho/suatonkho/{id}
        [HttpGet]
        [Route("suatonkho/{id}")]
        public IHttpActionResult SuaTonKho(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var tonkho = db.TonKho.Find(id);
                if (tonkho == null)
                {
                    return NotFound();
                }
                return Ok(tonkho);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy thông tin tồn kho.");
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("suatonkho/{id}")]
        public IHttpActionResult SuaTonKho(int id, TonKho ton)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var update = db.TonKho.Find(id);
                if (update == null)
                {
                    return NotFound();
                }
                update.SoLuongTon = ton.SoLuongTon;
                db.SaveChanges();
                return Ok("Cập nhật thông tin tồn kho thành công.");

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi cập nhật thông tin tồn kho.");

                return InternalServerError(ex);
            }
        }

        // GET: api/quanlytonkho/checkkho



        public async Task<List<TonKho>> CheckInventoryLevels(int lowStockThreshold, int highStockThreshold)
        {
            var lowStockProducts = db.TonKho
                .Where(t => t.SoLuongTon <= lowStockThreshold)
                .ToList();

            var highStockProducts = db.TonKho
                .Where(t => t.SoLuongTon >= highStockThreshold)
                .ToList();

            var alertProducts = lowStockProducts.Concat(highStockProducts).ToList();

            // Gửi cảnh báo cho người quản lý nếu có sản phẩm đạt mức cảnh báo
            if (alertProducts.Any())
            {
                string subject = "Cảnh báo tồn kho";
                string body = "Có sản phẩm tồn kho nằm ngoài ngưỡng định sẵn.";
                SendEmailAlert("nguuentoanbs2k4@gmail.com", subject, body);
            }

            return alertProducts;
        }


        [HttpGet]
        [Route("check")]
        public async Task<IHttpActionResult> CheckInventory([FromUri] int lowStockThreshold, [FromUri] int highStockThreshold)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var alertProducts = await CheckInventoryLevels(lowStockThreshold, highStockThreshold);
                if (alertProducts == null || !alertProducts.Any())
                {
                    return NotFound();
                }

                return Ok(alertProducts);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi cập nhật thông tin tồn kho.");
                return InternalServerError(ex);
            }
        }

        public void SendEmailAlert(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress("nguuentoanbs2k4@gmail.com", "From Name");
            var toAddress = new MailAddress(toEmail, "To Name");
            const string fromPassword = "Toanbg2k4";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

    }
}






