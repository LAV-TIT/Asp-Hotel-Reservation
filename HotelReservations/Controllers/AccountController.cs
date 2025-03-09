using HotelReservations.Models;
using HotelReservations.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservations.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [ActionName("Login")]
        public IActionResult Authentication(string? ReturnUrl = null)
        {
            ReturnUrl ??= Url.Content("~/");
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authentication(LoginRequest request, string? ReturnUrl = null)
        {
            ReturnUrl ??= Url.Content("~/");
            ViewBag.ReturnUrl = ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var LoginResult = await _userService.GetAuthenticatedUser(request);
            if (!LoginResult.IsSuccess)
            {
                ModelState.AddModelError("", LoginResult.Message);
                return View(request);
            }

            return LocalRedirect(ReturnUrl);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.CreateUser(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }

            var loginRequest = new LoginRequest
            {
                UserName = request.UserName,
                Password = request.Password
            };

            var loginResult = await _userService.GetAuthenticatedUser(loginRequest);
            if (loginResult.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (await _userService.LogUserOut())
            {
                return RedirectToAction("Login");
            }
            return BadRequest();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
