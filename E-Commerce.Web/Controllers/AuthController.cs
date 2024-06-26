﻿using E_Commerce.Web.Dto.Request;
using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Request;
using E_Commerce.Web.Models.Dto.Response;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        // Register sayfası
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Kullanıcı kaydı yapan metot.
        /// </summary>
        /// <param name="registerRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            ResponseDto result = await _authService.RegisterAsync(registerRequestDto);

            if (result != null & result.IsSuccess)
            {
                // toastr ile bildirim yollanacak.
                TempData["success"] = result?.Message;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // toastr ile hata bildirimi yollanacak.
                TempData["error"] = result?.Message;
                return View(registerRequestDto);
            }
        }

        /// <summary>
        /// Kullanıcı aktifleştirildikten sonra yönlendirildiği sayfa
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<IActionResult> Activate([FromQuery] string userEmail)
        {
            ResponseDto response =  await _authService.ActivateUser(userEmail);

            if (response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return View();
            }
            else
            {
                TempData["error"] = response?.Message;
                return View();
            }
        }

        // Login Sayfası
        public IActionResult Login()
        {
            return View();
        }
        
        /// <summary>
        /// Kullanıcı girişi yapan metot.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto response = await _authService.LoginAsync(loginRequestDto);

            if (response != null && response.IsSuccess)
            {
                LoginDto loginDto = JsonConvert.DeserializeObject<LoginDto>(Convert.ToString(response.Result));
                
                await SignInUser(loginDto);
                // kullanıcının token'ı tarayıcıda cookieye atandı.
                _tokenProvider.SetToken(loginDto.Token);

                TempData["success"] = response?.Message;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response?.Message;
                return View();
            }
        }
        
        /// <summary>
        /// Kullanıcı çıkışı yapan metot.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login", "Auth");
        }

        /// <summary>
        /// Bu metot, bir kullanıcının giriş yapmasını yöneten bir asenkron fonksiyondur. Metot, gelen Jwt token'ını okuyarak kullanıcının kimlik bilgilerini çözümler ve bu bilgileri kullanarak bir kimlik nesnesi oluşturur. Ardından, bu kimlik bilgilerini temel alan bir kimlik doğrulama şeması oluşturulur ve kullanıcıyı temsil eden bir özneye sahip bir asenkron kimlik doğrulama işlemi başlatılır. Bu işlem, ASP.NET Core uygulamalarındaki çerez tabanlı kimlik doğrulama şemasını kullanır. Sonuç olarak, kullanıcı başarıyla kimlik doğrulandığında, HttpContext.SignInAsync metoduyla kullanıcı oturumunu başlatır.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task SignInUser(LoginDto loginDto)
        {
            var handler = new JwtSecurityTokenHandler();

            // token okuma işlemi
            var jwt = handler.ReadJwtToken(loginDto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            // email ve rolleri ekleme
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
