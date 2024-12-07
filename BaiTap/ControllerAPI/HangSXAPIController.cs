using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/hangsx")]
    public class HangSXAPIController : ApiController
    {
        private readonly Model1 db = new Model1();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/hangsx
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAllHangsx()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false; // tắt tự động tạo proxy
                var dsHangsx = await db.Hang.ToListAsync();
                logger.Info("Lấy danh sách hãng sản xuất thành công.");
                return Ok(dsHangsx);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách hãng sản xuất.");
                return InternalServerError(ex);
            }
        }

        // GET: api/hangsx/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetHangSX(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var hang = await db.Hang.Include(x => x.SanPham).FirstOrDefaultAsync(m => m.HangID == id);
                if (hang == null)
                {
                    logger.Warn("Không tìm thấy hãng sản xuất với ID: {0}", id);
                    return NotFound(); // trả về lỗi 404 Not Found
                }
                logger.Info("Lấy thông tin hãng sản xuất thành công. ID: {0}", id);
                return Ok(hang); // trả về danh sách hãng sx
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy thông tin hãng sản xuất với ID: {0}", id);
                return InternalServerError(ex);
            }
        }
    }
}
