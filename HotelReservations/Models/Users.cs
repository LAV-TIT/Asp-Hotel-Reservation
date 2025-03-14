
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace HotelReservations.Models
{
	public class Users
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public required string UserName { get; set; }
		public required string Email { get; set; }
		public string? Avatar { get; set; }

		[StringLength(60)]
		public required string Password { get; set; }
		public bool Status { get; set; } = false;
		public DateTime? Created { get; set; }
		public DateTime? Modified { get; set; }
		public string? RoleName { get; set; } = string.Empty;
	}
}
