using System.ComponentModel.DataAnnotations;

namespace Hospital.Management.System.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "UserName is reuired")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is reuired")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is reuired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is reuired")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        public int Age { get; set; }

        public string PatientHistory { get; set; }


    }
}
