using AutoMapper;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KineMartAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        public CategoryController(IMapper mapper, ICategoryService categoryService, ILogger<CategoryController> 
                                                                                                        logger)
        {
            _mapper = mapper;
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync(List<CategoryDto> categoryDtos)
        {
            if (ModelState.IsValid && !categoryDtos.IsNullOrEmpty())
            {
                await _categoryService.AddCategoryAsync(_mapper.Map<IEnumerable<Category>>(categoryDtos));
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync(string? order,string? search_name,int? pageNumber)
        {
            _logger.LogInformation("Query Data From Table Category");
            return Ok(await _categoryService.GetCategoriesAsync(order!, search_name!, pageNumber ?? 1, 5));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id,CategoryDto categoryDto)
        {
            if (ModelState.IsValid && categoryDto !=null)
            {
                await _categoryService.UpdateCategoryAsync(id, _mapper.Map<Category>(categoryDto));
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetNumberOfCategoriesAsync()
        {
            _logger.LogInformation("Count Category");
            return Ok(await _categoryService.GetNumberOfCategoriesAsync());
        }
    }
}