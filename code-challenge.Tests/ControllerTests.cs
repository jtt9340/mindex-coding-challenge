using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net.Http;

namespace code_challenge.Tests.Integration
{
    /// <summary>
    /// An abstract superclass that contains common functionality for setting up and tearing down controller tests.
    /// Other controller tests should inherit from this class and call this class'
    /// <see cref="ControllerTests.InitializeClass">InitializeClass</see> and
    /// <see cref="ControllerTests.CleanUpTest">CleanUpTest</see> methods.
    /// </summary>
    public abstract class ControllerTests
    {
        protected static HttpClient HttpClient { get; private set; }
        private static TestServer _testServer;

        /// <summary>
        /// Start up a test server and create an HTTP client for this test server.
        /// </summary>
        /// <param name="context">Used to store information that is provided to unit tests</param>
        protected static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            HttpClient = _testServer.CreateClient();
        }

        /// <summary>
        /// Disposes of the test server and HTTP client used for unit tests.
        /// </summary>
        protected static void CleanUpTest()
        {
            HttpClient.Dispose();
            _testServer.Dispose();
        }
    }
}