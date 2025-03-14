using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
	public sealed class Customer : BaseEntity
	{
		[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CustomerId { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

    }
}
