using System.Collections.Generic;
using challenge.Models;

namespace challenge.Repositories
{
    public interface ICompensationRepository : IRepository
    {
        IEnumerable<Compensation> GetByEmployeeId(string employeeId);
        Compensation Add(Compensation compensation);
    }
}