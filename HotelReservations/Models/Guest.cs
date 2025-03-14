using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservations.Models
{
    public sealed class Guest: BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuestId { get; set; } // Primary Key

        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; set; } // Foreign Key to Reservation

        public bool IsPrimaryGuest { get; set; } = false; // Indicates if this is the primary guest
        // Navigation property
        public Reservation? Reservation { get; set; }
    }

}
