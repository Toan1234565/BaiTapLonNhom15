using BaiTap.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/comparison")]
    public class ComparisonApiController : ApiController
    {
        private Model1 db = new Model1();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        [Route("compare")]
        public async Task<IHttpActionResult> CompareProducts([FromBody] List<int> sanpham, [FromBody] List<Criteria> criteria)
        {
            try
            {
                var sp = await db.SanPham
                    .Where(x => sanpham.Contains(x.SanPhamID))
                    .Select(x => new SanPham
                    {
                        SanPhamID = x.SanPhamID,
                        TenSanPham = x.TenSanPham,
                        Gia = x.Gia ?? 0,
                        MoTa = x.MoTa,
                        HinhAnh = x.HinhAnh
                        // Add more fields as needed
                    })
                    .ToListAsync();

                if (!sp.Any())
                {
                    return Json(new { success = false, message = "No products found." });
                }

                logger.Info("Comparison successful");
                return Ok(new { success = true, data = sp });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Comparison failed");
                return InternalServerError(ex);
            }
        }
    }
}
