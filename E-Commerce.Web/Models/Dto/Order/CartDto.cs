using E_Commerce.Web.Models.Dto.Cart;

namespace E_Commerce.Web.Models.Dto.Order
{
    public class CartDto
    {
        public CartDto()
        {
            this.CartLines = new List<CartLineDto>();
        }

        public Guid Id { get; set; }

        // CustomerId FK
        public Guid UserId { get; set; }
        public UserDto? User { get; set; }

        public string? Address { get; set; }
        public decimal CartTotalPrice { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCompleted { get; set; }
        public bool Status { get; set; }

        public List<CartLineDto>?CartLines { get; set; }
    }
}
