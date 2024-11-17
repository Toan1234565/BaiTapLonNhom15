using Antlr.Runtime.Tree;
using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml.Linq;


namespace BaiTap.Controllers
{
    public class HangSXController : Controller
    {
        private Model1 db = new Model1();
        // GET: HangSX
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DSHangsx()
        {
            List<Hang> ds = db.Hang.ToList();
            return View(ds);
        }
        public ActionResult SP(int id)
        {
            var kq = db.Hang.Include(x => x.SanPham).FirstOrDefault(m => m.HangID == id);
            if (kq == null)
            {
                return HttpNotFound();
            }
            return View(kq);
        }
    }
}