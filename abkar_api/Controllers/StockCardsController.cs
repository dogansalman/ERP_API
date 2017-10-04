using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;

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
            return db.stockcards.OrderBy((sc => sc.name)).ToList();
        }

        //Detail Stock Card
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult detail(int id)
        {
            StockCards stockCard = db.stockcards.Find(id);
            if (stockCard == null) return NotFound();
            return Ok(stockCard);
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
                ExceptionController.Handle(e);
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
            stockCardDetail.updated_date = DateTime.Now;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok(stockCards);
        }

        //Delete Stock Card
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult delete(int id)
        {
            // TODO Check use in production !
            // TODO Delete all stock movement for this stock card

            StockCards stockCard = db.stockcards.Find(id);
            if (stockCard == null) return NotFound();
            db.stockcards.Remove(stockCard);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok();
        }

    }
}
