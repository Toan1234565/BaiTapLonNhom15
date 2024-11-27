using BaiTap.Models;
using System.Linq;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class TaiKhoanKhachHangController : Controller
    {
        private Model1 db = new Model1();

        // GET: QuanLyTaiKhoan
        public ActionResult Index()
        {
            var taiKhoanKHs = db.TaiKhoanKH.Include("KhachHang").ToList();
            if (taiKhoanKHs == null || !taiKhoanKHs.Any())
            {
                // Trả về thông báo nếu không có dữ liệu
                ViewBag.Message = "Không có dữ liệu tài khoản khách hàng.";
            }
            return View(taiKhoanKHs);
        }



        // GET: QuanLyTaiKhoan/Details/5
        public ActionResult ChiTiet(int id)
        {
            TaiKhoanKH taiKhoanKH = db.TaiKhoanKH.Find(id);
            if (taiKhoanKH == null)
            {
                return HttpNotFound();
            }

            var donHangs = db.DonHang.Where(dh => dh.KhachHangID == taiKhoanKH.KhachHangID).ToList();
            ViewBag.DonHangs = donHangs;

            return View(taiKhoanKH);
        }

        // GET: QuanLyTaiKhoan/Delete/5
        public ActionResult Delete(int id)
        {
            TaiKhoanKH taiKhoanKH = db.TaiKhoanKH.Find(id);
            if (taiKhoanKH == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoanKH);
        }

        // POST: QuanLyTaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoanKH taiKhoanKH = db.TaiKhoanKH.Find(id);
            if (taiKhoanKH == null)
            {
                return HttpNotFound();
            }

            db.TaiKhoanKH.Remove(taiKhoanKH);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
