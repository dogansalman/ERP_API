using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using abkar_api.Contexts;
using abkar_api.Libary.Identity;
using abkar_api.Models;
using abkar_api.ModelViews;

namespace abkar_api.Controllers.SupplierController
{
    [RoutePrefix("api/supplier/company")]
    public class SupplierController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "supplier")]
        public object Get()
        {
            int supplier_id = Identity.get(User);
            var supplier = db.suppliers.Where(s => s.id == supplier_id).FirstOrDefault();
            if (supplier == null) return NotFound();
            return supplier;

        }

        [HttpPut]
        [Route("")]
        [Authorize(Roles = "supplier")]
        public object Put([FromBody] Suppliers supplierForm)
        {
            int supplier_id = Identity.get(User);
            var supplier = db.suppliers.Where(s => s.id == supplier_id).FirstOrDefault();
            if (supplier == null) return NotFound();

            supplier.adress = supplierForm.adress;
            supplier.city = supplierForm.city;
            supplier.company = supplierForm.company;
            supplier.email = supplierForm.email;
            supplier.lastname = supplierForm.lastname;
            supplier.name = supplierForm.name;
            supplier.phone = supplierForm.phone;
            supplier.town = supplierForm.town;
            supplier.updated_date = DateTime.Now;
            db.SaveChanges();
            return Ok(supplierForm);
        }

        [HttpPut]
        [Route("password")]
        [Authorize(Roles = "supplier")]
        public IHttpActionResult password(_ComPassword supplierPassword)
        {
            int supplier_id = Identity.get(User);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Suppliers c = db.suppliers.Find(supplier_id);
            if (c == null) return NotFound();
            if (supplierPassword.newPassword != supplierPassword.reply || c.password != supplierPassword.password) return BadRequest();

            c.password = supplierPassword.newPassword;
            db.SaveChanges();

            return Ok(supplierPassword);

        }
    }
}
