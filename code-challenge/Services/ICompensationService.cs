using System.Collections.Generic;
using challenge.Models;

namespace challenge.Services
{
    public interface ICompensationService
    {
        IEnumerable<Compensation> GetByEmployeeId(string employeeId);
        Compensation Create(Compensation compensation);
    }
}