﻿namespace E_Commerce.OrderAPI.Models.Dto
{
    public class EmailDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public object Body { get; set; }
    }
}
