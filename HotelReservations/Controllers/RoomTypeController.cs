using HotelReservations.Data;
using HotelReservations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace HotelReservations.Controllers
{
	public class RoomTypeController : Controller
	{
        private readonly DataContext _context;
        public RoomTypeController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int pageIndex = 1, int pageSize = 4)
        {
            // Fetch all rooms ordered by RoomId
            var roomsTypesQuery = _context.RoomTypes
                .OrderByDescending(e => e.RoomTypeId).AsTracking();

            if (!string.IsNullOrEmpty(search))
            {
                roomsTypesQuery = roomsTypesQuery
                    .Where(r =>
                        r.RoomTypeName.ToLower().Contains(search.ToLower()) || (r.Beds.ToString() != null)
                    );
            }

            // Paginate the results
            var paginatedRoomsType = await PaginatedList<RoomType>.CreateAsync(roomsTypesQuery, pageIndex, pageSize);

            // Pass the search term and pagination info to the view
            ViewBag.Search = search;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = paginatedRoomsType.TotalPages;
            return View(paginatedRoomsType);
        }

        public IActionResult Create()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(RoomType roomtype, IFormFile photo)
		{
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (photo != null && photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "images", "roomtype");
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

                    roomtype.RoomTypeImage = Path.Combine("images", "roomtype", uniqueFileName); // Save the relative path
                }

                _context.RoomTypes.Add(roomtype);
                int insertedRows = _context.SaveChanges();
                return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
            }

			return View(roomtype); // Return the view with validation errors
		}

		public IActionResult Edit(int id)
        {
            var roomtype = _context.RoomTypes.Find(id);
            if (roomtype is null) return NotFound();
            return View("Edit", roomtype);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(RoomType roomtype, IFormFile photo)
		{
            var existingRoom = _context.RoomTypes.AsNoTracking().FirstOrDefault(r => r.RoomTypeId == roomtype.RoomTypeId);
            if (existingRoom != null)
            {
                roomtype.RoomTypeImage = existingRoom.RoomTypeImage;
            }

            // Handle file upload if a new image is provided
            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "roomtype");
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
                if (!string.IsNullOrEmpty(roomtype.RoomTypeImage))
                {
                    var oldImagePath = Path.Combine("wwwroot", roomtype.RoomTypeImage.TrimStart('/'));
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
                roomtype.RoomTypeImage = Path.Combine("images", "roomtype", uniqueFileName);
            }

            var roomtypes = _context.RoomTypes
                .Select(d => new SelectListItem
                {
                    Value = d.RoomTypeId.ToString(), // RoomTypeId as the value
                    Text = d.RoomTypeName  // RoomTypeName as the display text
                }).AsTracking().ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.roomTypes = roomtypes;

            // Update the room in the database
            _context.RoomTypes.Update(roomtype);
            int updatedRows = _context.SaveChanges();
            return updatedRows > 0 ? RedirectToAction(nameof(Index)) : View(roomtype);

        }

		[HttpGet]
        public IActionResult Details(int id)
        {
            var roomtype = _context.RoomTypes.Find(id);
            if (roomtype is null) return NotFound();
            return View("Details", roomtype);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var roomtype = _context.RoomTypes.Find(id);
            if (roomtype is null) return NotFound();
            return View("Delete", roomtype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var roomtype = _context.RoomTypes.Find(id);
            if (roomtype is null) return NotFound();

            // Delete the associated image file
            if (!string.IsNullOrEmpty(roomtype.RoomTypeImage))
            {
                var filePath = Path.Combine("wwwroot", roomtype.RoomTypeImage);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.RoomTypes.Remove(roomtype);
            int insertedRows = _context.SaveChanges();
            return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
        }
    }
}
