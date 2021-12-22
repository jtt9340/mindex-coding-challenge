using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _compensationContext.Compensations.Add(compensation);
            _logger.LogDebug($"Persisted compensation with ID '{compensation.CompensationId}'");
            return compensation;
        }

        public IEnumerable<Compensation> GetByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Getting compensation for employee with ID '{employeeId}'");
            return from compensation in _compensationContext.Compensations.Include("Employee")
                where compensation.Employee.EmployeeId == employeeId
                select compensation;
        }

        public Task SaveAsync() => _compensationContext.SaveChangesAsync();
    }
}