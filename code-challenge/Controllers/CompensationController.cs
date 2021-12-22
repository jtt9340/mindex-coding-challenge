using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

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

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationsByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Received compensation get request for employee with ID '{employeeId}'");

            var compensations = _compensationService.GetByEmployeeId(employeeId);

            if (!compensations.Any())
                return NotFound();

            return Ok(compensations);
        }

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