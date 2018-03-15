using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.ExceptionHandler;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/operations")]
    public class OperationsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Operations
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin,planning")]
        public List<Operations> get()
        {
            return db.operations.OrderByDescending(c => c.id).ToList();
        }

        //Get Operation
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult detail(int id)
        {
            Operations operation = db.operations.FirstOrDefault(p => p.id == id);
            if (operation == null) return NotFound();
            return Ok(operation);
        }

        //Add Operation
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "admin,planning")]
        public IHttpActionResult add([FromBody] Operations operation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.operations.Add(operation);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(operation);
        }

        //Update Operation
        [Route("{id}")]
        [Authorize(Roles = "admin,planning")]
        [HttpPut]
        public IHttpActionResult update([FromBody] Operations operation, int id)
        {
            Operations operationDetail = db.operations.Find(id);
            if (operationDetail == null) return NotFound();
            operationDetail.name = operation.name;
            operationDetail.updated_date = DateTime.Now;
            operationDetail.operation_time = operation.operation_time;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(operation);
        }

        //Delete Operation
        [Route("{id}")]
        [Authorize(Roles = "admin,planning")]
        [HttpDelete]
        public IHttpActionResult delete(int id)
        {
            Operations operation = db.operations.Find(id);
            if (operation == null) return NotFound();
            db.operations.Remove(operation);
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
