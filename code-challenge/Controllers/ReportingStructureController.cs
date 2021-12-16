using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;

namespace challenge.Controllers
{
    [Route("api/reportingStructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger,
            IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{employeeId}", Name = "getReportingStructureByEmployeeId")]
        public IActionResult GetReportingStructureByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Received ReportingStructure get request for '{employeeId}'");

            var reportingStructure = _reportingStructureService.GetByEmployeeId(employeeId);

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}