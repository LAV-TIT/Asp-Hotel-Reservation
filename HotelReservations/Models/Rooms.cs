using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
	public sealed class Rooms
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoomId { get; set; }

		[ForeignKey(nameof(RoomType))]
		public int RoomTypeId { get; set; }
		[Required]
		public string? RoomName { get; set; }
		public string? RoomImage { get; set; }
        [Required]
        public string? Description { get; set; }
		public int RoomSize { get; set; }
		public int Floor { get; set; }

        [Display(Name = "Room Status")]
        [Required]
        public string? Status { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		public decimal PricePerNight { get; set; }

		[StringLength(500)]
		public string? Amenities { get; set; }

		public int Capacity { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		// Navigation property for RoomType
		public RoomType?RoomType { get; set; }
        public ICollection<ReservationDetails>? ReservationDetails { get; set; }
    }
}
