using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;



namespace abkar_api.Controllers
{
    public class PersonnelController : ApiController
    {
        static readonly Personnel personelRepos = new Personnel();
        DatabaseContext db = new DatabaseContext();

        //get all personel
        [HttpGet]
        public List<Personnel> get()
        {
            return db.personnels.ToList();
        }

        //add new personel
        [HttpPost]
        public IHttpActionResult add([FromBody] Personnel personnel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.personnels.Add(personnel);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok(personnel);
        }
        [HttpPut]
        public IHttpActionResult update([FromBody] Personnel personnel, int id)
        {
            Personnel personnelDetail = db.personnels.Find(id);
            if (personnelDetail == null) return NotFound();

            personnelDetail.lastname = personnel.lastname;
            personnelDetail.name = personnel.name;
            personnelDetail.password = personnel.password;
            personnelDetail.username = personnel.username;
            personnelDetail.updated_date = DateTime.Now;
            personnelDetail.department_id = personnel.department_id;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok(personnel);
        }
        [HttpDelete]
        public IHttpActionResult delete(int id)
        {
            Personnel p = db.personnels.Find(id);
            if (p == null) return NotFound();
            db.personnels.Remove(p);

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
