﻿namespace E_Commerce.Web.Models.Dto.Product
{
    public class ProductCreateDto
    {
        public string ProductName { get; set; }
        public Guid CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockAmount { get; set; }
        public string? Author { get; set; } = "Anonim";
        public string? Publisher { get; set; } = "Anonim";
        public string ISBN { get; set; }
        public string? ImageUrl { get; set; } = "https://placehold.co/150x200";
    }
}
