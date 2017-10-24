using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.ExceptionHandler;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        DatabaseContext db = new DatabaseContext();
       
        // Orders
        [HttpGet]
        [Route("")]
        public object get()
        {
            return (
                 from o in db.orders
                 join c in db.customers on o.customer_id equals c.id
                 join os in db.orderstocks on o.id equals os.order_id
                 join sc in db.stockcards on os.stockcard_id equals sc.id
                 select new {
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

        // Order
        [HttpGet]
        [Route("{id}")]
        public object detail(int id)
        {
            Orders order = db.orders.Find(id);
            if (order == null) return NotFound();

            var orderstock = db.orderstocks
                .Where(ors => ors.order_id == id)
                .Join(
               db.stockcards,
               os => os.stockcard_id,
               sc => sc.id,
               (os, sc) => new
               {
                   order_stock = sc,
                   order_unit = os.order_unit
               }
            ).ToList();

            ICollection<OrderStock> orderstocks = new List<OrderStock>();
            orderstock.ForEach(os =>
            {
                orderstocks.Add(new OrderStock { order_stock = os.order_stock, order_unit = os.order_unit });
            });
            order.order_stocks = orderstocks;

            return order;

        }
       
        //Add
        [HttpPost]
        [Route("")]
        public IHttpActionResult add([FromBody] Orders orders)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.orders.Add(orders);

            db.SaveChanges();

            orders.order_stocks.ToList().ForEach(i =>
            {
                OrderStocks os = new OrderStocks()
                {
                    order_id = orders.id,
                    order_unit = i.order_unit,
                    stockcard_id = i.order_stock.id
                };
                db.orderstocks.Add(os);
            });

            db.SaveChanges();

            try
            {
       

            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
            return Ok(orders);
        }
    }
}
