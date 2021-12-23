using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net.Http;

namespace code_challenge.Tests.Integration
{
    public abstract class ControllerTests
    {
        protected static HttpClient HttpClient { get; private set; }
        private static TestServer _testServer;

        protected static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            HttpClient = _testServer.CreateClient();
        }

        protected static void CleanUpTest()
        {
            HttpClient.Dispose();
            _testServer.Dispose();
        }
    }
}