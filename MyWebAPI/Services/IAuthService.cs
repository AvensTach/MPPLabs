using MyWebAPI.DTOs;

namespace MyWebAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(UserLoginDto loginDto);
        Task<bool> RegisterAsync(UserRegisterDto registerDto, string role = "User");
    }
}