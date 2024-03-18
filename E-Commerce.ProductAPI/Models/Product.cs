namespace E_Commerce.ProductAPI.Models
{
    public class Product
    {
        // PK
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        // Kategori FK
        public Guid CategoryId { get; set; }
        // Kategori Navi Prop
        public Category Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockAmount { get; set; }
        public string? Author { get; set; } = "Anonim";
        public string? Publisher { get; set; } = "Anonim";
        public string ISBN { get; set; }
        public string? ImageUrl { get; set; } = "https://placehold.co/150x200";
    }
}
