namespace BaiTap.Controllers
{
    using BaiTap.Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;
  
    public class QuanLyKhuyenMai2Controller : Controller
    {
        private readonly Model1 db = new Model1();

        // GET: QuanLyKhuyenMai
        public ActionResult Index()
        {
            return View();
        }

        // API Methods

        [HttpGet]
        public JsonResult GetAllKhuyenMai()
        {
            var khuyenMaiList = db.KhuyenMai.ToList();
            return Json(khuyenMaiList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetKhuyenMaiById(int id)
        {
            var khuyenMai = db.KhuyenMai.Find(id);
            if (khuyenMai == null)
            {
                return Json(new { success = false, message = "Khuyến mãi không tồn tại" }, JsonRequestBehavior.AllowGet);
            }
            return Json(khuyenMai, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateKhuyenMai(KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMai.Add(khuyenMai);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true, message = "Tạo khuyến mãi thành công" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Tạo khuyến mãi thất bại: " + ex.Message });
                }
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public JsonResult SuaKM(KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                var existingKhuyenMai = db.KhuyenMai.Find(khuyenMai.KhuyenMaiID);
                if (existingKhuyenMai != null)
                {
                    existingKhuyenMai.TenKhuyenMai = khuyenMai.TenKhuyenMai;
                    existingKhuyenMai.NgayBD = khuyenMai.NgayBD;
                    existingKhuyenMai.NgayKT = khuyenMai.NgayKT;
                    existingKhuyenMai.Mota = khuyenMai.Mota;
                    existingKhuyenMai.LoaiKM = khuyenMai.LoaiKM;
                    existingKhuyenMai.GiaTri = khuyenMai.GiaTri;
                    existingKhuyenMai.DieuKien = khuyenMai.DieuKien;
                    existingKhuyenMai.Soluong = khuyenMai.Soluong;
                    existingKhuyenMai.GiaTriDonHangToiThieu = khuyenMai.GiaTriDonHangToiThieu;
                    existingKhuyenMai.DiemTichLuyToiThieu = khuyenMai.DiemTichLuyToiThieu;

                    try
                    {
                        db.SaveChanges();
                        return Json(new { success = true, message = "Cập nhật khuyến mãi thành công" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Cập nhật khuyến mãi thất bại: " + ex.Message });
                    }
                }
                return Json(new { success = false, message = "Khuyến mãi không tồn tại" });
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public JsonResult XoaKhuyenMai(int id)
        {
            var khuyenMai = db.KhuyenMai.Find(id);
            if (khuyenMai != null)
            {
                db.KhuyenMai.Remove(khuyenMai);
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true, message = "Xóa khuyến mãi thành công" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Xóa khuyến mãi thất bại: " + ex.Message });
                }
            }
            return Json(new { success = false, message = "Khuyến mãi không tồn tại" });
        }
    }
}
