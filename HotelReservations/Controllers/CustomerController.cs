using HotelReservations.Data;
using Microsoft.AspNetCore.Mvc;
using HotelReservations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public ActionResult Create(Customer custs, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (photo != null && photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "images", "customers");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = "upl_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + Path.GetFileName(photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }

                    custs.Avatar = Path.Combine("images", "customers", uniqueFileName); // Save the relative path
                }

                _context.Customers.Add(custs);
                int insertedRows = _context.SaveChanges();
                return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
            }
            return View(custs);
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
        public IActionResult Edit(Customer custs, IFormFile photo)
        {
            // Retrieve the existing room to ensure RoomImage is populated
            var existingRoom = _context.Customers.AsNoTracking().FirstOrDefault(r => r.CustomerId == custs.CustomerId);
            if (existingRoom != null)
            {
                custs.Avatar = existingRoom.Avatar;
            }

            // Handle file upload if a new image is provided
            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "customers");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name
                var uniqueFileName = "upl_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_" + Path.GetFileName(photo.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the new image
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }

                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(custs.Avatar))
                {
                    var oldImagePath = Path.Combine("wwwroot", custs.Avatar.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                        Console.WriteLine("Old image deleted successfully."); // Debugging
                    }
                    else
                    {
                        Console.WriteLine("Old image does not exist."); // Debugging
                    }
                }

                // Update the room's image path
                custs.Avatar = Path.Combine("images", "customers", uniqueFileName);
            }

           
            // Update the room in the database
            _context.Customers.Update(custs);
            int updatedRows = _context.SaveChanges();
            return updatedRows > 0 ? RedirectToAction(nameof(Index)) : View(custs);

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
            // Delete the associated image file
            if (!string.IsNullOrEmpty(custs.Avatar))
            {
                var filePath = Path.Combine("wwwroot", custs.Avatar);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Customers.Remove(custs);
			int insertedRows = _context.SaveChanges();
			return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
		}
	}
}
