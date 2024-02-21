namespace E_Commerce.Web.Models.Dto.Response
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = false;
    }
}
