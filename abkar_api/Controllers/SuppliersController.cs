﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/suppliers")]
    public class SuppliersController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Suppliers
        [Route("")]
        [HttpGet]
        public List<Suppliers> getSuppliers()
        {
            return db.suppliers.OrderBy(s => s.name).ToList();
        }

        //Get Supplier Detail
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult detail(int id)
        {
            Suppliers supplier = db.suppliers.FirstOrDefault(s => s.id == id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        //Add Supplier
        [Route("")]
        [HttpPost]
        public IHttpActionResult add([FromBody] Suppliers supplier)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.suppliers.Add(supplier);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok(supplier);
        }

        //Update Supplier
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult update([FromBody] Suppliers supplierDetail, int id)
        {
            Suppliers supplier = db.suppliers.Find(id);
            if (supplier == null) return NotFound();

            supplier.company = supplierDetail.company;
            supplier.adress = supplierDetail.adress;
            supplier.city = supplierDetail.city;
            supplier.town = supplierDetail.town;
            supplier.phone = supplierDetail.phone;
            supplier.email = supplierDetail.email;
            supplier.name = supplierDetail.name;
            supplier.lastname = supplierDetail.lastname;
            supplier.updated_date = DateTime.Now;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

                ExceptionController.Handle(e);
            }
            return Ok(supplier);

        }

        //Delete
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult delete(int id)
        {
            Suppliers supplier = db.suppliers.Find(id);
            if (supplier == null) return NotFound();
            db.suppliers.Remove(supplier);
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
