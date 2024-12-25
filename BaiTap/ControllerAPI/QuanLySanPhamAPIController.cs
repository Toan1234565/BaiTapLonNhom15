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
        // quản lý vòng đời 
        public async Task<IHttpActionResult> SanPham()
        {
            try

            {
                // Tắt proxy động tối ưu hóa hiệu xuất 
                db.Configuration.ProxyCreationEnabled = false;
                // lay danh sach san pham tu csdl
                var sanpham = db.SanPham.ToList();

                logger.Info("Lấy danh sách sản phẩm thành công.");
                // trả về dưới dạng json 
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
        [Route("suasanpham/{id}")]
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
        
        // POST: api/quanlysanpham/sua
        [HttpPost]
        [Route("suasanpham{id}")]
        public async Task<IHttpActionResult> Sua(int id, SanPham sanpham)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                
                //var tonkho = sp.TonKho;

                //if (string.IsNullOrEmpty(sanpham.TenSanPham))
                //{
                //    return BadRequest("Tên sản phẩm không được để trống");
                //}

                //if (sanpham.Soluong < tonkho.SoLuongTon)
                //{
                //    return BadRequest("Tổng số lượng sản phẩm không thể nhỏ hơn số lượng sản phẩm tồn kho");
                //}

                //if (sanpham.Soluong < 0)
                //{
                //    return BadRequest("Số lượng không được nhỏ hơn 0");
                //}

                //if (sanpham.Gia < 0)
                //{
                //    return BadRequest("Giá không được nhỏ hơn 0");
                //}
               
                var update = db.SanPham.Find(id);
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

              db.SaveChanges();
               
                logger.Info("Cập nhật sản phẩm thành công. ID: {0}", sanpham.SanPhamID);
                return Ok("thanh cong");
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Lỗi khi cập nhật thông tin tồn kho.");

                return InternalServerError(ex);
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
        public async Task<IHttpActionResult> ThemSanPham(PhieuNhapKhoViewModel model)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled=false;
                if (!ModelState.IsValid)
                {
                    logger.Warn("ModelState không hợp lệ.");
                    return BadRequest(ModelState);
                }

                var sanpham = model.SanPham;

                if (sanpham.Gia == null)
                {
                    return BadRequest("Giá sản phẩm không được để trống");
                }
                if (sanpham.HangID == null)
                {
                    return BadRequest("Vui lòng chọn hãng sản xuất.");
                }
                if (sanpham.DanhMucID == null)
                {
                    return BadRequest("Vui lòng chọn danh mục.");
                }

                // Khởi tạo ChiTietSanPham nếu nó null
                if (model.ChiTietSanPham == null)
                {
                    model.ChiTietSanPham = new ChiTietSanPham();
                }

                // Lấy thông tin chi tiết sản phẩm từ trang web
                ChiTietSanPham productDetails = await _productService.GetProductDetailsFromWebAsync(sanpham.TenSanPham);

                if (productDetails != null)
                {
                    model.ChiTietSanPham.ManHinh = productDetails.ManHinh ?? model.ChiTietSanPham.ManHinh;
                    model.ChiTietSanPham.HeDieuHanh = productDetails.HeDieuHanh ?? model.ChiTietSanPham.HeDieuHanh;
                    model.ChiTietSanPham.CameraTruoc = productDetails.CameraTruoc ?? model.ChiTietSanPham.CameraTruoc;
                    model.ChiTietSanPham.CameraSau = productDetails.CameraSau ?? model.ChiTietSanPham.CameraSau;
                    model.ChiTietSanPham.Chip = productDetails.Chip ?? model.ChiTietSanPham.Chip;
                    model.ChiTietSanPham.RAM = productDetails.RAM ?? model.ChiTietSanPham.RAM;
                    model.ChiTietSanPham.BoNhoTrong = productDetails.BoNhoTrong ?? model.ChiTietSanPham.BoNhoTrong;
                    model.ChiTietSanPham.Sim = productDetails.Sim ?? model.ChiTietSanPham.Sim;
                    model.ChiTietSanPham.Pin = productDetails.Pin ?? model.ChiTietSanPham.Pin;
                }

                string url = await _productService.GetProductImageAsync(sanpham.TenSanPham);
                if (!string.IsNullOrEmpty(url))
                {
                    sanpham.HinhAnh = url;
                    db.Entry(sanpham).State = EntityState.Modified;
                    await db.SaveChangesAsync(); // Lưu lại hình ảnh cập nhật
                }
                else
                {
                    logger.Warn("Không thể lấy URL hình ảnh.");
                }
                db.SanPham.Add(sanpham);
                await db.SaveChangesAsync(); // Lưu sản phẩm để lấy ID

                model.ChiTietSanPham.SanPhamID = sanpham.SanPhamID;
                db.ChiTietSanPham.Add(model.ChiTietSanPham);
                await db.SaveChangesAsync(); // Lưu chi tiết sản phẩm

                logger.Info("Thêm sản phẩm thành công. ID: {0}", sanpham.SanPhamID);
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
        public async Task<IHttpActionResult> XoaSanPham(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            using (var xoa = db.Database.BeginTransaction())
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
                    await db.SaveChangesAsync();

                    xoa.Commit();
                    logger.Info("Xóa sản phẩm thành công. ID: {0}", id);
                    return Ok();
                }
                catch (Exception ex)
                {
                    xoa.Rollback();
                    logger.Error(ex, "Lỗi khi xóa sản phẩm với ID: {0}", id);
                    return InternalServerError(ex);
                }
            }
        }


    }
}
