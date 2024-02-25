using AutoMapper;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KineMartAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService,ILogger<ProductController> logger, IMapper mapper)
        {
            _productService = productService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync(List<ProductDto> productDtos)
        {
            if (ModelState.IsValid && !productDtos.IsNullOrEmpty())
            {
                await _productService.AddProductAsync(_mapper.Map<IEnumerable<Product>>(productDtos));
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync(string? order,string? search,int? pageNumber)
        {
            _logger.LogInformation("Query Data From Table Product");
            return Ok(await _productService.GetProductsAsync(order!, search!,pageNumber ?? 1,5));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id,ProductDto productDto)
        {
            if (ModelState.IsValid && productDto!=null)
            {
                await _productService.UpdateProductAsync(id, _mapper.Map<Product>(productDto));
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetNumberOfProductsAsync()
        {
            _logger.LogInformation("Count Product");
            return Ok(await _productService.GetNumberOfProductsAsync());
        }
    }
}
