namespace challenge.Models
{
    /// <summary>
    /// ReportingStructure model object. This object stores a full <see cref="Employee">Employee</see> instance rather
    /// than just its ID.
    /// </summary>
    public class ReportingStructure
    {
        /// <summary>
        /// The employee for which this ReportingStructure refers to.
        /// </summary>
        public Employee Employee { get; set; }
        /// <summary>
        /// The number of reports that this employee has, as described in the README of this project.
        /// </summary>
        public int NumberOfReports { get; set; }
    }
}