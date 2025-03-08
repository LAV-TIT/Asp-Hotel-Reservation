using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservations.Models
{
    public sealed class Department
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        // Collection Navigation Property
        public ICollection<Employee> Employees { get; set; } = [];
    }
}
