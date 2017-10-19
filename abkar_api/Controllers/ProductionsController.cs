using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Libary.ExceptionHandler;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/productions")]
    public class ProductionsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [Route("")]
        [HttpPost]
        public IHttpActionResult add([FromBody] Productions production)
        {

          

       
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.productions.Add(production);
            db.SaveChanges();


            //production personnels
            production.production_personnels.ToList().ForEach(pp =>
            {
                ProductionPersonnel personnel = new ProductionPersonnel
                {
                    personel_id = pp.personnel.id,
                    production_id = production.id
                };
                db.production_personnels.Add(personnel);

                db.SaveChanges();

                //personnels operation
                pp.operations.ToList().ForEach(op =>
                {
                    ProductionPersonnelOperation personnelOperation = new ProductionPersonnelOperation
                    {
                        production_personel_id = personnel.id,
                        machine = op.machine.name,
                        operation = op.operation.name,
                        operation_time = op.operation_time
                    };

                    db.production_personnel_operation.Add(personnelOperation);
                });
                db.SaveChanges();


            });

            


         
            //create production

            /*

            using (var dbContext = new DatabaseContext())
            {
                //production personnels
                production.production_personnels.ToList().ForEach(pp =>
                {
                    ProductionPersonnel personnel = new ProductionPersonnel
                    {
                        personel_id = pp.personnel.id,
                        production_id = 1
                    };
                    db.production_personnels.Add(personnel);
           
                });

            }


             */





            return Ok();

        }
    }
}
