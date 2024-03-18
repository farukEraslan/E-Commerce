using E_Commerce.ProductAPI.Models.Dto.Product;
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

        [HttpGet("list/page={page:int}&size={size:int}")]
        public IActionResult GetAll([FromRoute] int page, int size)
        {
            var result = _productService.GetAll(page, size);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("getById/{productId:guid}")]
        public IActionResult GetById([FromRoute] Guid productId)
        {
            var result = _productService.GetById(productId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public IActionResult Create([FromBody] ProductCreateDto productCreateDto)
        {
            var result = _productService.Create(productCreateDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public IActionResult Update([FromBody] ProductUpdateDto productUpdateDto)
        {
            var result = _productService.Update(productUpdateDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("delete/{productId:guid}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid productId)
        {
            var result = _productService.Delete(productId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
