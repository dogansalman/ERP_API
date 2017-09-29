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
        public List<Orders> get()
        {
            return db.orders.OrderByDescending(o => o.over_date).ToList();
        }

        //Add
        [HttpPost]
        [Route("")]
        public IHttpActionResult add([FromBody] Orders orders)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.orders.Add(orders);
            try
            {
                db.SaveChanges();

                orders.order_stocks.ToList().ForEach(i =>
                {
                    OrderStocks os = new OrderStocks()
                    {
                        order_id = orders.id,
                        order_unit = i.order_unit,
                        stockcard_id = i.stockcard_id
                    };
                    db.orderstocks.Add(os);
                });

                db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                ExceptionController.Handle(ex);
            }

            return Ok(orders);
        }
    }
}
