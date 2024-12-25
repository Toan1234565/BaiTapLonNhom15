using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BaiTap.Models;
using ExcelDataReader;
using System;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/import")]
    public class ImportApiController : ApiController
    {
        private Model1 db = new Model1();

        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> UploadFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            List<ChiTietPhieuXuat> products = new List<ChiTietPhieuXuat>();

            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                using (var stream = new MemoryStream(buffer))
                {
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    var result = reader.AsDataSet();
                    var dataTable = result.Tables[0]; // Assuming first sheet

                    for (int i = 1; i < dataTable.Rows.Count; i++) // Skip header row
                    {
                        var row = dataTable.Rows[i];
                        var product = new ChiTietPhieuXuat
                        {
                            SanPhamID = int.Parse(row[0].ToString()),
                            SoLuong = int.Parse(row[1].ToString()),
                            DonGia = double.Parse(row[2].ToString())
                        };
                        products.Add(product);
                    }

                    var phieuXuat = new PhieuXuat
                    {
                        NgayXuat = DateTime.Now,
                        Kho = "Default", // Replace with actual logic
                        KhachHangID = 1, // Replace with actual logic
                        ChiTietPhieuXuat = products
                    };

                    db.PhieuXuat.Add(phieuXuat);
                    await db.SaveChangesAsync();
                }
            }

            return Ok(new { success = true, data = products });
        }
    }
}
