namespace E_Commerce.OrderAPI.Models.Dto.Cart
{
    public class CartDto
    {
        public Guid Id { get; set; }

        // CustomerId FK
        public Guid UserId { get; set; }
        public UserDto User { get; set; }

        public string? Address { get; set; }
        public decimal CartTotalPrice { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCompleted { get; set; }

        public List<CartLineDto> CartLines { get; set; }
    }
}
