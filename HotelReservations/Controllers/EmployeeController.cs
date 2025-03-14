using HotelReservations.Data;
using HotelReservations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelReservations.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DataContext _context;
        public EmployeeController(DataContext dataContext) { 
            _context = dataContext;
        }
        
        public async Task<IActionResult> Index(string search, int? deptId, int pageIndex = 1, int pageSize = 4)
        {
            // Fetch all Employees ordered by RoomId
            var empsQuery = _context.Employees
                .OrderByDescending(e => e.EmployeeId)
                .Include(e => e.Department)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                empsQuery = empsQuery
                    .Where(r =>
                        r.FirstName.ToLower().Contains(search.ToLower().Trim()) 
                        || r.LastName.ToLower().Contains(search.ToLower().Trim())
                        || r.Gender.ToLower().Contains(search.ToLower().Trim())
                        || r.Position.ToLower().Contains(search.ToLower().Trim())
                        || r.Email.ToLower().Contains(search.ToLower().Trim())
                        || r.Address.ToLower().Contains(search.ToLower().Trim())
                        || r.Phone.ToString().ToLower().Contains(search.ToLower().Trim())
                    );
            }
            
            // Apply dempartment filter
            if (deptId.HasValue && deptId > 0)
            {
                empsQuery = empsQuery.Where(r => r.DepartmentId == deptId);
            }
            // Paginate the results
            var pagination = await PaginatedList<Employee>.CreateAsync(empsQuery, pageIndex, pageSize);

            // Pass the search term and pagination info to the view
            ViewBag.Search = search;
            ViewBag.departmentId = deptId;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = pagination.TotalPages;

            var departments = _context.Departments
             .Select(d => new SelectListItem
             {
                 Value = d.DepartmentId.ToString(), // DepartmentId as the value
                 Text = d.DepartmentName   // DepartmentName as the display text
             }).ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.Departments = departments;

            return View(pagination);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _context.Departments
             .Select(d => new SelectListItem
             {
                 Value = d.DepartmentId.ToString(), // DepartmentId as the value
                 Text = d.DepartmentName   // DepartmentName as the display text
             }).ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.Departments = departments;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee emps, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (photo != null && photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot", "images", "employees");
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

                    emps.Avatar = Path.Combine("images", "employees", uniqueFileName); // Save the relative path
                }

                _context.Employees.Add(emps);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // If the model state is invalid, re-populate the roomtype dropdown
            var departments = _context.Departments
              .Select(d => new SelectListItem
              {
                  Value = d.DepartmentId.ToString(),
                  Text = d.DepartmentName
              })
              .ToList();
            ViewBag.Departments = departments;

            return View(emps);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var emps = _context.Employees.Find(id);
            if (emps is null) return NotFound();

            // Fetch the list of departments from the database
            var departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(), // DepartmentId as the value
                    Text = d.DepartmentName  // DepartmentName as the display text
                })
                .ToList();

            // Pass the departments to the view using ViewBag
            ViewBag.Departments = departments;
            return View(emps);
        }
        
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Employee emps, IFormFile photo)
		{
            // Retrieve the existing room to ensure RoomImage is populated
            var existingRoom = _context.Employees.AsNoTracking().FirstOrDefault(r => r.EmployeeId == emps.EmployeeId);
            if (existingRoom != null)
            {
                emps.Avatar = existingRoom.Avatar;
            }

            // Handle file upload if a new image is provided
            if (photo != null && photo.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "employees");
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
                if (!string.IsNullOrEmpty(emps.Avatar))
                {
                    var oldImagePath = Path.Combine("wwwroot", emps.Avatar.TrimStart('/'));
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
                emps.Avatar = Path.Combine("images", "employees", uniqueFileName);
            }

            var departments = _context.Departments
                 .Select(d => new SelectListItem
                 {
                     Value = d.DepartmentId.ToString(),
                     Text = d.DepartmentName
                 })
                 .ToList();

            ViewBag.Departments = departments;
            // Update the room in the database
            _context.Employees.Update(emps);
            int updatedRows = _context.SaveChanges();
            return updatedRows > 0 ? RedirectToAction(nameof(Index)) : View(emps);

		}

        //===========

        [HttpGet]
        public IActionResult Details(int id)
        {
            var emps = _context.Employees
            .Include(e => e.Department) // Include Department data
            .FirstOrDefault(e => e.EmployeeId == id);

            if (emps is null) return NotFound();

            return View("Details", emps);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var emps = _context.Employees
            .Include(e => e.Department).AsNoTracking() // Include Department data
            .FirstOrDefault(e => e.EmployeeId == id);

            if (emps is null) return NotFound();

            return View(emps);

            //var emps = _context.Rooms.Find(id);
            //if (rooms is null) return NotFound();
            //var roomtypes = _context.RoomTypes
            //    .Select(d => new SelectListItem
            //    {
            //        Value = d.RoomTypeId.ToString(),
            //        Text = d.RoomTypeName
            //    })
            //    .ToList();
            //// Pass the departments to the view using ViewBag
            //ViewBag.roomTypes = roomtypes;
            //return View("Delete", rooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)

        {
            var emps = _context.Employees.Find(id);
            if (emps is null) return NotFound();

            // Delete the associated image file
            if (!string.IsNullOrEmpty(emps.Avatar))
            {
                var filePath = Path.Combine("wwwroot", emps.Avatar);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Employees.Remove(emps);
            int insertedRows = _context.SaveChanges();
            return insertedRows > 0 ? RedirectToAction(nameof(Index)) : View();


           // var emps = _context.Employees
           //.Include(e => e.Department)
           //.FirstOrDefault(e => e.EmployeeId == id);

           // if (ModelState.IsValid)
           // {
           //     if (!string.IsNullOrEmpty(emps?.Avatar))
           //     {
           //         var filePath = Path.Combine("wwwroot", emps.Avatar);
           //         if (System.IO.File.Exists(filePath))
           //         {
           //             System.IO.File.Delete(filePath);
           //         }
           //     }
           //     _context.Employees.Remove(emps);
           //     _context.SaveChanges();
           //     return RedirectToAction(nameof(Index));
           // }

           // return View(emps);
            
        }

    }
}
