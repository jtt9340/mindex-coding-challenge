using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    /// <summary>
    /// Controller class that handles incoming HTTP requests for the <c>api/compensation</c> endpoint.
    /// </summary>
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Construct a new CompensationController.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}">ILogger</see> instance to log activities for
        /// this class</param>
        /// <param name="compensationService">The <see cref="ICompensationService">ICompensationService</see> instance
        /// to use for this class</param>
        /// <param name="employeeService">The <see cref="IEmployeeService">IEmployeeService</see> implementation to use
        /// for this class</param>
        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

        /// <summary>
        /// The <c>api/compensation</c> POST endpoint used to create new <see cref="Compensation">Compensation</see>
        /// objects. The fields required in the POST request content to create a new Compensation are a subset of
        /// the fields in a Compensation object: only the employee ID, salary, and effective date are needed.
        /// </summary>
        /// <param name="compensationDto">Object containing information about the new Compensation to create</param>
        /// <returns>HTTP status</returns>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] DataTransferCompensation compensationDto)
        {
            if (!TryDtoToCompensation(compensationDto, out var compensation))
            {
                _logger.LogError(compensationDto.EffectiveDate + " could not be parsed as a proper date");
                return BadRequest();
            }
            
            _logger.LogDebug("Received compensation create request for " +
                             $"employee with ID '{compensation.Employee.EmployeeId}'");

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationByEmployeeId",
                new {employeeId = compensation.Employee.EmployeeId}, compensation);
        }

        /// <summary>
        /// The <c>api/compensation/{employeeId}</c> GET endpoint used to get a Compensation object by employee ID
        /// (NOT compensation ID). Since an employee can have multiple compensations, this endpoint will return a list
        /// of them.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to get compensation(s) for</param>
        /// <returns>HTTP status and JSON response</returns>
        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationsByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Received compensation get request for employee with ID '{employeeId}'");

            var compensations = _compensationService.GetByEmployeeId(employeeId);

            if (!compensations.Any())
                return NotFound();

            return Ok(compensations);
        }

        /// <summary>
        /// Convert a <see cref="DataTransferCompensation">Data Transfer Compensation</see> object, which contains a
        /// subset of the fields of a <see cref="Compensation">Compensation</see> object, into a proper Compensation
        /// object by getting a complete <see cref="Employee">Employee</see> object from the employee ID in the given
        /// Data Transfer Compensation object and parsing the
        /// <c cref="DataTransferCompensation.EffectiveDate">DataTransferCompensation.EffectiveDate</c> into a proper
        /// <see cref="DateTime">DateTime</see> object. The converted Compensation object is returned via an out
        /// parameter.
        /// </summary>
        /// <param name="compensationDto">The Data Transfer Compensation object to convert into a (complete)
        /// Compensation object</param>
        /// <param name="compensation">The converted Compensation object, or null if parsing the effective date of a
        /// Data Transfer Compensation object failed</param>
        /// <returns>True if conversion succeeded, false otherwise. Conversion can fail if the effective date of a
        /// Data Transfer Compensation cannot be parsed into a DateTime instance</returns>
        private bool TryDtoToCompensation(DataTransferCompensation compensationDto, out Compensation compensation)
        {
            if (!DateTime.TryParse(compensationDto.EffectiveDate, out var effectiveDate))
            {
                compensation = null;
                return false;
            }

            compensation = new Compensation
            {
                CompensationId = null,
                Employee = _employeeService.GetById(compensationDto.Employee),
                Salary = compensationDto.Salary,
                EffectiveDate = effectiveDate
            };
            return true;
        }
    }
}