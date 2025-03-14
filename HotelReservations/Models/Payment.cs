using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; } // Primary Key

        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; } // Foreign Key to Reservation

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; } // Amount paid

        [Required, StringLength(50)]
        public required string PaymentMethod { get; set; } // e.g., Credit Card, Cash

        [Required, StringLength(50)]
        public required string PaymentStatus { get; set; } // e.g., Paid, Pending

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Reservation? Reservation { get; set; }
    }
}
