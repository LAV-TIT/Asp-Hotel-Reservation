using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public abstract class BaseEntity
    {
        public string? Avatar { get; set; }

        [Required, StringLength(100)]
        public string? FirstName { get; set; }

        [Required, StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        public string? Address { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
