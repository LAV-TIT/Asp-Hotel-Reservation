using Microsoft.AspNetCore.Mvc;

namespace HotelReservations.Controllers
{
    public class ReservationDetails : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
