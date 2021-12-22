using challenge.Models;
using System;

namespace challenge.Repositories
{
    public interface IEmployeeRepository : IRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
    }
}