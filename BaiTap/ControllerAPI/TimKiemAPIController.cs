using BaiTap.Models;
using System.Linq;
using System.Web.Http;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/timkiem")]
    public class TimKiemApiController : ApiController
    {
        private Model1 db = new Model1();

        [HttpGet]
        [Route("index")]
        public IHttpActionResult Index()
        {
            return Ok();
        }

        [HttpGet]
        [Route("timkiem")]
        public IHttpActionResult TimKiem(string name)
        {
            var kq = db.SanPham.Where(sp => sp.TenSanPham.Contains(name) || sp.MoTa.Contains(name)).ToList();
            return Ok(kq);
        }

        [HttpGet]
        [Route("locsp")]
        public IHttpActionResult LocSP(string name, int? IDHang, int? IDDanhMuc, double? to = null, double? from = null, string sx = null)
        {
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

            return Ok(kq.ToList());
        }

        [HttpGet]
        [Route("giatang")]
        public IHttpActionResult GiaTang()
        {
            var kq = db.SanPham.OrderBy(x => x.Gia).ToList();
            return Ok(kq);
        }

        [HttpGet]
        [Route("giagiam")]
        public IHttpActionResult GiaGiam()
        {
            var kq = db.SanPham.OrderByDescending(x => x.Gia).ToList();
            return Ok(kq);
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
        [Route("banchay2")]
        public IHttpActionResult Banchay2()
        {
            var kq = db.SanPham.OrderBy(x => x.SoLuongDaBan).Take(5).ToList();
            return Ok(kq);
        }
    }
}
