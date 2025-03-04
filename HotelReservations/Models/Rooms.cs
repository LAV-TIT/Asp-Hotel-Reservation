using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
	public sealed class Rooms
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoomId { get; set; }

		[ForeignKey(nameof(RoomType))]
		public required int RoomTypeId { get; set; }
		[Required]
		public required string? RoomName { get; set; }
		public string? RoomImage { get; set; }
		public required string? Description { get; set; }
		public required int RoomSize { get; set; }
		public required int Floor { get; set; }

        [Display(Name = "Room Status")]
        public bool? Status { get; set; } = true;

		[Column(TypeName = "decimal(18, 2)")]
		public decimal PricePerNight { get; set; }

		[StringLength(500)]
		public string? Amenities { get; set; }

		public int Capacity { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation property for RoomType
		public RoomType?RoomType { get; set; }
	}
}
