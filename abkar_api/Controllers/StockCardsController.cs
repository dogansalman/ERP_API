using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.ExceptionHandler;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/stockcards")]
    public class StockCardsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Stock Cards
        [Route("")]
        [Authorize(Roles = "admin,planning")]
        [HttpGet]
        public List<StockCards> get()
        {
            return db.stockcards.Where(sc => sc.deleted == false).OrderBy((sc => sc.name)).OrderByDescending(sc => sc.id).ToList();
        }

        //Detail Stock Card
        [Route("{id}")]
        [HttpGet]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult detail(int id)
        {
            StockCards sc = db.stockcards.Find(id);
            if (sc == null) return NotFound();
            sc.stockcard_process_no = db.stockcard_process_no.Where(spn => spn.stockcard_id == sc.id).ToList();
            return Ok(sc);
        }
        
        //Add Stock Card
        [Route("")]
        [HttpPost]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult add([FromBody] StockCards stockCard)
        {
            try
            {
                db.stockcards.Add(stockCard);
                db.SaveChanges();

                stockCard.stockcard_process_no.ToList().ForEach((spn => {
                    spn.stockcard_id = stockCard.id;
                    db.stockcard_process_no.Add(spn);
                }));

                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(stockCard);
        }

        //Update Stock Card
        [Route("{id}")]
        [HttpPut]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult update([FromBody] StockCards stockCards, int id)
        {
            StockCards stockCardDetail = db.stockcards.Find(id);
            if (stockCardDetail == null) return NotFound();
            stockCardDetail.name = stockCards.name;
            stockCardDetail.stock_type = stockCards.stock_type;
            stockCardDetail.per_production_unit = stockCards.per_production_unit;
            stockCardDetail.updated_date = DateTime.Now;

            db.stockcard_process_no.RemoveRange(db.stockcard_process_no.Where(spn => spn.stockcard_id == id).ToList());
            stockCards.stockcard_process_no.ToList().ForEach((spn => {
                spn.stockcard_id = id;
                db.stockcard_process_no.Add(spn);
            }));

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(stockCards);
        }

        //Delete Stock Card
        [Route("{id}")]
        [HttpDelete]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult delete(int id)
        {
            StockCards stockCard = db.stockcards.Find(id);
            if (stockCard == null) return NotFound();
            stockCard.deleted = true;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok();
        }

        //Get Stock Card With Process Numbers
        [Route("process")]
        [HttpGet]
        [Authorize(Roles = "admin,planning")]
        public object getWithProcNo()
        {
            var x = (from sc in db.stockcards
                     from spn in db.stockcard_process_no.Where(y => y.stockcard_id == sc.id).DefaultIfEmpty()
                     where !sc.deleted 
                     select new
                     {
                         id = sc.id,
                         name = sc.name,
                         code = sc.code,
                         unit = sc.unit,
                         stock_type= sc.stock_type,
                         created_date= sc.created_date,
                         updated_date = sc.updated_date,
                         process_no = spn
                     }
                     ).ToList();
            return x;

        }
    }
}
