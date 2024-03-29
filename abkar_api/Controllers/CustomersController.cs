﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;
using abkar_api.Libary.ExceptionHandler;
using System.Security.Claims;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
    
        DatabaseContext db = new DatabaseContext();

        //Get Customers
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "admin,planning")]
        public List<Customers> getCustomers()
        {
            return db.customers.Where(c => c.deleted == false).OrderByDescending(c => c.id).ToList();
        }

        //Get Customer Detail
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult getCustomer(int id)
        {
            Customers customer = db.customers.FirstOrDefault(p => p.id == id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        //Add Customer
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult add([FromBody] Models.Customers customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.customers.Add(customer);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionHandler.Handle(e);
            }
            return Ok(customer);
        }

  
        //Update Customer
        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("{id}")]
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
            customerDetail.state = customer.state;
            customerDetail.tax_name = customer.tax_name;
            customerDetail.tax_number = customer.tax_number;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e) 
            {
                ExceptionHandler.Handle(e);
            }
          
            return Ok(customerDetail);
        }
        
        //Delete customer
        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("{id}")]
        public IHttpActionResult delete(int id)
        {
            Customers customer = db.customers.Find(id);
            if(customer == null ) return NotFound();
            customer.deleted = true;         
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

        /*
         * Customer Order Monitoring 
         * 
         */
        //Get Customers
        [HttpGet]
        [Route("detail")]
        [Authorize]
        public object customerDetail()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            int id = int.Parse(claimsIdentity.FindFirst("id").Value);
            var cc = (from c in db.customers
                      where c.id == id
                      select new
                      {
                          company = c.company,
                          adress = c.adress,
                          email = c.email,
                          name =c.name,
                          lastname = c.lastname,
                          phone = c.phone,
                          created_date = c.created_date,
                          updated_date = c.updated_date,
                      }
                      ).FirstOrDefault();
            
            if (cc == null)  NotFound();
            return cc;
        }
    }



    
}
