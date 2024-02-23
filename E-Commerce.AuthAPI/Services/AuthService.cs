using AutoMapper;
using E_Commerce.AuthAPI.Data;
using E_Commerce.AuthAPI.Helpers;
using E_Commerce.AuthAPI.Models;
using E_Commerce.AuthAPI.Models.Dto;
using E_Commerce.AuthAPI.Models.Dto.Request;
using E_Commerce.AuthAPI.Models.Dto.Response;
using E_Commerce.AuthAPI.Models.Enum;
using E_Commerce.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _authAPIDatabase;
        private ResponseDto _response;
        private LoginResponseDto _loginResponse;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IMapper mapper, UserManager<AppUser> userManager, AppDbContext authAPIDatabase, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _authAPIDatabase = authAPIDatabase;
            _roleManager = roleManager;
            _response = new ResponseDto();
            _loginResponse = new LoginResponseDto();
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Kullanıcı oluşturmak için kullanılır.
        /// </summary>
        /// <param name="registerationRequestDto"></param>
        /// <returns>Durum mesajı döner.</returns>
        public async Task<ResponseDto> Register(RegisterRequestDto registerRequestDto)
        {
            var newUser = _mapper.Map<AppUser>(registerRequestDto);
            newUser.Status = UserStatus.OnayBekleniyor;

            try
            {
                var result = await _userManager.CreateAsync(newUser, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    var createdUser = _authAPIDatabase.AppUsers.First(user => user.Email == registerRequestDto.Email);
                    var user = _mapper.Map<UserDto>(createdUser);

                    _response.Result = user;
                    _response.Message = "Kullanıcı oluşturma başarılı.";
                    // onay emaili burada yollanacak.
                    EmailSendHelper.SendEmailProducer(registerRequestDto.Email);    // burada hangfire implement edilebilir.
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = result.Errors.FirstOrDefault().Description;
                }
                return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        /// <summary>
        /// Kullanıcıyı aktifleştirmek için kullanılır.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>Durum mesajı döner.</returns>
        public async Task<ResponseDto> UserActivate(string userEmail)
        {
            try
            {
                var user = _authAPIDatabase.AppUsers.First(user => user.Email == userEmail);

                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Kullanıcı zaten var.";
                }
                else if (user.Status == UserStatus.Aktif)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Kullanıcı zaten aktif.";
                }
                else
                {
                    user.Status = UserStatus.Aktif;
                    user.EmailConfirmed = true;
                    _authAPIDatabase.AppUsers.Update(user);
                    _authAPIDatabase.SaveChanges();

                    // kullanıcıya rol ataması burada yapılacak.
                    var result = await AssignRole(userEmail, "customer");       // default olarak customer atanmıştır, ihtiyaca göre değiştirilecek.

                    _response.IsSuccess = true;
                    _response.Message = "Kullanıcı başarıyla aktifleştirildi.";
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        /// <summary>
        /// Kullanıcıya rol atamak için kullanılır.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns>İşlem başarılıysa True, işlem başarısızsa False döner.</returns>
        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _authAPIDatabase.AppUsers.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    // rol yoksa, rolü oluştur.
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Kullanıcı girişi yapar.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns>LoginResponseDto tipinde bir cevap döner.</returns>
        public async Task<LoginResponseDto> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = _authAPIDatabase.AppUsers.FirstOrDefault(user => user.UserName.ToLower() == loginRequestDto.UserName.ToLower());
                if (user == null)
                {
                    _loginResponse.User = null;
                    _loginResponse.Token = "";
                    _loginResponse.Message = "Kullanıcı bulunamadı.";
                    return _loginResponse;
                }
                else if (user.Status == UserStatus.OnayBekleniyor || user.EmailConfirmed == false)
                {
                    _loginResponse.User = _mapper.Map<UserDto>(user);
                    _loginResponse.Token = "";
                    _loginResponse.Message = "Kullanıcının aktif edilmesi gerekiyor.";
                    return _loginResponse;
                }
                else
                {
                    var result = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                    if (!result)
                    {
                        _loginResponse.User = _mapper.Map<UserDto>(user);
                        _loginResponse.Token = "";
                        _loginResponse.Message = "Kullanıcı adı ya da parola hatalı.";
                        return _loginResponse;
                    }

                    var roles = await _userManager.GetRolesAsync(user);
                    // jwt token burada oluşturulacak.
                    var userToken = _jwtTokenGenerator.GenerateToken(user, roles);

                    _loginResponse.User = _mapper.Map<UserDto>(user);
                    _loginResponse.Token = userToken;
                    _loginResponse.Message = "Başarı ile giriş yapıldı.";
                    return _loginResponse;
                }
            }
            catch (Exception ex)
            {
                _loginResponse.User = null;
                _loginResponse.Token = "";
                _loginResponse.Message = ex.Message;
                return _loginResponse;
            }
        }
    }
}
