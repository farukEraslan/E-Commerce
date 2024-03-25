namespace E_Commerce.AuthAPI.Models.Dto
{
    public class LoginDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
