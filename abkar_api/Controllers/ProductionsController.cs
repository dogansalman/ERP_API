using System;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/productions")]
    public class ProductionsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "admin,planning")]
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
                ).OrderByDescending(o => o.production.id).ToList();
        }

        [Route("")]
        [HttpPost]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult add([FromBody] Productions production)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //update order production state
            Orders order = db.orders.Find(production.order_id);
            if (order == null) return NotFound();
            order.is_production = true;

            //update stock card real stock
            int stockcard_id = db.orderstocks.Where(os => os.order_id == production.order_id).FirstOrDefault().stockcard_id;
            StockCards sc = db.stockcards.Find(stockcard_id);
            sc.unit = sc.unit - production.unit;

            //add production
            db.productions.Add(production);
            db.SaveChanges();

            db.Configuration.ValidateOnSaveEnabled = false;
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
            db.Configuration.ValidateOnSaveEnabled = true;

            return Ok(production);

        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult update([FromBody] Productions production, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Productions pro = db.productions.Find(id);
            if (pro == null) return NotFound();

            //update stock
            var stockcard_id = db.orderstocks.Where(os => os.order_id == pro.order_id).FirstOrDefault().stockcard_id;

            StockCards sc = db.stockcards.Find(stockcard_id);
            sc.unit = (sc.unit + (pro.unit - production.unit));


            // update production
            pro.name = production.name;
            pro.description = production.description;
            pro.start_time = production.start_time;
            pro.end_time = production.end_time;
            pro.unit = production.unit;

            // delete production personel & operation
            var productionPersonnelId = db.production_personnels.Where(x => x.production_id == id).Select(s => s.id).ToList();
            db.production_personnels.RemoveRange(db.production_personnels.Where(pp => productionPersonnelId.Contains(pp.id)));
            db.production_personnel_operation.RemoveRange(db.production_personnel_operation.Where(ppo => productionPersonnelId.Contains(ppo.production_personel_id)));

            // create production personel & operation
            production.production_personnels.ToList().ForEach(pp =>
            {
                ProductionPersonnel personnel = new ProductionPersonnel
                {
                    personel_id = pp.personnel.id,
                    production_id = id
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
            db.SaveChanges();

            return Ok(production);
        }

        [Route("detail/{id}")]
        [HttpGet]
        [Authorize(Roles = "admin,planning")]
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
                         where pp.personel_id == per.id && pp.production_id == pro.id
                         select new {
                             machine = new {name = ppo.machine},
                             operation = new { name = ppo.operation},
                             operation_time = ppo.operation_time
                         }
                        ).ToList()


                    }
                    ).ToList()
                }
                ).FirstOrDefault();
        }

        [Route("dispatch/{production_id}")]
        [HttpPost]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult addDispatch([FromBody] StockMovements stockmovements, int production_id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Productions productions = db.productions.Find(production_id);
            if (productions.is_complate) return BadRequest();

            //get stockcard id
            var stockcard = (
                from p in db.productions
                where p.id == production_id
                join os in db.orderstocks on p.order_id equals os.order_id
                select new { os.stockcard_id }
                ).FirstOrDefault();
            if (stockcard == null) return NotFound();
            
            //add dispatch to stockmovement
            stockmovements.production_id = production_id;
            stockmovements.incoming_stock = false;
            stockmovements.stockcard_id = stockcard.stockcard_id;
            stockmovements.created_date = DateTime.Now;
            db.stockmovements.Add(stockmovements);
            db.SaveChanges();

            // create new junk stock movement
            if (stockmovements.junk > 0)
            {
                StockMovements junkStockmovement = new StockMovements()
                {
                    is_junk = true,
                    supplier = "",
                    unit = stockmovements.junk,
                    incoming_stock = false,
                    production_id = production_id,
                    waybill = "",
                    stockcard_id = stockcard.stockcard_id,
                    created_date = DateTime.Now
                };
                db.stockmovements.Add(junkStockmovement);
                db.SaveChanges();

            }


            // set produced unit in order
            OrderStocks order_stocks = db.orderstocks.Where(os => os.order_id == productions.order_id).FirstOrDefault();
            order_stocks.produced_orderstock = (order_stocks.produced_orderstock + stockmovements.unit);
            db.SaveChanges();

            // set produced unit in production
            productions.produced_unit += stockmovements.unit;

            // check & complate production
            if (productions.unit <= productions.produced_unit)
            {
                productions.is_complate = true;
                productions.updated_date = DateTime.Now;
            }

            db.SaveChanges();

            // check & complate orders
            Orders order = db.orders.Find(order_stocks.order_id);
            int totalProduced = db.productions.Where(pr => pr.order_id == order.id && pr.is_complate == true).Select(p => p.unit).DefaultIfEmpty(0).Sum();
            if (totalProduced == order_stocks.order_unit)
            {
                order.is_complated = true;
                order.is_production = false;
                order.updated_date = DateTime.Now;
                db.SaveChanges();
            }


            return Ok(stockmovements);
        }
        
        [Route("dispatch/{production_id}")]
        [HttpGet]
        [Authorize(Roles = "admin,planning")]
        public object getDispatchs(int production_id)
        {
            return db.stockmovements.Where(sc => sc.production_id == production_id).OrderByDescending(sc => sc.id).ToList();
       
        }
    }
}

