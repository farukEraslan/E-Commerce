using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        
        [NotMapped]
        public double CartTotal { get; set; }
    }
}
