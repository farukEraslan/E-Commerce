namespace E_Commerce.ProductAPI.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }
        // PK
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
