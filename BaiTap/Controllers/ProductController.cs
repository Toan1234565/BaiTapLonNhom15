////using BaiTap.Models;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using System.Web.Helpers;
////using System.Web.Mvc;
////using System.Web.Services.Description;

////public class ProductController : Controller
////{
////    private readonly ProductService _productService = new ProductService();
////    private Model1 db = new Model1();
////    public ActionResult Index()
////    {
////        return View();
////    }

////    public async Task<ActionResult> ProductDetails(string productName)
////    {
////        string imageUrl = await _productService.GetProductImageAsync(productName);
////        ViewBag.ImageUrl = imageUrl;
////        ViewBag.ProductName = productName;
////        return View();
////    }
////    public ActionResult Index1()
////    {
////        return View();
////    }
////    private readonly ProductService productService = new ProductService();




////    }

//using System.Web.Mvc;
//using BaiTap.Models;
//using System.Linq;

//namespace BaiTap.Controllers
//{
//    public class ProductsController : Controller
//    {
//        private readonly Model1 db = new Model1();

//        public ActionResult Compare()
//        {
//            var products = db.SanPham.ToList();
//            return View(products);
//        }

//        public ActionResult GetProducts()
//        {
//            var products = db.SanPham.ToList();
//            return PartialView("_ProductListPartial", products);
//        }
//        public ActionResult _ProductListPartial()
//        {
//            return View();
//        }
//    }
//}

using BaiTap.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BaiTap.Controllers
{
    public class ProductsController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        Model1 db = new Model1();
        public ActionResult Compare()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CompareProducts(List<int> productIds, List<Criteria> criteria)
        {
            var requestContent = new { productIds, criteria };
            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:44383/api/comparison/compare", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<dynamic>();
                return View("CompareResults", result);
            }

            return View("Error");
        }
        public ActionResult CompareResults() { return View(); }
        public ActionResult GetProducts()
        {
            var products = db.SanPham.ToList();
            return PartialView("_ProductListPartial", products);
        }
        public ActionResult SearchProducts(string name)
        {
            var kq = db.SanPham.Where(x => x.TenSanPham.ToLower().Contains(name.ToLower())).ToList();
            return PartialView("_ProductListPartial", kq);
        }
    }
}
