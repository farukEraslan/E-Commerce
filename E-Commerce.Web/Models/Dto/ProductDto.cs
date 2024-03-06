namespace E_Commerce.Web.Models.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockAmount { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public string ISBN { get; set; }
        public string? ImageUrl { get; set; }
    }
}
