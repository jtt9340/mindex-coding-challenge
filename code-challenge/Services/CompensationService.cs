using System.Collections.Generic;
using System.Linq;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
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
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
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
            return _compensationRepository.GetByEmployeeId(employeeId);
        }
    }
}