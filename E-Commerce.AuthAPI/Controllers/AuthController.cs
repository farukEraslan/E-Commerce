using E_Commerce.AuthAPI.Models.Dto;
using E_Commerce.AuthAPI.Models.Dto.Request;
using E_Commerce.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto user)
        {
            var resultMessage = await _authService.Register(user);
            if (!resultMessage.IsSuccess)
                return BadRequest(resultMessage);

            return Ok(resultMessage);
        }
    }
}
