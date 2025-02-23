    using AutoMapper;
using E_Commerce.AuthAPI.Data;
using E_Commerce.AuthAPI.Helpers;
using E_Commerce.AuthAPI.Models;
using E_Commerce.AuthAPI.Models.Dto;
using E_Commerce.AuthAPI.Models.Dto.Request;
using E_Commerce.AuthAPI.Models.Enum;
using E_Commerce.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _authAPIDatabase;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public AuthService(IMapper mapper, UserManager<AppUser> userManager, AppDbContext authAPIDatabase, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _authAPIDatabase = authAPIDatabase;
            _roleManager = roleManager;
            _response = new ResponseDto();
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Kullanıcı oluşturmak için kullanılır.
        /// </summary>
        /// <param name="registerationRequestDto"></param>
        /// <returns>Durum mesajı döner.</returns>
        public async Task<ResponseDto> Register(RegisterRequestDto registerRequestDto)
        {
            try
            {
                var hasUser = _authAPIDatabase.Users.FirstOrDefault(u => u.NormalizedEmail == registerRequestDto.Email.Trim().ToUpper());
                if (hasUser != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Kullanıcı zaten var";
                    return _response;
                }

                var newUser = _mapper.Map<AppUser>(registerRequestDto);
                newUser.Status = UserStatus.OnayBekleniyor;

                var result = await _userManager.CreateAsync(newUser, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    var createdUser = _authAPIDatabase.AppUsers.FirstOrDefault(newUser => newUser.Email == registerRequestDto.Email.Trim());
                    var userDto = _mapper.Map<UserDto>(createdUser);

                    _response.Result = userDto;
                    _response.Message = "Kullanıcı oluşturma başarılı.";
                    // onay emaili burada yollanacak.
                    //EmailSendHelper.SendEmailProducer(registerRequestDto.Email);    // burada hangfire implement edilebilir.
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
        /// Kullanıcı girişi yapar.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns>ResponseDto tipinde bir cevap döner.</returns>
        public async Task<ResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            LoginDto loginDto = new LoginDto();

            try
            {
                var user = _authAPIDatabase.AppUsers.FirstOrDefault(user => user.UserName.ToLower() == loginRequestDto.UserName.ToLower());
                if (user == null)
                {
                    loginDto.User = null;
                    loginDto.Token = " ";

                    _response.Result = loginDto;
                    _response.Message = "Kullanıcı bulunamadı.";
                    _response.IsSuccess = false;
                    return _response;
                }
                else if (user.Status == UserStatus.OnayBekleniyor || user.EmailConfirmed == false)
                {
                    loginDto.User = _mapper.Map<UserDto>(user);
                    loginDto.Token = " ";

                    _response.Result = loginDto;
                    _response.Message = "Kullanıcının aktif edilmesi gerekiyor.";
                    _response.IsSuccess = false;
                    return _response;
                }
                else
                {
                    var result = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                    if (!result)
                    {
                        loginDto.User = _mapper.Map<UserDto>(user);
                        loginDto.Token = " ";

                        _response.Result = loginDto;
                        _response.Message = "Kullanıcı adı ya da parola hatalı.";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var roles = await _userManager.GetRolesAsync(user);
                    // jwt token burada oluşturulacak.
                    var userToken = _jwtTokenGenerator.GenerateToken(user, roles);

                    loginDto.User = _mapper.Map<UserDto>(user);
                    loginDto.Token = userToken;

                    _response.Result = loginDto;
                    _response.Message = "Başarı ile giriş yapıldı.";
                    return _response;
                }
            }
            catch (Exception ex)
            {
                loginDto.User = null;
                loginDto.Token = " ";

                _response.Result = loginDto;
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
                    var result = await AssignRole(userEmail, "admin");       // default olarak customer atanmıştır, ihtiyaca göre değiştirilecek.

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
        /// Id'si verilen kullanıcıyı döndürür.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ResponseDto tipinde bir cevap döner.</returns>
        public async Task<ResponseDto> GetById(Guid userId)
        {
            var user = _authAPIDatabase.AppUsers.FirstOrDefault(u => u.Id == userId.ToString());
            if (user != null)
            {
                _response.Result = _mapper.Map<UserDto>(user);
                _response.Message = "Kullanıcı başarı ile listelendi.";
                return _response;
            }
            else
            {
                _response.IsSuccess = false;
                return _response;
            }
        }

        /// <summary>
        /// Müşterilerin email adreslerini getirir.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto> GetCustomerEmails()
        {
            var customers = await _userManager.GetUsersInRoleAsync("customer");

            List<string> customerEmails = new();
            foreach (var customer in customers)
            {
                customerEmails.Add(customer.Email);
            }
            _response.Result = customerEmails;
            _response.IsSuccess = true;
            _response.Message = "Müşteriler başarı ile listelendi.";
            return _response;
        }
    }
}
