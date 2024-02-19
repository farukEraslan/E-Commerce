using E_Commerce.AuthAPI.Models.Enum;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.AuthAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public UserStatus Status { get; set; }
    }
}
