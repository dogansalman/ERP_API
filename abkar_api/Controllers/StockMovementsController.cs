using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.ExceptionHandler;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/stockmovements")]
    public class StockMovementsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Stock Movements
        [Route("{stockCardId}")]
        [HttpGet]
        public List<StockMovements> get(int stockCardId)
        {
            return db.stockmovements.Where(sm => sm.stockcard_id == stockCardId).OrderByDescending(sm => sm.id).ToList();
        }

        //Add Stock Movement
        [Route("add/{stockCardId}")]
        [HttpPost]
        public IHttpActionResult add([FromBody] StockMovements stockmovements, int stockCardId)
        {
            //Stock Movement Add type is true
            stockmovements.incoming_stock = true;

            //Validation Request
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Stock Card Validation
            StockCards stockcards = db.stockcards.Find(stockCardId);
            if (stockcards == null) return NotFound();

            //Add Stock Movements
            db.stockmovements.Add(stockmovements);

            //Add Stock to Stock Card
            stockcards.unit = stockcards.unit + stockmovements.unit;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(stockmovements);
        }

        //Remove Stock Movement
        [Route("remove/{stockCardId}")]
        [HttpPut]
        public IHttpActionResult remove([FromBody] StockMovements stockmovements, int stockCardId)
        {
            //Stock Movement Remove type is false
            stockmovements.incoming_stock = false;

            //Validation Request
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Stock Card Validation
            StockCards stockcards = db.stockcards.Find(stockCardId);
            if (stockcards == null) return NotFound();

            //Add Stock Movements
            db.stockmovements.Add(stockmovements);

            //Remove Stock to Stock Card
            stockcards.unit = stockcards.unit - stockmovements.unit;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);

            }
            return Ok(stockcards);
        }
        
        [Route("")]
        [HttpPut]
        public IHttpActionResult changeWaybillNo( [FromBody] StockMovements stockmovement)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            StockMovements sm = db.stockmovements.Find(stockmovement.id);
            sm.waybill = stockmovement.waybill;
            db.SaveChanges();
            return Ok(stockmovement);
        }



    }

}
