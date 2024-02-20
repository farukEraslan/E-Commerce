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
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto newUser)
        {
            var result = await _authService.Register(newUser);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("user-activate")]
        public async Task<IActionResult> UserActivate(string userEmail)
        {
            var result = await _authService.UserActivate(userEmail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var result = await _authService.Login(loginRequestDto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
