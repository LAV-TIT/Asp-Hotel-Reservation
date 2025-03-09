using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
	public sealed class RoomType
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoomTypeId { get; set; }
		public string? RoomTypeName { get; set; }
		public string? RoomTypeImage { get; set; }
        [Display(Name = "Status")]
        //[Required(ErrorMessage = "The Status field is required.")]
        public bool? Status { get; set; } = true;
		public string? Description { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		public decimal BasePrice { get; set; }

		public int MaxOccupancy { get; set; }

		public int Beds { get; set; }
		public string? Amenities { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation property for Rooms
		public ICollection<Rooms>?Rooms { get; set; }
	}
}
