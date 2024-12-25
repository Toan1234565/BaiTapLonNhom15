using BaiTap.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using System.Data.Entity;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/baocao")]
    public class BaoCaoAPIController : ApiController
    {
        private readonly Model1 db = new Model1();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/baocao/tongdoanhthu
        [HttpGet]
        [Route("tongdoanhthu")]
        public async Task<IHttpActionResult> GetDoanhThuTheoNgay()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false; // tắt tự động tạo proxy

                // Lấy dữ liệu từ bảng DonHang
                var donHangs = await db.DonHang.ToListAsync();

                // Tính tổng doanh thu theo ngày
                var doanhThuTheoNgay = donHangs.GroupBy(dh => dh.NgayDatHang.Value.Date)
                                                .Select(g => new DoanhThuTheoNgay
                                                {
                                                    Ngay = g.Key,
                                                    TongTien = g.Sum(dh => dh.TongTien)
                                                }).ToList();

                logger.Info("Lấy doanh thu theo ngày thành công.");
                return Ok(doanhThuTheoNgay); // trả về doanh thu của các sản phẩm theo ngày
                return Ok(doanhThuTheoNgay); // Returns 200 OK with the revenue data
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy doanh thu theo ngày.");
                return InternalServerError(ex);
                return InternalServerError(ex); // Returns 500 Internal Server Error
            }
        }
    }

    public class DoanhThuTheoNgay
    {
        public DateTime Ngay { get; set; }
        public double? TongTien { get; set; }
    }
}
