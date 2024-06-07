using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Category;
using E_Commerce.Web.Models.Dto.Product;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace E_Commerce.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1, int size = 8)
        {
            List<CategoryDto>? categoryList = new();
            ResponseDto? response = await _categoryService.GetAllAsync(page, size);

            if (response != null && response.IsSuccess)
            {
                categoryList = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
                TempData["success"] = response?.Message;
            }
            else
                TempData["error"] = response?.Message;

            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View(new CategoryDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            ResponseDto? response = await _categoryService.CreateAsync(categoryDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction("Index", "Category");
            }
            else
                TempData["error"] = response?.Message;

            return View(categoryDto);
        }

        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            CategoryDto category = new();
            ResponseDto response = await _categoryService.GetByIdAsync(categoryDto.Id);
            if (response != null && response.IsSuccess)
            {
                category = JsonConvert.DeserializeObject<CategoryDto>(Convert.ToString(response.Result));
                TempData["success"] = response?.Message;
            }
            else
                TempData["error"] = response?.Message;  

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryDto)
        {
            ResponseDto? response = await _categoryService.UpdateAsync(categoryDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction("Index", "Category");
            }
            else
                TempData["error"] = response?.Message;

            return View(categoryDto);
        }

        public async Task<IActionResult> Delete(CategoryDto categoryDto)
        {
            ResponseDto? response = await _categoryService.DeleteAsync(categoryDto.Id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction("Index", "Category");
            }
            else
                TempData["error"] = response?.Message;

            return RedirectToAction("Index", "Category");
        }
    }
}
