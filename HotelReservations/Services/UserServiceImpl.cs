using HotelReservations.Data;
using HotelReservations.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using HotelReservations.Services;
namespace HotelReservations.Services
{
    public sealed class UserServiceImpl: IUserService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        private ILogger<UserServiceImpl> _logger;
        public UserServiceImpl(DataContext context, IHttpContextAccessor httpcontext, ILogger<UserServiceImpl> logger)
        {
            _context = context;
            _httpcontext = httpcontext;
            _logger = logger;
        }
        public async Task<bool> LogUserOut()
        {
            _logger.LogInformation("Ty log user out.....");
            try
            {
                var httpContext = _httpcontext.HttpContext;
                await httpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public async Task<MsgResponse> GetAuthenticatedUser(LoginRequest request)
        {
            var httpContext = _httpcontext.HttpContext;
            var user = _context.Users.Where(u => u.UserName == request.UserName).FirstOrDefault();

            if (user is null) return new MsgResponse(false, $"{request.UserName} Does not exists.");

            if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new MsgResponse(false, "Invalid Password");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.RoleName?? "")
            };
            var claimIdentities = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var ClaimsPrinciple = new ClaimsPrincipal(claimIdentities);
            await httpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimsPrinciple, new AuthenticationProperties
                {
                    IsPersistent = request.RememberMe,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1),
                });
            return (new MsgResponse(true, "Login Success"));
        }
        public async Task<MsgResponse> CreateUser(RegisterRequest req)
        {
            var existingUser = _context.Users.Where(x => x.UserName.Contains(req.UserName) || x.Email.Contains(req.Email)).FirstOrDefault();
            if (existingUser is not null)
            {
                return new MsgResponse(false, $"{req.UserName} And/OR {req.Email} Already Register.");
            }
            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);
            var user = new Users
            {
                Email = req.Email,
                Password = hash,
                UserName = req.UserName,
                RoleName = req.RoleName,
            };
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0 ?
                new MsgResponse(true, "New User Was Created.") :
                new MsgResponse(false, "There are something wrong.");
        }
    }
}





