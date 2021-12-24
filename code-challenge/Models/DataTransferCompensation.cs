namespace challenge.Models
{
    /// <summary>
    /// A subset of the fields stored in the <see cref="Compensation">Compensation</see> object.
    ///
    /// This object exists so that, when POSTing to the <c>api/compensation</c> endpoint, a client only has to specify
    /// an employee ID, salary, and date as a string, rather than needing to specify a complete Employee object, i.e.
    /// first and last name, position and department, etc. as well as all the fields of a
    /// <see cref="System.DateTime">DateTime</see> object.
    /// </summary>
    public class DataTransferCompensation
    {
        /// <summary>
        /// The ID of an employee to create a Compensation object for.
        /// </summary>
        public string Employee { get; set; }
        /// <summary>
        /// The salary of the compensation object to be created.
        /// </summary>
        public uint Salary { get; set; }
        /// <summary>
        /// The date for which this compensation if effective, as a string. When a compensation object is being created
        /// and persisted, this field will be parsed according to
        /// <see cref="System.DateTime.TryParse(string?, out System.DateTime)">DateTime.TryParse</see>, and so can be
        /// in any format recognized by that method.
        /// </summary>
        public string EffectiveDate { get; set; }
    }
}