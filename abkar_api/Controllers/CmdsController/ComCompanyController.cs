using abkar_api.Contexts;
using abkar_api.Libary.Identity;
using abkar_api.Models;
using abkar_api.ModelViews;
using System.Linq;
using System.Web.Http;

namespace abkar_api.Controllers.ComController
{
    [RoutePrefix("api/customer/company")]

    public class ComCompanyController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "customer")]
        public object get_()
        {
            int id = Identity.get(User);
            return (
               from c in db.customers
               where c.id == id
               select new
               {
                   company = c.company,
                   adress = c.adress,
                   email = c.email,
                   phone = c.phone,
                   name = c.name,
                   lastname = c.lastname
               }
               ).FirstOrDefault();

        }
        [HttpPut]
        [Route("")]
        [Authorize(Roles = "customer")]
        public IHttpActionResult update(_ComCustomers customer)
        {
            int id = Identity.get(User);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Customers c = db.customers.Find(id);
            if (c == null) return NotFound();
            c.adress = customer.adress;
            c.company = customer.company;
            c.email = customer.email;
            c.lastname = customer.lastname;
            c.name = customer.name;
            c.phone = customer.phone;
            db.SaveChanges();

            return Ok(customer);

        }

        [HttpPut]
        [Route("password")]
        [Authorize(Roles = "customer")]
        public IHttpActionResult password(_ComPassword customerPassword)
        {
            int id = Identity.get(User);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Customers c = db.customers.Find(id);
            if (c == null) return NotFound();
            if (customerPassword.newPassword != customerPassword.reply || c.password != customerPassword.password) return BadRequest();

            c.password = customerPassword.newPassword;
            db.SaveChanges();

            return Ok();

        }
    }

}
