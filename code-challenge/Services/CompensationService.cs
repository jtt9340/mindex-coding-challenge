using System.Collections.Generic;
using System.Linq;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    /// <summary>
    /// The service class which contains all the functionality used by the
    /// <see cref="challenge.Controllers.CompensationController">CompensationController</see>.
    /// </summary>
    public class CompensationService : ICompensationService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CompensationService> _logger;

        /// <summary>
        /// Construct a new CompensationService.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}">ILogger</see> instance to log activities for
        /// this class</param>
        /// <param name="employeeRepository">The <see cref="IEmployeeRepository">IEmployeeRepository</see>
        /// implementation to use for this class</param>
        public CompensationService(ILogger<CompensationService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        /// <inheritdoc cref="ICompensationService.Create"/>
        public Compensation Create(Compensation compensation)
        {
            if (compensation == null)
            {
                _logger.LogDebug("Compensation to create was null; not creating anything");
            }
            else
            {
                _logger.LogDebug($"Creating compensation for employee with ID '{compensation.Employee.EmployeeId}'");
                _employeeRepository.Add(compensation);
                _employeeRepository.SaveAsync().Wait();
            }
            
            return compensation;
        }

        /// <inheritdoc cref="ICompensationService.GetByEmployeeId"/>
        public IEnumerable<Compensation> GetByEmployeeId(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                _logger.LogDebug("No compensation with an employee whose ID is null or empty!");
                return Enumerable.Empty<Compensation>();
            }

            _logger.LogDebug($"Getting compensation for employee with ID '{employeeId}'");
            return _employeeRepository.GetByEmployeeId(employeeId);
        }
    }
}