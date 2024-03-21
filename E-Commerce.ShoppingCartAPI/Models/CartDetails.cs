using E_Commerce.ShoppingCartAPI.Models.Dto.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        public Guid Id { get; set; }
        public Guid CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }
        public Guid ProductId { get; set; }

        [NotMapped]
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
