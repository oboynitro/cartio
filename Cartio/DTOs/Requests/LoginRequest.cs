using System.ComponentModel.DataAnnotations;

namespace Cartio.DTOs.Requests
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; } = null!;
    }
}
