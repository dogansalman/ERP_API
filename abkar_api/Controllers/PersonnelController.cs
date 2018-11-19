using System;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.ExceptionHandler;


namespace abkar_api.Controllers
{
    [RoutePrefix("api/personnel")]
    public class PersonnelController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Personnel
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin,planning")]
        public object get()
        {
            return db.personnels.Join(
                db.departments,
                p => p.department_id,
                d => d.id,
                (p, d) => new
                {
                    Personnel = new { id = p.id,  name = p.name ,lastname = p.lastname, department_id = p.department_id, state = p.state, created_date = p.created_date, updated_date = p.updated_date, deleted = p.deleted },
                    Department = d
                })
                .Where(p => p.Personnel.deleted == false)
                .OrderByDescending(pd => pd.Personnel.name).ToList();
        }

        //Get Personnel Detail
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "admin,planning,quality,operation")]
        public IHttpActionResult getDetail(int id)
        {
            Personnel personel = db.personnels.FirstOrDefault(p => p.id == id);
            if (personel == null) return NotFound();
            return Ok(personel);
        }


        //Add Personnel
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "admin")]
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
                ExceptionHandler.Handle(e);
            }
            return Ok(personnel);
        }

        //Update Personnel
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "admin")]
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
            personnelDetail.state = personnel.state;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(personnel);
        }

        //Delete Personnel
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult delete(int id)
        {
            Personnel p = db.personnels.Find(id);
            if (p == null) return NotFound();
            p.deleted = true;
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
