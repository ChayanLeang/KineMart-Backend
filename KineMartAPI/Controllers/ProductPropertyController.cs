using KineMartAPI.ModelDtos;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KineMartAPI.Controllers
{
    [Route("api/product-property")]
    [ApiController]
    [Authorize]
    public class ProductPropertyController : ControllerBase
    {
        private readonly IProductPropertyService _productPropertyService;
        private readonly ILogger<ProductPropertyController> _logger;
        public ProductPropertyController(IProductPropertyService productPropertyService, 
                                              ILogger<ProductPropertyController> logger)
        {
            _productPropertyService = productPropertyService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductPropertyAsync(ImportDto importDto)
        {
            if (ModelState.IsValid && importDto!=null)
            {
                await _productPropertyService.AddProductPropertyAsync(importDto);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductPropertiesAsync(string? order,string? search,int? pageNumber)
        {
            _logger.LogInformation("Query Data From Table ProductProperty");
            return Ok(await _productPropertyService.GetProductPropertiesAsync(order!, search!,pageNumber??1,5));
        }
    }
}
