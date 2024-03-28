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

        /// <summary>
        /// Kullanıcı kaydı yapan metot.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto newUser)
        {
            var result = await _authService.Register(newUser);
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcı girişi yapan metot.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.Login(loginRequestDto);
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcı aktif eden metot.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet("activate-user")]
        public async Task<IActionResult> ActivateUser([FromQuery] string userEmail)
        {
            var result = await _authService.UserActivate(userEmail);
            return Ok(result);
        }

        /// <summary>
        /// Id'si verilen kullanıcıyı getiren metot.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getById/{userId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            var result = await _authService.GetById(userId);
            return Ok(result);
        }
    }
}
