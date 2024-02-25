using AutoMapper;
using KineMartAPI.ModelDtos;
using KineMartAPI.ModelEntities;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KineMartAPI.Controllers
{
    [Route("api/supplier")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;
        private readonly IMapper _mapper;
        
        public SupplierController(ISupplierService supplierService,ILogger<SupplierController> logger,
                                                                                       IMapper mapper)
        {
            _supplierService = supplierService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplierAsync(List<SupplierDto> supplierDtos)
        {
            if (ModelState.IsValid && !supplierDtos.IsNullOrEmpty())
            {
                await _supplierService.AddSupplierAsync(_mapper.Map<List<Supplier>>(supplierDtos));
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliersAsync(string? order,string? search,int? pageNumber)
        {
            _logger.LogInformation("Query Data From Table Supplier");
            return Ok(await _supplierService.GetSuppliersAsync(order!, search!,pageNumber??1,5));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplierAsync(int id,SupplierDto supplierDto)
        {
            if (ModelState.IsValid && supplierDto!=null)
            {
                await _supplierService.UpdateSupplierAsync(id, _mapper.Map<Supplier>(supplierDto));
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetNumberOfSuppliersAsync()
        {
            _logger.LogInformation("Count Supplier");
            return Ok(await _supplierService.GetNumberOfSuppliersAsync());
        }
    }
}
