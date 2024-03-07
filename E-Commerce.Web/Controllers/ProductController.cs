using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Product;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace E_Commerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto>? productList = new();

            ResponseDto? response = await _productService.GetAllAsync();

            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productList);
        }

        public IActionResult Create()
        {
            return View(new ProductCreateVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateAsync(productCreateDto);

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
            return View(productCreateDto);
        }
    }
}
