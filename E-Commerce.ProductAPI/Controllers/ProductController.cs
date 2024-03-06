using E_Commerce.ProductAPI.Models.Dto;
using E_Commerce.ProductAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create")]
        public IActionResult Create(ProductCreateDto productCreateDto)
        {
            // productCreateDto oluşturulacak.
            // kategori bilgisinin nasıl alınacağına karar verilecek.

            var result = _productService.Create(productCreateDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("update")]
        public IActionResult Update(ProductDto productDto)
        {
            // productUpdateDto oluşturulacak.
            // kategori bilgisinin nasıl alınacağına karar verilecek.

            var result = _productService.Update(productDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Guid productId)
        {
            var result = _productService.Delete(productId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("list")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAll()
        {
            var result = _productService.GetAll();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("getById")]
        public IActionResult GetById(Guid productId)
        {
            var result = _productService.GetById(productId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
