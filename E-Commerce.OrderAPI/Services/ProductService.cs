using E_Commerce.OrderAPI.Models.Dto;
using E_Commerce.OrderAPI.Models.Dto.Product;
using E_Commerce.OrderAPI.Services.IServices;
using Newtonsoft.Json;

namespace E_Commerce.OrderAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ProductDto> GetById(Guid productId)
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product/getById/{productId}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (apiResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<ProductDto>(apiResponse.Result.ToString());
            }
            return new ProductDto();
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync("api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (apiResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<List<ProductDto>>(apiResponse.Result.ToString());
            }
            return new List<ProductDto>();
        }

        public async Task<ProductDto> IncreaseProductStock(Guid productId)
        {
            // sepete eklenen ürün için stok miktarını 1 artıracak.
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product/increase-stock/{productId}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (apiResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<ProductDto>(apiResponse.Result.ToString());
            }
            return new ProductDto();
        }

        public async Task<ProductDto> DecreaseProductStock(Guid productId)
        {
            // sepete eklenen ürün için stok miktarını 1 azaltacak.
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product/decrease-stock/{productId}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (apiResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<ProductDto>(apiResponse.Result.ToString());
            }
            return new ProductDto();
        }
    }
}
