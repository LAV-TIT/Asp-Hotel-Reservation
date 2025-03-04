using HotelReservations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelReservations.Models;
using Microsoft.EntityFrameworkCore;

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
		public IActionResult Index()
		{
			var depts = _context.Departments.OrderByDescending(e => e.DepartmentId).ToList();
			return View(depts);
		}

		// GET: DepartmentController/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: DepartmentController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind("DepartmentName", "Description")] Department dept)
		{
            _context.Departments.Add(dept);
            int insertedRows = _context.SaveChanges();
            return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
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
            _context.Departments.Update(dept);
            int insertedRows = _context.SaveChanges();
            return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept is null) return NotFound();
            return View("Details", dept);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept is null) return NotFound();
            return View("Delete", dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept is null) return NotFound();
            _context.Departments.Remove(dept);
            int insertedRows = _context.SaveChanges();
            return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
        }
    }
}
