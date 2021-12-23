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
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.Compensations.Add(compensation);
            _logger.LogDebug($"Persisted compensation with ID '{compensation.CompensationId}'");
            return compensation;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.Include("DirectReports").SingleOrDefault(e => e.EmployeeId == id);
        }

        public IEnumerable<Compensation> GetByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Getting compensation for employee with ID '{employeeId}'");
            return _employeeContext.Compensations.Include("Employee")
                .Where(compensation => compensation.Employee.EmployeeId == employeeId);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
