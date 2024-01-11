using ToDoListAPI.Data.Models;

namespace ToDoListAPI.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginRequest request);

        Task<RegisterResponse> Register(RegisterRequest request);
    }
}
