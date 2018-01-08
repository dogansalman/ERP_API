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
                   order_unit = os.order_unit,
                   produced_orderstock = os.produced_orderstock,
                  
               }
            ).FirstOrDefault();

            var pn = db.orderstocks.Where(os => os.order_id == order.id).FirstOrDefault().process_no;
            if(pn != null) orderstock.order_stock.process_no = new StockCardProcessNo { id = 1, process_no = pn, stockcard_id = orderstock.order_stock.id };

            order.order_stocks = new OrderStock() {
                order_stock = orderstock.order_stock,
                order_unit = orderstock.order_unit,
                produced_orderstock = orderstock.produced_orderstock
            };

            return order;

        }
       
        //Add
        [HttpPost]
        [Route("")]
        public IHttpActionResult add([FromBody] Orders orders)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                db.orders.Add(orders);

                db.SaveChanges();

                OrderStocks os = new OrderStocks()
                {
                    order_id = orders.id,
                    order_unit = orders.order_stocks.order_unit,
                    stockcard_id = orders.order_stocks.order_stock.id
                };

                if (orders.order_stocks.order_stock.process_no != null)
                {
                    os.process_no = orders.order_stocks.order_stock.process_no.process_no;
                    os.process_name = orders.order_stocks.order_stock.process_no.name;
                }
                db.orderstocks.Add(os);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
            return Ok(orders);
        }
    }
}
