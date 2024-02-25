using KineMartAPI.ModelDtos;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KineMartAPI.Controllers
{
    [Route("api/sell")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public async Task<IActionResult> SellAsync(SaleDto saleDto)
        {
            if (ModelState.IsValid && saleDto !=null)
            {
                await _saleService.SellAsync(saleDto);
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}
