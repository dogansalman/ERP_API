﻿using System;
using System.Linq;
using System.Web.Http;
using abkar_api.Contexts;
using abkar_api.Models;
using abkar_api.Libary.Email;
using abkar_api.Libary.ExceptionHandler;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/supply/request")]
    public class SupplyRequistionsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Supply Requistions
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin,planning")]
        public object getSupplyRequisitions()
        {
            return db.supplyrequisitions.Join(
                db.stockcards,
                sr => sr.stockcard_id,
                sc => sc.id,
                (sr, sc) => new
                {
                    SupplyRequisitions = sr,
                    stockcard = sc.name,
                    stockcard_code = sc.code
                }).OrderByDescending(sr => sr.SupplyRequisitions.created_date).ToList();

        }

        //Add Supply Requisition
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult add([FromBody] SupplyRequisitions supplyrequistions)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.supplyrequisitions.Add(supplyrequistions);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                ExceptionHandler.Handle(ex);
            }

            if(supplyrequistions.notify)
            {
                Suppliers supplier = db.suppliers.FirstOrDefault(s => s.company == supplyrequistions.supplier);
                if(supplier != null)
                {
                    Email.Send(supplyrequistions.message, supplier.email, "Abkar Sipariş Bildirim");
                }
            }
            
            return Ok(supplyrequistions);

        }
        //Update Supply Requisition
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult update([FromBody] SupplyRequisitions supplyrequistions, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
           
      
       
            SupplyRequisitions sr = db.supplyrequisitions.Find(id);
         

            if (sr == null)
            {
                return NotFound();
            }

            sr.state = supplyrequistions.state;
            sr.real_unit = supplyrequistions.real_unit;
            sr.updated_date = DateTime.Now;

            //Add stock unit
            if(supplyrequistions.state == 1)
            {
                StockMovements sm = new StockMovements();
                StockCards sc = db.stockcards.Find(sr.stockcard_id);

                sm.created_date = DateTime.Now;
                sm.incoming_stock = true;
                sm.on_requisition = true;
                sm.supplier = sr.supplier;
                sm.waybill = supplyrequistions.waybill;
                sm.unit = sr.real_unit;
                sm.stockcard_id = sr.stockcard_id;

                db.stockmovements.Add(sm);

                sc.unit = sc.unit + sr.real_unit;
                
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

                ExceptionHandler.Handle(e);
            }


            return Ok(supplyrequistions);
        }

    }
}
