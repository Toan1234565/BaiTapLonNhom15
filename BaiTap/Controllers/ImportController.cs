using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaiTap.Models;

namespace BaiTap.Controllers
{
    public class ImportController : Controller
    {
        private readonly ImportService _importService = new ImportService();

        [HttpGet]
        public ActionResult ImportView()
        {
            return View(new List<ChiTietPhieuXuat>());
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var content = new StreamContent(file.InputStream);
                content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = file.FileName
                };
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                var products = await _importService.UploadFileAsync(content);
                return View("ImportView", products);
            }
            else
            {
                ViewBag.Message = "Vui lòng chọn file!";
                return View("ImportView", new List<ChiTietPhieuXuat>());
            }
        }
    }
}
