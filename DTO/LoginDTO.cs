using System.ComponentModel.DataAnnotations;

namespace Hospital.Management.System.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Email is reuired")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
