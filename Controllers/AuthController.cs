using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.Models;
using SmartParkingSystem.Services;

namespace SmartParkingSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            try
            {
                var registeredUser = _authService.Register(user);
                return Ok(ToResponse(registeredUser, _authService.CreateToken(registeredUser)));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var user = _authService.Login(request.Email, request.Password);
                return Ok(ToResponse(user, _authService.CreateToken(user)));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _authService.RevokeToken(Request.Headers.Authorization);
            return Ok(new { message = "Dole nga llogaria" });
        }

        private static object ToResponse(User user, string token)
        {
            return new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Role,
                Token = token
            };
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
