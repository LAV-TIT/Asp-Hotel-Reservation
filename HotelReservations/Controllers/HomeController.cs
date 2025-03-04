using Microsoft.AspNetCore.Mvc;

namespace HotelReservations.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
