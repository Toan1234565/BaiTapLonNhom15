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

namespace BaiTap.Controllers
{
    [RoutePrefix("api/quanlytonkho")]
    public class QuanLyTonKhoAPIController : ApiController
    {
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

        // POST: api/quanlytonkho/nhap
        [HttpPost]
        [Route("nhap")]
        public IHttpActionResult Nhap(PhieuNhapKhoViewModel model)
        {
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
                return RedirectToRoute("DefaultApi", new {controler = "QuanLyTonKho", action="NhapKho"});
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
    }
}