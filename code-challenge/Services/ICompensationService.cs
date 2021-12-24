using System.Collections.Generic;
using challenge.Models;

namespace challenge.Services
{
    /// <summary>
    /// Common interface for all Compensation service implementations.
    /// </summary>
    public interface ICompensationService
    {
        /// <summary>
        /// Get all compensations associated with a particular employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to get compensations for</param>
        /// <returns>All the compensations for a particular employee, including an empty enumerable if the given
        /// employee ID is null, empty, refers to a non-existent employee, or refers to an employee with
        /// no compensations</returns>
        IEnumerable<Compensation> GetByEmployeeId(string employeeId);
        
        /// <summary>
        /// Save the given Compensation object to the database.
        /// </summary>
        /// <param name="compensation">The Compensation object to save</param>
        /// <returns>The compensation object that was saved</returns>
        Compensation Create(Compensation compensation);
    }
}