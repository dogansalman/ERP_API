using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.ExceptionHandler;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/stocktypes")]
    public class StockTypesController : ApiController
    {
        DatabaseContext db = new DatabaseContext();
        
        //Get Stock Types
        [Route("")]
        [HttpGet]
        public List<StockTypes> get()
        {
            return db.stocktypes.OrderBy((st => st.name)).ToList();
        }
        
        //Stock Type Detail
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult detial(int id)
        {
            StockTypes stocktype = db.stocktypes.Find(id);
            if (stocktype == null) return NotFound();
            return Ok(stocktype);
        }
        
        //Add Stock Type
        [Route("")]
        [HttpPost]
        public IHttpActionResult add([FromBody] StockTypes stocktype)
        {
            db.stocktypes.Add(stocktype);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(stocktype);
        }

        //Update Stock Type
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult update([FromBody] StockTypes stocktype, int id)
        {
            StockTypes StockTypeDetail = db.stocktypes.Find(id);
            if (StockTypeDetail == null) return NotFound();
            StockTypeDetail.name = stocktype.name;
            StockTypeDetail.updated_date = DateTime.Now;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(stocktype);
        }

        //Delete Stock Type
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult delete(int id)
        {
            StockTypes stocktype = db.stocktypes.Find(id);
            if (stocktype == null) return NotFound();
            db.stocktypes.Remove(stocktype);
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
