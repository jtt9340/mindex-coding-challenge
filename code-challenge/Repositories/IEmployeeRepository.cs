using challenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        IEnumerable<Compensation> GetByEmployeeId(String employeeId);
        Employee Add(Employee employee);
        Compensation Add(Compensation compensation);
        Employee Remove(Employee employee);
        Task SaveAsync();
    }
}