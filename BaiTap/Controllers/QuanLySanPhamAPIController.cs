using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BaiTap.Controllers
{

    [RoutePrefix("api/quanlysanpham")]
    public class QuanLySanPhamAPIController : ApiController
    {

       
            // Khai báo DbContext
            private Model1 db = new Model1();

            // GET: api/quanlysanpham/sanpham
            [HttpGet]
            [Route("sanpham")]
            public IHttpActionResult GetSanPham()
            {
                var ds = db.SanPham.ToList();
                return Ok(ds);
            }

            // GET: api/quanlysanpham/sanpham/{id}
            [HttpGet]
            [Route("sanpham/{id}")]
            public IHttpActionResult GetSanPham(int id)
            {
                var sanpham = db.SanPham.Find(id);
                if (sanpham == null)
                {
                    return NotFound();
                }
                return Ok(sanpham);
            }

            // POST: api/quanlysanpham/sua
            [HttpPost]
            [Route("sua")]
            public IHttpActionResult SuaSanPham(SanPham sanpham)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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

                db.SaveChanges();
                return Ok(update);
            }

            // DELETE: api/quanlysanpham/xoa/{id}
            [HttpDelete]
            [Route("xoa/{id}")]
            public IHttpActionResult XoaSanPham(int id)
            {
                var sanpham = db.SanPham.Find(id);
                if (sanpham == null)
                {
                    return NotFound();
                }

                var chiTietSanPham = db.ChiTietSanPham.Where(x => x.SanPhamID == id).ToList();
                foreach (var chitiet in chiTietSanPham)
                {
                    db.ChiTietSanPham.Remove(chitiet);
                }
                db.SanPham.Remove(sanpham);
                db.SaveChanges();

                return Ok();
            }

            // GET: api/quanlysanpham/dschitiet
            [HttpGet]
            [Route("dschitiet")]
            public IHttpActionResult GetDSChiTiet()
            {
                var ds = db.ChiTietSanPham.ToList();
                return Ok(ds);
            }

            // GET: api/quanlysanpham/xemchitiet/{id}
            [HttpGet]
            [Route("xemchitiet/{id}")]
            public IHttpActionResult XemChiTiet(int id)
            {
                var chiTietSanPham = db.ChiTietSanPham.Where(c => c.SanPhamID == id).ToList();
                if (chiTietSanPham == null)
                {
                    return NotFound();
                }
                return Ok(chiTietSanPham);
            }

            // POST: api/quanlysanpham/them
            [HttpPost]
            [Route("them")]
            public IHttpActionResult ThemSanPham(PhieuNhapKhoViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.SanPham.Add(model.SanPham);
                db.SaveChanges();

                model.ChiTietSanPham.SanPhamID = model.SanPham.SanPhamID;
                db.ChiTietSanPham.Add(model.ChiTietSanPham);
                db.SaveChanges();

                return Ok(model);
            }
        }
    }


