using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BaiTap.ControllerAPI
{
    [RoutePrefix("api/quanlykhachhang")]
    public class QuanLyKhachHangAPIController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Model1 db = new Model1();

        // GET: api/quanlykhachhang/khachhang
        [HttpGet]
        [Route("khachhang")]
        public async Task<IHttpActionResult> KhachHang()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                // lay danh sach khach hang tu csdl
                var khachhang = db.KhachHang.ToList();
                await db.SaveChangesAsync();
                logger.Info("Lấy danh sách khách hàng thành công.");
                return Ok(khachhang);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách khách hàng.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlykhachhang/khachhang/{id}
        [HttpGet]
        [Route("khachhang/{id}")]
        public IHttpActionResult GetKhachHang(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var kh = db.KhachHang.Find(id);
                if (kh == null)
                {
                    logger.Warn("Không tìm thấy khach hang với ID: {0}", id);
                    return NotFound();
                }
                logger.Info("Lấy thông tin khach hang thành công. ID: {0}", id);
                return Ok(kh);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy thông tin khach hang với ID: {0}", id);
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlykhachhang/dschitiet
        [HttpGet]
        [Route("dschitietkh")]
        public IHttpActionResult GetDSChiTietKH()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ds = db.ChiTietKhachHang.ToList();
                return Ok(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách chi tiết khách hàng.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlykhachhang/chitiet/{id}
        [HttpGet]
        [Route("chitiet/{id}")]
        public IHttpActionResult ChiTietKH(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var chiTietKhachHang = db.ChiTietKhachHang.Where(c => c.KhachHangID == id).ToList();
                if (chiTietKhachHang == null)
                {
                    logger.Warn("Không tìm thấy chi tiết khach hang với ID: {0}", id);
                    return NotFound();
                }
                return Ok(chiTietKhachHang);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy chi tiết khach hang với ID: {0}", id);
                return InternalServerError(ex);
            }
        }
    }
}


