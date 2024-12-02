using System.Threading.Tasks;
using System.Web.Mvc;

public class ProductController : Controller
{
    private readonly ProductService _productService = new ProductService();

    public ActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> ProductDetails(string productName)
    {
        string imageUrl = await _productService.GetProductImageAsync(productName);
        ViewBag.ImageUrl = imageUrl;
        ViewBag.ProductName = productName;
        return View();
    }
}
