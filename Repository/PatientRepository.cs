using Hospital.Management.System.Interfaces;
using Hospital.Management.System.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Management.System.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly Context context;

        public PatientRepository(Context context)
        {
            this.context = context;
        }
        public PatientRepository()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id 
        { get; set; }     

        public List<Patient> GetAll()
        {
            return context.Patients.ToList();

        }

        public Patient GetById(string id)
        {
            return context.Patients.Include(d => d.Appointments).
                FirstOrDefault(p => p.Id == id);
        }

        public Patient GetByName(string Name)
        {
            return context.Patients.Include(d => d.Appointments).
                FirstOrDefault(x => x.UserName == Name);
        }

        public int Insert(Patient patient)
        {
            context.Patients.Add(patient);
            return context.SaveChanges();
        }

        public int Update(string id, Patient patient)
        {
            Patient oldPatient = GetById(id);
            if (oldPatient != null)
            {
                oldPatient.UserName = patient.UserName;
                oldPatient.Age = patient.Age;
                oldPatient.PatientHistory = patient.PatientHistory;
                return context.SaveChanges();
            }
            return 0;
        }
        public int Delete(string id)
        {
            Patient oldPatient = GetById(id);
            context.Patients.Remove(oldPatient);
            return context.SaveChanges();
        }
    }
}
