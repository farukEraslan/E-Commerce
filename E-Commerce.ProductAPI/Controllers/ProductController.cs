﻿using E_Commerce.ProductAPI.Models.Dto.Product;
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
        [Authorize(Roles = "admin")]
        public IActionResult Create(ProductCreateDto productCreateDto)
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
        public IActionResult Update(ProductUpdateDto productUpdateDto)
        {
            var result = _productService.Update(productUpdateDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("delete")]
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

        [HttpGet("list")]
        public IActionResult GetAll(int pageNumber, int pageSize)
        {
            var result = _productService.GetAll(pageNumber, pageSize);
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
