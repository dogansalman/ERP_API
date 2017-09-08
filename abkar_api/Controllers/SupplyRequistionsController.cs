using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using abkar_api.Contexts;
using abkar_api.Models;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/suppliers/request")]
    public class SupplyRequistionsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Supply Requistions
        [HttpGet]
        [Route("")]
        public List<SupplyRequisitions> getSupplyRequisitions()
        {
            return db.supplyrequisitions.OrderBy(sr => sr.created_date).ToList();
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

            return Ok(supplyrequistions);

        }
        
    }
}
