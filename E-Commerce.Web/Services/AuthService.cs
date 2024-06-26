﻿using E_Commerce.Web.Dto.Request;
using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Request;
using E_Commerce.Web.Services.IServices;
using E_Commerce.Web.Utility;
using Microsoft.AspNetCore.Authentication;

namespace E_Commerce.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITokenProvider _tokenProvider;

        public AuthService(IBaseService baseService, IHttpContextAccessor contextAccessor, ITokenProvider tokenProvider)
        {
            _baseService = baseService;
            _contextAccessor = contextAccessor;
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Kullanıcı kaydı yapan metot.
        /// </summary>
        /// <param name="registerRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            return await _baseService.SendAsync(new Models.RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = registerRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register"
            });
        }

        /// <summary>
        /// Kullanıcı aktifleştirmek için kullanılır.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<ResponseDto> ActivateUser(string userEmail)
        {
            return await _baseService.SendAsync(new Models.RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/activate-user?userEmail=" + userEmail
            });
        }

        /// <summary>
        /// Kullanıcı girişi yapan metot.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new Models.RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login"
            });
        }

        /// <summary>
        /// Kullanıcı çıkışı yapan metot.
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            await _contextAccessor.HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
        }

        /// <summary>
        /// Müşterilerin emaillerini getirir.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetCustomerEmails()
        {
            return await _baseService.SendAsync(new Models.RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/get-customer-emails"
            });
        }
    }
}
