using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public abstract class BaseEntity
    {
        public string? Avatar { get; set; }
		public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Gender { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
