using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hospital.Management.System.Models
{
    public class Appointment
    {
        public int ID { get; set; }
        public DateTime AppointmentTime  { get; set; }
        public string Notes { get; set; }

        public bool Confirmed { get; set; } = false;


        [ForeignKey("Patient")]
        public string PatientId { get; set; }

      
        public virtual Patient Patient { get; set; }
    }
}
