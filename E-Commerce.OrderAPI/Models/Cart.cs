namespace E_Commerce.OrderAPI.Models
{
    public class Cart
    {
        public Cart()
        {
            this.CartLines = new List<CartLine>();
        }

        // PK
        public Guid Id { get; set; }

        // CustomerId FK
        public Guid UserId { get; set; }

        public string? Address { get; set; }
        public decimal? CartTotalPrice { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsCompleted { get; set; } = false;

        public List<CartLine> CartLines { get; set; }
    }
}
