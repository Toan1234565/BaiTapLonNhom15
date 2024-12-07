using BaiTap.Models;
using Grpc.Core;
using NLog;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/timkiem")]
    public class TimKiemApiController : ApiController
    {
        private Model1 db = new Model1();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("index")]
        public IHttpActionResult Index()
        {
            return Ok();
        }

        [HttpGet]
        [Route("timkiemsanpham")]
        public IHttpActionResult TimKiem(string name)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var kq = db.SanPham.Where(sp => sp.TenSanPham.Contains(name) || sp.MoTa.Contains(name)).ToList();
                logger.Info("Lấy danh sách sản phẩm thành công.");
                return Ok(kq);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách sản phẩm.");
                return InternalServerError(ex);
            }

           
        }

        [HttpGet]
        [Route("locsanpham")]
        public IHttpActionResult LocSP(string name = null, int? IDHang = null, int? IDDanhMuc = null, double? to = null, double? from = null, string sx = null)
        {
          
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                IQueryable<SanPham> kq = db.SanPham;

                if (IDDanhMuc.HasValue && IDDanhMuc.Value != 0)
                {
                    kq = kq.Where(sp => sp.DanhMucID == IDDanhMuc.Value);
                }

                if (IDHang.HasValue && IDHang.Value != 0)
                {
                    kq = kq.Where(sp => sp.HangID == IDHang.Value);
                }

                if (from.HasValue && to.HasValue && from.Value > 0 && to.Value > 0)
                {
                    kq = kq.Where(sp => sp.Gia >= from.Value && sp.Gia <= to.Value);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    kq = kq.Where(sp => sp.TenSanPham.Contains(name));
                }

                switch (sx)
                {
                    case "Giatang":
                        kq = kq.OrderBy(sp => sp.Gia);
                        break;
                    case "Giagiam":
                        kq = kq.OrderByDescending(sp => sp.Gia);
                        break;
                    default:
                        kq = kq.OrderBy(sp => sp.Gia);
                        break;
                }
                var result = kq.ToList();
                if(result.Count == 0)
                {
                    logger.Info("khong tim thay thong tin san pham thoa ma dieu kien loc");
                    return NotFound();
                }
                logger.Info("lay danh sach thanh cong");
                return Ok(kq.ToList());
            }
            catch(Exception ex) 
            {
                logger.Error(ex,"lay danh sach that bai");
                return InternalServerError(ex); 
            }
            
        }

        [HttpGet]
        [Route("giatang")]
        public IHttpActionResult GiaTang()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var kq = db.SanPham.OrderBy(x => x.Gia).ToList();
                logger.Info("Lay danh sach thanh cong");
                return Ok(kq);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "lay danh sach that baij");
                return InternalServerError(ex);
            }
            
        }

        [HttpGet]
        [Route("giagiam")]
        public IHttpActionResult GiaGiam()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var kq = db.SanPham.OrderByDescending(x => x.Gia).ToList();
                logger.Error("Lay danh sach thanh cong");
                return Ok(kq);
            }
            catch( Exception ex)
            {
                logger.Error(ex, "lay danh sach thanh cong");
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("banchay1")]
        public IHttpActionResult Banchay1()
        {
            var sanpham = db.SanPham.ToList();
            var kq = sanpham.Select(x => new
            {
                SanPham = x,
                SoLuongDaban = x.Soluong.GetValueOrDefault() - x.TonKho.Sum(tk => tk.SoLuongTon)
            }).OrderByDescending(x => x.SoLuongDaban).Take(10).Select(x => x.SanPham).ToList();
            return Ok(kq);
        }

        [HttpGet]
        [Route("SOSANH")]
        public async Task<IHttpActionResult> Sosanh(int id1, int id2)
        {
            try
            {
                var sp1 = await db.SanPham.FirstOrDefaultAsync(x => x.SanPhamID == id1);
                var sp2 = await db.SanPham.FirstOrDefaultAsync(x => x.SanPhamID == id2);

                if (sp1 == null || sp2 == null)
                {
                    return Json(new { success = false, message = "One or both products not found." });
                }

                var comparisonResult = new
                {
                    sanpham1 = new
                    {
                        sp1.TenSanPham,
                        sp1.Gia,
                        sp1.MoTa
                    },
                    sanpham2 = new
                    {
                        sp2.TenSanPham,
                        sp2.Gia,
                        sp2.MoTa
                    }
                };

                logger.Info("Comparison successful");
                return Ok(new { success = true, comparisonResult });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Comparison failed");
                return InternalServerError(ex);
            }
        }

    }
}