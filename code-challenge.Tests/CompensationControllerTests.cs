using challenge.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests : ControllerTests
    {
        [ClassInitialize]
        public new static void InitializeClass(TestContext context) => ControllerTests.InitializeClass(context);

        [ClassCleanup]
        public new static void CleanUpTest() => ControllerTests.CleanUpTest();

        private static Task<HttpResponseMessage> CreateCompensation(DataTransferCompensation compensationDto)
        {
            var requestContent = new JsonSerialization().ToJson(compensationDto);
            return HttpClient.PostAsync("api/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json"));
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var compensationDto = new DataTransferCompensation
            {
                Employee = "b7839309-3348-463b-a7e3-5de1c168beb3",
                Salary = 70_000,
                EffectiveDate = "12-25-2021"
            };
            var expectedEmployee = new Employee
            {
                EmployeeId = compensationDto.Employee,
                FirstName = "Paul",
                LastName = "McCartney",
                Position = "Developer I",
                Department = "Engineering"
            };
            
            // Execute
            var response = CreateCompensation(compensationDto).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.AreEqual(expectedEmployee.EmployeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(expectedEmployee.FirstName, newCompensation.Employee.FirstName);
            Assert.AreEqual(expectedEmployee.LastName, newCompensation.Employee.LastName);
            Assert.AreEqual(expectedEmployee.Position, newCompensation.Employee.Position);
            Assert.AreEqual(expectedEmployee.Department, newCompensation.Employee.Department);
            Assert.IsFalse(newCompensation.Employee.DirectReports.Any());
            Assert.AreEqual(compensationDto.Salary, newCompensation.Salary);
            Assert.AreEqual(compensationDto.EffectiveDate, newCompensation.EffectiveDate.ToString("MM-dd-yyyy"));
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensations = new[]
            {
                CreateCompensation(new DataTransferCompensation
                {
                    Employee = employeeId,
                    Salary = 75_000,
                    EffectiveDate = "01-10-2021"
                }).Result.DeserializeContent<Compensation>(),
                // One year later this employee will get a raise
                CreateCompensation(new DataTransferCompensation
                {
                    Employee = employeeId,
                    Salary = 80_000,
                    EffectiveDate = "01-10-2022"
                }).Result.DeserializeContent<Compensation>()
            };

            var expectedCompensations = compensations.ToDictionary(
                compensation => compensation.CompensationId,
                compensation =>
                new Compensation
                {
                    CompensationId = compensation.CompensationId,
                    Employee = compensation.Employee,
                    Salary = compensation.Salary,
                    EffectiveDate = compensation.EffectiveDate
                });
            
            // Execute
            var getRequestTask = HttpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var actualCompensations = response.DeserializeContent<IList<Compensation>>();
            Assert.AreEqual(2, actualCompensations.Count);
            foreach (var actualCompensation in actualCompensations)
            {
                var expectedCompensation = expectedCompensations[actualCompensation.CompensationId];
                Assert.AreEqual(expectedCompensation.CompensationId, actualCompensation.CompensationId);
                Assert.AreEqual(expectedCompensation.Employee.EmployeeId, actualCompensation.Employee.EmployeeId);
                Assert.AreEqual(expectedCompensation.Employee.FirstName, actualCompensation.Employee.FirstName);
                Assert.AreEqual(expectedCompensation.Employee.LastName, actualCompensation.Employee.LastName);
                Assert.AreEqual(expectedCompensation.Employee.Position, actualCompensation.Employee.Position);
                Assert.AreEqual(expectedCompensation.Employee.Department, actualCompensation.Employee.Department);
                Assert.AreEqual(expectedCompensation.Salary, actualCompensation.Salary);
                Assert.AreEqual(expectedCompensation.EffectiveDate, actualCompensation.EffectiveDate);
            }
        }
    }
}