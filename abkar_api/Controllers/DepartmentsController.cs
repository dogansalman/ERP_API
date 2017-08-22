using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/departments")]
    public class DepartmentsController : ApiController
    {
        DatabaseContext db = new DatabaseContext();
        
        //Get Departments
        [HttpGet]
        [Route("")]
        public List<Departments> getDepartments()
        {
            return db.departments.OrderBy(d => d.name).ToList();  
        }


    }
}
