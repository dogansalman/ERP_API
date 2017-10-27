using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Libary.ExceptionHandler;
using abkar_api.Contexts;
using System.Data.Entity.SqlServer;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/productions")]
    public class ProductionsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("{id}")]
        public object gets(int id)
        {
            return (
                from p in db.productions
                where p.order_id == id
                join o in db.orders on p.order_id equals o.id
                select new
                {
                    production = p,
                    order = o,
                }
                ).ToList();
        }

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

        [Route("detail/{id}")]
        [HttpGet]
        public object get(int id)
        {
            
            return (
                from pro in db.productions
                where pro.id == id
            select new
                {
                    description = pro.description,
                    name = pro.name,
                    end_time = pro.end_time.ToString().Substring(0, 5),
                    start_time = pro.start_time.ToString().Substring(0, 5),
                    unit = pro.unit,
                    is_cancel = pro.is_cancel,
                    is_complate = pro.is_complate,
                    order_id = pro.order_id,
                    production_personnels = (
                    from proper in db.production_personnels
                    join per in db.personnels on proper.personel_id equals per.id
                    where proper.production_id == id
                    select new
                    {
                        personnel = per,
                        operations = (
                         from ppo in db.production_personnel_operation
                         join pp in db.production_personnels on ppo.production_personel_id equals pp.id 
                         where pp.personel_id == per.id
                         select new {ppo}
                        ).ToList()
                    }
                    ).ToList()
                }
                ).FirstOrDefault();
        }
    }
}
