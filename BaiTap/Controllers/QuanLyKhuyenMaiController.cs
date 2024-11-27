using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class QuanLyKhuyenMaiController : Controller
    {
        private readonly Model1 db = new Model1();

        // GET: QuanLyKhuyenMai
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DSKhuyenMai()
        {
            List<KhuyenMai> ds = db.KhuyenMai.ToList();
            return View(ds);
        }

        public ActionResult SuaKM(int id)
        {
            KhuyenMai km = db.KhuyenMai.Find(id);
            if (km == null)
            {
                return HttpNotFound();
            }
            return View(km);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaKM(KhuyenMai km)
        {
            if (!ModelState.IsValid)
            {
                return View(km);
            }

            var update = db.KhuyenMai.Find(km.KhuyenMaiID);
            if (update == null)
            {
                return HttpNotFound();
            }

            update.TenKhuyenMai = km.TenKhuyenMai;
            update.NgayBD = km.NgayBD;

            try
            {
                db.SaveChanges();
                return RedirectToAction("DSKhuyenMai");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Thay đổi thông tin khuyến mãi thất bại: " + ex.Message);
                return View(km);
            }
        }

        public ActionResult XoaKhuyenMai(int id)
        {
            KhuyenMai km = db.KhuyenMai.Find(id);
            if (km == null)
            {
                return HttpNotFound();
            }
            return View(km);
        }

        [HttpPost, ActionName("XoaKhuyenMai")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> XoaKhuyenMaiConfirmed(int id)
        {
            KhuyenMai km = await db.KhuyenMai.FindAsync(id);
            if (km == null)
            {
                return HttpNotFound();
            }

            db.KhuyenMai.Remove(km);
            await db.SaveChangesAsync();
            return RedirectToAction("DSKhuyenMai");
        }

        public ActionResult TaoKhuyenMai()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoKhuyenMai(KhuyenMai km)
        {
            if (!ModelState.IsValid)
            {
                return View(km);
            }

            db.KhuyenMai.Add(km);
            try
            {
                db.SaveChanges();
                return RedirectToAction("DSKhuyenMai");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Tạo khuyến mãi thất bại: " + ex.Message);
                return View(km);
            }
        }

        public ActionResult ApDungKhuyenMai(int? IDSP, int? KMID, double? GiaTriDH, int? Diem, int? diemdoi)
        {
            if (IDSP.HasValue && KMID.HasValue && GiaTriDH.HasValue && Diem.HasValue && diemdoi.HasValue)
            {
                var sanpham = db.SanPham.Find(IDSP.Value);
                var khuyenmai = db.KhuyenMai.Find(KMID.Value);
                if (sanpham != null && khuyenmai != null)
                {
                    bool apdung = false;

                    // Áp dụng giảm giá 10% nếu giá trị đơn hàng tối thiểu đạt yêu cầu
                    if (GiaTriDH >= khuyenmai.GiaTriDonHangToiThieu)
                    {
                        decimal giamgia = (decimal)sanpham.Gia * khuyenmai.GiaTri / 100;
                        sanpham.Gia -= (double)giamgia;
                        apdung = true;
                    }

                    // Áp dụng giảm giá cho các sản phẩm nhất định
                    if (khuyenmai.SanPhamKhuyenMai.Any(sp => sp.SanPhamID == IDSP.Value))
                    {
                        decimal giamgia = (decimal)sanpham.Gia * khuyenmai.GiaTri / 200;
                        sanpham.Gia -= (double)giamgia;
                        apdung = true;
                    }

                    // Áp dụng giảm giá dựa trên điểm tích lũy
                    if (Diem >= khuyenmai.DiemTichLuyToiThieu)
                    {
                        int solangiam = diemdoi.Value / khuyenmai.DiemTichLuyToiThieu;
                        decimal giamgia = (decimal)sanpham.Gia * (solangiam * 10) / 100;
                        sanpham.Gia -= (double)giamgia;
                        Diem -= diemdoi.Value;
                        apdung = true;
                    }

                    if (apdung)
                    {
                        khuyenmai.Soluong--;
                        if (khuyenmai.Soluong == 0)
                        {
                            // Handle out of stock promotion here, if needed
                        }
                        db.SaveChanges();
                        return Json(new { success = true, giaMoi = sanpham.Gia }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public ActionResult ChiTietKM()
        {
            return View();
        }
    }
}
