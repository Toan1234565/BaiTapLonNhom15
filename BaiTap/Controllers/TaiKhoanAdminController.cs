// Controller Admins
using BaiTap.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

public class TaiKhoanAdminController : Controller
{
    private Model1  db = new Model1();

    // GET: Admins
    public ActionResult Index()
    {
        List<Admins> ds = db.Admins.ToList();
        return View(ds);
    }

    // GET: Admins/Details/5
    public ActionResult ChiTiet(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Admins admin = db.Admins.Find(id);
        if (admin == null)
        {
            return HttpNotFound();
        }
        return View(admin);
    }

    // 
    public ActionResult TaoTaiKhoan()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult TaoTaiKhoan( Admins admin)
    {
        if (ModelState.IsValid)
        {
            db.Admins.Add(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(admin);
    }

    // GET: 
    public ActionResult Sua(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Admins admin = db.Admins.Find(id);
        if (admin == null)
        {
            return HttpNotFound();
        }
        return View(admin);
    }

    // POST: Admins/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Sua(Admins admin)
    {
        if (ModelState.IsValid)
        {
            db.Entry(admin).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(admin);
    }

  
    public ActionResult Xoa(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Admins admin = db.Admins.Find(id);
        if (admin == null)
        {
            return HttpNotFound();
        }
        return View(admin);
    }

  
    [HttpPost, ActionName("Xoa")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Admins admin = db.Admins.Find(id);
        db.Admins.Remove(admin);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
