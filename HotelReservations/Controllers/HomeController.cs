using HotelReservations.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservations.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        public HomeController(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IActionResult Index()
        {
			// Employees
			int totalEmployees = _context.Employees.Count();
			// Fetch the number of active employees (assuming you have a property like IsActive)
			int activeEmployees = _context.Employees.Count(e => (bool)e.IsActive);

			// Calculate the percentage of active employees
			double activeEmployeePercentage = totalEmployees > 0 ? (double)activeEmployees / totalEmployees * 100 : 0;

			ViewBag.TotalEmployees = totalEmployees;
			ViewBag.TotalempsAcive = activeEmployees;
			ViewBag.ActiveEmployeePercentage = activeEmployeePercentage;

			// Rooms

			int roomCount = _context.Rooms.Count();
			int activeRooms = _context.Rooms.Count(e => (bool)e.Status);
			double roomCountPercentage = roomCount > 0 ? (double) activeRooms / totalEmployees * 100 : 0;
			ViewBag.TotalRooms = roomCount;
			ViewBag.RoomCount = roomCountPercentage;
			ViewBag.TotalRooms = roomCount;
			ViewBag.TotalRoomsActive = activeRooms;


			// Customer
			int custsCount = _context.Customers.Count();
			int custsSctive = _context.Customers.Count();
			double custsCountPercentage = roomCount > 0 ? (double)custsSctive / custsCount * 100 : 0;
			ViewBag.CustsTotal = custsCount;
			ViewBag.CustsCountPercentage = custsCountPercentage;
			ViewBag.CustsActiveTotal = custsSctive;






			return View();

			
		}
        public IActionResult Dashboard()
        {
			// Fetch the total number of employees from the database
			return View();
		}
    }
}
