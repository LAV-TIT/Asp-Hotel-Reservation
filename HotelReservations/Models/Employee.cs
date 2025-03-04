using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservations.Models
{
    public sealed class Employee: BaseEntity
    {
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int EmployeeId { get; set; }

        [ForeignKey(nameof(Department))]
		public int DepartmentId { get; set; }
		public decimal? Salary { get; set; } = 0;
        public bool? IsActive { get; set; } = true;
        public DateTime? HireDate { get; set; }
        public string? Position { get; set; }

        // Department Referance beacuse one emp has one Department
        public Department?Department { get; set; }
    }
}
