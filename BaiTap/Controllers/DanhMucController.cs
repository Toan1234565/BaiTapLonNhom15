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
    public class DanhMucController : Controller
    {
        // GET: DanhMuc
      
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DsDanhMuc()
        {
            List<DanhMuc> ds = db.DanhMuc.ToList();
            return View(ds);
        }
            // GET: DanhMuc/HangSX/{IDDM}
            public ActionResult HangSX(int id)
            {
            var kq = db.DanhMuc.Include(x => x.Hang).FirstOrDefault(m => m.DanhMucID == id);
            if (kq == null)
                {
                    return HttpNotFound();
                }
                return View(kq);
            }
    }
}
