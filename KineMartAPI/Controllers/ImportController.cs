using KineMartAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KineMartAPI.Controllers
{
    [Route("api/import")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;
        private readonly ILogger<ImportController> _logger;
        public ImportController(IImportService importService, ILogger<ImportController> logger)
        {
            _importService = importService;
            _logger = logger;
        }

        [HttpPost("get-import-by-date")]
        public async Task<IActionResult> GetImportByDateAsync(DateFilter dateFilter)
        {
            if (ModelState.IsValid && dateFilter !=null)
            {
                _logger.LogInformation("Query Data From Table Import By Date");
                return Ok(await _importService.GetImportByDateAsync(dateFilter));
            }
            return BadRequest(ModelState);
        }

        [HttpGet("get-total-expense")]
        public async Task<IActionResult> GetTotalExpenseAsync()
        {
            _logger.LogInformation("Total Expense");
            return Ok(await _importService.GetTotalExpenseAsync());
        }
    }
}
