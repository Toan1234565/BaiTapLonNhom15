using BaiTap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BaiTap.Controllers
{
    [RoutePrefix("api/inventory")]
    public class CanhBaoController : ApiController
    {
        private Model1 db = new Model1();
        [HttpGet]
        [Route("check")]
        public IHttpActionResult CheckKho()
        {
            int soluong1 = 5;
            var tonkho = db.TonKho.Where(p => p.SoLuongTon <= soluong1).ToList();
            if (tonkho.Any())
            {
                // thuc hien hanh dong gui canh bao
                // gui thong tin ve cac san pham co ton kho thap
                return Ok(tonkho);
            }
            return Ok("Ton kho o muc an toan");
        }
    }
}
