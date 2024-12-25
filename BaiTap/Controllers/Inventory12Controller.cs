using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaiTap.Models;
using Newtonsoft.Json;

namespace BaiTap.Controllers
{
    public class Inventory12Controller : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: Inventory
        public async Task<ActionResult> Index()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            HttpClient client = new HttpClient(handler);
            HttpResponseMessage response = await client.GetAsync("https://api-manager.us-east-a.apiconnect.automation.ibm.com/manager/api-connect-th-0/inventory");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var inventories = JsonConvert.DeserializeObject<List<TonKho>>(data);
                
                if (data != null)
                {
                    return View(inventories);
                }
                ViewBag.Thongbao = "tai danh sach san pham that bai";
                return View("Error");
            }
            return View(new List<TonKho>());
        }

        // GET: Inventory/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"https://api-manager.us-east-a.apiconnect.automation.ibm.com/manager/api-connect-th-0/inventory/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var inventory = JsonConvert.DeserializeObject<TonKho>(data);
                return View(inventory);
            }
            return HttpNotFound();
        }

        // POST: Inventory/Upload
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var filePath = Path.Combine(Server.MapPath("~/App_Data/uploads"), Path.GetFileName(file.FileName));
                file.SaveAs(filePath);

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = file.FileName
                };
                content.Add(fileContent);

                HttpResponseMessage response = await client.PostAsync("https://api-manager.us-east-a.apiconnect.automation.ibm.com/manager/api-connect-th-0/inventory/upload", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}
