using Hospital.Management.System.Models;
using System;
using System.Collections.Generic;

namespace Hospital.Management.System.Interfaces
{
    public interface IPatientRepository
    {
  
        List<Patient> GetAll();
        Patient GetById(string id);
        public Patient GetByName(string Name);

        int Insert(Patient patient);
        int Update(string id, Patient patient);
        int Delete(string id);
       
    }
}
