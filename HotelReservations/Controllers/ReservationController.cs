using HotelReservations.Data;
using HotelReservations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelReservations.Controllers
{
    public class ReservationController : Controller
    {
        private readonly DataContext _context;
        public ReservationController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string search, int pageIndex = 1, int pageSize = 4)
        {
            // Fetch all rooms ordered by RoomId
            var ReserQuery = _context.Reservations
                .OrderByDescending(e => e.ReservationId)
                .AsNoTracking();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                ReserQuery = ReserQuery
                    .Where(r =>
                        r.Customer.ToString().ToLower().Contains(search.ToLower())
                    ); 
            }

            // Paginate the results
            var paginatedBooking = await PaginatedList<Reservation>.CreateAsync(ReserQuery, pageIndex, pageSize);
            
            ViewBag.Search = search;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = paginatedBooking.TotalPages;

            var customer = _context.Customers
               .Select(d => new SelectListItem
               {
                   Value = d.CustomerId.ToString(), 
                   Text = d.FirstName +" "+ d.LastName,
               })
               .ToList();
            ViewBag.Customers = customer;

            return View(paginatedBooking);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("Income", "Expense", "IdCard", "PasswordNo")]
        public ActionResult Create(Reservation reser, IFormFile photo)
        {
            
            return View(reser);
        }
    }
}