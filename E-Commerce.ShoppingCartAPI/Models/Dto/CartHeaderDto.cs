using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.ShoppingCartAPI.Models.Dto
{
    public class CartHeaderDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public double CartTotal { get; set; }
    }
}
