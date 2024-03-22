namespace E_Commerce.Web.Models.Dto.Order
{
    public class CreateCartDto
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
