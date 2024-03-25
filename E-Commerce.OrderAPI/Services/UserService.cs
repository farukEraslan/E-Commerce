using E_Commerce.OrderAPI.Models.Dto;
using E_Commerce.OrderAPI.Services.IServices;
using Newtonsoft.Json;

namespace E_Commerce.OrderAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserDto> GetById(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("User");
            var response = await client.GetAsync($"api/auth/getById/{userId}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (apiResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<UserDto>(apiResponse.Result.ToString());
            }
            else
            {
                return new UserDto();
            }
        }
    }
}
