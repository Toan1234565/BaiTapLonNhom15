
using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/quanlysanpham")]
    public class QuanLySanPhamAPIController : ApiController
    {
        private readonly ProductService _productService = new ProductService();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Model1 db = new Model1();

        // GET: api/quanlysanpham/sanpham
        [HttpGet]
        [Route("sanpham")]
        public async Task<IHttpActionResult> SanPham()
        {
            try
            {
                // Tắt proxy động
                db.Configuration.ProxyCreationEnabled = false;
                // lay danh sach san pham tu csdl
                var sanpham = db.SanPham.ToList();

                logger.Info("Lấy danh sách sản phẩm thành công.");
                return Ok(sanpham);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách sản phẩm.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlysanpham/sanpham/{id}
        [HttpGet]
        [Route("sanpham/{id}")]
        public IHttpActionResult GetSanPham(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var sp = db.SanPham.Find(id);
                if (sp == null)
                {
                    logger.Warn("Không tìm thấy sản phẩm với ID: {0}", id);
                    return NotFound();
                }
                logger.Info("Lấy thông tin sản phẩm thành công. ID: {0}", id);
                return Ok(sp);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy thông tin sản phẩm với ID: {0}", id);
                return InternalServerError(ex);
            }
        }
    }
}
