using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Libary.ExceptionHandler;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/productions")]
    public class ProductionsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [Route("")]
        [HttpPost]
        public IHttpActionResult add([FromBody] Productions production)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //update order production state
            Orders order = db.orders.Find(production.order_id);
            if (order == null) return NotFound();
            order.is_production = true;

            //update stock card real stock
            int stockcard_id = db.orderstocks.Find(production.order_id).stockcard_id;
            StockCards sc = db.stockcards.Find(stockcard_id);
            sc.unit = sc.unit - production.unit;

            //add production
            db.productions.Add(production);
            db.SaveChanges();

            //production personnels
            production.production_personnels.ToList().ForEach(pp =>
            {
                ProductionPersonnel personnel = new ProductionPersonnel
                {
                    personel_id = pp.personnel.id,
                    production_id = production.id
                };
                db.production_personnels.Add(personnel);

                db.SaveChanges();

                //personnels operation
                pp.operations.ToList().ForEach(op =>
                {
                    ProductionPersonnelOperation personnelOperation = new ProductionPersonnelOperation
                    {
                        production_personel_id = personnel.id,
                        machine = op.machine.name,
                        operation = op.operation.name,
                        operation_time = op.operation_time
                    };

                    db.production_personnel_operation.Add(personnelOperation);
                });
                db.SaveChanges();
            });

            return Ok(production);

        }
    }
}
