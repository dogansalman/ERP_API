using System;
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


    }
}
