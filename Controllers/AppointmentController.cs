using Hospital.Management.System.Interfaces;
using Hospital.Management.System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hospital.Management.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetAll()
        {
            List<Appointment> appointmentList = appointmentRepository.GetAll();
            if (appointmentList == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(appointmentList);
        }


        [HttpGet("{id:int}", Name = "getAppointment")]
        [Authorize(Roles = "admin")]

        public IActionResult GetByID(int id)
        {
            Appointment appointment = appointmentRepository.GetById(id);
            if (appointment == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(appointment);
        }


        [HttpGet("PatientID/{id:alpha}")]
        [Authorize(Roles = "admin")]
        public IActionResult GetByPatientID(string id)
        {
            Appointment appointment = appointmentRepository.GetByPatientID(id);
            if (appointment == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(appointment);

        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult New(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appointmentRepository.Insert(appointment);
                    string url = Url.Link("getAppointment", new { id = appointment.ID });
                    return Created(url, appointment);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);

        }


        [HttpPut("{id:int}")]
        [Authorize]

        public IActionResult Edit(int id, Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appointmentRepository.Update(id, appointment);

                    return Ok(appointment);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);

        }
        [HttpPut("ConfirmAppointment/{id:int}")]
        [Authorize]
        public IActionResult Confirm(int id)
        {
            try
            {
                 appointmentRepository.Confirm(id);
                return Ok("Confirmed Appointment");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                  
                  int test =  appointmentRepository.Delete(id);
                    if (test == 0)
                    {
                        return BadRequest("Not Allowed to deleted");

                    }
                    return Ok("Data deleted");
            }
            return BadRequest(ModelState);

        }
    }
}

