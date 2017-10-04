using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        DatabaseContext db = new DatabaseContext();
       
        //Get
        [HttpGet]
        [Route("")]
        public object get()
        {

            return db.orders.OrderByDescending(o => o.over_date)
                .Join(db.customers,
                o => o.customer_id,
                c => c.id,
                (o, c) => new
                {
                    customer = c,
                    order = o
                }
                ).OrderByDescending(o => o.order.created_date).ToList();
        }
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
                ExceptionController.Handle(ex);
            }
            return Ok(orders);
        }
    }
}
