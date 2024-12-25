using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class TaiKhoanKhachHangController : Controller
    {
        private readonly Model1 db = new Model1();

        // GET: TaiKhoanKhachHang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DSTaiKhoan()
        {
            List<TaiKhoanKH> ds = db.TaiKhoanKH.ToList();
            return View(ds);
        }

        public ActionResult SuaTaiKhoan(int id)
        {
            TaiKhoanKH tk = db.TaiKhoanKH.Find(id);
            if (tk == null)
            {
                return HttpNotFound();
            }
            return View(tk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaTaiKhoan(TaiKhoanKH tk)
        {
            if (!ModelState.IsValid)
            {
                return View(tk);
            }

            var update = db.TaiKhoanKH.Find(tk.TaiKhoanID);
            if (update == null)
            {
                return HttpNotFound();
            }

            update.TenDangNhap = tk.TenDangNhap;
            update.MatKhau = tk.MatKhau;
            update.TrangThai = tk.TrangThai;

            try
            {
                db.SaveChanges();
                return RedirectToAction("DSTaiKhoan");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Thay đổi thông tin tài khoản thất bại: " + ex.Message);
                return View(tk);
            }
        }

        public ActionResult XoaTaiKhoan(int id)
        {
            TaiKhoanKH tk = db.TaiKhoanKH.Find(id);
            if (tk == null)
            {
                return HttpNotFound();
            }
            return View(tk);
        }

        [HttpPost, ActionName("XoaTaiKhoan")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> XoaTaiKhoanConfirmed(int id)
        {
            TaiKhoanKH tk = await db.TaiKhoanKH.FindAsync(id);
            if (tk == null)
            {
                return HttpNotFound();
            }

            db.TaiKhoanKH.Remove(tk);
            await db.SaveChangesAsync();
            return RedirectToAction("DSTaiKhoan");
        }

        public ActionResult TaoTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoTaiKhoan(TaiKhoanKH tk)
        {
            if (!ModelState.IsValid)
            {
                return View(tk);
            }

            db.TaiKhoanKH.Add(tk);
            try
            {
                db.SaveChanges();
                return RedirectToAction("DSTaiKhoan");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Tạo tài khoản thất bại: " + ex.Message);
                return View(tk);
            }
        }

        public ActionResult ChiTietTaiKhoan(int id)
        {
            TaiKhoanKH tk = db.TaiKhoanKH.Find(id);
            if (tk == null)
            {
                return HttpNotFound();
            }
            return View(tk);
        }
    }
}
