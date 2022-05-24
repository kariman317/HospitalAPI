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
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        [HttpGet]
        
        [Authorize(Roles ="admin")]
        public IActionResult GetAll()
        {
            List<Patient> patientList = patientRepository.GetAll();
            if (patientList == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(patientList);
        }

        [Authorize(Roles = "admin")]

        [HttpGet("{id:alpha}", Name = "getPatient")]
        public IActionResult GetByID(string id)
        {
            Patient patient = patientRepository.GetById(id);
            if (patient == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(patient);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{Name:alpha}")]
        public IActionResult GetByName(string Name)
        {
            Patient patient = patientRepository.GetByName(Name);
            if (patient == null)
            {
                return BadRequest("Not Found");
            }
            return Ok(patient);

        }





        [HttpPut("{id:alpha}")]
        [Authorize]

        public IActionResult Edit(string id, Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    patientRepository.Update(id, patient);

                    return Ok(patient);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);

        }


        [HttpDelete]
        [Authorize]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    patientRepository.Delete(id);

                    return StatusCode(204, "Data deleted");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);

        }
    }
}
