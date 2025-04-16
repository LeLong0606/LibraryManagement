using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOs;
using LibraryManagement.Services;

namespace LibraryManagement.Controllers
{
    [Route("LabraryAPI/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly AdminsService _adminsService;

        public AdminsController(AdminsService adminsService)
        {
            _adminsService = adminsService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(AdminDTO adminDto)
        {
            var admin = await _adminsService.RegisterAsync(adminDto);
            return StatusCode(201, new { message = "Registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var token = await _adminsService.LoginAsync(request.Email, request.Password);

            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}