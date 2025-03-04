using HotelReservations.Data;
using HotelReservations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace HotelReservations.Controllers
{
	public class RoomController : Controller
	{
		private readonly DataContext _context;
		public RoomController(DataContext dataContext)
		{
			_context = dataContext;
		}
        [HttpGet]
		public async Task<IActionResult> Index(string search, int pageIndex = 1, int pageSize = 4)
		{
            // Fetch all rooms ordered by RoomId
            var roomsQuery = _context.Rooms
                .OrderByDescending(e => e.RoomId)
                .Include(e => e.RoomType)
                .AsTracking();

            if (!string.IsNullOrEmpty(search))
            {
                roomsQuery = roomsQuery
                    .Where(r =>
                        r.RoomName.ToLower().Contains(search.ToLower()) || (r.RoomType != null // Search by RoomTypeName
                        && r.RoomType.RoomTypeName.ToLower().Contains(search.ToLower())) || (r.Floor != null  // Search by RoomTypeName
                        && r.Floor.ToString().ToLower().Contains(search.ToLower())) // Search by Floor
                    );
            }

            // Paginate the results
            var paginatedRooms = await PaginatedList<Rooms>.CreateAsync(roomsQuery, pageIndex, pageSize);

            // Pass the search term and pagination info to the view
            ViewBag.Search = search;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = paginatedRooms.TotalPages;

            return View(paginatedRooms);
        }

		[HttpGet]
		public IActionResult Create()
		{
            var roomtypes = _context.RoomTypes
                .Select(d => new SelectListItem
                {
                    Value = d.RoomTypeId.ToString(), // RoomTypeId as the value
                    Text = d.RoomTypeName  // RoomTypeName as the display text
                })
                .ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.roomTypes = roomtypes;

            return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Rooms rooms, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (photo != null && photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "images", "rooms");
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

                    rooms.RoomImage = Path.Combine("images", "rooms", uniqueFileName); // Save the relative path
                }

                _context.Rooms.Add(rooms);
                int insertedRows = _context.SaveChanges();
                return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
            }

            // If the model state is invalid, re-populate the roomtype dropdown
            var roomtypes = _context.RoomTypes
                .Select(d => new SelectListItem
                {
                    Value = d.RoomTypeId.ToString(), // RoomTypeId as the value
                    Text = d.RoomTypeName  // RoomTypeName as the display text
                })
                .ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.roomTypes = roomtypes;
            return View(rooms);
        }

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var rooms = _context.Rooms.Find(id);
			if (rooms is null) return NotFound();

            // Fetch the list of roomtype from the database
            var roomtypes = _context.RoomTypes
                .Select(d => new SelectListItem
                {
                    Value = d.RoomTypeId.ToString(), // RoomTypeId as the value
                    Text = d.RoomTypeName  // RoomTypeName as the display text
                })
                .ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.roomTypes = roomtypes;
            return View(rooms);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Rooms rooms, IFormFile photo)
        {
			// Retrieve the existing room to ensure RoomImage is populated
			var existingRoom = _context.Rooms.AsNoTracking().FirstOrDefault(r => r.RoomId == rooms.RoomId);
			if (existingRoom != null)
			{
				rooms.RoomImage = existingRoom.RoomImage;
			}

			// Handle file upload if a new image is provided
			if (photo != null && photo.Length > 0)
			{
				var uploadsFolder = Path.Combine("wwwroot", "images", "rooms");
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
				if (!string.IsNullOrEmpty(rooms.RoomImage))
				{
					var oldImagePath = Path.Combine("wwwroot", rooms.RoomImage.TrimStart('/'));
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
				rooms.RoomImage = Path.Combine("images", "rooms", uniqueFileName);
			}

			var roomtypes = _context.RoomTypes
				.Select(d => new SelectListItem
				{
					Value = d.RoomTypeId.ToString(), // RoomTypeId as the value
					Text = d.RoomTypeName  // RoomTypeName as the display text
				})
				.ToList();

			// Pass the departments to the view using ViewBag
			ViewBag.roomTypes = roomtypes;

			// Update the room in the database
			_context.Rooms.Update(rooms);
			int updatedRows = _context.SaveChanges();
			return updatedRows > 0 ? RedirectToAction(nameof(Index)) : View(rooms);
		}

		//===========

		[HttpGet]
		public IActionResult Details(int id)
		{
            var rooms = _context.Rooms.Find(id);
			if (rooms is null) return NotFound();
            var roomtypes = _context.RoomTypes
                .Select(d => new SelectListItem
                {
                    Value = d.RoomTypeId.ToString(), // RoomTypeId as the value
                    Text = d.RoomTypeName  // RoomTypeName as the display text
                })
                .ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.roomTypes = roomtypes;
            return View("Details", rooms);

        }

		[HttpGet]
		public IActionResult Delete(int id)
		{
            var rooms = _context.Rooms.Find(id);

            if (rooms is null) return NotFound();

            var roomtypes = _context.RoomTypes
                .Select(d => new SelectListItem
                {
                    Value = d.RoomTypeId.ToString(), // RoomTypeId as the value
                    Text = d.RoomTypeName  // RoomTypeName as the display text
                })
                .ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.roomTypes = roomtypes;

            return View("Delete", rooms);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		public IActionResult ConfirmDelete(int id)
		{
            var rooms = _context.Rooms.Find(id);
            if (rooms is null) return NotFound();

            // Delete the associated image file
            if (!string.IsNullOrEmpty(rooms.RoomImage))
            {
                var filePath = Path.Combine("wwwroot", rooms.RoomImage);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Rooms.Remove(rooms);
            int insertedRows = _context.SaveChanges();
            return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();
        }
	}
}

