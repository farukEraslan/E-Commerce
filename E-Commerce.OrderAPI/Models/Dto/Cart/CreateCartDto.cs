namespace E_Commerce.OrderAPI.Models.Dto.Cart
{
    public class CreateCartDto
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
