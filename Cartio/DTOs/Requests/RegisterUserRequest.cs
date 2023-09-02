using System.ComponentModel.DataAnnotations;

namespace Cartio.DTOs.Requests
{
    public class RegisterUserRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string FullName { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; } = null!;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
