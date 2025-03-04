using HotelReservations.Data;
using Microsoft.AspNetCore.Mvc;
using HotelReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelReservations.Controllers
{
	public class CustomerController : Controller
	{
		private readonly DataContext _context;
		public CustomerController(DataContext context)
		{
			_context = context;
		}
        public async Task<IActionResult> Index(string search, int pageIndex = 1, int pageSize = 4)
        {
            // Fetch all Employees ordered by RoomId
            var custsQuery = _context.Customers
                .OrderByDescending(e => e.CustomerId).AsTracking();

            if (!string.IsNullOrEmpty(search))
            {
                custsQuery = custsQuery
                    .Where(r =>
                        r.FirstName.ToLower().Contains(search.ToLower().Trim())
                        || r.FirstName.ToLower().Contains(search.ToLower().Trim())
                        || r.Gender.ToLower().Contains(search.ToLower().Trim())
                        || r.Email.ToLower().Contains(search.ToLower().Trim())
                        || r.Address.ToLower().Contains(search.ToLower().Trim())
                        || r.Phone.ToString().ToLower().Contains(search.ToLower().Trim())
                    );
            }

            // Paginate the results
            var pagination = await PaginatedList<Customer>.CreateAsync(custsQuery, pageIndex, pageSize);

            // Pass the search term and pagination info to the view
            ViewBag.Search = search;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = pagination.TotalPages;

            return View(pagination);
        }

        [HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		//[Bind("Income", "Expense", "IdCard", "PasswordNo")]
		public IActionResult Create(Customer custs)
		{
			_context.Customers.Add(custs);
			int insertedRows = _context.SaveChanges();
			return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
		}
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var custs = _context.Customers.Find(id);
			if (custs is null) return NotFound();
			return View("Edit", custs);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Customer custs)
		{
			_context.Customers.Update(custs);
			int insertedRows = _context.SaveChanges();
			return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			var custs = _context.Customers.Find(id);
			if (custs is null) return NotFound();
			return View("Details", custs);
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			var custs = _context.Customers.Find(id);
			if (custs is null) return NotFound();
			return View("Delete", custs);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		public IActionResult ConfirmDelete(int id)
		{
			var custs = _context.Customers.Find(id);
			if (custs is null) return NotFound();
			_context.Customers.Remove(custs);
			int insertedRows = _context.SaveChanges();
			return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
		}
	}
}
