using E_Commerce.OrderAPI.Models.Dto.Cart;

namespace E_Commerce.OrderAPI.Models.Dto
{
    public class EmailDto
    {
        public string ToEmail { get; set; }
        public string Subject
        {
            get { return "Sipariş Onayı - BookSeller Project"; }
        }
        public object Body { get; set; }

    }
}
