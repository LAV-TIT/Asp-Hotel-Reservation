using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public sealed class ReservationDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationDetailId { get; set; } // Primary key

        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; } // Foreign key to Reservation

        [ForeignKey(nameof(Rooms))]
        public int RoomId { get; set; } // Foreign key to Room

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceAtBooking { get; set; } // Price of the room at the time of booking

        [Required]
        public int NumberOfNights { get; set; } // Number of nights the room is booked

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Subtotal { get; set; } // PriceAtBooking * NumberOfNights

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Reservation? Reservation { get; set; } // Link to the reservation
        public Rooms? Rooms { get; set; } // Link to the room

    }
}
