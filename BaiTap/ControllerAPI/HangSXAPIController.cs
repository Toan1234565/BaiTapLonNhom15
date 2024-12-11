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
    [RoutePrefix("api/hangsx")]//định nghĩa tiền tố URL cho các phương thức.

    public class HangSXAPIController : ApiController
    {
        private readonly Model1 db = new Model1();
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /*Lấy danh sách tất cả các hãng sx từ cơ sở dữ liệu.
        Ghi log thông tin nếu thành công, và log lỗi nếu gặp lỗi.
        */
        //Lấy danh sách tất cả các hangSX.
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


        //Lấy danh sách hãng sản xuất cho danh mục có ID cụ thể.
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
                    return NotFound(); //trả về lỗi 404 Not Found
                }
                logger.Info("Lấy thông tin hãng sản xuất thành công. ID: {0}", id);
                return Ok(hang); //trả về danh sách hãng sản xuất
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy thông tin hãng sản xuất với ID: {0}", id);
                return InternalServerError(ex);
            }
        }

        //Thêm mới một danh mục
        // POST: api/hangsx
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateHangSX(Hang newHang)
        {
            if (newHang == null)
            {
                return BadRequest("Hãng sản xuất không hợp lệ.");
            }

            try
            {
                db.Hang.Add(newHang);
                await db.SaveChangesAsync();

                logger.Info("Thêm hãng sản xuất thành công. ID: {0}", newHang.HangID);
                return CreatedAtRoute("DefaultApi", new { id = newHang.HangID }, newHang); // trả về danh sách các hãng sx đã được cập nhật
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi thêm hãng sản xuất.");//Trả về lỗi 404 nếu không tìm thấy danh mục, và ghi log thông tin hoặc lỗi.
                return InternalServerError(ex);
            }
        }

        //Xóa danh mục có ID cụ thể.
        // DELETE: api/hangsx/{id}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteHangSX(int id)
        {
            try
            {
                var hang = await db.Hang.FindAsync(id);
                if (hang == null)
                {
                    logger.Warn("Không tìm thấy hãng sản xuất với ID: {0}", id);
                    return NotFound(); // Trả về lỗi 404 Not Found
                }

                db.Hang.Remove(hang);
                await db.SaveChangesAsync();

                logger.Info("Xóa hãng sản xuất thành công. ID: {0}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi xóa hãng sản xuất với ID: {0}", id);
                return InternalServerError(ex);
            }
        }
    }
}
    