namespace E_Commerce.Web.Models.Dto.Order
{
    public class RemoveCartDto
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
