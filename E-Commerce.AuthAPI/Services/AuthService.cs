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

namespace E_Commerce.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _authAPIDatabase;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(IMapper mapper, UserManager<AppUser> userManager, AppDbContext authAPIDatabase, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _authAPIDatabase = authAPIDatabase;
            _roleManager = roleManager;
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

                    ResponseDto response = new()
                    {
                        Result = user,
                        Message = "Registeration completed successfully."
                    };
                    // onay emaili burada yollanacak.
                    EmailSendHelper.SendEmailProducer(registerRequestDto.Email);
                    return response;
                    
                }
                else
                {
                    ResponseDto response = new()
                    {
                        IsSuccess = false,
                        Message = result.Errors.FirstOrDefault().Description
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                ResponseDto response = new()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
                return response;
            }
        }

        /// <summary>
        /// Kullanıcıyı aktifleştirmek için kullanılır.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>Durum mesajı döner.</returns>
        public async Task<ResponseDto> UserActivate(string userEmail)
        {
            ResponseDto response = new();
            var user = _authAPIDatabase.AppUsers.First(user => user.Email == userEmail);

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Kullanıcı zaten var.";
            }
            else if (user.Status == UserStatus.Aktif)
            {
                response.IsSuccess = false;
                response.Message = "Kullanıcı zaten aktif.";
            }
            else
            {
                user.Status = UserStatus.Aktif;
                user.EmailConfirmed = true;
                _authAPIDatabase.AppUsers.Update(user);
                _authAPIDatabase.SaveChanges();

                // kullanıcıya rol ataması burada yapılacak.
                var result = await AssignRole(userEmail, "customer");       // default olarak customer atanmıştır, ihtiyaca göre değiştirilecek.

                response.IsSuccess = true;
                response.Message = "Kullanıcı başarıyla aktifleştirildi.";
            }
            return response;
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
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }


    }
}
