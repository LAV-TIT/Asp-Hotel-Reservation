using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public sealed class LoginRequest
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} Is needed.")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "{0} Is needed.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Display(Name = "Number Me")]
        public bool RememberMe { get; set; } = false;
    }
}
