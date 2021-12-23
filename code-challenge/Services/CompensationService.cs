using System.Collections.Generic;
using System.Linq;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, IEmployeeRepository compensationRepository)
        {
            _employeeRepository = compensationRepository;
            _logger = logger;
        }

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