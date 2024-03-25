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
        private ResponseDto _response;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
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
            if (result.Token == "")
            {
                _response.IsSuccess = false;
                _response.Message = result.Message;
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.Message = result.Message;
            _response.Result = result;
            return Ok(_response);
        }

        [HttpGet("getById/{userId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            var result = await _authService.GetById(userId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
