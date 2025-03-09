using Microsoft.AspNetCore.Mvc;

namespace HotelReservations.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}