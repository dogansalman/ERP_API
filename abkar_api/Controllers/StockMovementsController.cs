﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;

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
            return db.stockmovements.Where(sm => sm.stockcard_id == stockCardId).OrderBy(sm => sm.created_date).ToList();
        }

        //Add Stock Movement
        [Route("add/{stockCardId}")]
        [HttpPost]
        public IHttpActionResult add([FromBody] StockMovements stockmovements, int stockCardId)
        {
            //Stock Movement Add type is true
            stockmovements.movement_type = true;

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
                ExceptionController.Handle(e);
            }
            return Ok(stockmovements);
        }

        //Remove Stock Movement
        [Route("remove/{stockCardId}")]
        [HttpPut]
        public IHttpActionResult remove([FromBody] StockMovements stockmovements, int stockCardId)
        {
            //Stock Movement Remove type is false
            stockmovements.movement_type = false;

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
                ExceptionController.Handle(e);

            }
            return Ok(stockcards);
        }
        


    }

}