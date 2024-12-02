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
                foreach (var sp in sanpham)
                {
                    string url = await _productService.GetProductImageAsync(sp.TenSanPham);
                    if (!string.IsNullOrEmpty(url)) // cap nhat khi url khong rong
                    {
                        sp.HinhAnh = url;
                        db.Entry(sp).State = EntityState.Modified;
                    }
                }
                // luu hinh anh vao co so du lieu 
                await db.SaveChangesAsync();
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
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
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

        // POST: api/quanlysanpham/sua
        [HttpPut]
        [Route("sua")]
        public IHttpActionResult Sua(SuaSanPham sp)
        {
            var sanpham = sp.SanPham;
            var tonkho = sp.TonKho;
            if (string.IsNullOrEmpty(sanpham.TenSanPham))
            {
                return BadRequest("Tên sản phẩm không được để trống");
            }

            if (sanpham.Soluong < tonkho.SoLuongTon)
            {
                return BadRequest("Tổng số lượng sản phẩm không thể nhỏ hơn số lượng sản phẩm tồn kho");
            }

            if (sanpham.Soluong < 0)
            {
                return BadRequest("Số lượng không được nhỏ hơn 0");
            }

            if (sanpham.Gia < 0)
            {
                return BadRequest("Giá không được nhỏ hơn 0");
            }

            var update = db.SanPham.Find(sanpham.SanPhamID);
            if (update == null)
            {
                return NotFound();
            }

            update.TenSanPham = sanpham.TenSanPham;
            update.Soluong = sanpham.Soluong;
            update.Gia = sanpham.Gia;
            update.MoTa = sanpham.MoTa;
            update.HangID = sanpham.HangID;
            update.DanhMucID = sanpham.DanhMucID;
            update.HinhAnh = sanpham.HinhAnh;

            var id = db.SaveChanges();
            if(id > 0)
            {
                logger.Info("Sửa sản phẩm thành công. ID: {0}", sanpham.SanPhamID);
                return Ok(update);
            }
            else
            {
                logger.Info("that bai");
                return BadRequest("sua thong tin that bai");
            }
           
        }
        

        // GET: api/quanlysanpham/dschitiet
        [HttpGet]
        [Route("dschitiet")]
        public IHttpActionResult GetDSChiTiet()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ds = db.ChiTietSanPham.ToList();
                return Ok(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy danh sách chi tiết sản phẩm.");
                return InternalServerError(ex);
            }
        }

        // GET: api/quanlysanpham/xemchitiet/{id}
        [HttpGet]
        [Route("xemchitiet/{id}")]
        public IHttpActionResult XemChiTiet(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var chiTietSanPham = db.ChiTietSanPham.Where(c => c.SanPhamID == id).ToList();
                if (chiTietSanPham == null)
                {
                    logger.Warn("Không tìm thấy chi tiết sản phẩm với ID: {0}", id);
                    return NotFound();
                }
                return Ok(chiTietSanPham);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi lấy chi tiết sản phẩm với ID: {0}", id);
                return InternalServerError(ex);
            }
        }

        // POST: api/quanlysanpham/them
        [HttpPost]
        [Route("them")]
        public IHttpActionResult ThemSanPham(PhieuNhapKhoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    logger.Warn("ModelState không hợp lệ.");
                    return BadRequest(ModelState);
                }

                db.SanPham.Add(model.SanPham);
                db.SaveChanges();

                model.ChiTietSanPham.SanPhamID = model.SanPham.SanPhamID;
                db.ChiTietSanPham.Add(model.ChiTietSanPham);
                db.SaveChanges();

                logger.Info("Thêm sản phẩm thành công. ID: {0}", model.SanPham.SanPhamID);
                return Ok(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi thêm sản phẩm.");
                return InternalServerError(ex);
            }
        }
        // GET: QuanLySanPham/Xoa/{id}
        public IHttpActionResult Xoa(int id)
        {
            var sp = db.SanPham.Find(id);
            if (sp == null)
            {
                return NotFound();
            }
            return Ok(sp);
        }

        // DELETE: api/quanlysanpham/xoa/{id}
        [HttpDelete]
        [Route("xoa/{id}")]
        public IHttpActionResult XoaSanPham(int id)
        {
            try
            {
                var sanpham = db.SanPham.Find(id);
                if (sanpham == null)
                {
                    logger.Warn("Không tìm thấy sản phẩm với ID: {0}", id);
                    return NotFound();
                }

                var chiTietSanPham = db.ChiTietSanPham.Where(x => x.SanPhamID == id).ToList();
                foreach (var chitiet in chiTietSanPham)
                {
                    db.ChiTietSanPham.Remove(chitiet);
                }
                db.SanPham.Remove(sanpham);
                db.SaveChanges();

                logger.Info("Xóa sản phẩm thành công. ID: {0}", id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Lỗi khi xóa sản phẩm với ID: {0}", id);
                return InternalServerError(ex);
            }
        }

    }
}
