using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.Services;

namespace SmartParkingSystem.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class ParkingSessionsController : ControllerBase
    {
        private readonly ParkingSessionService _sessionService;
        private readonly AuthService _authService;

        public ParkingSessionsController(ParkingSessionService sessionService, AuthService authService)
        {
            _sessionService = sessionService;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!_authService.IsTokenValid(Request.Headers.Authorization))
                return Unauthorized(new { message = "Duhet te kycesh" });

            return Ok(_sessionService.GetAll());
        }

        [HttpPost("start")]
        public IActionResult Start(StartSessionRequest request)
        {
            if (!_authService.IsTokenValid(Request.Headers.Authorization))
                return Unauthorized(new { message = "Duhet te kycesh" });

            try
            {
                var session = _sessionService.StartSession(request.ParkingSpotId, request.VehiclePlate);
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}/end")]
        public IActionResult End(int id)
        {
            if (!_authService.IsTokenValid(Request.Headers.Authorization))
                return Unauthorized(new { message = "Duhet te kycesh" });

            try
            {
                var session = _sessionService.EndSession(id);
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class StartSessionRequest
    {
        public int ParkingSpotId { get; set; }

        public string VehiclePlate { get; set; } = string.Empty;
    }
}
