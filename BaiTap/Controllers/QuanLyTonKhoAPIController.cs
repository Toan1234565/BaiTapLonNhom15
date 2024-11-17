using BaiTap.Models;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BaiTap.Controllers
{
    public class QuanLyTonKhoAPIController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/QuanLyTonKhoApi
        [HttpGet]
        public IHttpActionResult GetTonKho()
        {
            var tonKho = db.TonKho.ToList();
            return Ok(tonKho);
        }

        // GET: api/QuanLyTonKhoApi/5
        [HttpGet]
        public IHttpActionResult GetTonKho(int id)
        {
            var tonKho = db.TonKho.Find(id);
            if (tonKho == null)
            {
                return NotFound();
            }
            return Ok(tonKho);
        }

        // POST: api/QuanLyTonKhoApi
        [HttpPost]
        public IHttpActionResult PostTonKho(TonKho tonKho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TonKho.Add(tonKho);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tonKho.TonKhoID }, tonKho);
        }

        // PUT: api/QuanLyTonKhoApi/5
        [HttpPut]
        public IHttpActionResult PutTonKho(int id, TonKho tonKho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tonKho.TonKhoID)
            {
                return BadRequest();
            }

            db.Entry(tonKho).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TonKhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/QuanLyTonKhoApi/5
        [HttpDelete]
        public IHttpActionResult DeleteTonKho(int id)
        {
            var tonKho = db.TonKho.Find(id);
            if (tonKho == null)
            {
                return NotFound();
            }

            db.TonKho.Remove(tonKho);
            db.SaveChanges();

            return Ok(tonKho);
        }

        private bool TonKhoExists(int id)
        {
            return db.TonKho.Count(e => e.TonKhoID == id) > 0;
        }
    }
}
