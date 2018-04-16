using abkar_api.Contexts;
using abkar_api.Libary.Identity;
using System.Linq;
using System.Web.Http;

namespace abkar_api.Controllers.ComController
{
    [RoutePrefix("api/customer/orders")]
    public class ComOrdersController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "customer")]
        public object get_()
        {
            int id = Identity.get(User);

            return (
                 from o in db.orders
                 join c in db.customers on o.customer_id equals c.id
                 join os in db.orderstocks on o.id equals os.order_id
                 join sc in db.stockcards on os.stockcard_id equals sc.id
                 where o.customer_id == id
                 select new
                 {
                     customer = c,
                     order = o,
                     order_stock = os,
                     stockcard = sc,
                     produced_unit = (
                     from p in db.productions
                     where p.is_cancel == false && p.is_complate == false && p.order_id == o.id
                     select (p.unit)
                     ).DefaultIfEmpty(0).Sum()
                 }
                 ).OrderByDescending(olist => olist.order.created_date).ToList();
        }
    }
}
