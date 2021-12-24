using System.Linq;
using challenge.Models;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    /// <summary>
    /// The service class which contains all the functionality used by the
    /// <see cref="challenge.Controllers.ReportingStructureController">ReportingStructureController</see>.
    /// </summary>
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly ILogger<ReportingStructureService> _logger;
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Construct a new ReportingStructureController.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}">ILogger</see> instance to log activities for
        /// this class</param>
        /// <param name="employeeService">The <see cref="IEmployeeService">IEmployeeService</see> implementation
        /// to use for this class</param>
        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Recursively descend into an employee's direct reports and count all the direct reports that the given
        /// employee has, either directly or indirectly.
        ///
        /// More information on how a report count is computing can be found in this project's README.
        /// </summary>
        /// <param name="employee">The employee to compute the number of direct reports for</param>
        /// <returns>The report count</returns>
        private int ComputeReportCount(Employee employee)
        {
            _logger.LogDebug($"{employee.FirstName} {employee.LastName} has " +
                             $"{employee.DirectReports.Count} direct reports");

            // Needed to eagerly fetch all direct reports for all descendants
            return employee.DirectReports.Count + employee.DirectReports.Sum(e =>
                ComputeReportCount(_employeeService.GetById(e.EmployeeId)));
        }

        /// <inheritdoc cref="IReportingStructureService.GetByEmployeeId"/>
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