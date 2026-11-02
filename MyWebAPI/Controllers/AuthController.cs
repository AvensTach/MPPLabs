using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DTOs;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            // First registered user is automatically an Admin for testing purposes, others are User
            string role = request.Username.ToLower() == "admin" ? "Admin" : "User";

            var success = await _authService.RegisterAsync(request, role);
            if (!success) return BadRequest("Username already exists.");

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var response = await _authService.LoginAsync(request);

            // Handle invalid login scenarios
            if (response == null) return Unauthorized("Invalid username or password.");

            return Ok(response);
        }
    }
}