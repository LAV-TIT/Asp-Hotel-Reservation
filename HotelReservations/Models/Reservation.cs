using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public sealed class Reservation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; set; } // Primary key

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; } // Foreign key to Customer

        [Required]
        public DateTime? CheckInDate { get; set; }

        [Required]
        public DateTime? CheckOutDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalCost { get; set; } // Total cost of the reservation

        [Required]
        [StringLength(50)]
        public string? Status { get; set; } = "Pending"; // e.g., Pending, Confirmed, Cancelled, Completed

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow; // When the reservation was created

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow; // Last updated timestamp

        // Navigation properties
        public Customer? Customer { get; set; } // Link to the customer who made the reservation
        public ICollection<ReservationDetails>? ReservationDetails { get; set; } // Details of the reservation (rooms booked)
        public ICollection<Guest>? Guests { get; set; } // Link to guest
    }
}
