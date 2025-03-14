using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models.ViewModels
{
    public class ReservationViewModel
    {
        // Customer Information
        [Required]
        public int CustomerId { get; set; }

        // Room Information
        [Required]
        public int RoomId { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        // Reservation Details
        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int NumberOfNights { get; set; }

        [Required]
        public decimal PriceAtBooking { get; set; }

        // Additional fields if needed
        public string? SpecialRequests { get; set; }

        // Lists for dropdowns
        public List<Customer>? Customers { get; set; }
        public List<Rooms>? Rooms { get; set; }
        public List<RoomType>? RoomTypes { get; set; }
    }
}
