using E_Commerce.Web.Dto.Request;
using E_Commerce.Web.Models.Dto.Request;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login(LoginRequestDto loginRequestDto)
        {
            return View();
        }

        public IActionResult Register(RegisterRequestDto registerRequestDto)
        {
            return View();
        }
    }
}
