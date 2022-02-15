using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace Restful.IntegrationTests
{
    public class TestApplication : IDisposable
    {
        private readonly TestServer _testServer;

        public HttpClient HttpClient { get; }

        public TestApplication()
        {
            var builder = WebHost.CreateDefaultBuilder().UseStartup<StartupIntegrationTest>();

            _testServer = new TestServer(builder);

            HttpClient = _testServer.CreateClient();
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            _testServer.Dispose();
        }
    }
}
