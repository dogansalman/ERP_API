using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Contexts;
using abkar_api.Models;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/supply/request")]
    public class SupplyRequistionsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Supply Requistions
        [HttpGet]
        [Route("")]
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

                ExceptionController.Handle(ex);
            }

            if(supplyrequistions.notify)
            {
                Suppliers supplier = db.suppliers.FirstOrDefault(s => s.company == supplyrequistions.supplier);
                if(supplier != null)
                {
                    EmailController.Send(supplyrequistions.message, supplier.email, "Abkar Sipariş Bildirim");
                }
            }
            
            return Ok(supplyrequistions);

        }
        
    }
}
