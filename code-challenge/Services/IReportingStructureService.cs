using challenge.Models;

namespace challenge.Services
{
    /// <summary>
    /// Common interface for all ReportingStructure service implementations.
    /// </summary>
    public interface IReportingStructureService
    {
        /// <summary>
        /// Get the ReportingStructure for a particular employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to get a ReportingStructure for</param>
        /// <returns>The ReportingStructure for the given employee, or null if the given employee ID is null, empty,
        /// or refers to an employee that does not exist</returns>
        ReportingStructure GetByEmployeeId(string employeeId);
    }
}