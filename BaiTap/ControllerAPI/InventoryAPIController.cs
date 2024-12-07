using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BaiTap.ControllerAPI
{
    public class InventoryAPIController : ApiController
    {
        [RoutePrefix("api/inventory")]
        public class InventoryController : ApiController
        {
            private readonly InventoryService _inventoryService = new InventoryService();

            [HttpGet]
            [Route("check")]
            public async Task<IHttpActionResult> CheckInventory()
            {
                int lowStockThreshold = 10; // Ngưỡng tồn kho thấp
                int highStockThreshold = 100; // Ngưỡng tồn kho cao

                var alertProducts = await _inventoryService.CheckInventoryLevels(lowStockThreshold, highStockThreshold);
                if (alertProducts == null || !alertProducts.Any())
                {
                    return NotFound();
                }

                return Ok(alertProducts);
            }
        }
    }

}
