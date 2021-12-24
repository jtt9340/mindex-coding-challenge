using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;

namespace challenge.Controllers
{
    /// <summary>
    /// Controller class that handles incoming HTTP requests for the <c>api/reportingStructure</c> endpoint.
    /// </summary>
    [Route("api/reportingStructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        /// <summary>
        /// Construct a new ReportingStructureController.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}">ILogger</see> instance to log activities for
        /// this class</param>
        /// <param name="reportingStructureService">The
        /// <see cref="IReportingStructureService">IReportingStructureService</see> implementation to use for this
        /// class</param>
        public ReportingStructureController(ILogger<ReportingStructureController> logger,
            IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        /// <summary>
        /// The <c>api/reportingStructure</c> GET endpoint for getting a <see cref="challenge.Models.ReportingStructure">
        /// ReportingStructure</see> object by employee ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to get a ReportingStructure object for</param>
        /// <returns>HTTP status and JSON response</returns>
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