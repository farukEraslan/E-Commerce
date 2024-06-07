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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1, int size = 8)
        {
            List<ProductDto>? productList = new();
            ResponseDto? productResponse = await _productService.GetAllAsync(page, size);

            if (productResponse != null && productResponse.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(productResponse.Result));
                TempData["success"] = productResponse?.Message;
            }
            else
                TempData["error"] = productResponse?.Message;

            return View(productList);
        }
        public async Task<IActionResult> Create()
        {
            ProductCreateVM productCreateVM = new();
            ResponseDto? response = await _categoryService.GetAllAsync();

            if (response != null && response.IsSuccess)
                productCreateVM.CategoryDto = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result));
            else
                TempData["error"] = response?.Message;

            productCreateVM.ProductCreateDto = new();
            return View(productCreateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateAsync(productCreateVM.ProductCreateDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(productCreateVM);
        }
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            ProductUpdateVM productUpdateVM = new();
            ResponseDto? categoryResponse = await _categoryService.GetAllAsync();
            ResponseDto? productResponse = await _productService.GetByIdAsync(productDto.Id);

            if (categoryResponse != null && categoryResponse.IsSuccess)
            {
                productUpdateVM.CategoryDto = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(categoryResponse.Result));
                productUpdateVM.ProductUpdateDto = JsonConvert.DeserializeObject<ProductUpdateDto>(Convert.ToString(productResponse.Result));
                TempData["success"] = categoryResponse?.Message;
            }
            else
                TempData["error"] = categoryResponse?.Message;

            return View(productUpdateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateAsync(productUpdateDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> Delete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteAsync(productDto.Id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction("Index", "Product");
        }
    }
}
;