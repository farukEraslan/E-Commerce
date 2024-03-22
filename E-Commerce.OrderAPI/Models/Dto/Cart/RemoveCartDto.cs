namespace E_Commerce.OrderAPI.Models.Dto.Cart
{
    public class RemoveCartDto
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
