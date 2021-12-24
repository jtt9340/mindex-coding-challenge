using System;

namespace challenge.Models
{
    /// <summary>
    /// Compensation model object. This object stores a full <see cref="Employee">Employee</see> instance rather than
    /// just its ID, so when constructing a Compensation object, an employee will need to be pulled from an
    /// <see cref="challenge.Repositories.IEmployeeRepository">Employee repository</see> when setting the Employee field.
    /// </summary>
    public class Compensation
    {
        /// <summary>
        /// The ID of this Compensation object, as a GUID.
        /// </summary>
        public string CompensationId { get; set; }
        /// <summary>
        /// The Employee that this object is compensation for.
        /// </summary>
        public Employee Employee { get; set; }
        /// <summary>
        /// The salary for the referenced employee.
        /// </summary>
        public uint Salary { get; set; }
        /// <summary>
        /// The date and time for which this compensation is effective.
        /// </summary>
        public DateTime EffectiveDate { get; set; }
    }
}