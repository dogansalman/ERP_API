﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using abkar_api.Models;
using abkar_api.Contexts;

namespace abkar_api.Controllers
{
    [RoutePrefix("api/machines")]
    public class MachinesController : ApiController
    {
        DatabaseContext db = new DatabaseContext();

        //Get Machines
        [HttpGet]
        [Route("")]
        public List<Machines> get()
        {
            return db.machines.OrderByDescending(c => c.id).ToList();
        }

        //Get Machine
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult detail(int id)
        {
            Machines machine = db.machines.FirstOrDefault(p => p.id == id);
            if (machine == null) return NotFound();
            return Ok(machine);
        }

        //Add Machine
        [HttpPost]
        [Route("")]
        public IHttpActionResult add([FromBody] Machines machine)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.machines.Add(machine);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok(machine);
        }

        //Update Operation
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult update([FromBody] Machines machine, int id)
        {
            Machines machineDetail = db.machines.Find(id);
            if (machineDetail == null) return NotFound();
            machineDetail.name = machine.name;
            machineDetail.updated_date = DateTime.Now;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ExceptionController.Handle(e);
            }
            return Ok(machine);
        }

        //Delete Operation
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult delete(int id)
        {
            Machines machine = db.machines.Find(id);
            if (machine == null) return NotFound();
            db.machines.Remove(machine);
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
