using System.Linq;
using System.Web.Http;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin")]
        public object get()
        {
            int personels = db.personnels.Count();
            int stockcards = db.stockcards.Count();
            int orders = db.orders.Count();
            int productions = db.productions.Count();
            int customers = db.customers.Count();
            int suppliers = db.suppliers.Count();
            return new {
                personnels = personels,
                stockcards = stockcards,
                orders = orders,
                production = productions,
                customers = customers,
                suppliers = suppliers
            };
        }

        [HttpGet]
        [Route("stockcards/{unit}")]
        [Authorize(Roles = "admin")]
        public object stockcards(int unit)
        {
            return (
                from sc in db.stockcards
                where sc.unit <= unit
                select new { id = sc.id, code = sc.code, name = sc.name, unit = sc.unit }
                ).ToList();
        }

        [HttpGet]
        [Route("production/{year}")]
        [Authorize(Roles = "admin")]
        public object production(int year)
        {
            return
             (from d in db.productions
              where d.is_complate && d.created_date.Year == year
              group d by new
              {
                  Month = d.created_date.Month
              } into g
              select new
              {
                  Month = g.Key.Month,
                  Total = g.Sum(x => x.unit),
              }
            ).AsEnumerable()
             .Select(g => new
             {
                 Month = g.Month,
                 Unit = g.Total,
             }).ToList();
        }

        [HttpGet]
        [Route("production/best")]
        [Authorize(Roles = "admin")]
        public object bestProduction()
        {
            var stockcards = (
                 from p in db.productions
                 join os in db.orderstocks on p.order_id equals os.order_id
                 join sc in db.stockcards on os.stockcard_id equals sc.id
                 group sc by new
                 {
                     sc.id
                 } into sc
                 select new
                 {
                     name = (from x in db.stockcards where x.id == sc.Key.id select x.name).FirstOrDefault(),
                     id = sc.Key.id

                 }).AsEnumerable()
                 .Select(sc => new
                 {
                     name = sc.name,
                     id = sc.id,
                     unit = 0

                 }).Take(10);

            return stockcards.ToList().Select(x => new
            {
                name = x.name,
                unit = (
                from p in db.productions
                join os in db.orderstocks on p.order_id equals os.order_id
                join sc in db.stockcards on os.stockcard_id equals sc.id
                where sc.id == x.id
                where p.is_complate
                select new { unit = p.unit }
                ).Sum(xx => xx.unit)
            }).ToList().OrderByDescending(xxx => xxx.unit);

        }

    }
}
