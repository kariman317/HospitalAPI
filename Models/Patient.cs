using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Management.System.Models
{
    public class Patient : IdentityUser
    {


        public int Age { get; set; }

        public string PatientHistory { get; set; }



        public virtual ICollection<Appointment> Appointments { get; set; }

    }
}
