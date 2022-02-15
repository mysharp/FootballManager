using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Restful.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Restful.IntegrationTests
{
    [TestClass]
    public class RootControllerShould
    {
        [TestMethod]
        public async Task NoContentWhenOtherMediaType()
        {
            var builder = WebHost.CreateDefaultBuilder().UseStartup<StartupIntegrationTest>();

            using (var server = new TestServer(builder))
            {
                var httpClient = server.CreateClient();

                var response = await httpClient.GetAsync("/api");

                response.EnsureSuccessStatusCode();

                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                Assert.IsTrue(string.IsNullOrEmpty(content));
            }
        }

        [TestMethod]
        public async Task ReturnLinkResourceListWhenSpecificMediaType()
        {
            var builder = WebHost.CreateDefaultBuilder().UseStartup<StartupIntegrationTest>();

            using (var server = new TestServer(builder))
            {
                var httpClient = server.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.johnson.hateoas+json");

                var response = await httpClient.GetAsync("/api");

                response.EnsureSuccessStatusCode();

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<LinkResource>>(content);
                Assert.IsTrue(list.Count > 0);

                var self = list.Single(x => x.Group == "self");
                Assert.IsNotNull(self);
            }
        }
    }
}
