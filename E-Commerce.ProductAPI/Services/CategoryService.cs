using AutoMapper;
using E_Commerce.ProductAPI.Data;
using E_Commerce.ProductAPI.Models;
using E_Commerce.ProductAPI.Models.Dto;
using E_Commerce.ProductAPI.Models.Dto.Category;
using E_Commerce.ProductAPI.Services.IServices;

namespace E_Commerce.ProductAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CategoryService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        public ResponseDto Create(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var hasCategory = _appDbContext.Categories.FirstOrDefault(category => category.CategoryName.Trim().ToUpper() == categoryCreateDto.CategoryName.Trim().ToUpper());
                if (hasCategory != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Kategori zaten var.";
                    return _response;
                }

                var result = _appDbContext.Categories.Add(_mapper.Map<Category>(categoryCreateDto));
                _appDbContext.SaveChanges();
                _response.Message = "Kategori başarı ile eklendi.";
                _response.Result = categoryCreateDto;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto Delete(Guid categoryId)
        {
            try
            {
                var category = _appDbContext.Categories.SingleOrDefault(c => c.Id == categoryId);
                var result = _appDbContext.Categories.Remove(category);
                _appDbContext.SaveChanges();
                _response.Message = "Kategori başarı ile silindi.";
                _response.Result = category;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto GetAll()
        {
            try
            {
                var categories = _mapper.Map<List<CategoryDto>>(_appDbContext.Categories.ToList());
                _response.Message = "Kategoriler başarı ile listelendi.";
                _response.Result = categories;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto GetAll(int page, int size)
        {
            try
            {
                var categories = _mapper.Map<List<CategoryDto>>(_appDbContext.Categories.Skip((page - 1) * size).Take(size).ToList());
                _response.Message = "Kategoriler başarı ile listelendi.";
                _response.Result = categories;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto GetById(Guid categoryId)
        {
            try
            {
                var category = _appDbContext.Categories.SingleOrDefault(c => c.Id == categoryId);
                _response.Message = "Kategori başarı ile listelendi..";
                _response.Result = _mapper.Map<CategoryDto>(category);
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto Update(CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                var category = _appDbContext.Categories.SingleOrDefault(p => p.Id == categoryUpdateDto.Id);
                if (category.CategoryName.Trim().ToUpper() == categoryUpdateDto.CategoryName.Trim().ToUpper())
                {
                    _response.IsSuccess = false;
                    _response.Message = "Kategori zaten var.";
                    return _response;
                }

                var result = _appDbContext.Categories.Update(_mapper.Map(categoryUpdateDto, category));
                _appDbContext.SaveChanges();
                _response.Message = "Kategori başarı ile güncellendi.";
                _response.Result = categoryUpdateDto;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }
    }
}
