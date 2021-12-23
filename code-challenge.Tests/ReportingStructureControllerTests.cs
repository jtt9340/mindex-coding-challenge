using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System.Linq;
using System.Net;
using System.Net.Http;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests : ControllerTests
    {
        [ClassInitialize]
        public new static void InitializeClass(TestContext context) => ControllerTests.InitializeClass(context);

        [ClassCleanup]
        public new static void CleanUpTest() => ControllerTests.CleanUpTest();
        
        [TestMethod]
        public void GetReportingStructureByEmployeeId_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedPosition = "Development Manager";
            var expectedDepartment = "Engineering";
            var expectedDirectReports = new[]
            {
                "b7839309-3348-463b-a7e3-5de1c168beb3",
                "03aa1462-ffa9-4978-901b-7c001562cf6f"
            };
            var expectedNumberOfReports = 4;
            
            // Execute
            var getRequestTask = HttpClient.GetAsync($"api/reportingStructure/{employeeId}");
            var response = getRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.Employee.LastName);
            Assert.AreEqual(expectedPosition, reportingStructure.Employee.Position);
            Assert.AreEqual(expectedDepartment, reportingStructure.Employee.Department);
            CollectionAssert.AreEquivalent(expectedDirectReports, reportingStructure.Employee.DirectReports?.Select(e => e.EmployeeId).ToArray());
            Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports);
        }
    }
}