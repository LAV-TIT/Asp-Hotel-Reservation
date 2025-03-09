using HotelReservations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelReservations.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelReservations.Controllers
{
	public class DepartmentController : Controller
	{
		// GET: DepartmentController
		private readonly DataContext _context;
		public DepartmentController(DataContext dataContext)
		{
            _context = dataContext;
		}
		
        public async Task<IActionResult> Index(string search, int pageIndex = 1, int pageSize = 4)
        {
            // Fetch all Employees ordered by RoomId
            var deptsQuery = _context.Departments.OrderByDescending(e => e.DepartmentId).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                deptsQuery = deptsQuery
                    .Where(r =>
                        r.DepartmentName.ToLower().Contains(search.ToLower().Trim())
                        || r.Description.ToLower().Contains(search.ToLower().Trim())
                    );
            }

            // Paginate the results
            var pagination = await PaginatedList<Department>.CreateAsync(deptsQuery, pageIndex, pageSize);

            // Pass the search term and pagination info to the view
            ViewBag.Search = search;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = pagination.TotalPages;
            return View(pagination);
        }

        //GET: DepartmentController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind("DepartmentName", "Description")] Department dept)
		{
            if (ModelState.IsValid)
            {
                
                // Add the new department to the database
                _context.Departments.Add(dept);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); // Redirect to the index page after creation
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            
            return View(dept);

        }
        public IActionResult Edit(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept is null) return NotFound();
            return View("Edit", dept);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department dept)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(dept);
                _context.SaveChanges();
          
                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        public IActionResult Delete(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(Department dept)
        {

            if (ModelState.IsValid)
            {
                _context.Departments.Remove(dept);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }
    }
}
