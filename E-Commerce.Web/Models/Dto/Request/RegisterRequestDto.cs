﻿namespace E_Commerce.Web.Dto.Request
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string UserName
        {
            get
            {
                return Email;
            }
        }
    }
}
