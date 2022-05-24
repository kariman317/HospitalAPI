using System.ComponentModel.DataAnnotations;

namespace Hospital.Management.System.DTO
{
    public class ResetPasswordTokenDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
