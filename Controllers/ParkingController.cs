using Microsoft.AspNetCore.Mvc;
using SmartParkingSystem.Models;
using SmartParkingSystem.Services;

namespace SmartParkingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly ParkingService _service;
        private readonly AuthService _authService;

        public ParkingController(ParkingService service, AuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? search = null)
        {
            return Ok(_service.List(search));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] ParkingSpot spot)
        {
            if (!_authService.IsAdmin(Request.Headers.Authorization))
                return Unauthorized(new { message = "Vetem admini mund te shtoje parkingje" });

            try
            {
                _service.Add(spot);
                return Ok(new { message = "Parking u shtua me sukses" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] ParkingSpot spot)
        {
            if (!_authService.IsAdmin(Request.Headers.Authorization))
                return Unauthorized(new { message = "Vetem admini mund te ndryshoje parkingje" });

            try
            {
                spot.Id = id;
                _service.Update(spot);
                return Ok(new { message = "Parking u perditesua me sukses" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_authService.IsAdmin(Request.Headers.Authorization))
                return Unauthorized(new { message = "Vetem admini mund te fshije parkingje" });

            try
            {
                _service.Delete(id);
                return Ok(new { message = "Parking u fshi me sukses" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
