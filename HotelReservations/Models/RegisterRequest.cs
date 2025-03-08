using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public sealed class RegisterRequest
    {
        [Required(ErrorMessage = "{0} is needed.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "{0} is needed.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "{0} is needed.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} is at lease {2}")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "{0} is needed.")]
        [Compare(nameof(Password), ErrorMessage = "{1} is not match with {1}")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} is at lease {2}")]
        public required string ConfirmPassword { get; set; }
        public string? RoleName { get; set; }
    }
}
