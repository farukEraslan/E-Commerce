using E_Commerce.OrderAPI.Models.Dto.Product;

namespace E_Commerce.OrderAPI.Models
{
    public class CartLine
    {
        // PK
        public Guid Id { get; set; }

        // Product FK
        public Guid ProductId { get; set; }

        // Cart FK
        public Guid CartId { get; set; }
        // Cart Navi Prop
        public Cart Cart { get; set; }

        public int Quantity { get; set; }
        public decimal CartLinePrice { get; set; }
    }
}
