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

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var categories = _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("list/page={page:int}&size={size:int}")]
        public IActionResult GetAll([FromRoute] int page, int size)
        {
            var categories = _categoryService.GetAll(page, size);
            return Ok(categories);
        }

        [HttpGet("getById/{categoryId:guid}")]
        public IActionResult GetById([FromRoute] Guid categoryId)
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

        [HttpDelete("delete/{categoryId:guid}")]
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
