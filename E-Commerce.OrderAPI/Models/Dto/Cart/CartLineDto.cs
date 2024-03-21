namespace E_Commerce.OrderAPI.Models.Dto.Cart
{
    public class CartLineDto
    {
        // PK
        public Guid Id { get; set; }
        // Product FK
        public Guid ProductId { get; set; }
        // Cart FK
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public decimal CartLinePrice { get; set; }
    }
}
