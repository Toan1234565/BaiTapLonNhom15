using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using BaiTap.Models;

namespace BaiTap.Controllers
{
    public class TimKiemController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private Model1 db = new Model1();
        public ActionResult Index()
        {
            ViewBag.HangList = new SelectList(db.Hang.ToList(), "HangID", "TenHang");
            ViewBag.DanhMucList = new SelectList(db.DanhMuc.ToList(), "DanhMucID", "TenDanhMuc");
            return View();
        }

        public async Task<ActionResult> TimKiem(string name)
        {
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44383/api/timkiem/timkiem?name={name}");
            if (response.IsSuccessStatusCode)
            {
                var kq = await response.Content.ReadAsAsync<List<SanPham>>();
                return View(kq);
            }
            return RedirectToAction("Error","QuanLySanPham");
        }
        public async Task<ActionResult> LocSP(string name, int ? IDHang, int ? IDDanhMuc, double ? to, double ? from, string sx)
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:4483/api/timkiem/locsp?name = {name}&IDHang = {IDHang} & IDDanhMuc= {IDDanhMuc} & from = {from} & to = {to} & sx = {sx}");
            if (response.IsSuccessStatusCode)
            {
                var kq = await response.Content.ReadAsAsync<List<SanPham>>();
                return View(kq);
            }
            return RedirectToAction("Error", "QuanLySanPham");
        }
        // Tạo hàm GET cho các hành động khác tương tự như `TimKiem`
    }
}
