using System.ComponentModel.DataAnnotations;

namespace Hospital.Management.System.DTO
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Email is Requied")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage = "NewPassword is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmNewPassword is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }

    }
}
