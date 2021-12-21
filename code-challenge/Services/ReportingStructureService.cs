using System.Linq;
using challenge.Models;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly ILogger<ReportingStructureService> _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        private int ComputeReportCount(Employee employee)
        {
            if (employee.DirectReports == null || !employee.DirectReports.Any())
            {
                _logger.LogDebug($"{employee.FirstName} {employee.LastName} has no direct reports");
                return 0;
            }

            _logger.LogDebug($"{employee.FirstName} {employee.LastName} has " +
                             $"{employee.DirectReports.Count} direct reports");

            // Needed to eagerly fetch all direct reports for all descendants
            return employee.DirectReports.Count + employee.DirectReports.Sum(e => ComputeReportCount(_employeeService.GetById(e.EmployeeId)));
        }

        public ReportingStructure GetByEmployeeId(string employeeId)
        {
            var employee = _employeeService.GetById(employeeId);

            if (employee == null)
            {
                return null;
            }

            return new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = ComputeReportCount(employee)
            };
        }
    }
}