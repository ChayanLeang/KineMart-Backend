using KineMartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KineMartAPI.Controllers
{
    [Route("api/export")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _exportService;
        private readonly ILogger<ExportController> _logger;
        public ExportController(IExportService exportService, ILogger<ExportController> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        [HttpPost("get-export-by-date")]
        public async Task<IActionResult> GetExportByDateAsync(DateFilter dateFilter)
        {
            if(ModelState.IsValid && dateFilter != null)
            {
                _logger.LogInformation("Query Data From Table Export By Date");
                return Ok(await _exportService.GetExportByDateAsync(dateFilter));
            }
            return BadRequest(ModelState);
        }

        [HttpGet("get-total-revenus")]
        public async Task<IActionResult> GetTotalRevenusAsync()
        {
            _logger.LogInformation("Total Revenus");
            return Ok(await _exportService.GetTotalRevenusAsync());
        }
    }
}
