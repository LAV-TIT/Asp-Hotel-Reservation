using HotelReservations.Models;

namespace HotelReservations.Services
{
    public interface IUserService
    {
        Task<MsgResponse> CreateUser(RegisterRequest req);
        Task<MsgResponse> GetAuthenticatedUser(LoginRequest request);
        Task<bool> LogUserOut();
    }
}
