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
        [HttpGet]
        public List<StockCards> get()
        {
            return db.stockcards.Where(sc => sc.deleted == false).OrderBy((sc => sc.name)).ToList();
        }

        //Detail Stock Card
        [Route("{id}")]
        [HttpGet]
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
        public IHttpActionResult add([FromBody] StockCards stockCard)
        {
            db.stockcards.Add(stockCard);
            try
            {
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
        public IHttpActionResult update([FromBody] StockCards stockCards, int id)
        {
            StockCards stockCardDetail = db.stockcards.Find(id);
            if (stockCardDetail == null) return NotFound();
            stockCardDetail.name = stockCards.name;
            stockCardDetail.stock_type = stockCards.stock_type;
            stockCardDetail.per_production_unit = stockCards.per_production_unit;
            stockCardDetail.updated_date = DateTime.Now;
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

    }
}
