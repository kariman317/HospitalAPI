using Hospital.Management.System.Models;
using System;
using System.Collections.Generic;

namespace Hospital.Management.System.Interfaces
{
    public interface IAppointmentRepository
    {
        Guid Id { get; set; }
        List<Appointment> GetAll();
        Appointment GetById(int id);
        public Appointment GetByPatientID(string id);
        int Insert(Appointment appointment);
        int Update(int id, Appointment appointment);

        public int Confirm(int id);
        int Delete(int id);
    }
}
