﻿using E_Commerce.ShoppingCartAPI.Models.Dto.Product;

namespace E_Commerce.ShoppingCartAPI.Models.Dto
{
    public class CartDetailsDto
    {
        public Guid Id { get; set; }
        public Guid CartHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
    }
}
