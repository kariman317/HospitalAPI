using Hospital.Management.System.Interfaces;
using Hospital.Management.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Management.System.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly Context context;

        public AppointmentRepository(Context context)
        {
            this.context = context;
        }
        public AppointmentRepository()
        {
            Id = Guid.NewGuid();

        }
        public Guid Id { get; set; }

        public List<Appointment> GetAll()
        {
            return context.Appointments.ToList();
        }

        public Appointment GetById(int id)
        {
            return context.Appointments.FirstOrDefault(x => x.ID == id);
        }
     
        public Appointment GetByPatientID(string id)
        {
            return context.Appointments.FirstOrDefault(x => x.PatientId == id);
        }

        public int Insert(Appointment appointment)
        {
            context.Appointments.Add(appointment);
            return context.SaveChanges();
        }

        public int Update(int id, Appointment appointment)
        {
            Appointment oldAppointment = GetById(id);
            if (oldAppointment != null)
            { 
                oldAppointment.AppointmentTime = appointment.AppointmentTime;
                oldAppointment.Notes = appointment.Notes;
                return context.SaveChanges();
            }
            return 0;
        }
        public int Confirm(int id)
        {
            Appointment oldAppointment = GetById(id);
            if (oldAppointment != null)
            {
                oldAppointment.Confirmed = true;
                return context.SaveChanges();
            }
            return 0;
        }
        public int Delete(int id)
        {
            Appointment oldAppointment = GetById(id);
            if (oldAppointment.AppointmentTime.Date == DateTime.Now.Date)
            {
                //want to display message
                return 0;
            }
            context.Appointments.Remove(oldAppointment);
            return context.SaveChanges();
        }
    }
}
