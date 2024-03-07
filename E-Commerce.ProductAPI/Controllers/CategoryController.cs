using E_Commerce.ProductAPI.Models.Dto.Category;
using E_Commerce.ProductAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.ProductAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("list")]
        public IActionResult GetAll(int pageNumber, int pageSize)
        {
            var categories = _categoryService.GetAll(pageNumber, pageSize);
            return Ok(categories);
        }

        [HttpGet("getById")]
        public IActionResult GetById(Guid categoryId)
        {
            var result = _categoryService.GetById(categoryId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public IActionResult Create(CategoryCreateDto categoryCreateDto)
        {
            var result = _categoryService.Create(categoryCreateDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public IActionResult Update(CategoryUpdateDto categoryUpdateDto)
        {
            var result = _categoryService.Update(categoryUpdateDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid categoryId)
        {
            var result = _categoryService.Delete(categoryId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
