using BaiTap.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using System;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/danhmuc")]
    public class DanhMucAPIController : ApiController
    {
        private readonly Model1 db = new Model1();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/danhmuc
        //Lấy danh sách tất cả các danh mục.
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetDanhMuc()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false; // tắt tự động tạo proxy
                db.Configuration.ProxyCreationEnabled = false; //tắt tự động tạo proxy
                var dsDanhMuc = await db.DanhMuc.ToListAsync();
                logger.Info("Lấy danh sách danh mục thành công.");
                return Ok(dsDanhMuc);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách danh mục.");
                return InternalServerError(ex);
            }
        }

        // GET: api/danhmuc/hangsx/{id}
        //Lấy danh sách hãng sản xuất cho danh mục có ID cụ thể.
        
        [HttpGet]
        [Route("hangsx/{id}")]
        public async Task<IHttpActionResult> GetHangSX(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var category = await db.DanhMuc.Include(x => x.Hang).FirstOrDefaultAsync(m => m.DanhMucID == id);
                if (category == null)
                {
                    logger.Warn("Không tìm thấy danh mục với ID: {0}", id);
                    return NotFound(); // trả về lỗi 404 Not Found
                }
                logger.Info("Lấy thông tin hãng sản xuất thành công cho danh mục ID: {0}", id);
                return Ok(category.Hang); // trả về danh sách hãng sản xuất
                 // trả về danh mục các hãng
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy hãng sản xuất cho danh mục với ID: {0}", id);
                return InternalServerError(ex);
            }

        }

        // POST: api/danhmuc
        //Thêm mới một danh mục.
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateDanhMuc(DanhMuc newDanhMuc)
        {
            if (newDanhMuc == null)
            {
                return BadRequest("Danh mục không hợp lệ.");
            }

            try
            {
                db.DanhMuc.Add(newDanhMuc);
                await db.SaveChangesAsync();

                logger.Info("Thêm danh mục thành công. ID: {0}", newDanhMuc.DanhMucID);
                return CreatedAtRoute("DefaultApi", new { id = newDanhMuc.DanhMucID }, newDanhMuc); // trả về các danh mục đã tạo
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi thêm danh mục.");
                return InternalServerError(ex);
            }
        }

        // DELETE: api/danhmuc/{id}
        //Xóa danh mục có ID cụ thể.
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteDanhMuc(int id)
        {
            try
            {
                var danhMuc = await db.DanhMuc.FindAsync(id);
                if (danhMuc == null)
                {
                    logger.Warn("Không tìm thấy danh mục với ID: {0}", id);
                    return NotFound(); // trả về lỗi 404 Not Found
                }

                db.DanhMuc.Remove(danhMuc);
                await db.SaveChangesAsync();

                logger.Info("Xóa danh mục thành công. ID: {0}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi xóa danh mục với ID: {0}", id);
                return InternalServerError(ex);
            }
        }
    }
}
