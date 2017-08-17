using abkar_api.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using abkar_api.Contexts;
using System.Linq;
namespace abkar_api.Controllers
{
    public class CustomersController : ApiController
    {
        static readonly Customers customerRepos = new Customers();

        DatabaseContext db = new DatabaseContext();
        
        //get customers
        [HttpGet]
        public List<Customers> get()
        {
            return db.customers.ToList();
        }

        //add customer
        [HttpPost]
        public IHttpActionResult add([FromBody] Customers customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.customers.Add(customer);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok(customer);
        }

        //add customer
        [HttpPost]
        public IHttpActionResult addd()
        {
            
            return Ok();
        }

        //update customer
        [HttpPut]
        public IHttpActionResult update([FromBody] Customers customer, int id)
        {
            Customers customerDetail = db.customers.Find(id);
            if (customerDetail == null) return NotFound();
            customerDetail.company = customer.company;
            customerDetail.adress = customer.adress;
            customerDetail.city = customer.city;
            customerDetail.email = customer.email;
            customerDetail.lastname = customer.lastname;
            customerDetail.name = customer.name;
            customerDetail.password = customer.password;
            customerDetail.phone = customer.phone;
            customerDetail.updated_date = DateTime.Now;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e) 
            {
                ExceptionController.Handle(e);
            }
          
            return Ok(customerDetail);
        }
        //delete customer
        [HttpDelete]
        public IHttpActionResult delete(int id)
        {
            Customers customer = db.customers.Find(id);
            if(customer == null ) return NotFound();
            db.customers.Remove(customer);            
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
